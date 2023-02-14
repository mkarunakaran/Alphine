using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sumday.Infrastructure.Extensions;
using System;
using System.Diagnostics;

namespace Sumday.Service.ShareHolder
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            if(Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(
                  options =>
                  {
                      var baseUrl = Configuration.GetHost();
                      Uri uri = new Uri(baseUrl);
                      options.Configuration = Configuration.Get<string>("redis:configuration");
                      options.InstanceName = uri.Host;
                  });
            }
            services.AddShareHolder();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sumday.Service.ShareHolder", Version = "v1" });
            });
            services.Configure<TelemetryConfiguration>(
            (o) =>
            {
                o.InstrumentationKey = Configuration.Get<string>("ApplicationInsights:InstrumentationKey") ?? string.Empty;
                o.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                o.TelemetryChannel.DeveloperMode = Environment.IsDevelopment() || Debugger.IsAttached;
                if (Environment.IsDevelopment() && o.TelemetryChannel is ServerTelemetryChannel)
                {
                    var telemetryChannel = o.TelemetryChannel as ServerTelemetryChannel;
                    telemetryChannel.MaxTransmissionBufferCapacity = 7864320;
                }
            });
            services.AddApplicationInsightsTelemetry(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sumday.Service.ShareHolder v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
