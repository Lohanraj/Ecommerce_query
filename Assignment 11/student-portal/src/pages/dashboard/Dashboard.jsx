import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

function linkClass({ isActive }) {
  return isActive ? "side-link side-link--active" : "side-link";
}

export default function Dashboard() {
  const { user } = useAuth();

  return (
    <section className="dashboard">
      <p className="eyebrow">Dashboard</p>
      <h1>Welcome to Student Dashboard{user ? `, ${user.name.split(" ")[0]}` : ""}</h1>

      <div className="dashboard__layout">
        <nav className="dashboard__sidebar">
          <NavLink to="/dashboard/profile" className={linkClass}>
            Profile
          </NavLink>
          <NavLink to="/dashboard/my-courses" className={linkClass}>
            My Courses
          </NavLink>
          <NavLink to="/dashboard/settings" className={linkClass}>
            Settings
          </NavLink>
        </nav>

        <div className="dashboard__content">
          <Outlet />
        </div>
      </div>
    </section>
  );
}
