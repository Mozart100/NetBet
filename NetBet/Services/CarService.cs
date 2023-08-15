using AutoMapper;
using NetBet.DataAccess.Models;
using NetBet.DataAccess.Repository;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;
using NetBet.Services.Validations;
using System.Linq.Expressions;

namespace NetBet.Services
{
    public interface ICarService
    {
        Task<GetCarQueryResponse> GetCarsAsync(GetCarQueryRequest request);
        Task<RemoveCarResponse> RemoveCarAsync(RemoveCarRequest request);
        Task<AddCarResponse> StoreCarAsync(AddCarRequest request);
    }

    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly ICarValidationService _carValidationService;

        public CarService(ILogger<CarService> logger,
            IMapper mapper,
            ICarRepository carRepository,
            ICarValidationService carValidationService)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._carRepository = carRepository;
            this._carValidationService = carValidationService;
        }

        public async Task<GetCarQueryResponse> GetCarsAsync(GetCarQueryRequest request)
        {
            if (request == null)
            {
                //Retrieve all cars!!
                //var cars = await _carRepository.GetAllActiveCarsAsync();
                var cars = await _carRepository.GetAllAsync();
                return CreateValidResponse(cars);
            }

            await _carValidationService.GetCarQueryRequestValidateAsync(request);

            if (request.CarId != null)
            {
                var car = await _carRepository.GetFirstAsync(x => x.Id == request.CarId);
                return CreateValidResponse(new List<Car> { car });
            }

            IEnumerable<Car> carsFound = null;

            if (request.OrOperandOperation)
            {
                carsFound = await _carRepository.GetAllAsync(x =>
                        (request.CarGroupType != null && x.CarGroupType == request.CarGroupType) ||
                        (request.DrivingAgeGroup != null && x.DrivingAgeGroup == request.DrivingAgeGroup) ||
                        (request.CarDrivingPlates.IsNotEmpty() && x.CarDrivingPlates.Contains(request.CarDrivingPlates, StringComparison.OrdinalIgnoreCase)) ||
                        (request.NameModel.IsNotEmpty() && x.NameModel.Contains(request.NameModel, StringComparison.OrdinalIgnoreCase)) ||
                        (request.IsActive != null && x.IsActive == request.IsActive)
                        );
            }
            else
            {

                Expression<Func<Car, bool>> combinedCondition = null;

                if (request.CarGroupType != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<Car>(combinedCondition, x => x.CarGroupType == request.CarGroupType);
                }

                if (request.DrivingAgeGroup != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<Car>(combinedCondition, x => x.DrivingAgeGroup == request.DrivingAgeGroup);
                }

                if (request.CarDrivingPlates.IsNotEmpty())
                {
                    combinedCondition = ExpressionHelper.CreateCondition<Car>(combinedCondition, x => x.CarDrivingPlates.Contains(request.CarDrivingPlates, StringComparison.OrdinalIgnoreCase));
                }

                if (request.NameModel.IsNotEmpty())
                {
                    combinedCondition = ExpressionHelper.CreateCondition<Car>(combinedCondition, x => x.NameModel.Contains(request.NameModel, StringComparison.OrdinalIgnoreCase));
                }

                if (request.IsActive != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<Car>(combinedCondition, x => x.IsActive == request.IsActive);
                }

                Expression<Func<int, bool>> expr = i => i < 5;
                Func<Car, bool> deleg2 = combinedCondition.Compile();

                // Now use the combinedCondition in the GetAllAsync method
                carsFound = await _carRepository.GetAllAsync(deleg2);

            }

            return CreateValidResponse(carsFound);

            //Inner function.
            GetCarQueryResponse CreateValidResponse(IEnumerable<Car> instances)
            {
                var response = new GetCarQueryResponse();
                response.Cars = _mapper.Map<IEnumerable<CarItem>>(instances);
                response.IsOperationPassed = true;
                response.Request = request;
                return response;
            }
        }

        public async Task<RemoveCarResponse> RemoveCarAsync(RemoveCarRequest request)
        {
            await _carValidationService.RemoveCarRequestValidateAsync(request);

            var res = await _carRepository.DecommissionCarAsync(x => x.Id == request.CarId);
            return new RemoveCarResponse
            {
                Request = request,
                IsOperationPassed = true,
            };
        }

        public async Task<AddCarResponse> StoreCarAsync(AddCarRequest request)
        {
            await _carValidationService.AddCarRequestValidateAsync(request);

            var car = _mapper.Map<Car>(request);
            car.IsActive = true;
            var storedCar = await _carRepository.InsertAsync(car);

            var response = _mapper.Map<AddCarResponse>(storedCar);
            response.IsOperationPassed = true;
            response.Request = request;

            return response;
        }
    }
}
