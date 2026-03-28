import { useState, useEffect } from "react";
import API_URL from "../../config.js";

/* ===============================
   Profile Page
================================ */
export default function Profile() {
  const [profile, setProfile] = useState(null);
  const [editing, setEditing] = useState(false);
  const [loading, setLoading] = useState(true);

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    favoriteGenres: [],
    bio: "",
    theme: "Disco",
  });

  /* ===============================
     FETCH PROFILE (Backend Ready)
  ================================ */
  useEffect(() => {
    // Replace with your real endpoint later
    fetch('${API_URL}/api/profile')
      .then((res) => {
        if (!res.ok) throw new Error("No profile found");
        return res.json();
      })
      .then((data) => {
        setProfile(data);
        setFormData(data);
        setLoading(false);
      })
      .catch(() => {
        setLoading(false); // fallback to create profile
      });
  }, []);

  /* ===============================
     HANDLERS
  ================================ */
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const toggleGenre = (genre) => {
    setFormData((prev) => ({
      ...prev,
      favoriteGenres: prev.favoriteGenres.includes(genre)
        ? prev.favoriteGenres.filter((g) => g !== genre)
        : [...prev.favoriteGenres, genre],
    }));
  };

  const handleSave = () => {
    if (!formData.name || !formData.email) {
      alert("Name and email are required.");
      return;
    }

    // POST or PUT later
    setProfile(formData);
    setEditing(false);
  };

  /* ===============================
     UI
  ================================ */
  if (loading) {
    return <p className="text-white p-6">Loading profile...</p>;
  }

  return (
    <div className="p-6 text-white max-w-6xl mx-auto">

      {/* ===============================
          HEADER
      ================================ */}
      <div className="flex items-center gap-6 mb-10">
        <div className="w-28 h-28 rounded-full bg-linear-to-br from-pink-500 to-purple-700 
                        flex items-center justify-center text-4xl font-bold neon-text">
          {(profile?.name || formData.name || "U")[0]?.toUpperCase()}
        </div>

        <div>
          <h2 className="text-3xl font-bold neon-text">
            {profile ? profile.name : "Create Your Profile"}
          </h2>
          {profile && (
            <p className="text-gray-300 mt-1">{profile.email}</p>
          )}
        </div>
      </div>

      
      {profile && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-12">
          <StatCard title="Movies Watched" value="128" />
          <StatCard title="Favorite Genres" value={profile.favoriteGenres.length} />
          <StatCard title="Lists Created" value="4" />
        </div>
      )}

   
      <div className="bg-gray-900/80 rounded-xl p-6 shadow-xl">

        {!editing && profile ? (
          <>
            <InfoRow label="Bio" value={profile.bio || "No bio yet"} />
            <InfoRow
              label="Favorite Genres"
              value={profile.favoriteGenres.join(", ") || "None selected"}
            />
            <InfoRow label="Theme" value={profile.theme} />

            <button
              onClick={() => setEditing(true)}
              className="mt-6 px-6 py-2 rounded-full bg-pink-600 hover:bg-pink-500 
                         transition neon-text"
            >
              Edit Profile
            </button>
          </>
        ) : (
          <>
            
            <div className="grid gap-4 max-w-xl">

              <Input
                label="Name"
                name="name"
                value={formData.name}
                onChange={handleChange}
              />

              <Input
                label="Email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
              />

              <Textarea
                label="Bio"
                name="bio"
                value={formData.bio}
                onChange={handleChange}
                placeholder="Tell us about your movie taste..."
              />

              <div>
                <label className="block mb-2 text-sm font-semibold">
                  Favorite Genres
                </label>
                <div className="flex flex-wrap gap-2">
                  {[
                    "Action",
                    "Comedy",
                    "Drama",
                    "Horror",
                    "Sci-Fi",
                    "Romance",
                    "Thriller",
                    "Animation",
                  ].map((genre) => (
                    <button
                      key={genre}
                      onClick={() => toggleGenre(genre)}
                      className={`px-3 py-1 rounded-full border transition
                        ${
                          formData.favoriteGenres.includes(genre)
                            ? "bg-pink-600 border-pink-500 neon-text"
                            : "border-gray-600 text-gray-300 hover:bg-gray-800"
                        }`}
                    >
                      {genre}
                    </button>
                  ))}
                </div>
              </div>

              <div>
                <label className="block mb-2 text-sm font-semibold">
                  App Theme
                </label>
                <select
                  name="theme"
                  value={formData.theme}
                  onChange={handleChange}
                  className="w-full p-2 rounded bg-black border border-gray-700"
                >
                  <option>Disco</option>
                  <option>Dark</option>
                  <option>Neon</option>
                  <option>Minimal</option>
                </select>
              </div>

              <div className="flex gap-4 mt-6">
                <button
                  onClick={handleSave}
                  className="px-6 py-2 rounded-full bg-green-600 hover:bg-green-500 neon-text"
                >
                  Save Profile
                </button>

                {profile && (
                  <button
                    onClick={() => setEditing(false)}
                    className="px-6 py-2 rounded-full border border-gray-600 hover:bg-gray-800"
                  >
                    Cancel
                  </button>
                )}
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}


function StatCard({ title, value }) {
  return (
    <div className="rounded-xl bg-gray-900/80 p-6 text-center shadow-lg">
      <h4 className="text-sm text-gray-400 mb-2">{title}</h4>
      <div className="text-3xl font-bold neon-text">{value}</div>
    </div>
  );
}

function InfoRow({ label, value }) {
  return (
    <div className="mb-4">
      <p className="text-sm text-gray-400">{label}</p>
      <p className="text-lg">{value}</p>
    </div>
  );
}

function Input({ label, ...props }) {
  return (
    <div>
      <label className="block mb-1 text-sm font-semibold">{label}</label>
      <input
        {...props}
        className="w-full p-2 rounded bg-black border border-gray-700"
      />
    </div>
  );
}

function Textarea({ label, ...props }) {
  return (
    <div>
      <label className="block mb-1 text-sm font-semibold">{label}</label>
      <textarea
        {...props}
        rows={3}
        className="w-full p-2 rounded bg-black border border-gray-700 resize-none"
      />
    </div>
  );
}
