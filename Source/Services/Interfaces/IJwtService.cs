using System.Security.Claims;

namespace OpenMovies.Services;

/// <summary>
/// Interface for a service responsible for creating JSON Web Token (JWT) based on provided user information (claims identity).
/// </summary>
/// <remarks>
/// The <c>IJwtService</c> interface defines the contract for a service responsible for generating JSON Web Tokens (JWTs)
/// based on the provided user information, represented by a <see cref="ClaimsIdentity"/>.
/// </remarks>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT based on the provided claims identity.
    /// </summary>
    /// <param name="claimsIdentity">User information and claims to be included in the JWT.</param>
    /// <returns>Generated JWT as a string.</returns>
    /// <remarks>
    /// The <c>GenerateToken</c> method creates a JWT using the claims and user information encapsulated in the provided
    /// <paramref name="claimsIdentity"/>. The resulting JWT can be used for authentication and authorization purposes.
    /// </remarks>
    string GenerateToken(ClaimsIdentity claimsIdentity);
}