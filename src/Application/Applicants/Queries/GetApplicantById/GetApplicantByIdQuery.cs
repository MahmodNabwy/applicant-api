using Application.Applicants.Commands.CreateApplicant;
using Application.Common.Interfaces.Persistence;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Applicants.Queries.GetApplicantById;

public class GetApplicantByIdQuery : IRequest<ApplicantDto>
{
    public int Id { get; set; }
}

public class GetApplicantByIdQueryHandler : IRequestHandler<GetApplicantByIdQuery, ApplicantDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetApplicantByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApplicantDto> Handle(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException();
        }
        return _mapper.Map<ApplicantDto>(entity);
    }
} 