import { useState, useEffect } from "react";
import API_URL from "../../config.js";

export default function Profile() {
  const [profile, setProfile] = useState(null);
  const [editing, setEditing] = useState(false);
  const [loading, setLoading] = useState(true);

  const [formData, setFormData] = useState({
    favoriteGenres: [],
    bio: "",
    theme: "Disco",
  });

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const token = localStorage.getItem("token");

fetch(`${API_URL}/api/Profile`, {
  headers: {
    Authorization: `Bearer ${token}`
  }
});

        if (!token) {
          throw new Error("Not logged in");
        }

        //const res = await fetch(`${API_URL}/api/Profile`, { 
        const res = await fetch(`${API_URL}/api/Profile`, {
          headers: {
            Authorization: `Bearer ${token}`, // THIS IS THE FIX
          },
        });

        if (res.status === 401) {
          throw new Error("Not logged in");
        }

        const data = await res.json();

        setProfile(data);
        setFormData({
          favoriteGenres: data.favoriteGenres || [],
          bio: data.bio || "",
          theme: data.theme || "Disco",
        });

      } catch (err) {
        console.error(err);
        setProfile(null);
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

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

 const handleSave = async () => {
  try {
    const token = localStorage.getItem("token");

    const res = await fetch(`${API_URL}/api/Profile`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(formData),
    });

    if (!res.ok) {
      const text = await res.text();
      throw new Error(text);
    }

    const updated = await res.json();

    // THIS IS THE FIX
    setProfile(updated);
    setFormData({
      favoriteGenres: updated.favoriteGenres || [],
      bio: updated.bio || "",
      theme: updated.theme || "Disco",
    });

    setEditing(false);

  } catch (err) {
    console.error(err);
    alert(err.message);
  }
};

  if (loading) {
    return <p className="text-white p-6">Loading profile...</p>;
  }

  if (!profile) {
    return (
      <div className="text-white p-6 text-center">
        <p>You must log in to view your profile.</p>
      </div>
    );
  }

  return (
    <div className="px-4 sm:px-6 py-6 text-white max-w-6xl mx-auto">

      <div className="flex flex-col sm:flex-row items-center gap-4 sm:gap-6 mb-8 sm:mb-10 text-center sm:text-left">
        <div className="w-20 h-20 sm:w-28 sm:h-28 rounded-full bg-linear-to-br from-pink-500 to-purple-700 
                        flex items-center justify-center text-4xl font-bold neon-text">
          {profile.username ? profile.username[0].toUpperCase() : "U"}
        </div>

        <div>
          <h2 className="text-xl sm:text-3xl font-bold neon-text">
            {profile.username || "Your Profile"}
          </h2>
        </div>
      </div>

      <div className="bg-gray-900/80 rounded-xl p-4 sm:p-6 shadow-xl">

        {!editing ? (
          <>
            <InfoRow label="Bio" value={profile.bio || "No bio yet"} />
            <InfoRow
              label="Favorite Genres"
              value={profile.favoriteGenres.join(", ") || "None"}
            />
            <InfoRow label="Theme" value={profile.theme} />

            <button
              onClick={() => setEditing(true)}
              className="mt-6 px-6 py-3 sm:py-2 rounded-full bg-pink-600 hover:bg-pink-500 neon-text w-full sm:w-auto"
            >
              Edit Profile
            </button>
          </>
        ) : (
          <div className="grid gap-4 w-full max-w-xl">

            <Textarea
              label="Bio"
              name="bio"
              value={formData.bio}
              onChange={handleChange}
            />

            <div>
              <label className="block mb-2 text-sm font-semibold">
                Favorite Genres
              </label>

              <div className="flex flex-wrap gap-2 justify-center sm:justify-start">
                {["Action","Comedy","Drama","Horror","Sci-Fi","Romance"].map((g) => (
                  <button
                    key={g}
                    onClick={() => toggleGenre(g)}
                    className={`px-3 py-2 sm:py-1 rounded-full border ${
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

            <button
              onClick={handleSave}
              className="px-6 py-3 sm:py-2 rounded-full bg-green-600 w-full sm:w-auto"
            >
              Save
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

function InfoRow({ label, value }) {
  return (
    <div className="mb-4">
      <p className="text-sm text-gray-400">{label}</p>
      <p className="text-sm sm:text-lg">{value}</p>
    </div>
  );
}

function Textarea({ label, ...props }) {
  return (
    <div>
      <label className="block mb-1 text-sm font-semibold">{label}</label>
      <textarea {...props} className="w-full p-3 sm:p-2 rounded bg-black border border-gray-700" />
    </div>
  );
}