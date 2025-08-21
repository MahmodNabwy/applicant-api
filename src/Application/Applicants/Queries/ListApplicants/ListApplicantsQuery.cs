using Application.Common.Interfaces.Persistence;
using Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Applicants.Queries.ListApplicants;

public class ListApplicantsQuery : IRequest<ApplicantDataGetterDto>
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
public class ApplicantDataGetterDto
{
    public Pager Page { get; set; }
    public List<ApplicantListItemDto> Data { get; set; }
}
public class ApplicantListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string EmailAdress { get; set; } = string.Empty;
    public bool Hired { get; set; }
    public int Age { get; set; }
}

public class ListApplicantsQueryHandler : IRequestHandler<ListApplicantsQuery, ApplicantDataGetterDto>
{
    private readonly IApplicationDbContext _dbContext;

    public ListApplicantsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicantDataGetterDto> Handle(ListApplicantsQuery request, CancellationToken cancellationToken)
    {
        var ordered = _dbContext.Applicants.OrderByDescending(x => x.Id).AsQueryable();
        int totalCount = await ordered.CountAsync();
        var page = new Pager();
        page.Set(request.PageSize, request.Page, totalCount);
        ordered = ordered.AddPage(page.Skip, page.PageSize);

        var data = await ordered.Select(x => new ApplicantListItemDto
        {
            Id = x.Id,
            Name = x.Name,
            FamilyName = x.FamilyName,
            Address = x.Address,
            EmailAdress = x.EmailAdress,
            Age = x.Age,
            Country = x.CountryOfOrigin,
            Hired = x.Hired,
        }).ToListAsync();
        return new ApplicantDataGetterDto
        {
            Data = data,
            Page = page
        };
    }
}
