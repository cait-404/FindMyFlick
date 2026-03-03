import { useState } from "react";
import { Link } from "react-router-dom";
import { FaEye, FaEyeSlash } from "react-icons/fa";

export default function LoginSignup() {

  const [isLogin, setIsLogin] = useState(true);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    name: ""
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showPassword, setShowPassword] = useState(false);

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

  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);
    setError("");
    setSuccess("");

    try {
      // Demo mock authentication
      await new Promise(r => setTimeout(r, 800));

      setSuccess(isLogin
        ? "Logged in successfully!"
        : "Account created!");

      setFormData({
        email: "",
        password: "",
        name: ""
      });

    } catch {
      setError("Something went wrong. Please try again.");
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

          {/* NAME FIELD (Signup only) */}
          {!isLogin && (
            <div>
              <label className="block text-sm text-gray-300 mb-1">
                Name
              </label>

              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 rounded-md bg-gray-800 text-gray-100
                           focus:ring-2 focus:ring-pink-500 outline-none"
              />
            </div>
          )}

          {/* EMAIL */}
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

          {/* PASSWORD WITH SHOW/HIDE TOGGLE ⭐ */}
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

          {error && <p className="text-red-500 text-sm">{error}</p>}
          {success && <p className="text-green-400 text-sm">{success}</p>}

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-pink-600 hover:bg-pink-500 transition font-semibold 
                       py-2 rounded-md shadow-lg disabled:opacity-70"
          >
            {loading
              ? "Processing..."
              : isLogin
                ? "Login"
                : "Sign up"}
          </button>

        </form>

        {/* Toggle Login / Signup */}
        <p className="mt-6 text-center text-gray-400 text-sm">
          {isLogin ? "Don't have an account?" : "Already have an account?"}

          <button
            type="button"
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