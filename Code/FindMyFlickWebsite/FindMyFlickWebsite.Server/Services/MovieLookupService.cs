using System.Threading;
using System.Threading.Tasks;

namespace FindMyFlickWebsite.Server.Services
{
    /// <summary>
    /// DB-first, API-fallback movie lookup (we will implement this in small steps).
    /// Not wired up yet.
    /// </summary>
    public class MovieLookupService
    {
        // TODO (next step): inject your DbContext here
        // private readonly YourDbContext _db;

        // TODO (later): inject your external API client here
        // private readonly ITmdbClient _tmdb;

        public MovieLookupService(
            // TODO (next step): YourDbContext db
            // TODO (later): ITmdbClient tmdb
        )
        {
            // TODO (next step): _db = db;
            // TODO (later): _tmdb = tmdb;
        }

        /// <summary>
        /// Later: try DB first; if missing, call API; then save to DB and return.
        /// For now: returns null so it compiles safely.
        /// </summary>
        public Task<object?> GetOrFetchByTmdbIdAsync(int tmdbId, CancellationToken ct = default)
        {
            return Task.FromResult<object?>(null);
        }

        public Task<object?> GetOrFetchByImdbIdAsync(string imdbId, CancellationToken ct = default)
        {
            return Task.FromResult<object?>(null);
        }
    }
}