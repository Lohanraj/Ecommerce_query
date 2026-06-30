import { createContext, useContext, useState } from "react";

const AuthContext = createContext(null);

const VALID_USERNAME = "student";
const VALID_PASSWORD = "student123";

function readStoredUser() {
  const raw = localStorage.getItem("slp_user");
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}

export function AuthProvider({ children }) {
  const [user, setUser] = useState(readStoredUser);

  function login(username, password) {
    if (!username.trim()) return { ok: false, error: "Username is required." };
    if (!password.trim()) return { ok: false, error: "Password is required." };
    if (username !== VALID_USERNAME || password !== VALID_PASSWORD) {
      return { ok: false, error: "Invalid username or password." };
    }
    const loggedInUser = { name: "Student User", username };
    localStorage.setItem("slp_user", JSON.stringify(loggedInUser));
    setUser(loggedInUser);
    return { ok: true };
  }

  function logout() {
    localStorage.removeItem("slp_user");
    setUser(null);
  }

  const value = { user, isAuthenticated: Boolean(user), login, logout };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
  return ctx;
}
