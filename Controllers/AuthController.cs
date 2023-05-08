//using FitnessTracker.Areas.Identity.Pages.Account;
//using FitnessTracker.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace FitnessTracker.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly SignInManager<FitnessUser> _signInManager;
//        private readonly UserManager<FitnessUser> _userManager;

//        public AuthController(IConfiguration configuration, SignInManager<FitnessUser> signInManager, UserManager<FitnessUser> userManager)
//        {
//            _configuration = configuration;
//            _signInManager = signInManager;
//            _userManager = userManager;
//        }

//        [AllowAnonymous]
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            // 'LoginModel' does not contain a definition for 'Email' and no accessible extension method 'Email' accepting a first argument of type 'LoginModel' could be found

//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user == null)
//            {
//                return BadRequest(new { message = "Username or password is incorrect" });
//            }
            
//            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            
//            if (!result.Succeeded)
//            {

//                return BadRequest(new { message = "Username or password is incorrect" });
//            }
            
//            var token = GenerateJwtToken(user);
            
//            return Ok(new
//            {
//                token = new JwtSecurityTokenHandler().WriteToken(token),
//                expiration = token.ValidTo
//            });
//        }


//        // other actions
//    }
//}
