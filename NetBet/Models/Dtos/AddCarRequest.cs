using NetBet.DataAccess.Models;

namespace NetBet.Models.Dtos
{
    public class AddCarRequest
    {
        public string CarDrivingPlates { get; set; }
        public CarGroupType CarGroupType { get; set; }
        public DrivingAgeGroup DrivingAgeGroup { get; set; }
        public string NameModel { get; set; }
    }

  

    public class AddCarResponse : NetBetReponseBase<AddCarRequest>
    {
        public int CarId { get; set; }
    }
}
