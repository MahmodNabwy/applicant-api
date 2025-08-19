using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Applicants.Queries.ListApplicants;

public class ListApplicantsQuery : IRequest<List<ApplicantListItemDto>>
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public class ApplicantListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string EmailAdress { get; set; } = string.Empty;
    public bool Hired { get; set; }
}

public class ListApplicantsQueryHandler : IRequestHandler<ListApplicantsQuery, List<ApplicantListItemDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListApplicantsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApplicantListItemDto>> Handle(ListApplicantsQuery request, CancellationToken cancellationToken)
    {
        var ordered = _dbContext.Applicants.OrderByDescending(x => x.Id).AsQueryable();
        if (request.Page.HasValue && request.PageSize.HasValue && request.Page > 0 && request.PageSize > 0)
        {
            ordered = ordered.Skip((request.Page.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
        }
        return ordered.Select(x => new ApplicantListItemDto
        {
            Id = x.Id,
            Name = x.Name,
            FamilyName = x.FamilyName,
            EmailAdress = x.EmailAdress,
            Hired = x.Hired
        }).ToList();
    }
} 