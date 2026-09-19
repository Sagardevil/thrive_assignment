# AI Usage & Prompt Log

## Overview

AI assistance (ChatGPT / Gemini) was utilized throughout the development lifecycle to scaffold project structures, formulate business logic, and troubleshoot environment configuration issues.

## Key Prompts & Iterations

1. **Initial Setup & Architecture:**
   - _Prompt:_ "Generate ASP.NET Core Web API models and services for a lending decision engine based on Loan Amount, Asset Value, and Credit Score rules."
   - _Iteration:_ Configured the service layer as a Singleton to retain in-memory state across requests.
2. **Business Rule Implementation:**
   - _Prompt:_ "Implement LTV and credit score decision logic for loans >= £1M and loans < £1M based on tiered thresholds."
   - _Correction:_ Ensured strict boundary handling for edge cases (e.g., LTV of exactly 60% and 90%).
3. **Troubleshooting:**
   - _Prompt:_ "Fix runtime TypeLoadException for OpenApi assembly in .NET Web API."
   - _Correction:_ Resolved dependency version mismatches between Swashbuckle and Microsoft.OpenApi packages.
