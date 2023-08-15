using NetBet.Models.Dtos;
using FluentValidation;

namespace NetBet.Services.Validations
{
    public class RemoveCarRequestValidator : AbstractValidator<RemoveCarRequest>
    {
        public RemoveCarRequestValidator()
        {
            RuleFor(req => req.CarId).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
