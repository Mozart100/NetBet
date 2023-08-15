using FluentValidation;
using NetBet.DataAccess.Repository;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;

namespace NetBet.Services.Validations
{
    public interface ICarValidationService
    {
        Task AddCarRequestValidateAsync(AddCarRequest request);
        Task GetCarQueryRequestValidateAsync(GetCarQueryRequest request);
        Task RemoveCarRequestValidateAsync(RemoveCarRequest request);
    }

    public class CarValidationService : ServiceValidatorBase, ICarValidationService
    {
        private readonly ILogger<CarValidationService> _logger;
        private readonly ICarRepository _carRepository;

        public CarValidationService(ILogger<CarValidationService> logger,
            ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }

        public async Task AddCarRequestValidateAsync(AddCarRequest request)
        {
            var validator = new AddCarRequestValidator();
            var validationResult = validator.Validate(request);

            var errors = Dissect(validationResult);

            Validate(errors);

            if (await _carRepository.GetFirstAsync(x => x.NameModel.Equals(request.NameModel, StringComparison.OrdinalIgnoreCase)) != null)
            {
                errors.Add(new NetBetError(propertyName: nameof(AddCarRequest.NameModel), errorMessage: "Model Name should be unique"));
                Validate(errors);
            }
        }

        public async Task GetCarQueryRequestValidateAsync(GetCarQueryRequest request)
        {
            var validator = new GetCarQueryRequestValidator();
            var validationResult = validator.Validate(request);

            var errors = Dissect(validationResult);
            Validate(errors);

            if (request.CarId != null)
            {
                if (await _carRepository.GetFirstAsync(x => x.NameModel.Equals(request.NameModel, StringComparison.OrdinalIgnoreCase)) != null)
                {
                    errors.Add(new NetBetError(propertyName: nameof(GetCarQueryRequest.CarId), errorMessage: "Should be bigger than 0."));
                    Validate(errors);
                }
            }
        }

        public async Task RemoveCarRequestValidateAsync(RemoveCarRequest request)
        {
            var validator = new RemoveCarRequestValidator();
            var validationResult = validator.Validate(request);

            var errors = Dissect(validationResult);
            Validate(errors);

        }
    }
}
