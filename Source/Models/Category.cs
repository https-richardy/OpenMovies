namespace OpenMovies.Models;


public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<Movie> Movies { get; set; } = new List<Movie>();

    public Category() {  }  // Empty constructor for Entity Framework

    # pragma warning restore
    public Category(string name)
    {
        Name = name;
    }
}