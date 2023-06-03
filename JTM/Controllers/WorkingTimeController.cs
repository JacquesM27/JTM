using JTM.CQRS.Query.WorkingTime;
using JTM.DTO.WorkingTime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JTM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingTimeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkingTimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{workingTimeId}")]
        public async Task<ActionResult<DetailsWorkingTimeDto>> GetWorkingTime([FromRoute]int workingTimeId)
        {
            var query = new GetWorkingTimeQuery(workingTimeId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<BasicWorkingTimeDto>>> GetWorkingTimes(WorkingTimeQueryDto request)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<ActionResult> AddWorkingTime(WorkingTimeDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<ActionResult> UpdateWorkingTime(WorkingTimeDto request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task<ActionResult> DeleteWorkingTime(int workingTimeId) 
        {
            throw new NotImplementedException();
        }
    }
}
