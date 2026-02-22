using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarningTaxonomyController : ControllerBase
    {
        private readonly FindmyflickContext _context;

        public WarningTaxonomyController(FindmyflickContext context)
        {
            _context = context;
        }

        public sealed class TopicDto
        {
            public int DtddTopicId { get; set; }
            public string TopicName { get; set; } = "";
        }

        public sealed class SubcategoryDto
        {
            public int SubcategoryId { get; set; }
            public string SubcategoryName { get; set; } = "";
            public List<TopicDto> Topics { get; set; } = new();
        }

        public sealed class CategoryDto
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; } = "";
            public List<SubcategoryDto> Subcategories { get; set; } = new();

            // Topics that map directly to the category without a subcategory mapping.
            public List<TopicDto> TopicsDirect { get; set; } = new();
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetTaxonomy()
        {
            // Category -> Subcategory -> Topics
            const string sql = @"
SELECT
  wc.category_id,
  wc.category_name,
  ws.subcategory_id,
  ws.subcategory_name,
  w.dtdd_topic_id,
  w.topic_name
FROM public.warnings w
LEFT JOIN public.warning_category_topics wct
  ON wct.dtdd_topic_id = w.dtdd_topic_id
LEFT JOIN public.warning_categories wc
  ON wc.category_id = wct.category_id
LEFT JOIN public.warning_subcategory_topics wst
  ON wst.dtdd_topic_id = w.dtdd_topic_id
LEFT JOIN public.warning_subcategories ws
  ON ws.subcategory_id = wst.subcategory_id
WHERE wc.category_id IS NOT NULL
ORDER BY wc.category_name, ws.subcategory_name NULLS LAST, w.topic_name;";

            var categories = new Dictionary<int, CategoryDto>();

            // IMPORTANT:
            // Do NOT dispose the DbConnection you get from EF.
            // EF owns it; disposing it can cause ObjectDisposedException later.
            DbConnection conn = _context.Database.GetDbConnection();

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                // If the provider is Npgsql, this will be an NpgsqlCommand under the hood.
                // But we don't need to cast; DbCommand is enough.
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var categoryId = reader.GetInt32(0);
                    var categoryName = reader.GetString(1);

                    int? subcategoryId = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                    string? subcategoryName = reader.IsDBNull(3) ? null : reader.GetString(3);

                    var topicId = reader.GetInt32(4);
                    var topicName = reader.GetString(5);

                    if (!categories.TryGetValue(categoryId, out var cat))
                    {
                        cat = new CategoryDto
                        {
                            CategoryId = categoryId,
                            CategoryName = categoryName
                        };
                        categories.Add(categoryId, cat);
                    }

                    if (subcategoryId is null)
                    {
                        if (!cat.TopicsDirect.Any(t => t.DtddTopicId == topicId))
                        {
                            cat.TopicsDirect.Add(new TopicDto
                            {
                                DtddTopicId = topicId,
                                TopicName = topicName
                            });
                        }

                        continue;
                    }

                    var sub = cat.Subcategories.FirstOrDefault(s => s.SubcategoryId == subcategoryId.Value);
                    if (sub == null)
                    {
                        sub = new SubcategoryDto
                        {
                            SubcategoryId = subcategoryId.Value,
                            SubcategoryName = subcategoryName ?? ""
                        };
                        cat.Subcategories.Add(sub);
                    }

                    if (!sub.Topics.Any(t => t.DtddTopicId == topicId))
                    {
                        sub.Topics.Add(new TopicDto
                        {
                            DtddTopicId = topicId,
                            TopicName = topicName
                        });
                    }
                }

                // Clean ordering
                foreach (var cat in categories.Values)
                {
                    cat.TopicsDirect = cat.TopicsDirect.OrderBy(t => t.TopicName).ToList();
                    cat.Subcategories = cat.Subcategories
                        .OrderBy(s => s.SubcategoryName)
                        .Select(s =>
                        {
                            s.Topics = s.Topics.OrderBy(t => t.TopicName).ToList();
                            return s;
                        })
                        .ToList();
                }

                return Ok(categories.Values.OrderBy(c => c.CategoryName).ToList());
            }
            finally
            {
                // Leave the connection lifecycle to EF, but closing here is safe if we opened it.
                if (conn.State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }
        }
    }
}