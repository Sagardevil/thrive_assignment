# Blackfinch Lending Platform Simulation

An automated loan evaluation engine and analytics platform built with a .NET Core Web API backend and a React JavaScript frontend.

---

## 🏗️ Architecture & Stack

- **Backend:** C# ASP.NET Core Web API (.NET 8)
- **Frontend:** React.js (JavaScript, CSS3)
- **Data Storage:** In-Memory collection (`List<ApplicationRecord>`) with thread-safe locking

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v16+ recommended)

---

### Step 1: Run the Backend API

1. Open your terminal and navigate to the API directory:
   ```bash
   cd LendingPlatform.API
   Restore dependencies and start the API server:Bashdotnet restore
   dotnet run
   The backend API will start listening on http://localhost:5270.Step 2: Run the React FrontendOpen a new terminal window and navigate to the UI directory:Bashcd lending-platform-ui
   Install npm packages:Bashnpm install
   Start the React development server:Bashnpm start
   The application will open automatically in your browser at http://localhost:3000.⚡ API EndpointsMethodEndpointDescriptionPOST/api/lending/applyEvaluates a loan application based on Loan Amount, Asset Value, and Credit Score.GET/api/lending/metricsReturns global platform analytics (total applicants, approval rates, total value written, mean LTV).🏛️ Business Rules & CriteriaThe evaluation engine applies the following automated decision logic:General ConstraintsMin Loan Amount: £100,000Max Loan Amount: £1,500,000Threshold CriteriaLoans $\ge$ £1,000,000:Approved only if $\text{LTV} \le 60\%$ AND $\text{Credit Score} \ge 950$.Loans < £1,000,000:$\text{LTV} \ge 90\% \rightarrow \text{Declined}$$\text{LTV} < 60\% \rightarrow \text{Credit Score} \ge 750$$\text{LTV} < 80\% \rightarrow \text{Credit Score} \ge 800$$\text{LTV} < 90\% \rightarrow \text{Credit Score} \ge 900$💡 Architectural Decisions & Production ConsiderationsIn-Memory State & Thread Safety: An in-memory singleton list is used for rapid evaluation and simplicity. Mutex locking (lock (_applications)) is implemented to maintain thread safety across concurrent API requests.Production Persistence: For production deployment, the in-memory collection can be swapped with Entity Framework Core mapped to a relational database (PostgreSQL or SQL Server).
   ```
