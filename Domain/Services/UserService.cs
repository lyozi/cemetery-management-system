using Domain.DTOs;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading.Tasks;

namespace Domain.Services
{
  public class UserService : IUserService
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginDTO model)
    {
      IdentityUser? user = null;
      if (model.UserIdentifier.Contains("@"))
      {
        user = await _userManager.FindByEmailAsync(model.UserIdentifier);
      }
      else
      {
        user = await _userManager.FindByNameAsync(model.UserIdentifier);
      }
      if (user == null)
      {
        return new LoginResponseDTO { Succeeded = false, Message = "User not found" };
      }

      var result = await _signInManager
        .PasswordSignInAsync(user, model.Password, false, false);
      if (result.Succeeded)
      {
        var roles = await _userManager.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "Member";
        var response = new LoginResponseDTO
        {
          Succeeded = true,
          Message = "Login successful",
          Username = user.UserName,
          Role = role
        };
        return response;
      }
      return new LoginResponseDTO { Succeeded = false, Message = "Invalid login attempt" };
    }

    public async Task<(bool Succeeded, string Message)> RegisterAsync(RegistrationDTO model)
    {
      var user = new IdentityUser { UserName = model.Username, Email = model.Email };
      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
        return (true, "Registration successful");
      }

      return (false, "Registration failed");
    }

    public async Task LogoutAsync()
    {
      await _signInManager.SignOutAsync();
    }
  }
}
