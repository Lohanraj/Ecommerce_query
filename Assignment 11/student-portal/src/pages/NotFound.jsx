import { useNavigate } from "react-router-dom";

export default function NotFound() {
  const navigate = useNavigate();

  return (
    <section className="page page--center">
      <h1>404 - Page Not Found</h1>
      <p>The page you are looking for does not exist.</p>
      <button className="btn btn--primary" onClick={() => navigate("/")}>
        Go to Home
      </button>
    </section>
  );
}
