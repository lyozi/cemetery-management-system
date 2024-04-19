using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [ApiController]
    [Authorize] // Áthelyezzük az Authorize attribútumot a Controller szintjére
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("userrole")]
        public async Task<IActionResult> GetUserRole()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("pinguser")]
        [AllowAnonymous]
        public IActionResult PingUser()
        {
            string role;
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            if (email == "Unknown")
            {
                role = "Unauthorized";
            }
            else
            {
                role = HttpContext.User.FindFirstValue(ClaimTypes.Role) ?? "Member";
            }
            return Json(new { Email = email, Role = role });
        }
    }
}
