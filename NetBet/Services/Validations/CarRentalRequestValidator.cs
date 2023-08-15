using NetBet.Models.Dtos;
using FluentValidation;

namespace NetBet.Services.Validations
{
    public class CarRentalRequestValidator : AbstractValidator<CarRentalRequest>
    {
        public CarRentalRequestValidator()
        {
            RuleFor(req => req.CarID).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
            RuleFor(req => req.AgentId).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

            RuleFor(req => req.To).GreaterThan(req => req.From).WithMessage("{PropertyName} To should be bigger than From.");
        }
    }
}
