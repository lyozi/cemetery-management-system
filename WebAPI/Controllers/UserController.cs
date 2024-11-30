using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Identity.Controllers
{
	[ApiController]
	public class UserController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public UserController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("customLogin")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return Unauthorized(new { Message = "User not found" });
				}

				var roles = await _userManager.GetRolesAsync(user);
				var role = roles.FirstOrDefault() ?? "NoRole";

				return Json(new { Email = user.Email, Role = role });
			}
			return Unauthorized(new { Message = "Invalid login attempt" });
		}


		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok();
		}

		public class LoginModel
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}

		public class LoginResponseModel
		{
			public string Email { get; set; } = string.Empty;
			public string Role { get; set; } = string.Empty;
		}
	}
}
