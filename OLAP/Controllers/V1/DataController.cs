using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OLAP.API.Application.Authentication;
using OLAP.API.Application.Commands.Data;
using OLAP.API.Models.Response.Data;

namespace OLAP.API.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    public class DataController : Controller
    {
        private readonly IMediator _mediator;

        public DataController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Add Data
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult<List<DataReadModel>>> AddData([FromBody] DataAddCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == false)
                return BadRequest("Data have been added.");

            return Ok(result);
        }
    }
}
