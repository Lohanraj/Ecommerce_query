import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const redirectTo = location.state?.from?.pathname || "/dashboard";

  function handleSubmit(event) {
    event.preventDefault();
    const result = login(username, password);
    if (!result.ok) {
      setError(result.error);
      return;
    }
    setError("");
    navigate(redirectTo, { replace: true });
  }

  return (
    <section className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <p className="eyebrow">Sign in</p>
        <h1>Login</h1>

        <label className="field">
          <span>Username</span>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="student"
            autoComplete="username"
          />
        </label>

        <label className="field">
          <span>Password</span>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="student123"
            autoComplete="current-password"
          />
        </label>

        {error && <p className="error-text">{error}</p>}

        <button type="submit" className="btn btn--primary btn--block">
          Login
        </button>

        <p className="hint-text">
          Try the demo account &mdash; username <code>student</code>, password{" "}
          <code>student123</code>.
        </p>
      </form>
    </section>
  );
}
