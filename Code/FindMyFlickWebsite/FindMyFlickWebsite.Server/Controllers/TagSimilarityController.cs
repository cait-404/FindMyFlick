using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//chat generated using: My databse has a series of tags for each movie. I want to be able to have a similarity score of movies with the closest
//matching tags to the tags inputed. and output the X highest scores. This should use K means clustering in a new controller

//refactor for inherrnt fields (genre, etc)
//
namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagSimilarityController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        public TagSimilarityController(FindmyflickContext context)
        {
            _context = context;
        }

        // Request DTO
        public class SimilarityRequest
        {
            public List<string> TagNames { get; set; } = new();
            public int K { get; set; } = 5;               // number of clusters for k-means
            public int Top { get; set; } = 10;            // number of top results to return
            public int MaxSamples { get; set; } = 2000;   // optional cap on how many movies to load
        }

        // Response DTO
        public class SimilarityResult
        {
            public string ImdbId { get; set; } = string.Empty;
            public int ParsedId { get; set; }
            public string Title { get; set; } = string.Empty;
            public int? Year { get; set; }
            public string? Poster { get; set; }
            public double SimilarityScore { get; set; }  // 0..1 (cosine similarity)
            public List<string> Tags { get; set; } = new();
            public int ClusterId { get; set; }
            public double DistanceToNearestClusterCenter { get; set; }
        }

        // POST api/TagSimilarity/similar
        // Accepts a set of tags and returns the top-N most similar movies using k-means clustering + cosine similarity.
        [HttpPost("similar")]
        [ProducesResponseType(typeof(IEnumerable<SimilarityResult>), 200)]
        public async Task<IActionResult> FindSimilarByTags([FromBody] SimilarityRequest req)
        {
            try
            {
                if (req == null || req.TagNames == null || !req.TagNames.Any())
                    return BadRequest(new { message = "TagNames must contain at least one tag." });

                // Normalize input tags
                var inputTags = req.TagNames
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim().ToLowerInvariant())
                    .Distinct()
                    .ToList();

                if (!inputTags.Any())
                    return BadRequest(new { message = "TagNames must contain at least one non-empty tag." });

                // Load movie rows with their warnings/tags (only "yes" answers) - capped by MaxSamples
                var loadedMovies = await _context.Movies
                    .Include(m => m.MovieWarnings).ThenInclude(w => w.DtddTopic)
                    .AsNoTracking()
                    .ToListAsync();

                if (loadedMovies == null || !loadedMovies.Any())
                    return Ok(Array.Empty<SimilarityResult>());

                // Optionally cap samples (keeps behavior predictable)
                if (req.MaxSamples > 0 && loadedMovies.Count > req.MaxSamples)
                    loadedMovies = loadedMovies.Take(req.MaxSamples).ToList();

                // Build a tag vocabulary: union of all tags across movies + input tags
                var movieTagLists = loadedMovies.Select(m =>
                    (m.ImdbId ?? string.Empty,
                     Tags: (m.MovieWarnings ?? Array.Empty<MovieWarning>())
                        .Where(w => string.Equals(w.Answer?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
                        .Select(w => (w.DtddTopic?.TopicName ?? string.Empty).Trim().ToLowerInvariant())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList()
                    )).ToList();

                var vocabulary = new HashSet<string>(movieTagLists.SelectMany(x => x.Tags));
                foreach (var t in inputTags) vocabulary.Add(t);
                var vocabList = vocabulary.OrderBy(v => v).ToList(); // stable ordering
                var tagIndex = vocabList.Select((t, i) => new { t, i }).ToDictionary(x => x.t, x => x.i);

                if (!vocabList.Any())
                    return Ok(Array.Empty<SimilarityResult>());

                // Build movie vectors (binary vectors)
                var movieVectors = new List<double[]>();
                var movieMeta = new List<(string ImdbId, string Title, int? Year, string? Poster, List<string> Tags)>();

                foreach (var (imdbId, Tags) in movieTagLists)
                {
                    var movie = loadedMovies.FirstOrDefault(m => string.Equals(m.ImdbId, imdbId, StringComparison.OrdinalIgnoreCase));
                    var vec = new double[vocabList.Count];
                    foreach (var tag in Tags)
                    {
                        if (tagIndex.TryGetValue(tag, out var idx))
                            vec[idx] = 1.0;
                    }

                    movieVectors.Add(vec);
                    movieMeta.Add((
                        ImdbId: imdbId ?? string.Empty,
                        Title: movie?.Title ?? "(Untitled)",
                        Year: movie?.ReleaseYear,
                        Poster: movie?.PosterUrl,
                        Tags: Tags
                    ));
                }

                // Build input vector
                var inputVec = new double[vocabList.Count];
                foreach (var t in inputTags)
                {
                    if (tagIndex.TryGetValue(t, out var idx))
                        inputVec[idx] = 1.0;
                }

                // If all-zero input vector (shouldn't happen because inputTags were added to vocab), handle conservatively
                var allZeroInput = inputVec.All(v => v == 0.0);
                if (allZeroInput)
                    return BadRequest(new { message = "Input tags did not match any known tag vocabulary." });

                // Run k-means clustering on movieVectors
                var k = Math.Max(1, Math.Min(req.K, movieVectors.Count));
                var kmeans = KMeans.Cluster(movieVectors, k, maxIterations: 100, rngSeed: 0);

                // Find nearest cluster center to the input vector
                var clusterDistances = new double[kmeans.Centers.Length];
                for (int i = 0; i < kmeans.Centers.Length; i++)
                {
                    clusterDistances[i] = Distance.Euclidean(kmeans.Centers[i], inputVec);
                }

                var nearestCluster = Array.IndexOf(clusterDistances, clusterDistances.Min());

                // Prepare results: compute similarity score (cosine) for each movie,
                // include cluster assignment and distance to its cluster center.
                var results = new List<SimilarityResult>(movieVectors.Count);
                for (int i = 0; i < movieVectors.Count; i++)
                {
                    var vec = movieVectors[i];
                    var clusterId = kmeans.Assignments[i];
                    var center = kmeans.Centers[clusterId];
                    var distToCenter = Distance.Euclidean(vec, center);
                    var simToInput = Similarity.Cosine(vec, inputVec);

                    results.Add(new SimilarityResult
                    {
                        ImdbId = movieMeta[i].ImdbId,
                        ParsedId = ParseImdbToInt(movieMeta[i].ImdbId),
                        Title = movieMeta[i].Title,
                        Year = movieMeta[i].Year,
                        Poster = movieMeta[i].Poster,
                        SimilarityScore = simToInput,
                        Tags = movieMeta[i].Tags,
                        ClusterId = clusterId,
                        DistanceToNearestClusterCenter = distToCenter
                    });
                }

                // Optionally prefer movies from the nearest cluster: sort so that cluster members come first,
                // but still ordered by similarity score. This preserves k-means usage while ranking by true similarity.
                var final = results
                    .OrderByDescending(r => r.ClusterId == nearestCluster)    // cluster members first
                    .ThenByDescending(r => r.SimilarityScore)
                    .Take(Math.Max(1, req.Top))
                    .ToList();

                return Ok(final);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
            }
        }

        // -------------------------
        // Helpers: parsing & simple math utilities
        // -------------------------
        private static int ParseImdbToInt(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId)) return 0;
            var digits = new string(imdbId.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var result) ? result : 0;
        }

        private static class Distance
        {
            public static double Euclidean(double[] a, double[] b)
            {
                var sum = 0.0;
                var n = Math.Min(a.Length, b.Length);
                for (int i = 0; i < n; i++)
                {
                    var d = a[i] - b[i];
                    sum += d * d;
                }
                return Math.Sqrt(sum);
            }
        }

        private static class Similarity
        {
            public static double Cosine(double[] a, double[] b)
            {
                double dot = 0.0, na = 0.0, nb = 0.0;
                var n = Math.Min(a.Length, b.Length);
                for (int i = 0; i < n; i++)
                {
                    dot += a[i] * b[i];
                    na += a[i] * a[i];
                    nb += b[i] * b[i];
                }
                if (na == 0 || nb == 0) return 0.0;
                return dot / (Math.Sqrt(na) * Math.Sqrt(nb));
            }
        }

        // Very small KMeans implementation (deterministic when rngSeed provided).
        private class KMeans
        {
            public double[][] Centers { get; private set; } = Array.Empty<double[]>();
            public int[] Assignments { get; private set; } = Array.Empty<int>();

            private KMeans() { }

            public static KMeans Cluster(List<double[]> points, int k, int maxIterations = 100, int rngSeed = 0)
            {
                var rnd = new Random(rngSeed);
                var dim = points.FirstOrDefault()?.Length ?? 0;
                var centers = new double[k][];

                // KMeans++ style initialization (deterministic-ish with seed)
                centers[0] = points[rnd.Next(points.Count)].ToArray();
                var distances = new double[points.Count];
                for (int c = 1; c < k; c++)
                {
                    double total = 0;
                    for (int i = 0; i < points.Count; i++)
                    {
                        var d = Distance.Euclidean(points[i], centers.Take(c).First());
                        // compute min distance to any existing center
                        var minD = double.MaxValue;
                        for (int j = 0; j < c; j++)
                        {
                            var dd = Distance.Euclidean(points[i], centers[j]);
                            if (dd < minD) minD = dd;
                        }
                        distances[i] = minD * minD; // probability weight (squared distance)
                        total += distances[i];
                    }

                    if (total <= 0)
                    {
                        centers[c] = points[rnd.Next(points.Count)].ToArray();
                        continue;
                    }

                    // pick by weighted probability
                    var threshold = rnd.NextDouble() * total;
                    double cum = 0;
                    int selected = 0;
                    for (int i = 0; i < distances.Length; i++)
                    {
                        cum += distances[i];
                        if (cum >= threshold)
                        {
                            selected = i;
                            break;
                        }
                    }
                    centers[c] = points[selected].ToArray();
                }

                var assignments = new int[points.Count];
                Array.Fill(assignments, -1);

                for (int iter = 0; iter < maxIterations; iter++)
                {
                    var changed = false;

                    // Assignment step
                    for (int i = 0; i < points.Count; i++)
                    {
                        var best = -1;
                        var bestDist = double.MaxValue;
                        for (int c = 0; c < k; c++)
                        {
                            var d = Distance.Euclidean(points[i], centers[c]);
                            if (d < bestDist)
                            {
                                bestDist = d;
                                best = c;
                            }
                        }

                        if (assignments[i] != best)
                        {
                            assignments[i] = best;
                            changed = true;
                        }
                    }

                    // If no change, convergence
                    if (!changed && iter > 0) break;

                    // Update step: recompute centers
                    var newCenters = new double[k][];
                    var counts = new int[k];
                    for (int c = 0; c < k; c++)
                    {
                        newCenters[c] = new double[dim];
                    }

                    for (int i = 0; i < points.Count; i++)
                    {
                        var c = assignments[i];
                        counts[c]++;
                        var pt = points[i];
                        for (int d = 0; d < dim; d++)
                            newCenters[c][d] += pt[d];
                    }

                    for (int c = 0; c < k; c++)
                    {
                        if (counts[c] == 0)
                        {
                            // reinitialize empty center
                            newCenters[c] = points[rnd.Next(points.Count)].ToArray();
                        }
                        else
                        {
                            for (int d = 0; d < dim; d++)
                                newCenters[c][d] /= counts[c];
                        }
                    }

                    centers = newCenters;
                }

                return new KMeans
                {
                    Centers = centers,
                    Assignments = assignments
                };
            }
        }
    }
}