Prompt 1: Project Architecture & File Structure
Prompt:
I am making a lending platform in .NET C#. I want to use conventional .NET folder and file naming so that any engineer can immediately understand the layout. I want my project to be clean and industry-ready. What should the folder and file structure look like?
My project is a lending platform with these requirements:
•	Inputs: Loan amount (GBP), Asset value, Applicant credit score (1–999).
•	Outputs: Loan decision (Successful/Declined), Total number of applicants (grouped by status), Total value of loans written to date, and Mean average LTV across all applications.
Response:
Here is a standard, clean ASP.NET Core Web API directory structure following clean architecture principles and idiomatic C# naming conventions:
Plaintext
LendingPlatform/
├── LendingPlatform.API/
│   ├── Controllers/
│   │   └── LendingController.cs
│   ├── Models/
│   │   ├── ApplicationRequest.cs
│   │   ├── ApplicationResponse.cs
│   │   ├── ApplicationRecord.cs
│   │   └── MetricsResponse.cs
│   ├── Services/
│   │   ├── ILendingService.cs
│   │   └── LendingService.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── LendingPlatform.API.csproj
├── LendingPlatform.Tests/
│   └── LendingServiceTests.cs
└── LendingPlatform.sln
Key Design Choices:
1.	Controllers/: Hosts HTTP routing endpoints (POST /api/lending/apply, GET /api/lending/metrics).
2.	Models/: Separates incoming payloads (ApplicationRequest), outgoing results (ApplicationResponse), and internal aggregated states (ApplicationRecord, MetricsResponse).
3.	Services/: Encapsulates decision logic, LTV calculations, and thread-safe in-memory metrics collection behind the ILendingService interface.
Prompt 2: Implementing Business Rules Engine
Prompt:
I need to implement the business rules for evaluating loan applications in C# based on Loan Amount, Asset Value, and Credit Score.
Here are the rules:
•	Loan amount must be between £100,000 and £1,500,000.
•	Calculate LTV = (Loan Amount / Asset Value) * 100.
•	If Loan Amount >= £1,000,000: Approve ONLY if LTV <= 60% AND Credit Score >= 950.
•	If Loan Amount < £1,000,000:
o	LTV >= 90% -> Decline.
o	LTV < 60% -> Approve if Credit Score >= 750.
o	LTV < 80% -> Approve if Credit Score >= 800.
o	LTV < 90% -> Approve if Credit Score >= 900.
Can you show me a clean implementation of this evaluation method in C#?
Response:
Here is a clean C# implementation using guard clauses and structured conditional evaluation:
C#
public ApplicationResponse EvaluateApplication(ApplicationRequest request)
{
    if (request.AssetValue <= 0)
        return Decline("Asset value must be greater than zero.", 0);

    decimal ltv = (request.LoanAmount / request.AssetValue) * 100;
if (request.LoanAmount < 100000 || request.LoanAmount > 1500000)
    return Decline("Loan amount must be between £100,000 and £1,500,000.", ltv);

bool isSuccessful = false;
string reason;

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

return new ApplicationResponse
{
    IsSuccessful = isSuccessful,
    DecisionReason = reason,
    Ltv = Math.Round(ltv, 2)
};
}
Prompt 3: Frontend Interface & CORS Integration
Prompt:
How do I build a React frontend with JavaScript that submits loan applications to my ASP.NET Core API at http://localhost:5270/api/lending/apply and displays both the evaluation outcome and live system metrics? Also, how do I resolve CORS issues between React (port 3000) and .NET Web API (port 5270)?
Response:
1. React Frontend Implementation (src/App.js): Use standard fetch() calls to send JSON payloads and trigger a metrics re-fetch whenever an application is submitted.
JavaScript
const handleSubmit = async (e) => {
  e.preventDefault();
  const payload = {
    loanAmount: parseFloat(formData.loanAmount),
    assetValue: parseFloat(formData.assetValue),
    creditScore: parseInt(formData.creditScore, 10),
  };

  const res = await fetch('http://localhost:5270/api/lending/apply', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  const data = await res.json();
  setDecision(data);
  fetchMetrics(); // Refresh live dashboard numbers
};
2. Enabling CORS in ASP.NET Core (Program.cs): Register a CORS policy allowing origins from http://localhost:3000:
C#
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowReactApp");

