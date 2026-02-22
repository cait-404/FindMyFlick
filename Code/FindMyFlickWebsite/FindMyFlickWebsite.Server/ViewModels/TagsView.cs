using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FindMyFlickWebsite.Server.Models
{
    public class TagsView
    {
        [JsonPropertyName("Plot tags")]
        public List<PlotTag> PlotTags { get; set; } = new List<PlotTag>();

        [JsonPropertyName("Trigger tags")]
        public List<TriggerTag> TriggerTags { get; set; } = new List<TriggerTag>();

        [JsonPropertyName("Person tags")]
        public List<PersonTag> PersonTags { get; set; } = new List<PersonTag>();

        // Shared base for tag shape (no vote fields here)
        public abstract class TagBase
        {
            [JsonPropertyName("tagType")]
            public string? TagType { get; set; }

            [JsonPropertyName("tagID")]
            public int TagID { get; set; }

            [JsonPropertyName("tagName")]
            public string? TagName { get; set; }
        }

        public class PlotTag : TagBase { }

        public class TriggerTag : TagBase { }

        public class PersonTag : TagBase { }
    }
}
