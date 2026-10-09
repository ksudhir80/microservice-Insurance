import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/authService";

function Login() {
  const navigate = useNavigate();

  const [username, setUsername] = useState("admin");
  const [password, setPassword] = useState("Password123!");
  const [error, setError] = useState("");

  const handleLogin = async () => {
    try {
      setError("");

      await login(username, password);

      navigate("/");
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Login failed."
      );
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "100px auto" }}>
      <h2>Insurance Portal Login</h2>

      <div style={{ marginBottom: 15 }}>
        <label>Username</label>

        <input
          type="text"
          value={username}
          onChange={(e) =>
            setUsername(e.target.value)
          }
          style={{
            width: "100%",
            padding: 10,
          }}
        />
      </div>

      <div style={{ marginBottom: 15 }}>
        <label>Password</label>

        <input
          type="password"
          value={password}
          onChange={(e) =>
            setPassword(e.target.value)
          }
          style={{
            width: "100%",
            padding: 10,
          }}
        />
      </div>

      {error && (
        <div style={{ color: "red", marginBottom: 15 }}>
          {error}
        </div>
      )}

      <button onClick={handleLogin}>
        Login
      </button>
    </div>
  );
}

export default Login;