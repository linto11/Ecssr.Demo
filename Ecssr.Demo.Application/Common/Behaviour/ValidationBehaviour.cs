using Ecssr.Demo.Application.Common.Enums;
using Ecssr.Demo.Application.Common.Exceptions;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common.Utility;
using FluentValidation;
using MediatR;

namespace Ecssr.Demo.Application.Common.Behaviour
{
    /// <summary>
    /// This class is part of the FluentValidation library. In this part we parse the error message to our custom required class.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        /// <summary>
        /// This methods parses the vlaidation error message from Fluent Validation to our custom entity.
        /// </summary>
        /// <param name="request">The input request</param>
        /// <param name="next">Request handler delegate</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Throw the bad request exception</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var validationFailures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if(validationFailures.Count > 0)
                {
                    var validationErrors = new List<ValidationError>();
                    foreach (var validationFailure in validationFailures)
                        validationErrors.Add(new ValidationError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            ErrorNumber = (ErrorNumber)SafeType.SafeInt(validationFailure.ErrorCode)
                        });

                    //get distinct
                    validationErrors = validationErrors.GroupBy(v => v.ErrorNumber).Select(g => g.First()).ToList();

                    BadRequestException.Throw("The parameters contain invalid data.", validationErrors);
                }
            }

            return await next();
        }
    }
}
