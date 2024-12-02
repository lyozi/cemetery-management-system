using Domain.DTOs;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/User")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO model)
		{
			var (Succeeded, Message, Response) = await _userService.LoginAsync(model);
			if (Succeeded)
			{
				return Ok(Response);
			}
			return Unauthorized(Message);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
		{
			var (Succeeded, Message) = await _userService.RegisterAsync(model);
			if (Succeeded)
			{
				return Ok(Message);
			}
			return BadRequest(Message);
		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.LogoutAsync();
			return Ok("Logout successful");
		}
	}
}
