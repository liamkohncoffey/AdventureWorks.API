using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json", "application/problem+json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public abstract class ApiBaseController : ControllerBase
    {
        protected NotFoundObjectResult NotFound(IComparable resourceId)
        {
            return new NotFoundObjectResult(new ProblemDetails
            {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Resource with identifier '{resourceId}' not found",
                Instance = HttpContext.Request.GetEncodedPathAndQuery(),
                Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404"
            });
        }

        protected ConflictObjectResult Conflict(string errorMessage)
        {
            return new ConflictObjectResult(new ProblemDetails
            {
                Title = "Conflict",
                Status = StatusCodes.Status409Conflict,
                Detail = errorMessage,
                Instance = HttpContext.Request.GetEncodedPathAndQuery(),
                Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409"
            });
        }
    }
}
