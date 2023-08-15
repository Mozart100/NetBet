namespace NetBet.Models.Dtos
{
    public class RemoveCarRequest
    {
        public int CarId { get; set; }
    }


    public class RemoveCarResponse : NetBetReponseBase<RemoveCarRequest>
    {

    }
}
