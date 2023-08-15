using NetBet.Services.Validations;

namespace NetBet.Models.Dtos
{
    public class ErrorSection
    {
        public string Message { get; set; }
        public IEnumerable<NetBetError> Errors { get; set; }
    }

    public class NetBetReponseBase<TRequest> where TRequest : class
    {
        public TRequest Request { get; set; }
        public bool IsOperationPassed { get; set; } = true;
        public ErrorSection ErrorSection { get; set; }
    }

}
