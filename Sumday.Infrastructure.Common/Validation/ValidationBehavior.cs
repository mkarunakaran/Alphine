using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Sumday.Infrastructure.Common.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidationBehavior(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validator = this.validatorFactory.GetValidator<TRequest>();
            if (validator != null)
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResult = await validator.ValidateAsync(context);
                var failures = validationResult.Errors.Where(f => f != null).ToList();
                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
