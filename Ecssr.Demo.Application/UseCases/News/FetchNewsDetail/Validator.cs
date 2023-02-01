using Ecssr.Demo.Application.Common.Enums;
using Ecssr.Demo.Common.Utility;
using FluentValidation;

namespace Ecssr.Demo.Application.UseCases.News.FetchNewsDetail
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            _ = RuleFor(r => r.Id).NotNull()
                .WithMessage(ErrorNumber.NewsIdIsRequired.GetDescription())
                .WithErrorCode(((int)ErrorNumber.NewsIdIsRequired).ToString())
                .WithName(ErrorNumber.NewsIdIsRequired.ToString())
                .DependentRules(() =>
                {
                    RuleFor(r => r.Id)
                    .Must(BeValidGuid)
                    .WithMessage(ErrorNumber.InvalidNewsId.GetDescription())
                    .WithErrorCode(((int)ErrorNumber.InvalidNewsId).ToString())
                    .WithName(ErrorNumber.InvalidNewsId.ToString());
                });
        }

        private static bool BeValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
