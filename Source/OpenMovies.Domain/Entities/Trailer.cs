using System.Text.Json.Serialization;

namespace OpenMovies.Domain.Enitites;

public class Trailer : Entity
{

    [JsonIgnore]
    public Movie Movie { get; set; }
    public string Link { get; set; } = string.Empty;

    #pragma warning disable CS8618
    public Trailer() {  }  // Empty constructor for Entity Framework

    public Trailer(string link, Movie movie)
    {
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
