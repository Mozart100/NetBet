namespace NetBet.DataAccess.Models
{
    public enum CarGroupType
    {
        A =1,
        B,
        C,
        D
    }


    //Ages
    public enum DrivingAgeGroup
    {
        Junior = 18,
        Midd = 25,
        Senior = 33,
        Veteran = 40
    }

    public class Car : EntityDbBase
    {
        public string CarDrivingPlates { get; set; }
        public string NameModel { get; set; }
        public CarGroupType CarGroupType { get; set; }
        public DrivingAgeGroup DrivingAgeGroup { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
