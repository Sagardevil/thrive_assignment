namespace LendingPlatform.API.Models;

public class ApplicationRecord
{
  public decimal LoanAmount { get; set; }
  public decimal Ltv { get; set; }
  public bool IsSuccessful { get; set; }
}