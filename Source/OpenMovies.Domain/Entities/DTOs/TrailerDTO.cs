using OpenMovies.Models.Enums;

namespace OpenMovies.DTOs;

public class TrailerDTO
{
    public TrailerType Type { get; set; }
    public TrailerPlataform Plataform { get; set; }
    public string Link { get; set; } = string.Empty;
}
