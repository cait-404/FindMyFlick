using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels
{
    public class ProfileUpdate
    {
        public List<string> FavoriteGenres { get; set; } = new();
        public string Theme { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}