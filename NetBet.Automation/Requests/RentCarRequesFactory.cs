using NetBet.DataAccess.Models;
using NetBet.Models.Dtos;

namespace NetBet.Automation.Requests
{
    public static class RentCarRequesFactory
    {
        public static CarRentalRequest Create(int carId)
        {
            return new CarRentalRequest
            {
                CarID = carId,
                AgentId = 1,
                From = DateOnly.Parse("1/1/2001"),
                To = DateOnly.Parse("1/5/2001"),
                LocationRentTypes = LocationRentTypes.Yehuda,
            };
        }
    }
}