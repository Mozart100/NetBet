using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBet.Models.Dtos;
using NetBet.Services.Validations;

namespace NetBet.Controllers
{
    public class NetBetControllerBase: ControllerBase
    {
        protected async Task<TResponse> ErrorWrapper<TRequest, TResponse>(Func<Task<TResponse>> callback) where TResponse : NetBetReponseBase<TRequest>, new()
                                                                                                          where TRequest:class
        {
            TResponse response = null;
            try
            {
                response = await callback();
            }
            catch (NetBetException netBetException)
            {
                response = new TResponse();
                response.ErrorSection = new ErrorSection
                {
                    Message = "There was a problem please resolve it",
                    Errors = netBetException.NetBetErrors

                };
                response.IsOperationPassed = false;
            }
            catch (Exception ex)
            {
                response = new TResponse();
                response.ErrorSection = new ErrorSection { Message = ex.Message };
                response.IsOperationPassed = false;
            }


            return response;
        }

    }
}
