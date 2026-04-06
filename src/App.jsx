import './App.css'; 
import { Routes, Route, NavLink, Link, useNavigate } from 'react-router-dom';
import { useState } from "react";
import { FaSearch } from 'react-icons/fa';
import Footer from "./components/Footer";
import ProfileMenu from "./components/ProfileMenu"; 

import Home from './assets/pages/Home';
import Discover from './assets/pages/Discover';
import Genres from './assets/pages/Genres';
import About from './assets/pages/About';
import Profile from './assets/pages/Profile';
import Search from './assets/pages/Search';
import LoginSignup from './assets/pages/LoginSignup';
import MovieDetails from './assets/pages/MovieDetails';
import Filters from './assets/pages/Filters';

import { MovieProvider } from './context/MovieContext'; // Use the context provider

export default function App() {
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();

  const token = localStorage.getItem("token"); 

  const handleSearch = () => {
    if (!searchTerm.trim()) return;
    navigate(`/search?query=${encodeURIComponent(searchTerm.trim())}`);
    setSearchTerm('');
  };

  return (
    <MovieProvider>
      {/* NAVBAR */}
      <nav className="flex justify-between items-center p-4 bg-black/80 text-white flex-wrap gap-4">
        <h1 className="text-2xl font-bold text-pink-500">FindMyFlick</h1>

        <div className="flex gap-6 items-center flex-wrap">
          <NavLink to="/" className={({isActive}) => isActive ? "nav-link active-nav" : "nav-link"}>Home</NavLink>
          <NavLink to="/discover" className={({isActive}) => isActive ? "nav-link active-nav" : "nav-link"}>Discover</NavLink>
          <NavLink to="/filters" className={({isActive}) => isActive ? "nav-link active-nav" : "nav-link"}>Advanced Search</NavLink>
          <NavLink to="/genres" className={({isActive}) => isActive ? "nav-link active-nav" : "nav-link"}>Genres</NavLink>
          <NavLink to="/about" className={({isActive}) => isActive ? "nav-link active-nav" : "nav-link"}>About</NavLink>

          {/* SEARCH BAR */}
          <div className="flex gap-2 items-center">
            <input
              type="text"
              placeholder="Search movies..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && handleSearch()}
              className="p-2 rounded-md text-black text-sm"
            />
            <button
              onClick={handleSearch}
              className="btn-neon disco-glow px-3 py-2 rounded-md flex items-center justify-center"
            >
              <FaSearch />
            </button>
          </div>

          {/* LOGIN / PROFILE */}
          {!token ? (
            <Link
              to="/auth"
              className="px-5 py-2 rounded-full font-semibold transition
                         bg-linear-to-r from-[#ff39e1] to-[#ff6ed0]
                         shadow-[0_0_15px_#ff39e1]
                         hover:shadow-[0_0_30px_#ff6ed0]
                         hover:scale-105"
            >
              Login / Signup
            </Link>
          ) : (
            <ProfileMenu />
          )}
        </div>
      </nav>

      {/* ROUTES */}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/discover" element={<Discover />} />
        <Route path="/filters" element={<Filters />} />
        <Route path="/genres" element={<Genres />} />
        <Route path="/about" element={<About />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/search" element={<Search />} />
        <Route path="/movie/:id" element={<MovieDetails />} />
        <Route path="/auth" element={<LoginSignup />} />
        <Route path="/terms" element={<About />} />
        <Route path="/privacy" element={<About />} />
        <Route path="/project-info" element={<About />} />
      </Routes>

      <Footer />
    </MovieProvider>
  );
}