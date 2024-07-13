using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public IdentityController(SignInManager<User> signInManager, UserManager<User> userManager, JwtTokenService jwtTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(User user)
        {
            string message = "";
            IdentityResult result = new();

            try
            {
                User user_ = new User()
                {
                    Name = user.UserName,
                    Email = user.Email,
                    UserName = user.UserName,
                };
                result = await _userManager.CreateAsync(user_, user.PasswordHash);

                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, please try again. " + ex.Message);
            }
            return Ok(new { message = "Registered successfully", result = result });
        }

        //[HttpPost("login")]
        //public async Task<ActionResult> LoginUser(Login login)
        //{
        //    string message = "";

        //    try
        //    {
        //        User user_ = await _userManager.FindByEmailAsync(login.Email);
        //        if (user_ != null)
        //        {
        //            login.Username = user_.UserName;

        //            if (!user_.EmailConfirmed)
        //            {
        //                user_.EmailConfirmed = true;
        //            }

        //            var result = await _signInManager.PasswordSignInAsync(user_, login.Password, login.Remember, false);

        //            if (!result.Succeeded)
        //            {
        //                return Unauthorized(new { message = "Check your login credentials and try again" });
        //            }

        //            user_.LastLogin = DateTime.Now;
        //            var updateResult = await _userManager.UpdateAsync(user_);

        //            var token = _jwtTokenService.GenerateToken(user_);
        //            return Ok(new { message = "Login successful", token });
        //        }
        //        else
        //        {
        //            return BadRequest(new { message = "Please check your credentials and try again" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Something went wrong, please try again. " + ex.Message });
        //    }
        //}

        [HttpPost("LoginUser")]
        public async Task<ActionResult> LoginUser(Login login)
        {
            try
            {
                User user_ = await _userManager.FindByEmailAsync(login.Email);
                if (user_ != null)
                {
                    login.Username = user_.UserName;

                    if (!user_.EmailConfirmed)
                    {
                        user_.EmailConfirmed = true;
                    }

                    var result = await _signInManager.PasswordSignInAsync(user_, login.Password, login.Remember, false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized(new { message = "Check your login credentials and try again" });
                    }

                    user_.LastLogin = DateTime.Now;
                    await _userManager.UpdateAsync(user_);

                    var token = _jwtTokenService.GenerateToken(user_);
                    return Ok(new { message = "Login successful", token });
                }
                else
                {
                    return BadRequest(new { message = "Please check your credentials and try again" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong, please try again. " + ex.Message });
            }
        }

        [HttpGet("logout"), Authorize]
        public async Task<ActionResult> LogoutUser()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong, please try again, " + ex.Message });
            }
            return Ok(new { message = "You are free to go" });
        }

        [HttpGet("home/{email}"), Authorize]
        public async Task<ActionResult> HomePage(string email)

        {
            User userInfo = await _userManager.FindByEmailAsync(email);
            if (userInfo == null)
            {
                return BadRequest(new { message = "Something went wrong, please try again" });
            }
            return Ok(new { userInfo = userInfo });
        }

        [HttpGet("xhtlekd"), Authorize]
        public async Task<ActionResult> CheckUser()
        {
            try
            {
                var user_ = HttpContext.User;
                var userName = user_.Identity?.Name;
                if (userName == null)
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(new { message = "Logged in", user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong, please try again. " + ex.Message });
            }
        }
    }
}