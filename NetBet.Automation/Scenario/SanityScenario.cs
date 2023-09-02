using FluentAssertions;
using NetBet.Automation.Requests;
using NetBet.DataAccess.Models;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;
using System.Runtime.InteropServices;
using Xunit;

namespace NetBet.Automation.Scenario
{
    internal class SanityScenario : ScenarioBase
    {
        private List<StaticRequest> _carStaticRequests;
        private List<CarRentalRequestMock> _rentCarRequestsHistory;


        public SanityScenario(List<StaticRequest> carRequests, string baseUrl)
            : base(baseUrl)
        {
            RentCarUrl = $"{baseUrl}/rent/rentcar";
            GetRentalCarsUrl = $"{baseUrl}/rent/GetRentalCars";


            



            RemoveCarUrl = $"{baseUrl}/car/RemoveCar";
            AddCarUrl = $"{baseUrl}/car/AddCar";

            GetCarUrl = $"{baseUrl}/car/GetCars";

            this._carStaticRequests = carRequests;


            _rentCarRequestsHistory = new List<CarRentalRequestMock>();
            var request = RentCarRequesFactory.Create(_carStaticRequests.First().AddCarResponse.CarId);
            var mock = new CarRentalRequestMock
            {
                RentCarRequest = request
            };
            _rentCarRequestsHistory.Add(mock);
        }

        public string GetRentalCarsUrl { get; }

        public string GetCarUrl { get; set; }
        public string AddCarUrl { get; }
        public string RemoveCarUrl { get; }
        public string RentCarUrl { get; }

        public override string ScenarioName => "Testing";

        public override string Description => "Testing all kind scenario.s";

        protected override async Task RunScenario()
        {
            await PopulateData();
            await Failed_Renting_Overlapping_Tests();
            await Query_Or_And_Operand_Car();
            await Add_Remoev_Car();

            await Query_Or_And_Operand_Rental();
        }

        private async Task Query_Or_And_Operand_Rental()
        {
            var agentId = 122;
            var carLocationRentTypes = LocationRentTypes.East;

            var request = new CarRentalRequest
            {
                AgentId = agentId,
                CarID =1,
                LocationRentTypes = carLocationRentTypes,
                From = DateOnly.Parse("5/5/1998"),
                To = DateOnly.Parse("10/10/1998"),
            };

            var carResponse = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, request);
            Assert.True(carResponse.IsOperationPassed);

            var queryCarRequest = new GetRentalQueryRequest
            {
                OrOperandOperation = true,
                AgentId = agentId,
                LocationRentTypes = carLocationRentTypes,
            };

            var carQueryResponse = await RunPutCommand<GetRentalQueryRequest, GetRentalQueryResponse>(GetRentalCarsUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            carQueryResponse.IsOperationPassed.Should().BeTrue();
            Assert.Equal(1, carQueryResponse.RentalReceipts.SafeCount());


            queryCarRequest = new GetRentalQueryRequest
            {
                OrOperandOperation = true,
                AgentId = agentId +200,
                LocationRentTypes = carLocationRentTypes,
            };

            carQueryResponse = await RunPutCommand<GetRentalQueryRequest, GetRentalQueryResponse>(GetRentalCarsUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(1, carQueryResponse.RentalReceipts.SafeCount());


            queryCarRequest = new GetRentalQueryRequest
            {
                OrOperandOperation = false,
                AgentId = agentId + 200,
                LocationRentTypes = carLocationRentTypes,
            };

            carQueryResponse = await RunPutCommand<GetRentalQueryRequest, GetRentalQueryResponse>(GetRentalCarsUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(0, carQueryResponse.RentalReceipts.SafeCount());

        }
        private async Task Query_Or_And_Operand_Car()
        {
            var modelName = "nathan";
            var plates = "ab-ab-ab";

            var request = new AddCarRequest
            {
                NameModel = modelName,
                CarDrivingPlates = plates,
                CarGroupType = CarGroupType.B,
                DrivingAgeGroup = DrivingAgeGroup.Senior
            };

            var carResponse = await RunPostCommand<AddCarRequest, AddCarResponse>(AddCarUrl, request);
            Assert.True(carResponse.IsOperationPassed);

            var queryCarRequest = new GetCarQueryRequest
            {
                OrOperandOperation = true,
                NameModel = modelName,
                CarDrivingPlates = plates,
            };

            var carQueryResponse = await RunPutCommand<GetCarQueryRequest, GetCarQueryResponse>(GetCarUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(1, carQueryResponse.Cars.SafeCount());


             queryCarRequest = new GetCarQueryRequest
            {
                OrOperandOperation = false,
                NameModel = modelName,
                CarDrivingPlates = plates+"abs",
            };

            carQueryResponse = await RunPutCommand<GetCarQueryRequest, GetCarQueryResponse>(GetCarUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(0, carQueryResponse.Cars.SafeCount());

        }

        private async Task Add_Remoev_Car()
        {
            var modelName = "test";
            var plates = "ab-ab-ab";

            var request = new AddCarRequest
            {
                NameModel = modelName,
                CarDrivingPlates = plates,
                CarGroupType = CarGroupType.B,
                DrivingAgeGroup = DrivingAgeGroup.Senior
            };

            var carResponse = await RunPostCommand<AddCarRequest, AddCarResponse>(AddCarUrl, request);
            Assert.True(carResponse.IsOperationPassed);

            var queryCarRequest = new GetCarQueryRequest
            {
                OrOperandOperation = false,
                IsActive = false,
                NameModel = modelName,
            };

            var carQueryResponse = await RunPutCommand<GetCarQueryRequest, GetCarQueryResponse>(GetCarUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(0, carQueryResponse.Cars.SafeCount());

            var deActivateCar = new RemoveCarRequest
            {
                CarId = carResponse.CarId
            };

            var removeCarResponse = await RunPutCommand<RemoveCarRequest, RemoveCarResponse>(RemoveCarUrl, deActivateCar);
            Assert.True(removeCarResponse.IsOperationPassed);


            carQueryResponse = await RunPutCommand<GetCarQueryRequest, GetCarQueryResponse>(GetCarUrl, queryCarRequest);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(1, carQueryResponse.Cars.SafeCount());

        }

        private async Task Failed_Renting_Overlapping_Tests()
        {
            var overlapRequest = RentCarRequesFactory.Create(_rentCarRequestsHistory.First().RentCarResponse.CarId);
            var from = overlapRequest.From.AddDays(1);
            overlapRequest.From = from;

            var response = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, overlapRequest);
            Assert.False(response.IsOperationPassed);
            _rentCarRequestsHistory.Add(new CarRentalRequestMock { RentCarRequest = overlapRequest, RentCarResponse = response });



            overlapRequest = RentCarRequesFactory.Create(_rentCarRequestsHistory.First().RentCarResponse.CarId);
            from = overlapRequest.From.AddDays(1);
            overlapRequest.From = from;

            response = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, overlapRequest);
            Assert.False(response.IsOperationPassed);
            _rentCarRequestsHistory.Add(new CarRentalRequestMock { RentCarRequest = overlapRequest, RentCarResponse = response });




            var deltaMonth = 6;
            overlapRequest = RentCarRequesFactory.Create(_rentCarRequestsHistory.First().RentCarResponse.CarId);
            from = overlapRequest.From.AddMonths(deltaMonth);
            var to = overlapRequest.To.AddMonths(deltaMonth);

            overlapRequest.From = from;
            overlapRequest.To = to;

            response = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, overlapRequest);
            Assert.True(response.IsOperationPassed);
            _rentCarRequestsHistory.Add(new CarRentalRequestMock { RentCarRequest = overlapRequest, RentCarResponse = response });

        }

        private async Task PopulateData()
        {
            foreach (var requestMock in _rentCarRequestsHistory)
            {
                var response = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, requestMock.RentCarRequest);
                requestMock.RentCarResponse = response;
                Assert.True(response.IsOperationPassed);
                requestMock.RentCarResponse = response;
            }
        }
    }
}
