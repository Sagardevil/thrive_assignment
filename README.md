# Lending Platform Simulation

An automated loan evaluation engine and analytics platform built with a **.NET Core Web API backend** and a **React JavaScript frontend**.

---

## Architecture & Stack

* **Backend:** C# ASP.NET Core Web API (.NET 8)
* **Frontend:** React.js (JavaScript, CSS3)
* **Data Storage:** In-memory collection (`List<ApplicationRecord>`) with thread-safe locking

---

## Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js](https://nodejs.org/) (v16+ recommended)

---

### Step 1: Run the Backend API

Open a terminal and navigate to the API directory:

```bash
cd LendingPlatform.API
```

Restore the dependencies:

```bash
dotnet restore
```

Start the API server:

```bash
dotnet run
```

The backend API will start listening on:

```text
http://localhost:5270
```

---

### Step 2: Run the React Frontend

Open a **new terminal window** and navigate to the UI directory:

```bash
cd lending-platform-ui
```

Install the required npm packages:

```bash
npm install
```

Start the React development server:

```bash
npm start
```

The application will open automatically in your browser at:

```text
http://localhost:3000
```

---

## API Endpoints

| Method | Endpoint               | Description                                                                                                     |
| ------ | ---------------------- | --------------------------------------------------------------------------------------------------------------- |
| `POST` | `/api/lending/apply`   | Evaluates a loan application based on Loan Amount, Asset Value, and Credit Score.                               |
| `GET`  | `/api/lending/metrics` | Returns global platform analytics including total applicants, approval rate, total value written, and mean LTV. |

---

## Business Rules & Criteria

The evaluation engine applies the following automated decision logic.

### General Constraints

| Rule                |      Value |
| ------------------- | ---------: |
| Minimum Loan Amount |   £100,000 |
| Maximum Loan Amount | £1,500,000 |

### Threshold Criteria

#### Loans ≥ £1,000,000

Applications are approved only when:

* **LTV ≤ 60%**
* **Credit Score ≥ 950**

#### Loans < £1,000,000

The following LTV and credit-score rules are applied:

| LTV Range | Minimum Credit Score |
| --------- | -------------------: |
| LTV < 60% |                  750 |
| LTV < 80% |                  800 |
| LTV < 90% |                  900 |
| LTV ≥ 90% |             Declined |

---

## Architectural Decisions & Production Considerations

### In-Memory State & Thread Safety

The current implementation uses an in-memory singleton collection for rapid evaluation and simplicity.

A `lock` mechanism is used around access to the application collection to maintain thread safety when multiple API requests are processed concurrently.

### Production Persistence

For a production deployment, the in-memory collection could be replaced with **Entity Framework Core** and a relational database such as:

* PostgreSQL
* SQL Server

This would provide persistent storage, better scalability, transaction support, and reliable data recovery across application restarts.

### Additional Production Considerations

For a production-ready implementation, the following improvements could also be considered:

* Persistent database storage using Entity Framework Core
* Dependency injection with appropriate service lifetimes
* Centralized exception handling and structured logging
* Authentication and authorization
* Input validation and API versioning
* Automated unit and integration testing
* Database migrations
* Monitoring and health checks
* Containerization using Docker
* CI/CD pipeline for automated builds and deployments
* Configuration through environment variables and secure secrets management

---

## Testing

The backend business logic can be tested using unit tests covering scenarios such as:

* Valid and invalid loan amounts
* Different LTV ranges
* Credit score threshold boundaries
* High-value loan applications
* Applications exactly at LTV boundaries
* Concurrent application requests
* Metrics calculation

---

## Project Structure

```text
Blackfinch Lending Platform
│
├── LendingPlatform.API
│   ├── Controllers
│   ├── Models
│   ├── Services
│   └── Program.cs
│
├── lending-platform-ui
│   ├── src
│   ├── public
│   └── package.json
│
└── README.md
```

---

## Notes

This project is a simulation of a lending decision platform. The current implementation prioritizes simplicity and demonstrates the core loan evaluation rules, API design, thread-safe in-memory state management, and frontend integration.

The production considerations above describe how the system could be extended for a real-world deployment.
