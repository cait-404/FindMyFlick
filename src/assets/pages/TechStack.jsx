// TechStack.jsx
export default function TechStack() {
  return (
    <div className="min-h-screen px-6 py-12 text-white max-w-4xl mx-auto">
      <h1 className="text-4xl font-extrabold neon-text mb-6">Tech Stack</h1>

      <div className="space-y-4 text-gray-300">
        <p><span className="text-white font-semibold">Frontend:</span> React, React Router, Tailwind CSS</p>
        <p><span className="text-white font-semibold">Backend:</span> ASP.NET Core Web API</p>
        <p><span className="text-white font-semibold">Database:</span> PostgreSQL with Entity Framework Core</p>
        <p><span className="text-white font-semibold">Authentication:</span> JWT-based authentication</p>
        <p><span className="text-white font-semibold">API Tools:</span> Swagger / NSwag</p>
      </div>

      <p className="text-gray-400 mt-6">
        This stack allows for a scalable, responsive, and modern web application experience.
      </p>
    </div>
  );
}