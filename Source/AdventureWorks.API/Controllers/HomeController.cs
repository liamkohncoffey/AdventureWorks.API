using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ApiBaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult RedirectToSwaggerUiPage()
        {
            return Redirect("/swagger");
        }
    }
}
