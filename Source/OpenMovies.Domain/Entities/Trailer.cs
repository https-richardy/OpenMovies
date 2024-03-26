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
        Link = GenerateEmbeddedLink(link);
        Movie = movie;
    }

    private string GenerateEmbeddedLink(string link)
    {
        // Extracting the video identifier. The video identifier is after the "v=" parameter
        string videoIdentifier = link.Split("v=")[1];
        string embeddedLink = $"https://www.youtube.com/embed/{videoIdentifier}";

        return embeddedLink;
    }
}
