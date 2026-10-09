import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiGet } from "../services/apiClient";
import {
  getUsername,
  getUserRole,
  logout,
} from "../services/authService";
interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  city: string;
  state: string;
}

function Dashboard() {
  const [customer, setCustomer] = useState<Customer | null>(null);
  const [loading, setLoading] = useState(true);
  const username = getUsername();
const role = getUserRole();
const navigate = useNavigate();
 useEffect(() => {
  const loadCustomer = async () => {
    try {
      const data = await apiGet<Customer>(
        "/api/customers/1"
      );

      setCustomer(data);
    } catch (error) {
      console.error(error);
      navigate("/login");
    } finally {
      setLoading(false);
    }
  };

  loadCustomer();
}, [navigate]);

  return (
    <div style={{ padding: "40px", fontFamily: "Arial" }}>
        <p>
  Logged in as: <strong>{username}</strong> (
  <strong>{role}</strong>)
</p>
      <h1>Insurance Portal</h1>
<button
  onClick={() => {
    logout();
    navigate("/login");
  }}
  style={{
    padding: "10px 20px",
    cursor: "pointer",
    float: "right",
  }}
>
  Logout
</button>
      <p>Auto & Home Insurance</p>

      <hr />

      <h2>Customer Dashboard</h2>

      {loading && <p>Loading customer...</p>}

      {!loading && customer && (
        <div>
          <h3>
            Welcome, {customer.firstName} {customer.lastName}
          </h3>

          <p>Email: {customer.email}</p>
          <p>Phone: {customer.phone}</p>
          <p>
            Location: {customer.city}, {customer.state}
          </p>
        </div>
      )}

      {!loading && !customer && (
        <p>Unable to load customer information.</p>
      )}

      <hr />

      <h2>Insurance Products</h2>
       <button
  style={{
    width: "180px",
    height: "50px",
    marginRight: "15px",
    cursor: "pointer",
  }}
  onClick={() => navigate("/quotes")}
>
  Quote History
</button>

<button
  style={{
    width: "180px",
    height: "50px",
    marginRight: "15px",
    cursor: "pointer",
  }}
  onClick={() => navigate("/auto-quote")}
>
  🚗 Auto Insurance
</button>

<button
  style={{
    width: "180px",
    height: "50px",
    cursor: "pointer",
  }}
>
  🏠 Home Insurance
</button>
    </div>
  );
}

export default Dashboard;