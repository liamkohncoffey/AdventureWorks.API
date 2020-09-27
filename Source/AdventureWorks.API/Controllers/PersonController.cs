using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Api.Extensions;
using AdventureWorks.Service;
using AdventureWorks.Service.Dtos.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Api.Controllers
{
    [Authorize]
    [Route("Person")]
    public class PersonController : ApiBaseController
    {
        private readonly IAdventureWorksService _adventureWorksService;
        public PersonController(IAdventureWorksService adventureWorksService)
        {
            _adventureWorksService = adventureWorksService;
        }

        /// <summary>
        /// GET returns a persons name
        /// </summary>
        [HttpGet("Name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonsNameByBusinessId(Guid rowGuid, CancellationToken cancellation)
        {
            var response = await _adventureWorksService.GetPersonsNameAsync(rowGuid, cancellation);
            if (response == null)
                return NotFound($"Couldn't find the person for the given rowGuid {rowGuid}");
            return Ok(response.Map());
        }

        /// <summary>
        /// PATCH Updates a persons Name
        /// </summary>
        [HttpPatch("Name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdatePersonBybussinessId(Guid rowGuid, [FromBody] PersonNameBaseDto personName, CancellationToken cancellation)
        {
            var response = await _adventureWorksService.PatchPersonsNameAsync(rowGuid, personName, cancellation);
            if (response == null)
                return NotFound($"Couldn't find the person for the given rowGuid {rowGuid}");
            return Ok(response.Map());
        }
            
        /// <summary>
        /// GET returns persons basic information
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonByBusinessEntity(Guid rowGuid, CancellationToken cancellation)
        {
            var response = await _adventureWorksService.GetPersonAsync(rowGuid, cancellation);
            if (response == null)
                return NotFound($"Couldn't find the person for the given rowGuid {rowGuid}");
            return Ok(response.Map());
        }
    }
}
