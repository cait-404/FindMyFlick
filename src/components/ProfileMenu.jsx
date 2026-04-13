import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import API_URL from "../config.js";

export default function ProfileMenu() {
  const [open, setOpen] = useState(false);
  const [user, setUser] = useState(null); // NEW
  const navigate = useNavigate();

  const API = API_URL;

  // ETCH USER DATA
  useEffect(() => {
    const fetchUser = async () => {
      const token = localStorage.getItem("token");

      if (!token) return;

      try {
        const res = await fetch(`${API}/api/Profile`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });

        if (!res.ok) throw new Error("Failed to fetch user");

        const data = await res.json();
        setUser(data);
      } catch (err) {
        console.error(err);
      }
    };

    fetchUser();
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
    window.location.reload();
  };

  return (
    <div className="relative">
      <div
        onClick={() => setOpen(!open)}
        className="w-11 h-11 sm:w-10 sm:h-10 rounded-full bg-linear-to-br from-pink-500 to-purple-700 
           flex items-center justify-center text-lg font-bold cursor-pointer active:scale-95 transition"
      >
        {/* ✅ FIXED HERE */}
        {user?.username ? user.username[0].toUpperCase() : "U"}
      </div>

      {open && (
        <div className="absolute right-0 mt-2 w-44 bg-gray-900 rounded-lg shadow-lg overflow-hidden z-50">
          <button
            onClick={() => navigate("/profile")}
            className="block w-full text-left px-4 py-3 hover:bg-gray-800 active:bg-gray-700 transition"
          >
            Profile
          </button>

          <button
            onClick={handleLogout}
            className="block w-full text-left px-4 py-3 hover:bg-gray-800 active:bg-gray-700 transition text-pink-400"
          >
            Logout
          </button>
        </div>
      )}
    </div>
  );
}