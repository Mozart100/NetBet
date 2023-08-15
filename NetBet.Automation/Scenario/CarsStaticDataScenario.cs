
using NetBet.Automation.Requests;
using NetBet.DataAccess.Models;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;
using Xunit;

namespace NetBet.Automation.Scenario
{
    /// <summary>
    /// This class populate period type slots
    /// </summary>
    public class CarsStaticDataScenario : ScenarioBase
    {

        public CarsStaticDataScenario(string baseUrl) : base(baseUrl)
        {
            AddCarUrl = $"{BaseUrl}/car/addcar";

            GetAllCarsUrl = $"{BaseUrl}/car/getallcars";

            GetCarsUrl = $"{baseUrl}/car/GetCars";

            CarRequests = new List<StaticRequest>();

            Initialize_AddCarRequest();
        }

        public List<StaticRequest> CarRequests { get; }

        public string GetCarsUrl { get; }
        public string AddCarUrl { get; }
        public string GetAllCarsUrl { get; }

        public override string ScenarioName => "Populating static data.";

        public override string Description => "Populate static data regarding trading cars...";

        protected override async Task RunScenario()
        {
            await PopulateData();
            await SearchTests();

        }

        private async Task SearchTests()
        {
            var carQueryResponse = await Get<GetCarQueryResponse>(GetAllCarsUrl);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(CarRequests.Count, carQueryResponse.Cars.SafeCount());



            carQueryResponse = await Get<GetCarQueryResponse>(GetAllCarsUrl);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(CarRequests.Count, carQueryResponse.Cars.SafeCount());


            var request = new GetCarQueryRequest
            {
                NameModel = "M"
            };

            carQueryResponse = await RunPostCommand<GetCarQueryRequest, GetCarQueryResponse>(GetCarsUrl, request, isPostRequest: false);
            Assert.True(carQueryResponse.IsOperationPassed);
            Assert.Equal(1, carQueryResponse.Cars.SafeCount());

        }

        private async Task PopulateData()
        {
            foreach (var request in CarRequests)
            {
                var response = await RunPostCommand<AddCarRequest, AddCarResponse>(AddCarUrl, request.AddCarRequest);
                request.AddCarResponse = response;
            }
        }

        private void Initialize_AddCarRequest()
        {
            var staticRequest = new StaticRequest
            {
                AddCarRequest = new AddCarRequest
                {
                    NameModel = "Subaru",
                    CarDrivingPlates = "AA-AAA_AA",
                    CarGroupType = CarGroupType.B,
                    DrivingAgeGroup = DrivingAgeGroup.Senior
                }
            };
            CarRequests.Add(staticRequest);

            staticRequest = new StaticRequest
            {
                AddCarRequest = new AddCarRequest
                {
                    NameModel = "BMW",
                    CarDrivingPlates = "bbb-bbbA_bbb",
                    CarGroupType = CarGroupType.A,
                    DrivingAgeGroup = DrivingAgeGroup.Veteran
                }
            };
            CarRequests.Add(staticRequest);
        }
    }
}
