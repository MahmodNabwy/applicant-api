using Application.Common.Interfaces.ApiServices;
using Application.Common.Interfaces.Persistence;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Applicants.Commands.CreateApplicant;

public class ApplicantDto : IMapFrom<Applicant>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CountryOfOrigin { get; set; } = string.Empty;
    public string EmailAdress { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool Hired { get; set; }
}

public class CreateApplicantCommand : IRequest<ApplicantDto>
{
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CountryOfOrigin { get; set; } = string.Empty;
    public string EmailAdress { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool? Hired { get; set; }
}

public class CreateApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
{
    public CreateApplicantCommandValidator(ICountryValidationApi countryValidationApi)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5);
        RuleFor(x => x.FamilyName)
            .NotEmpty()
            .MinimumLength(5);
        RuleFor(x => x.Address)
            .NotEmpty()
            .MinimumLength(10);
        RuleFor(x => x.EmailAdress)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Age)
            .InclusiveBetween(20, 60);
        RuleFor(x => x.Hired)
            .NotNull();
        RuleFor(x => x.CountryOfOrigin)
            .NotEmpty()
            .MustAsync(async (country, ct) => await countryValidationApi.IsValidCountryByFullName(country, ct))
            .WithMessage("CountryOfOrigin must be a valid country");
    }
}

public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, ApplicantDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateApplicantCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApplicantDto> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = new Applicant
        {
            Name = request.Name,
            FamilyName = request.FamilyName,
            Address = request.Address,
            CountryOfOrigin = request.CountryOfOrigin,
            EmailAdress = request.EmailAdress,
            Age = request.Age,
            Hired = request.Hired ?? false
        };
        _dbContext.Applicants.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ApplicantDto>(entity);
    }
} 