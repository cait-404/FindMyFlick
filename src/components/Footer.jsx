import fmy from "../assets/images/fmy.png";
import { NavLink } from "react-router-dom";
export default function Footer() {
  return (
    <footer className="mt-16 bg-black/60 text-white border-t border-purple-900">

      <div className="max-w-6xl mx-auto px-6 py-10">

        <div className="grid md:grid-cols-3 gap-10 mb-8">

          {/* Description */}
          <div>
  <img
    src={fmy}
    alt="Find My Flick banner"
    className="w-40 sm:w-48 mb-3"
  />

  <p className="opacity-80 text-sm">
    FindMyFlick helps users discover movies quickly and easily.
    Browse films, explore genres, and find something great to watch
    without endlessly scrolling through streaming platforms.
  </p>
</div>

          {/* Navigation */}
          <div>
            <h4 className="footer-heading font-semibold mb-3">
  Explore
</h4>

            <ul className="space-y-2 text-sm opacity-80">
              
        <li>
  <NavLink
    to="/discover"
    end
    className={({ isActive }) =>
      isActive ? "footer-link active" : "footer-link"
    }
  >
    Browse Movies
  </NavLink>
</li>

<li>
  <NavLink
    to="/genres"
    end
    className={({ isActive }) =>
      isActive ? "footer-link active" : "footer-link"
    }
  >
    Genres
  </NavLink>
</li>

<li>
  <NavLink
    to="/filters"
    end
    className={({ isActive }) =>
      isActive ? "footer-link active" : "footer-link"
    }
  >
    Search
  </NavLink>
</li>

<li>
  <NavLink
    to="/"
    end
    className={({ isActive }) =>
      isActive ? "footer-link active" : "footer-link"
    }
  >
    Featured Movies
  </NavLink>
</li>
            </ul>
          </div>

          {/* Project */}
          <div>
            <h4 className="footer-heading font-semibold mb-3">
  Project
</h4>

            <ul className="space-y-2 text-sm opacity-80">
             <li>
  <NavLink to="/about" end className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}>
    About
  </NavLink>
</li>


<li>
  <NavLink to="/roadmap" end className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}>
    Future Roadmap
  </NavLink>
</li>

<li>
  <NavLink to="/contact" end className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}>
    Contact
  </NavLink>
</li>
            </ul>
          </div>

        </div>

        <div className="border-t border-purple-900 pt-6 text-sm flex flex-col gap-4 md:flex-row md:items-center md:justify-between opacity-70">

          <p>
            FindMyFlick ©2026. Built for movie discovery and exploration.
          </p>

       <div className="flex flex-wrap gap-4 mt-3 md:mt-0">

 

  <NavLink
    to="/terms"
    className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}
  >
    Terms of Use
  </NavLink>

  <NavLink
    to="/privacy"
    className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}
  >
    Privacy Policy
  </NavLink>

  <NavLink
    to="/project-info"
    className={({ isActive }) => isActive ? "footer-link active" : "footer-link"}
  >
    Project Info
  </NavLink>

</div>

        </div>

      </div>
    </footer>
  );
}