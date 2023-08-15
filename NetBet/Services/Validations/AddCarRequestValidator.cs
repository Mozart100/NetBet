using NetBet.Models.Dtos;
using FluentValidation;

namespace NetBet.Services.Validations
{
    public class AddCarRequestValidator : AbstractValidator<AddCarRequest>
    {
        public AddCarRequestValidator()
        {
            RuleFor(req => (int)req.CarGroupType).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
            RuleFor(req => (int)req.DrivingAgeGroup).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
