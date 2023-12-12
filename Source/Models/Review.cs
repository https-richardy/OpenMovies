using Microsoft.AspNetCore.Identity;

namespace OpenMovies.Models;

public class Review
{
    public bool Liked { get; set; }
    public string Comment { get; set; } = string.Empty;

    public Movie Movie { get; set; }
    public IdentityUser User { get; set; }

    # pragma warning disable CS8618
    public Review() {  }  // Empty constructor for Entity Framework (CS8618)

    # pragma warning restore
    public Review(bool liked, string comment, Movie movie, IdentityUser user)
    {
        Liked = liked;
        Comment = comment;
        Movie = movie;
        User = user;
    }
}