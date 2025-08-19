using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Applicants.Commands.DeleteApplicant;

public class DeleteApplicantCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteApplicantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            return; // idempotent delete
        }
        _dbContext.Applicants.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
} 