using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using OpenMovies.Domain.Enitites.Enums;

namespace OpenMovies.Domain.Enitites;

public class Review : Entity
{
    public bool Liked { get; set; }
    public string Comment { get; set; } = string.Empty;

    [JsonIgnore]
    public Movie Movie { get; set; }
    public IdentityUser User { get; set; }

    public ReviewClassification Classification { get; set; }
    public bool ContainsSpoiler { get; set; }

    # pragma warning disable CS8618
    public Review() {  }  // Empty constructor for Entity Framework (CS8618)

    # pragma warning restore
    public Review(bool liked, string comment, Movie movie, IdentityUser user, ReviewClassification classification, bool containsSpoiler)
    {
        Liked = liked;
        Comment = comment;
        Movie = movie;
        User = user;
        Classification = classification;
        ContainsSpoiler = containsSpoiler;
    }
}