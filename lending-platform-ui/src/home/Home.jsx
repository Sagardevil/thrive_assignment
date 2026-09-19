import React from "react";
import { useState, useEffect } from "react";

function Home() {
  const [formData, setFormData] = useState({
    loanAmount: "",
    assetValue: "",
    creditScore: "",
  });

  const [decision, setDecision] = useState(null);
  const [metrics, setMetrics] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const API_BASE_URL = "http://localhost:5270/api/lending";

  // Fetch metrics on initial load
  useEffect(() => {
    fetchMetrics();
  }, []);

  const fetchMetrics = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/metrics`);
      if (!response.ok) throw new Error("Failed to fetch metrics");
      const data = await response.json();
      setMetrics(data);
    } catch (err) {
      console.error("Error fetching metrics:", err);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setDecision(null);
    setLoading(true);

    const payload = {
      loanAmount: parseFloat(formData.loanAmount),
      assetValue: parseFloat(formData.assetValue),
      creditScore: parseInt(formData.creditScore, 10),
    };

    try {
      const response = await fetch(`${API_BASE_URL}/apply`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) throw new Error("Failed to evaluate application");

      const data = await response.json();
      setDecision(data);

      // Refresh global metrics after submission
      fetchMetrics();
    } catch (err) {
      setError(
        "Server communication error. Make sure backend is running on port 5270.",
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <header className="header">
        <h1>Blackfinch Lending Platform</h1>
        <p>Automated Loan Evaluation System</p>
      </header>

      <div className="grid">
        {/* Application Form */}
        <section className="card">
          <h2>Loan Application</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="loanAmount">Loan Amount (£)</label>
              <input
                type="number"
                id="loanAmount"
                name="loanAmount"
                placeholder="e.g. 250000"
                value={formData.loanAmount}
                onChange={handleChange}
                required
                min="1"
              />
            </div>

            <div className="form-group">
              <label htmlFor="assetValue">Asset Value (£)</label>
              <input
                type="number"
                id="assetValue"
                name="assetValue"
                placeholder="e.g. 400000"
                value={formData.assetValue}
                onChange={handleChange}
                required
                min="1"
              />
            </div>

            <div className="form-group">
              <label htmlFor="creditScore">Credit Score (1–999)</label>
              <input
                type="number"
                id="creditScore"
                name="creditScore"
                placeholder="e.g. 820"
                value={formData.creditScore}
                onChange={handleChange}
                required
                min="1"
                max="999"
              />
            </div>

            <button type="submit" disabled={loading} className="btn-submit">
              {loading ? "Evaluating..." : "Submit Application"}
            </button>
          </form>

          {error && <div className="alert alert-error">{error}</div>}

          {/* Decision Outcome */}
          {decision && (
            <div
              className={`decision-box ${decision.isSuccessful ? "success" : "declined"}`}
            >
              <h3>
                Outcome: {decision.isSuccessful ? "SUCCESSFUL" : "DECLINED"}
              </h3>
              <p>
                <strong>Reason:</strong> {decision.decisionReason}
              </p>
              <p>
                <strong>Calculated LTV:</strong> {decision.ltv}%
              </p>
            </div>
          )}
        </section>

        {/* System Metrics */}
        <section className="card">
          <h2>System Statistics</h2>
          {metrics ? (
            <div className="metrics-list">
              <div className="metric-item">
                <span>Total Applications:</span>
                <strong>{metrics.totalApplicants}</strong>
              </div>
              <div className="metric-item">
                <span>Successful Applications:</span>
                <strong className="text-success">
                  {metrics.successfulApplicants}
                </strong>
              </div>
              <div className="metric-item">
                <span>Declined Applications:</span>
                <strong className="text-danger">
                  {metrics.declinedApplicants}
                </strong>
              </div>
              <div className="metric-item">
                <span>Total Value Written:</span>
                <strong>£{metrics.totalValueWritten.toLocaleString()}</strong>
              </div>
              <div className="metric-item">
                <span>Mean LTV Across All Apps:</span>
                <strong>{metrics.meanLtv}%</strong>
              </div>
            </div>
          ) : (
            <p>Loading metrics...</p>
          )}
        </section>
      </div>
    </div>
  );
}

export default Home;
