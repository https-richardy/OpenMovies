using OpenMovies.Models;

namespace OpenMovies.DTOs;

public class ReviewDTO
{
    public bool Liked { get; set; }
    public bool ContainsSpoiler { get; set; }

    public string Comment { get; set; } = string.Empty;
    public Classification Classification { get; set; }
}