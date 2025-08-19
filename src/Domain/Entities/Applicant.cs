namespace Domain.Entities;

public class Applicant
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