using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: Role/LoginAndRetrieveUserRoles
        [HttpPost]
        [Route("Role/LoginAndRetrieveUserRoles")]
        public async Task<IActionResult> LoginAndRetrieveUserRoles([FromBody] UserModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles[0]);
        }

        [HttpPost]
        [Route("Role/RegisterAndSetMemberRole")]
        public async Task<IActionResult> RegisterAndSetUserRole([FromBody] UserModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return Ok("Member");
        }

        public class UserModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
