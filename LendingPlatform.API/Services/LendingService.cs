using LendingPlatform.API.Models;

namespace LendingPlatform.API.Services;

public class LendingService : ILendingService
{
  private readonly List<ApplicationRecord> _applications = new();

  public ApplicationResponse EvaluateApplication(ApplicationRequest request)
  {
    if (request.AssetValue <= 0)
    {
      return Decline("Asset value must be greater than zero.", 0);
    }

    decimal ltv = (request.LoanAmount / request.AssetValue) * 100;

    // General limits[cite: 1]
    if (request.LoanAmount < 100000 || request.LoanAmount > 1500000)
    {
      return Decline("Loan amount must be between £100,000 and £1,500,000.", ltv);
    }

    bool isSuccessful = false;
    string reason;

    // Loans >= £1M[cite: 1]
    if (request.LoanAmount >= 1000000)
    {
      if (ltv <= 60 && request.CreditScore >= 950)
      {
        isSuccessful = true;
        reason = "Approved under >=£1M threshold criteria.";
      }
      else
      {
        reason = "Declined: Loans >= £1M require LTV <= 60% and Credit Score >= 950.";
      }
    }
    // Loans < £1M[cite: 1]
    else
    {
      if (ltv >= 90)
      {
        reason = "Declined: LTV is 90% or higher.";
      }
      else if (ltv < 60 && request.CreditScore >= 750)
      {
        isSuccessful = true;
        reason = "Approved (LTV < 60%).";
      }
      else if (ltv < 80 && request.CreditScore >= 800)
      {
        isSuccessful = true;
        reason = "Approved (LTV < 80%).";
      }
      else if (ltv < 90 && request.CreditScore >= 900)
      {
        isSuccessful = true;
        reason = "Approved (LTV < 90%).";
      }
      else
      {
        reason = "Declined: Credit score too low for the required LTV tier.";
      }
    }

    var record = new ApplicationRecord
    {
      LoanAmount = request.LoanAmount,
      Ltv = ltv,
      IsSuccessful = isSuccessful
    };

    lock (_applications)
    {
      _applications.Add(record);
    }

    return new ApplicationResponse
    {
      IsSuccessful = isSuccessful,
      DecisionReason = reason,
      Ltv = Math.Round(ltv, 2)
    };
  }

  public MetricsResponse GetMetrics()
  {
    lock (_applications)
    {
      int total = _applications.Count;
      int successful = _applications.Count(a => a.IsSuccessful);
      int declined = total - successful;
      decimal totalValue = _applications.Where(a => a.IsSuccessful).Sum(a => a.LoanAmount);
      decimal meanLtv = total == 0 ? 0 : _applications.Average(a => a.Ltv);

      return new MetricsResponse
      {
        TotalApplicants = total,
        SuccessfulApplicants = successful,
        DeclinedApplicants = declined,
        TotalValueWritten = totalValue,
        MeanLtv = Math.Round(meanLtv, 2)
      };
    }
  }

  private ApplicationResponse Decline(string reason, decimal ltv)
  {
    var record = new ApplicationRecord
    {
      LoanAmount = 0,
      Ltv = ltv,
      IsSuccessful = false
    };

    lock (_applications)
    {
      _applications.Add(record);
    }

    return new ApplicationResponse
    {
      IsSuccessful = false,
      DecisionReason = reason,
      Ltv = Math.Round(ltv, 2)
    };
  }
}