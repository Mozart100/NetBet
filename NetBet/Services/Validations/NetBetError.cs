using NetBet.DataAccess.Models;
using NetBet.Models.Dtos;

namespace NetBet.Services.Validations
{
    public class NetBetError
    {
        public NetBetError(string errorMessage, string propertyName)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
        public string PropertyName { get; }

    }


    public class NetBetException : Exception
    {
        public NetBetException(params NetBetError[] netBetError)
        {
            NetBetErrors = netBetError;
        }

        public NetBetError[] NetBetErrors { get; }
    }
}