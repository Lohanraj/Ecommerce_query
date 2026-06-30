import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Home() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  return (
    <section className="hero">
      <p className="eyebrow">Student Learning Portal</p>
      <h1>Welcome to Student Learning Portal</h1>
      <p className="hero__lead">
        Learn React, Web API, and Full Stack Development from one place.
      </p>
      <div className="button-row">
        <button className="btn btn--primary" onClick={() => navigate("/courses")}>
          View Courses
        </button>
        <button
          className="btn btn--ghost"
          onClick={() => navigate(isAuthenticated ? "/dashboard" : "/login")}
        >
          Go to Dashboard
        </button>
      </div>
    </section>
  );
}
