using System.Collections.Generic;

namespace FindMyFlickWebsite.Server.DataModels
{
    public class ProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> FavoriteGenres { get; set; } = new();
        public string Theme { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}