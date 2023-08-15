namespace NetBet.DataAccess.Models
{
    public enum LocationRentTypes
    {
        North,
        South,
        East,
        West,
        Yehuda
    }

    public class RentDetails : EntityDbBase
    {
        public int CarID { get; set; }
        public int AgentId { get; set; }
        public LocationRentTypes LocationRentTypes { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }
}
