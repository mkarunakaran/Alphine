using System.Linq;
using Microsoft.Extensions.Configuration;
using Sumday.Infrastructure.Common.Messaging;
using Sumday.Infrastructure.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SendGridExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IEmailSender)))
            {
                services.AddOptions<SendGridConfiguration>()
                            .Configure<IConfiguration>((opt, config) => config.BindTo("sendGrid", opt));
                services.AddSingleton<IEmailSender, SendGridEmailSender>();
            }

            return services;
        }
    }
}
