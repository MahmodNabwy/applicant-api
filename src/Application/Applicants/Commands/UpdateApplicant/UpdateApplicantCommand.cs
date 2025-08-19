using Application.Common.Interfaces.ApiServices;
using Application.Common.Interfaces.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Applicants.Commands.UpdateApplicant;

public class UpdateApplicantCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CountryOfOrigin { get; set; } = string.Empty;
    public string EmailAdress { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool? Hired { get; set; }
}

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    public UpdateApplicantCommandValidator(ICountryValidationApi countryValidationApi)
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

public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateApplicantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            throw new Application.Common.Exceptions.NotFoundException();
        }

        entity.Name = request.Name;
        entity.FamilyName = request.FamilyName;
        entity.Address = request.Address;
        entity.CountryOfOrigin = request.CountryOfOrigin;
        entity.EmailAdress = request.EmailAdress;
        entity.Age = request.Age;
        entity.Hired = request.Hired ?? false;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
} 