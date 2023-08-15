using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBet.Models.Dtos;
using NetBet.Services;

namespace NetBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : NetBetControllerBase
    {
        private readonly ILogger<RentController> _logger;
        private readonly ICarRentalService _rentalService;

        public RentController(ILogger<RentController> logger,
            ICarRentalService rentalService)
        {
            this._logger = logger;
            this._rentalService = rentalService;
        }


        [HttpPost]
        [Route("RentCar")]
        public async Task<CarRentalResponse> RentCarAsync([FromBody] CarRentalRequest request)
        {
            return await ErrorWrapper<CarRentalRequest, CarRentalResponse>(async () => await _rentalService.CarRentalCarAsync(request));
        }

        [HttpGet]
        [Route("GetAllRentalReceipts")]
        public async Task<GetRentalQueryResponse> GetAllRentalReceiptsAsync()
        {
            return await ErrorWrapper<GetRentalQueryRequest, GetRentalQueryResponse>(async () => await this._rentalService.GetCarRentailReceiptAsync(null));
        }

        //The preferred option for data retrieval is using the query string. However, in my previous companies,
        //we used the HTTP PUT request for this purpose due to security considerations.
        [HttpPut]
        [Route("GetRentalCars")]
        public async Task<GetRentalQueryResponse> GetCarQueryAsync(GetRentalQueryRequest request)
        {
            return await ErrorWrapper<GetRentalQueryRequest, GetRentalQueryResponse>(async () => await this._rentalService.GetCarRentailReceiptAsync(request));
        }
    }
}
