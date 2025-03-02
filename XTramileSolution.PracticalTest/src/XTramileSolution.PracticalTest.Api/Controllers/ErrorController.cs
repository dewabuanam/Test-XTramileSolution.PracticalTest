using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace XTramileSolution.PracticalTest.Api.Controllers
{
    [ApiController]
    [Route("error")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (context == null)
            {
                return Problem(
                    title: "An unexpected error occurred",
                    statusCode: (int)HttpStatusCode.InternalServerError
                );
            }
 
            var traceId = HttpContext.TraceIdentifier; 

            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail =  "An unexpected error occurred",
                Instance = HttpContext.Request.Path,
                Extensions = { { "traceId", traceId } } 
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }
    }
}