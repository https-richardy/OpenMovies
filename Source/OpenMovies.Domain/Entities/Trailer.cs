using System.Text.Json.Serialization;
using OpenMovies.Models.Enums;

namespace OpenMovies.Models;

public class Trailer : Entity
{
    public TrailerType Type { get; set; }
    public TrailerPlataform Plataform { get; set; }

    [JsonIgnore]
    public Movie Movie { get; set; }
    public string Link { get; set; } = string.Empty;

    #pragma warning disable CS8618
    public Trailer() {  }  // Empty constructor for Entity Framework

    public Trailer(TrailerType type, TrailerPlataform plataform, string link, Movie movie)
    {
        Type = type;
        Plataform = plataform;
        Link = link;
        Movie = movie;
    }

    public string GenerateEmbeddedLink()
    {
        switch (Plataform)
        {
            case TrailerPlataform.Youtube:

            string videoId = Link.Split("v=")[1];
            return $"https://www.youtube.com/embed/{videoId}";

            case TrailerPlataform.Vimeo:

            string vimeoId = Link.Split("/")[3];
            return $"https://player.vimeo.com/video/{vimeoId}";

            default: return Link;
        }
    }
}
