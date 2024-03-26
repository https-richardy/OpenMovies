namespace OpenMovies.Models;

public class Director : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set;} = string.Empty;

    public Director() {  } // Empty constructor for Entity Framework

    public Director(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}