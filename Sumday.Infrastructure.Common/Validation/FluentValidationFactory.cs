using System;
using FluentValidation;

namespace Sumday.Infrastructure.Common.Validation
{
    public class FluentValidationFactory : ValidatorFactoryBase
    {
        private readonly IServiceProvider serviceProvider;

        public FluentValidationFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return this.serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
