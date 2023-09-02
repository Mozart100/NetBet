using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NetBet.Services.Validations;
using System.Net;

namespace NetBet.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {

        //public ErrorHandlingFilterAttribute(IOptions<ApiBehaviorOptions> options)
        //{
        //    var api = options.Value;
        //}


        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            ObjectResult objectResult = null;
            if (context.Exception is NetBetException netBetException)
            {
                var exception = context.Exception;
                var problemDetails = new ProblemDetails
                {
                    Title = "An error occurred while processing.",
                    Status = (int)HttpStatusCode.InternalServerError,
                };

                problemDetails.Extensions.Add("IsOperationPassed", false);
                foreach (var error in netBetException.NetBetErrors)
                {
                    problemDetails.Extensions.Add($"Validation issue: {error.PropertyName}", error.ErrorMessage);
                }
                objectResult = new ObjectResult(problemDetails);
            }
            else
            {
                var exception = context.Exception;
                var problemDetails = new ProblemDetails
                {
                    Title = "An error occurred while processing.",
                    Status = (int)HttpStatusCode.InternalServerError,
                };

                problemDetails.Extensions.Add("IsOperationPassed", false);
                problemDetails.Extensions.Add("Exception", exception.Message);
                objectResult = new ObjectResult(problemDetails);
            }

            context.Result = objectResult;
            context.ExceptionHandled = true;
        }




        public async Task OnExceptionAsync_Origin(ExceptionContext context)
        {
            //await base.OnExceptionAsync(context);

            var exception = context.Exception;
            context.Result = new ObjectResult(new { Error = "An error occurred while processing." })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }

    }
}
