using FluentValidation;
using NetBet.DataAccess.Repository;
using NetBet.Models.Dtos;

namespace NetBet.Services.Validations
{
    public interface ICarRentalValidationService
    {
        Task CarRentalRequestValidateAsync(CarRentalRequest request);
        Task GetCarRentailQueryRequestValidateAsync(GetRentalQueryRequest request);
    }

    public class CarRentalValidationService : ServiceValidatorBase, ICarRentalValidationService
    {
        private readonly ILogger<CarRentalValidationService> _logger;
        private readonly ICarRepository _carRepository;
        private readonly IRentalDateSlotService _rentalDateSlotService;
        private readonly ICarRentalRepository _carRentalRepository;

        public CarRentalValidationService(ILogger<CarRentalValidationService> logger,
            ICarRepository carRepository,
            IRentalDateSlotService rentalDateSlotService,
            ICarRentalRepository carRentalRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            this._rentalDateSlotService = rentalDateSlotService;
            this._carRentalRepository = carRentalRepository;
        }

        public async Task CarRentalRequestValidateAsync(CarRentalRequest request)
        {
            var validator = new CarRentalRequestValidator();
            var validationResult = validator.Validate(request);

            var errors = Dissect(validationResult);

            Validate(errors);

            if (await _carRepository.GetFirstAsync(x => x.Id == request.CarID) == null)
            {
                errors.Add(new NetBetError(propertyName: nameof(CarRentalRequest.CarID), errorMessage: "Such car doent exists."));
                Validate(errors);
            }

            var isAvailable = await _rentalDateSlotService.IsAvailable(request.CarID, request.From, request.To);

            if (isAvailable == false)
            {
                errors.Add(new NetBetError(propertyName: "Rental Dates", errorMessage: "On this dates this car already rented - Please pick different dates."));
                Validate(errors);
            }
        }

        public async Task GetCarRentailQueryRequestValidateAsync(GetRentalQueryRequest request)
        {
            var validator = new GetRentalQueryRequestValidator();
            var validationResult = validator.Validate(request);
            var errors = Dissect(validationResult);
            Validate(errors);

            if (request.RentalReceiptId != null)
            {
                var item = await _carRentalRepository.GetFirstAsync(x => x.Id == request.RentalReceiptId);
                if (item == null)
                {
                    errors.Add(new NetBetError(propertyName: nameof(GetRentalQueryRequest.RentalReceiptId), errorMessage: "Such car doent exists."));
                    Validate(errors);
                }
            }

            //The validator ensures that if either To or From is available, then the other one must also be present.
            if (request.From != null && request.CarID != null)
            {
                var isAvailable = await _rentalDateSlotService.IsAvailable(request.CarID.Value, request.From.Value, request.To.Value);
                if (isAvailable == false)
                {
                    errors.Add(new NetBetError(propertyName: "Rental Dates", errorMessage: "On this dates this car is already rented - Please pick different dates."));
                    Validate(errors);
                }
            }
        }
    }


}
