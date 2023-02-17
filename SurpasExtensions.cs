using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sumday.BoundedContext.SharedKernel;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;
using Sumday.Infrastructure.Common.Caching;
using Sumday.Infrastructure.Common.Http;
using Sumday.Infrastructure.Common.Security;
using Sumday.Infrastructure.Common.Security.Certificates;
using Sumday.Infrastructure.Common.Validation;
using Sumday.Infrastructure.Extensions;
using Sumday.Infrastructure.Surpas;
using Sumday.Infrastructure.Surpas.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SurpasExtensions
    {
        public static IServiceCollection AddSurpas(this IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(ISurpasService)))
            {
                return services;
            }

            services.AddOptions<SurpasConfiguration>()
             .Configure<IConfiguration>((opt, config) => config.Bind("surpas", opt));

            services.AddHttpClient<ISurpasService, SurpasService>()
                .SetHandlerLifetime(TimeSpan.FromDays(1))
                .ConfigureHttpClient((sp, client) =>
                {
                    var surpasConfig = sp.GetRequiredService<IOptions<SurpasConfiguration>>().Value;
                    client.BaseAddress = new Uri(surpasConfig.Url);
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                })
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var handler = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Manual, UseProxy = false };
                    var certificateProvider = sp.GetRequiredService<ICertificateStore>();
                    var surpasConfig = sp.GetRequiredService<IOptions<SurpasConfiguration>>().Value;
                    var surpasCertificate = certificateProvider.Get(surpasConfig.CertificateThumbPrint);
                    handler.ClientCertificates.Add(surpasCertificate);
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    handler.MaxConnectionsPerServer = 10;
                    return handler;
                })
                .AddHttpMessageHandler(sp =>
                {
                    var logger = sp.GetService<ILogger<SurpasService>>();
                    var surpasConfig = sp.GetRequiredService<IOptions<SurpasConfiguration>>().Value;
                    return new HttpClientLoggingHandler(logger) { ExtendedLogging = surpasConfig.ExtendedLogging };
                })
                .AddPolly();

            services.AddSendGridEmailSender();
            services.AddTransient<ICommandUseCaseInteractor, CommandUseCaseInteractor>();
            services.AddTransient<IQueryUseCaseInteractor, QueryUseCaseInteractor>();
            services.AddCertificates();
            var assemblies = AppDomain.CurrentDomain.GetAllAssemblies();
            services.AddMediatR(assemblies.ToArray());
            services.AddValidatorsFromAssemblies(assemblies);
            services.AddAutoMapper(
                   (sp, cfg) =>
                   {
                       cfg.ConstructServicesUsing(type => sp.GetRequiredService(type));
                       cfg.AllowNullCollections = true;
                   }, assemblies);
            services.AddSingleton<IValidatorFactory, FluentValidationFactory>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IAggregateCollection<>), typeof(AggregateCollection<>));

            var specificationTypes = assemblies.Where(assmb => assmb.GetName().Name.StartsWith("Sumday.BoundedContext"))
                .SelectMany(a => a.ExportedTypes).Where(t => t.Implements(typeof(ISpecification<>)) && !t.IsAbstract).ToList();

            foreach (var specification in specificationTypes)
            {
                var aggregateType = specification.FindOpenGenericInterface(typeof(ISpecification<>)).GetGenericArguments()[0];
                var interfaceHandlerType = typeof(IGetSpecificationEvaluator<,>).MakeGenericType(aggregateType, specification);
                var concreteHandlerType = typeof(GetCachedSpecificationEvaluator<,>).MakeGenericType(aggregateType, specification);
                if (!services.Any(x => x.ServiceType == interfaceHandlerType))
                {
                    services.AddTransient(interfaceHandlerType, concreteHandlerType);
                }
            }

            var allSpecificationTypes = assemblies.Where(assmb => assmb.GetName().Name.StartsWith("Sumday.BoundedContext"))
             .SelectMany(a => a.ExportedTypes).Where(t => t.Implements(typeof(IAllSpecification<>)) && !t.IsAbstract).ToList();

            foreach (var allSpecification in allSpecificationTypes)
            {
                var aggregateType = allSpecification.FindOpenGenericInterface(typeof(IAllSpecification<>)).GetGenericArguments()[0];
                var interfaceHandlerType = typeof(IGetAllSpecificationEvaluator<,>).MakeGenericType(aggregateType, allSpecification);
                var concreteHandlerType = typeof(GetCachedAllSpecificationEvaluator<,>).MakeGenericType(aggregateType, allSpecification);
                if (!services.Any(x => x.ServiceType == interfaceHandlerType))
                {
                    services.AddTransient(interfaceHandlerType, concreteHandlerType);
                }
            }

            services.AddSingleton<ICacheProvider, DistributedCacheProvider>();
            services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(x => x.AssignableTo(typeof(ISetAndGetBasedonSpecification<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(x => x.AssignableTo(typeof(IAggregateService<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(x => x.AssignableTo(typeof(IEntityService<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.AddScoped<ICallContext, SurpasCallContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
