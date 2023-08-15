using NetBet.DataAccess.Models;

namespace NetBet.Models.Dtos
{
    public class CarItem
    {
        public int CarId { get; set; }
        public string CarDrivingPlates { get; set; }
        public string NameModel { get; set; }
        public CarGroupType CarGroupType { get; set; }
        public DrivingAgeGroup DrivingAgeGroup { get; set; }
        public bool IsActive { get; set; }

    }

    public class GetQueryRequestBase
    {
        //Is OperandOperation true than  all the search properties ||
        //Otherwise all properties && - Should be combine into AND
        public bool OrOperandOperation { get; set; } = true;
    }

    public class GetCarQueryRequest : GetQueryRequestBase
    {
        public int? CarId { get; set; }
        public string? CarDrivingPlates { get; set; }
        public string? NameModel { get; set; }
        public CarGroupType? CarGroupType { get; set; }
        public DrivingAgeGroup? DrivingAgeGroup { get; set; }
        public bool? IsActive { get; set; }
    }


    public class GetCarQueryResponse : NetBetReponseBase<GetCarQueryRequest>
    {
        public IEnumerable<CarItem> Cars { get; set; }
    }




}
