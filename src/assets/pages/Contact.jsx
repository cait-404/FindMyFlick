// Contact.jsx
export default function Contact() {
  return (
    <div className="min-h-screen px-4 sm:px-6 py-10 sm:py-12 text-white max-w-4xl mx-auto">

      {/* Header */}
      <h1 className="text-3xl sm:text-4xl font-extrabold neon-text mb-4">Contact Us</h1>

      <p className="text-gray-300 mb-8 sm:mb-10 leading-relaxed max-w-2xl text-sm sm:text-base">
        Have feedback, ideas, or just want to talk about movies?  
        FindMyFlick is built with users in mind, and your input helps shape
        where it goes next.
      </p>

      {/* Contact Cards */}
      <div className="grid gap-4 sm:gap-6 sm:grid-cols-2">
        {/* Email */}
        <div className="bg-black/40 border border-gray-700 rounded-xl p-4 sm:p-5 shadow-md hover:shadow-[0_0_20px_#ff52d9] transition col-span-1 sm:col-span-2">
          <h2 className="text-lg font-semibold text-white mb-2">Email us!</h2>
          <p className="text-gray-300 text-sm mb-3">
            For questions, feedback, or collaboration inquiries.
          </p>
          <p className="text-pink-400 break-all">
            support@findmyflick.com
          </p>
        </div>
      </div>

      {/* Divider */}
      <div className="my-8 sm:my-12 h-px bg-gradient-to-r from-transparent via-gray-700 to-transparent" />

      {/* Closing Section */}
      <div className="text-center max-w-2xl mx-auto">
        <p className="text-gray-300 leading-relaxed">
          FindMyFlick is an evolving project focused on making movie discovery
          more personal, thoughtful, and user-driven.
        </p>

        <p className="text-gray-400 mt-4 text-sm sm:text-base">
          Whether it's a feature idea, bug report, or just a recommendation —
          your voice helps shape the experience.
        </p>
      </div>
    </div>
  );
}