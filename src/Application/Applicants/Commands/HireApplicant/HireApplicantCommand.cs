using Application.Common.Interfaces.Persistence;
using FluentValidation;
using MediatR;
using FluentValidation.Results;

namespace Application.Applicants.Commands.HireApplicant;

public class HireApplicantCommand : IRequest
{
	public int Id { get; set; }
}

public class HireApplicantCommandValidator : AbstractValidator<HireApplicantCommand>
{
	public HireApplicantCommandValidator(IApplicationDbContext dbContext)
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.MustAsync(async (id, ct) =>
			{
				var entity = await dbContext.Applicants.FindAsync(new object[] { id }, ct);
				if (entity == null)
				{
					// Let the handler produce 404 for not found
					return true;
				}
				return !entity.Hired;
			})
			.WithMessage("This applicant is already hired");
	}
}

public class HireApplicantCommandHandler : IRequestHandler<HireApplicantCommand>
{
	private readonly IApplicationDbContext _dbContext;

	public HireApplicantCommandHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(HireApplicantCommand request, CancellationToken cancellationToken)
	{
		var entity = await _dbContext.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);
		if (entity == null)
		{
			throw new Application.Common.Exceptions.NotFoundException();
		}

		if (entity.Hired)
		{
			throw new Application.Common.Exceptions.ValidationException(new[]
			{
				new ValidationFailure("Hired", "This applicant is already hired")
			});
		}

		entity.Hired = true;
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}


