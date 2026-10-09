import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import AutoQuote from "./pages/AutoQuote";
import AutoQuoteResult from "./pages/AutoQuoteResult";
import QuoteHistory from "./pages/QuoteHistory";
import ProtectedRoute from "./components/ProtectedRoute";
function App() {
  return (
    <BrowserRouter>
<Routes>
  <Route path="/login" element={<Login />} />

  <Route
    path="/"
    element={
      <ProtectedRoute>
        <Dashboard />
      </ProtectedRoute>
    }
  />

  <Route
    path="/auto-quote"
    element={
      <ProtectedRoute>
        <AutoQuote />
      </ProtectedRoute>
    }
  />

  <Route
    path="/auto-quote/result"
    element={
      <ProtectedRoute>
        <AutoQuoteResult />
      </ProtectedRoute>
    }
  />

  <Route
    path="/quotes"
    element={
      <ProtectedRoute>
        <QuoteHistory />
      </ProtectedRoute>
    }
  />
</Routes>
    </BrowserRouter>
  );
}

export default App;