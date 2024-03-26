namespace OpenMovies.Domain.Enitites;

public class Trailer : Entity
{
    public string Link { get; set; } = string.Empty;

    public Trailer(string link)
    {
        Link = GenerateEmbeddedLink(link);
    }

    private string GenerateEmbeddedLink(string link)
    {
        // Extracting the video identifier. The video identifier is after the "v=" parameter
        string videoIdentifier = link.Split("v=")[1];
        string embeddedLink = $"https://www.youtube.com/embed/{videoIdentifier}";

        return embeddedLink;
    }
}
