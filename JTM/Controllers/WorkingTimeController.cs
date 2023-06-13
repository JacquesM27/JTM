using FluentValidation;
using JTM.CQRS.Command.WorkingTime;
using JTM.CQRS.Query.WorkingTime;
using JTM.CQRS.Query.WorkingTime.GetWorkingTimes;
using JTM.DTO.WorkingTime;
using JTM.DTO.WorkingTime.AddWorkingTime;
using JTM.DTO.WorkingTime.UpdateWorkingTime;
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
        private readonly IValidator<AddWorkingTimeDto> _addValidator;
        private readonly IValidator<UpdateWorkingTimeDto> _updateValidator;

        public WorkingTimeController(
            IMediator mediator,
            IValidator<AddWorkingTimeDto> validator,
            IValidator<UpdateWorkingTimeDto> updateValidator)
        {
            _mediator = mediator;
            _addValidator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet("{workingTimeId}")]
        public async Task<ActionResult<DetailsWorkingTimeDto>> GetWorkingTime([FromRoute]int workingTimeId)
        {
            var query = new GetWorkingTimeQuery(workingTimeId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasicWorkingTimeDto>>> GetWorkingTimes(WorkingTimeQueryDto request)
        {
            var query = new GetWorkingTimesQuery(
                request.EmployeesId,
                request.StartDate,
                request.EndDate,
                request.CompaniesId,
                request.NoteSearchPhrase,
                request.PageNumber,
                request.PageSize);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddWorkingTime(AddWorkingTimeDto request)
        {
            await _addValidator.ValidateAndThrowAsync(request);
            //string? user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(user);

            var command = new AddWorkingTimeCommand(
                request.WorkingDate,
                request.Seconds,
                request.Note,
                request.CompanyId,
                request.EmployeeId,
                userId);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPut("{workingTimeId}")]
        public async Task<ActionResult> UpdateWorkingTime([FromRoute] int workingTimeId, UpdateWorkingTimeDto request)
        {
            await _updateValidator.ValidateAndThrowAsync(request, default);

            string? user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(user);

            var command = new UpdateWorkingTimeCommand(
                workingTimeId,
                request.Id,
                request.WorkingDate,
                request.Seconds,
                request.Note,
                request.CompanyId,
                request.EmployeeId,
                userId);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete("{workingTimeId}")]
        public async Task<ActionResult> DeleteWorkingTime([FromRoute]int workingTimeId) 
        {
            string? user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(user);

            var command = new DeleteTimeCommand(workingTimeId, userId);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
