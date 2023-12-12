namespace OpenMovies.DTOs;

public class ReviewDTO
{
    public bool Liked { get; set; }
    public string Comment { get; set; } = string.Empty;
}