import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

function linkClass({ isActive }) {
  return isActive ? "nav-link nav-link--active" : "nav-link";
}

export default function Navbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    const confirmed = window.confirm("Log out of the Student Learning Portal?");
    if (!confirmed) return;
    logout();
    navigate("/login");
  }

  return (
    <header className="navbar">
      <div className="navbar__brand">
        <span className="navbar__mark">SLP</span>
        <span>Student Learning Portal</span>
      </div>

      <nav className="navbar__links">
        <NavLink to="/" className={linkClass} end>
          Home
        </NavLink>
        <NavLink to="/about" className={linkClass}>
          About
        </NavLink>
        <NavLink to="/courses" className={linkClass}>
          Courses
        </NavLink>
        <NavLink to="/contact" className={linkClass}>
          Contact
        </NavLink>

        {isAuthenticated ? (
          <>
            <NavLink to="/dashboard" className={linkClass}>
              Dashboard
            </NavLink>
            <span className="navbar__user">Hi, {user?.name}</span>
            <button className="nav-link nav-link--button" onClick={handleLogout}>
              Logout
            </button>
          </>
        ) : (
          <NavLink to="/login" className={linkClass}>
            Login
          </NavLink>
        )}
      </nav>
    </header>
  );
}
