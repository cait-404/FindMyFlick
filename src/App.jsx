import './App.css';
import { useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { Routes, Route, NavLink, useNavigate } from 'react-router-dom';
import Search from './assets/pages/Search';
 
import Home from './assets/pages/Home';
import Discover from './assets/pages/Discover';
import Genres from './assets/pages/Genres';
import About from './assets/pages/About';
import Profile from './assets/pages/Profile';
 
function App() {
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();
 
  const handleSearch = () => {
    if (searchTerm.trim() !== '') {
      //navigate(`/search?query=${searchTerm}`);
      navigate(`/search?query=${encodeURIComponent(searchTerm.trim())}`);
      setSearchTerm('');
    }
  };
 
  return (
    <>
      <nav className="w-full bg-black text-white px-6 py-4 flex justify-between items-center disco-glow">
        <h1 className="text-2xl font-bold tracking-wider neon-text">
          FindMyFlick
        </h1>
 
        <ul className="flex gap-6 text-lg font-medium items-center">
          <li>
            <NavLink to="/" className={({ isActive }) =>
              isActive ? "nav-link active-nav" : "nav-link"
            }>
              Home
            </NavLink>
          </li>
 
          <li>
            <NavLink to="/discover" className={({ isActive }) =>
              isActive ? "nav-link active-nav" : "nav-link"
            }>
              Discover
            </NavLink>
          </li>
 
          <li>
            <NavLink to="/genres" className={({ isActive }) =>
              isActive ? "nav-link active-nav" : "nav-link"
            }>
              Genres
            </NavLink>
          </li>
 
          <li>
            <NavLink to="/about" className={({ isActive }) =>
              isActive ? "nav-link active-nav" : "nav-link"
            }>
              About
            </NavLink>
          </li>
 
          <li>
            <NavLink to="/profile" className={({ isActive }) =>
              isActive ? "nav-link active-nav" : "nav-link"
            }>
              Profile
            </NavLink>
          </li>
 
          <li className="flex gap-1 items-center">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search..."
              className="p-1 rounded-md text-black text-sm"
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  handleSearch();
                }
              }}
            />
 
            <button
              onClick={handleSearch}
              className="bg-white/10 hover:bg-white/20 px-2 py-1 rounded-md text-white text-sm flex items-center justify-center"
            >
              <FaSearch />
            </button>
          </li>
        </ul>
      </nav>
 
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/discover" element={<Discover />} />
        <Route path="/genres" element={<Genres />} />
        <Route path="/about" element={<About />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/search" element={<Search />} />
      </Routes>
    </>
  );
}
 
export default App;