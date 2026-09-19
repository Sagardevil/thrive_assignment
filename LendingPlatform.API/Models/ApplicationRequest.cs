namespace LendingPlatform.API.Models;

public class ApplicationRequest
{
  public decimal LoanAmount { get; set; }
  public decimal AssetValue { get; set; }
  public int CreditScore { get; set; }
}