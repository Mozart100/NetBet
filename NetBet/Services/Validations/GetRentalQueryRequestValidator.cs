using FluentValidation;
using NetBet.Models.Dtos;

namespace NetBet.Services.Validations
{
    internal class GetRentalQueryRequestValidator : AbstractValidator<GetRentalQueryRequest>
    {
        public GetRentalQueryRequestValidator()
        {
            //RuleFor(req => req.CarID).Must(carId => carId == null || carId > 0).WithMessage("{PropertyName} can be null or is should be greater than zero.");
            //RuleFor(req => req.AgentId).Must(agentId => agentId == null || agentId > 0).WithMessage("{PropertyName} can be null or is should be greater than zero.");
            //RuleFor(req => req.To).GreaterThan(req=>req.From).WithMessage("{PropertyName} To should be bigger than From.");

            //RuleFor(req => req.From).NotNull().When(req => req.To == null) // Additional condition to check if 'To' is null
            //        .WithMessage("To and From should be provided.");


            //RuleFor(req => req.To).NotNull().When(req => req.From == null) // Additional condition to check if 'To' is null
            //        .WithMessage("To and From should be provided.");


            RuleFor(req => req.CarID).Must(carId => carId == null || carId > 0).WithMessage("{PropertyName} can be null or should be greater than zero.");
            RuleFor(req => req.AgentId).Must(agentId => agentId == null || agentId > 0).WithMessage("{PropertyName} can be null or should be greater than zero.");

            RuleFor(req => req.To).GreaterThan(req => req.From).WithMessage("{PropertyName} To should be bigger than From.")
                .When(req => req.From != null && req.To != null); // Apply the rule only when both From and To are not null

            RuleFor(req => req.From).NotNull().When(req => req.To == null) // Additional condition to check if 'To' is null
                .WithMessage("To and From should be provided.")
                .Unless(req => req.To == null); // Allow From to be null only if To is also null

            RuleFor(req => req.To).NotNull().When(req => req.From == null) // Additional condition to check if 'From' is null
                .WithMessage("To and From should be provided.")
                .Unless(req => req.From == null); // Allow To to be null only if From is also null


        }
    }
}