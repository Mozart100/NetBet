using NetBet.DataAccess.Models;

namespace NetBet.Models.Dtos
{
    public class CarRentalItem
    {
        public int RentalReceiptId { get; set; }

        public int CarID { get; set; }
        public int AgentId { get; set; }
        public LocationRentTypes LocationRentTypes { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class GetRentalQueryRequest : GetQueryRequestBase
    {
        public int ? RentalReceiptId { get; set; }
        public int? CarID { get; set; }
        public int? AgentId { get; set; }
        public LocationRentTypes? LocationRentTypes { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
    }

    public class GetRentalQueryResponse : NetBetReponseBase<GetRentalQueryRequest>
    {
        public IEnumerable<CarRentalItem> RentalReceipts { get; set; }
    }


}
