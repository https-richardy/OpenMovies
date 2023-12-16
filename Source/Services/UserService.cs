using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace OpenMovies.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    # pragma warning disable CS8619
    public Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal)
    {
        return _userManager.GetUserAsync(principal);
    }
}
