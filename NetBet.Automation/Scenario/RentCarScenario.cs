using NetBet.Automation.Requests;
using NetBet.DataAccess.Models;
using NetBet.Infrastracture;
using NetBet.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace NetBet.Automation.Scenario
{
    internal class RentCarScenario : ScenarioBase
    {

        private List<StaticRequest> _carRequests;
        private List<CarRentalRequestMock> _rentCarRequests;

        public RentCarScenario(List<StaticRequest> carRequests, string baseUrl)
            :base(baseUrl)
        {
            RentCarUrl = $"{baseUrl}/rent/rentcar";
            GetAllRentalReceiptsUrl = $"{baseUrl}/rent/GetAllRentalReceipts";


            this._carRequests = carRequests;

            _rentCarRequests = new List<CarRentalRequestMock>
            {
                new CarRentalRequestMock
                {
                    RentCarRequest = new CarRentalRequest
                    {
                        CarID=  _carRequests.First().AddCarResponse.CarId,
                        AgentId = 1,
                        From = DateOnly.Parse("1/1/2000"),
                        To = DateOnly.Parse("1/2/2000"),
                        LocationRentTypes = LocationRentTypes.Yehuda,
                    }
                }
            };
        }
        public string GetAllRentalReceiptsUrl { get;  }

        public string RentCarUrl { get; }

        public override string ScenarioName => "Renting car";

        public override string Description => "Renting cars by checking availiable dates";

        protected override async Task RunScenario()
        {
            await PopulateData();
            await QueryCarRenting_Tests();

        }

        private async Task QueryCarRenting_Tests()
        {
            var carQueryResponse = await Get<GetRentalQueryResponse>(GetAllRentalReceiptsUrl);
            Assert.Equal(_rentCarRequests.SafeCount(), carQueryResponse.RentalReceipts.Count());
        }

        private async Task PopulateData()
        {
            foreach (var requestMock in _rentCarRequests)
            {
                var response = await RunPostCommand<CarRentalRequest, CarRentalResponse>(RentCarUrl, requestMock.RentCarRequest);
                requestMock.RentCarResponse = response;
            }
        }
    }
}
