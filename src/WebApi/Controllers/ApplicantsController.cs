

using Application.Applicants.Commands.CreateApplicant;
using Application.Applicants.Commands.DeleteApplicant;
using Application.Applicants.Commands.HireApplicant;
using Application.Applicants.Commands.UpdateApplicant;
using Application.Applicants.Queries.GetApplicantById;
using Application.Applicants.Queries.ListApplicants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicantsController : ApiControllerBase
{
    private readonly ILogger<ApplicantsController> _logger;

    public ApplicantsController(ILogger<ApplicantsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApplicantDataGetterDto>> List([FromQuery] ListApplicantsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicantDto>> GetById([FromRoute] int id)
    {
        return await Mediator.Send(new GetApplicantByIdQuery { Id = id });
    }

    [HttpPost]
    public async Task<ActionResult<ApplicantDto>> Create([FromBody] CreateApplicantCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateApplicantCommand command)
    {
        command.Id = id;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteApplicantCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/hire")]
    public async Task<ActionResult> Hire([FromRoute] int id)
    {
        await Mediator.Send(new HireApplicantCommand { Id = id });
        return NoContent();
    }
} 
