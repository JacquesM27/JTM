using FluentValidation;
using JTM.CQRS.Command.Company.AddCompany;
using JTM.CQRS.Command.Company.DeleteCompany;
using JTM.CQRS.Command.Company.UpdateCompany;
using JTM.CQRS.Query.Company;
using JTM.DTO.Company;
using JTM.DTO.Company.AddCompany;
using JTM.DTO.Company.UpdateCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AddCompanyDto> _addValidator;
        private readonly IValidator<UpdateCompanyDto> _updateValidator;

        public CompanyController(
            IMediator mediator,
            IValidator<AddCompanyDto> validator,
            IValidator<UpdateCompanyDto> updateValidator)
        {
            _mediator = mediator;
            _addValidator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var command = new GetCompaniesQuery();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<CompanyDto>> GetCompany([FromRoute] int companyId)
        {
            var command = new GetCompanyQuery(companyId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddCompany(AddCompanyDto request)
        {
            await _addValidator.ValidateAndThrowAsync(request);

            var command = new AddCompanyCommand(request.Name);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{companyId}")]
        public async Task<ActionResult> UpdateCompany([FromRoute] int companyId, UpdateCompanyDto request)
        {
            await _updateValidator.ValidateAndThrowAsync(request);

            var command = new UpdateCompanyCommand(request.Name, request.Id, companyId);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{companyId}")]
        public async Task<ActionResult> DeleteCompany([FromRoute] int companyId)
        {
            var command = new DeleteCompanyCommand(companyId);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
