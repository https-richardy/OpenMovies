using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace OpenMovies.Services;

public interface IUserService
{
    Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);
}