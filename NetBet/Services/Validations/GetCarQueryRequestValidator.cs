using NetBet.Models.Dtos;
using FluentValidation;

namespace NetBet.Services.Validations
{
    public class GetCarQueryRequestValidator : AbstractValidator<GetCarQueryRequest>
    {
        public GetCarQueryRequestValidator()
        {
            RuleFor(req => req.CarId).Must(carId => carId == null || carId > 0).WithMessage("{PropertyName} can be null or is should be greater than zero.");
        }
    }
}
