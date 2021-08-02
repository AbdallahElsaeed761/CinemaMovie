using CinemaMovie.Models;
using CinemaMovie.Services;
using CinemaMovie.Viewsmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CinemaMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationDbContext db,UserManager<ApplicationUser> manager
            ,SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _manager = manager;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (EmailExist(model.Email))
                {
                    return BadRequest("this Email is Used");
                }
                if (UserExist(model.UserName))
                {
                    return BadRequest("this User is Used");
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                    

                };
                var result =await _manager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    var token = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action("RegisterConfirmation", "Account", new
                    { Id = user.Id, Token = HttpUtility.UrlEncode(token) }, Request.Scheme);

                    var txt = "Please confirm Register to our website";
                    var link = "<a href=\"" + confirmLink + "\">ConfirmRegistertion</a>";
                    var title = "RegisterConfirmation";
                    if (await SendGridApi.Execute(user.Email,user.UserName,txt,link,title))
                    {
                        // return Ok("Registertion Complete");
                        return Ok();
                    }
                    
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound();
           
        }

        private bool UserExist(string user)
        {
             return _db.Users.Any(z => z.UserName == user);
        }

        private bool EmailExist(string email)
        {
            return _db.Users.Any(x => x.Email == email);
        }
        [HttpGet]
        public  async Task<IActionResult> RegisterConfirmation(string id,string token)
        {
            if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            var user = await _manager.FindByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            var result = await _manager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            if (result.Succeeded)
            {
                return Ok("registertion success");
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model==null)
            {
                return NotFound();
            }
            var user = await _manager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }
            if (!user.EmailConfirmed)
            {
                return Unauthorized("you don’t confirmed Yet !!");
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,true);
            if (result.Succeeded)
            {
                return Ok("Login success");
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("User Account is Lockout");
            }
            //else
            //{
            //    return BadRequest(result);
            //}
            return NoContent();

        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }
        private async Task CreateAdmin()
        {
            var admin = await _manager.FindByNameAsync("Admin");
            if (admin==null)
            {
                var user = new ApplicationUser()
                {
                    Email= "abdullahalsaeed36@gmail.com",
                    UserName="Admin",
                    EmailConfirmed=true
                };
             var res=   await _manager.CreateAsync(user, "123*aA");
                if (res.Succeeded)
                {
                    await _manager.AddToRoleAsync(user, "Admin");
                }
            }
        }

    }
}
