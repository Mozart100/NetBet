using NetBet.DataAccess.Models;

namespace NetBet.Models.Dtos
{
    public class CarRentalRequest
    {
        public int CarID { get; set; }
        public int AgentId { get; set; }
        public LocationRentTypes LocationRentTypes { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }

    public class CarRentalResponse : NetBetReponseBase<CarRentalRequest>
    {
        public int RentId { get; set; }
        public int CarId { get; set; }
    }

}
