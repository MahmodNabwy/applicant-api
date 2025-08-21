using System.Net.Http.Json;
using Application.Common.Interfaces.ApiServices;

namespace Infrastructure.ApiServices;

public class CountryValidationApi : ICountryValidationApi
{
    private readonly HttpClient _httpClient;

    public CountryValidationApi(HttpClient httpClient)
    {
        // New API base URL
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://restcountries.com");
    }

    public async Task<bool> IsValidCountryByFullName(string countryName, CancellationToken cancellationToken)
    {
        try
        {
            // Updated endpoint (v3.1 instead of v2)
            var response = await _httpClient.GetAsync($"/v3.1/name/{Uri.EscapeDataString(countryName)}?fullText=true", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return !string.IsNullOrWhiteSpace(content);
        }
        catch (Exception ex)
        {

            return false;
        }
    }
}
