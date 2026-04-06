import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import API_URL from "../../config.js";

export default function LoginSignup() {
  const navigate = useNavigate();

  const [isLogin, setIsLogin] = useState(true);
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
    favoriteGenres: [],
    bio: "",
    theme: "Disco"
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const API = API_URL;

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const toggleMode = () => {
    setError("");
    setSuccess("");
    setIsLogin(prev => !prev);
  };

  // ✅ Toggle genres
  const toggleGenre = (genre) => {
    setFormData((prev) => ({
      ...prev,
      favoriteGenres: prev.favoriteGenres.includes(genre)
        ? prev.favoriteGenres.filter((g) => g !== genre)
        : [...prev.favoriteGenres, genre],
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);
    setError("");
    setSuccess("");

    if (!isLogin) {
      if (!formData.username || !formData.email || !formData.password || !formData.confirmPassword) {
        setError("Please fill in all fields");
        setLoading(false);
        return;
      }

      if (formData.password !== formData.confirmPassword) {
        setError("Passwords do not match");
        setLoading(false);
        return;
      }

      if (formData.password.length < 15) {
        setError("Password must be at least 15 characters");
        setLoading(false);
        return;
      }
    }

    try {
      let response;

      if (isLogin) {
        response = await fetch(`${API}/api/Account/login`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            username: formData.username,
            password: formData.password
          })
        });
      } else {
        response = await fetch(`${API}/api/Account/register`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            username: formData.username,
            email: formData.email,
            password: formData.password,
            confirmPassword: formData.confirmPassword,
            favoriteGenres: formData.favoriteGenres,
            theme: formData.theme,
            bio: formData.bio
          })
        });
      }

      if (!response.ok) {
        const text = await response.text();
        let data = null;
        try { data = JSON.parse(text); } catch {}

        if (Array.isArray(data)) {
          throw new Error(data.map(e => e.description).join(", "));
        }

        throw new Error(
          (data?.message ?? data?.title ?? text) || "Server error"
        );
      }

      if (isLogin) {
  const data = await response.json();
  const token = data.token || data.accessToken || data;

  localStorage.setItem("token", token);

  setSuccess("Logged in successfully!");

  setTimeout(() => {
    navigate("/profile");
  }, 1000);

} else {
  // 🔥 LOGIN AFTER REGISTER
  const loginRes = await fetch(`${API}/api/Account/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username: formData.username,
      password: formData.password
    })
  });

  const loginData = await loginRes.json();
  const token = loginData.token || loginData.accessToken || loginData;

  localStorage.setItem("token", token);

  // 🔥 THIS IS WHAT YOU JUST TESTED (AUTO RUN IT)
  await fetch(`${API}/api/Profile`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify({
      favoriteGenres: formData.favoriteGenres,
      bio: formData.bio,
      theme: formData.theme
    })
  });

  setSuccess("Account created!");

  setTimeout(() => {
    navigate("/profile");
  }, 1000);
}
      setFormData({
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
        favoriteGenres: [],
        bio: "",
        theme: "Disco"
      });

    } catch (err) {
      setError(err.message || "Something went wrong.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-linear-to-br from-gray-900 via-black to-gray-800 px-4">
      <div className="w-full max-w-md p-8 rounded-xl bg-gray-900/80 shadow-xl text-white">

        <h2 className="text-3xl font-bold text-center neon-text mb-6">
          {isLogin ? "Welcome Back" : "Create an Account"}
        </h2>

        <form onSubmit={handleSubmit} className="space-y-5">

          {/* USERNAME */}
          <div>
            <label className="block text-sm text-gray-300 mb-1">
              Username
            </label>
            <input
              type="text"
              name="username"
              value={formData.username}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                         focus:ring-2 focus:ring-pink-500 outline-none"
            />
          </div>

          {/* EMAIL */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Email
              </label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                           focus:ring-2 focus:ring-pink-500 outline-none"
              />
            </div>
          )}

          {/* PASSWORD */}
          <div className="relative">
            <label className="block text-sm text-gray-300 mb-1">
              Password
            </label>
            <input
              type={showPassword ? "text" : "password"}
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100 pr-10
                         focus:ring-2 focus:ring-pink-500 outline-none"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-3 top-9 text-gray-400 hover:text-pink-400"
            >
              {showPassword ? <FaEyeSlash /> : <FaEye />}
            </button>
          </div>

          {/* CONFIRM PASSWORD */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Confirm Password
              </label>
              <input
                type={showPassword ? "text" : "password"}
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                           focus:ring-2 focus:ring-pink-500 outline-none"
              />
            </div>
          )}

          {/* BIO */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Bio
              </label>
              <textarea
                name="bio"
                value={formData.bio}
                onChange={handleChange}
                className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                           focus:ring-2 focus:ring-pink-500 outline-none"
              />
            </div>
          )}

          {/* THEME */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Theme
              </label>
              <input
                name="theme"
                value={formData.theme}
                onChange={handleChange}
                className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                           focus:ring-2 focus:ring-pink-500 outline-none"
              />
            </div>
          )}

          {/* GENRES */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Favorite Genres
              </label>
              <div className="flex flex-wrap gap-2">
                {["Action","Comedy","Drama","Horror","Sci-Fi","Romance"].map((g) => (
                  <button
                    type="button"
                    key={g}
                    onClick={() => toggleGenre(g)}
                    className={`px-3 py-1 rounded-full border ${
                      formData.favoriteGenres.includes(g)
                        ? "bg-pink-600 border-pink-500"
                        : "border-gray-600"
                    }`}
                  >
                    {g}
                  </button>
                ))}
              </div>
            </div>
          )}

          {error && <p className="text-red-500 text-sm">{error}</p>}
          {success && <p className="text-green-400 text-sm">{success}</p>}

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-pink-600 hover:bg-pink-500 transition font-semibold 
                       py-2 rounded-md shadow-lg disabled:opacity-70"
          >
            {loading ? "Processing..." : isLogin ? "Login" : "Sign up"}
          </button>
        </form>

        <p className="mt-6 text-center text-gray-400 text-sm">
          {isLogin ? "Don't have an account?" : "Already have an account?"}
          <button
            onClick={toggleMode}
            className="text-pink-400 hover:underline ml-1"
          >
            {isLogin ? "Sign up" : "Login"}
          </button>
        </p>

        <p className="text-center text-xs text-gray-500 mt-4">
          <Link className="hover:text-pink-400" to="/">
            ← Back to Home
          </Link>
        </p>
      </div>
    </div>
  );
}