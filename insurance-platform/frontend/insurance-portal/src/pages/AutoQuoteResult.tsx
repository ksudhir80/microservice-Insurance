import { useLocation, useNavigate } from "react-router-dom";

export default function AutoQuoteResult() {
  const navigate = useNavigate();
  const location = useLocation();

  const quote =
    location.state ||
    JSON.parse(
      localStorage.getItem("autoQuoteResult") || "null"
    );

  if (!quote) {
    return (
      <div style={{ padding: "40px", textAlign: "center" }}>
        <h2>No quote found</h2>

        <button onClick={() => navigate("/auto-quote")}>
          Create New Quote
        </button>
      </div>
    );
  }

  return (
    <div
      style={{
        maxWidth: "800px",
        margin: "40px auto",
        padding: "30px",
      }}
    >
      <button onClick={() => navigate("/auto-quote")}>
        ← Back to Quote
      </button>

      <div
        style={{
          border: "1px solid #ddd",
          borderRadius: "15px",
          padding: "35px",
          marginTop: "30px",
        }}
      >
        <h1>🎉 Auto Quote Generated</h1>

        <hr />

        <h2>Quote Details</h2>

        <p>
          <strong>Quote ID:</strong>{" "}
          {quote.quoteId}
        </p>

        <p>
          <strong>Customer ID:</strong>{" "}
          {quote.customerId}
        </p>

        <p>
          <strong>Status:</strong>{" "}
          {quote.status}
        </p>

        <hr />

        <h2>Premium Calculation</h2>

        <p>
          Base Premium:{" "}
          <strong>
            ${Number(quote.basePremium).toFixed(2)}
          </strong>
        </p>

        <p>
          Risk Adjustment:{" "}
          <strong>
            ${Number(quote.riskAdjustment).toFixed(2)}
          </strong>
        </p>

        <p>
          Coverage Adjustment:{" "}
          <strong>
            ${Number(quote.coverageAdjustment).toFixed(2)}
          </strong>
        </p>

        <div
          style={{
            background: "#f5f5f5",
            padding: "20px",
            borderRadius: "10px",
            marginTop: "20px",
          }}
        >
          <h2>Annual Premium</h2>

          <h1>
            ${Number(quote.premium).toFixed(2)}
          </h1>

          <h3>
            Monthly Premium: $
            {Number(quote.monthlyPremium).toFixed(2)}
          </h3>
        </div>

        <button
          onClick={() => navigate("/auto-quote")}
          style={{
            marginTop: "25px",
            padding: "12px 30px",
          }}
        >
          Create Another Quote
        </button>
      </div>
    </div>
  );
}