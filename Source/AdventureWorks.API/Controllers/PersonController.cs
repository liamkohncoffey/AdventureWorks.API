using System.Threading.Tasks;
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
        /// This Get method returns a persons name
        /// </summary>
        [HttpGet("Name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonsNameByBusinessId(int businessId)
        {
            var response = await _adventureWorksService.GetPersonsNameAsync(businessId);
            if (response == null)
                return NotFound($"Couldn't find the person for the given businessId {businessId}");
            return Ok(response);
        }

        /// <summary>
        /// This Patch method Updates a persons Name
        /// </summary>
        [HttpPatch("Name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdatePersonBybussinessId(int businessId, [FromBody] PersonNameBaseDto personName)
        {
            var response = await _adventureWorksService.PatchPersonsNameAsync(businessId, personName);
            if (response == null)
                return NotFound($"Couldn't find the person for the given businessId {businessId}");
            return Ok(response);
        }
            
        /// <summary>
        /// This Gets a persons basic information
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPersonByBusinessEntity(int businessId)
        {
            var response = await _adventureWorksService.GetPersonAsync(businessId);
            if (response == null)
                return NotFound($"Couldn't find the person for the given businessId {businessId}");
            return Ok(response);
        }
    }
}
