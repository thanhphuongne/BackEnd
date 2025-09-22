namespace BackEnd.Application.DTOs.Owner;

public class PaymentSettingsDto
{
    public decimal Commission { get; set; }
    public List<string> AcceptedMethods { get; set; } = new();
    public BankDetailsDto? BankDetails { get; set; }
}

public class BankDetailsDto
{
    public string AccountName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string BankName { get; set; } = null!;
}