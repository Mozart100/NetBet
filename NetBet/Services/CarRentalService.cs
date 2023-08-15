using AutoMapper;
using NetBet.DataAccess.Models;
using NetBet.DataAccess.Repository;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;
using NetBet.Services.Validations;
using System.Linq.Expressions;

namespace NetBet.Services
{
    public interface ICarRentalService
    {
        Task<CarRentalResponse> CarRentalCarAsync(CarRentalRequest request);
        Task<GetRentalQueryResponse> GetCarRentailReceiptAsync(GetRentalQueryRequest request);
    }

    public class CarRentalService : ICarRentalService
    {
        private readonly ILogger<CarRentalService> _logger;
        private readonly IMapper _mapper;
        private readonly ICarRentalValidationService _rentValidationService;
        private readonly ICarRentalRepository _carRentalRepository;
        private readonly IRentalDateSlotService _rentalDateSlotService;

        public CarRentalService(ILogger<CarRentalService> logger,
            IMapper mapper,
            ICarRentalValidationService rentValidationService,
            ICarRentalRepository carRentalRepository,
            IRentalDateSlotService rentalDateSlotService)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._rentValidationService = rentValidationService;
            this._carRentalRepository = carRentalRepository;
            this._rentalDateSlotService = rentalDateSlotService;
        }

        public async Task<CarRentalResponse> CarRentalCarAsync(CarRentalRequest request)
        {
            //Inside also check for available dates pair a car.
            await _rentValidationService.CarRentalRequestValidateAsync(request);

            var rentalDetail = _mapper.Map<RentDetails>(request);
            var stored = await _carRentalRepository.InsertAsync(rentalDetail);


            var response = _mapper.Map<CarRentalResponse>(stored);

            _rentalDateSlotService.Add(request.CarID, request.From, request.To);

            response.IsOperationPassed = true;
            response.Request = request;
            return response;
        }

        public async Task<GetRentalQueryResponse> GetCarRentailReceiptAsync(GetRentalQueryRequest request)
        {
            if (request == null)
            {
                //Retrieve all rentailDetails!!
                var rentDetails = await _carRentalRepository.GetAllAsync();
                return CreateValidResponse(rentDetails);
            }

            await _rentValidationService.GetCarRentailQueryRequestValidateAsync(request);

            if (request.RentalReceiptId != null)
            {
                var receipt = await _carRentalRepository.GetFirstAsync(x => x.Id == request.RentalReceiptId);
                return CreateValidResponse(new List<RentDetails> { receipt });
            }

            IEnumerable<RentDetails> receipnts = null;

            if (request.OrOperandOperation)
            {
                receipnts = await _carRentalRepository.GetAllAsync(x =>
                    (request.CarID != null && x.CarID == request.CarID) ||
                    (request.AgentId != null && x.AgentId == request.AgentId) ||
                    (request.LocationRentTypes != null && x.LocationRentTypes == request.LocationRentTypes));
            }
            else
            {
                Expression<Func<RentDetails, bool>> combinedCondition = null;
                if (request.CarID != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<RentDetails>(combinedCondition, x => x.CarID == request.CarID);
                }

                if (request.AgentId != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<RentDetails>(combinedCondition, x => x.AgentId == request.AgentId);
                }

                if (request.LocationRentTypes != null)
                {
                    combinedCondition = ExpressionHelper.CreateCondition<RentDetails>(combinedCondition, x => x.LocationRentTypes == request.LocationRentTypes);
                }

                Expression<Func<int, bool>> expr = i => i < 5;
                Func<RentDetails, bool> deleg2 = combinedCondition.Compile();

                receipnts = await _carRentalRepository.GetAllAsync(deleg2);
            }

            return CreateValidResponse(receipnts);

            GetRentalQueryResponse CreateValidResponse(IEnumerable<RentDetails> instances)
            {
                var response = new GetRentalQueryResponse();
                response.RentalReceipts = _mapper.Map<IEnumerable<CarRentalItem>>(instances);
                response.IsOperationPassed = true;
                response.Request = request;
                return response;
            }
        }
    }

}
