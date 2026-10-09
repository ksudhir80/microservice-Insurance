import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiGet } from "../services/apiClient";

interface Quote {
  id: number;
  quoteId: string;
  customerId: number;
  vehicleMake: string;
  vehicleModel: string;
  vehicleYear: number;
  premium: number;
  monthlyPremium: number;
  status: string;
  createdAt: string;
}

export default function QuoteHistory() {
  const navigate = useNavigate();

  const [quotes, setQuotes] = useState<Quote[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

useEffect(() => {
  const loadQuotes = async () => {
    try {
      const data = await apiGet<Quote[]>(
        "/api/quotes"
      );

      setQuotes(data);
    } catch (err) {
      console.error(err);

      setError(
        "Unable to load quotes. Please try again."
      );
    } finally {
      setLoading(false);
    }
  };

  loadQuotes();
}, []);

  if (loading) {
    return (
      <div style={{ padding: "40px" }}>
        <h2>Loading quotes...</h2>
      </div>
    );
  }

  return (
    <div
      style={{
        maxWidth: "1100px",
        margin: "40px auto",
        padding: "20px",
      }}
    >
      <button
        onClick={() => navigate("/")}
        style={{
          padding: "10px 20px",
          marginBottom: "25px",
        }}
      >
        ← Dashboard
      </button>

      <h1>Quote History</h1>

      <p>
        Previously generated insurance quotes
      </p>

      {error && (
        <div
          style={{
            padding: "15px",
            background: "#ffe5e5",
            color: "red",
            marginTop: "20px",
          }}
        >
          {error}
        </div>
      )}

      {!error && quotes.length === 0 && (
        <div style={{ marginTop: "30px" }}>
          <h3>No quotes found.</h3>

          <button
            onClick={() => navigate("/auto-quote")}
          >
            Create First Quote
          </button>
        </div>
      )}

      {quotes.length > 0 && (
        <table
          style={{
            width: "100%",
            borderCollapse: "collapse",
            marginTop: "30px",
          }}
        >
          <thead>
            <tr>
              <th>Quote ID</th>
              <th>Vehicle</th>
              <th>Year</th>
              <th>Premium</th>
              <th>Monthly</th>
              <th>Status</th>
              <th>Created</th>
              <th></th>
            </tr>
          </thead>

          <tbody>
            {quotes.map((quote) => (
              <tr key={quote.id}>
                <td>{quote.quoteId}</td>

                <td>
                  {quote.vehicleMake}{" "}
                  {quote.vehicleModel}
                </td>

                <td>{quote.vehicleYear}</td>

                <td>
                  ${Number(quote.premium).toFixed(2)}
                </td>

                <td>
                  $
                  {Number(
                    quote.monthlyPremium
                  ).toFixed(2)}
                </td>

                <td>{quote.status}</td>

                <td>
                  {new Date(
                    quote.createdAt
                  ).toLocaleDateString()}
                </td>

                <td>
                  <button
                    onClick={() =>
                      navigate(
                        `/auto-quote/result/${quote.quoteId}`
                      )
                    }
                  >
                    View
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      <div style={{ marginTop: "30px" }}>
        <button
          onClick={() => navigate("/auto-quote")}
          style={{
            padding: "12px 25px",
          }}
        >
          + Create New Quote
        </button>
      </div>
    </div>
  );
}