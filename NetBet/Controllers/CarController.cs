using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBet.Models.Dtos;
using NetBet.Services;
using NetBet.Services.Validations;

namespace NetBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : NetBetControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarService _carService;

        public CarController(ILogger<CarController> logger,
            ICarService carService)
        {
            this._logger = logger;
            this._carService = carService;
        }

        //For automation
        [HttpPost]
        [Route("AddCar")]
        public async Task<AddCarResponse> AddCar([FromBody] AddCarRequest request)
        {
            return await ErrorWrapper<AddCarRequest, AddCarResponse>(async () => await _carService.StoreCarAsync(request));
        }


        [HttpGet]
        [Route("GetAllCars")]
        public async Task<GetCarQueryResponse> GetAllCars()
        {
            return await ErrorWrapper<GetCarQueryRequest, GetCarQueryResponse>(async () => await _carService.GetCarsAsync(null));
        }

        //The preferred option for data retrieval is using the query string. However, in my previous companies,
        //we used the HTTP PUT request for this purpose due to security considerations.
        [HttpPut]
        [Route("GetCars")]
        public async Task<GetCarQueryResponse> GetCarQueryAsync(GetCarQueryRequest request)
        {
            return await ErrorWrapper<GetCarQueryRequest, GetCarQueryResponse>(async () => await _carService.GetCarsAsync(request));
        }

        //Serves as delete
        [HttpPut]
        [Route("removecar")]
        public async Task<RemoveCarResponse> RemoveCarAsync(RemoveCarRequest request)
   {
            return await ErrorWrapper<RemoveCarRequest, RemoveCarResponse>(async () => await _carService.RemoveCarAsync(request));
        }

    }
}
