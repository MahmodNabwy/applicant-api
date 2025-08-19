namespace Application.Common.Interfaces.ApiServices;

public interface ICountryValidationApi
{
    Task<bool> IsValidCountryByFullName(string countryName, CancellationToken cancellationToken);
} 