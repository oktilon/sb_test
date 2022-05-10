using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using web_app.Models;
using web_app.Services;

namespace web_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UsersService _usersService;

        public UserController(ILogger<UserController> logger, UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            _logger.LogInformation("Acquired users list");
            return await _usersService.GetAsync();
        }

        [HttpPost]
        public IActionResult Post(UserDTO newUser)
        {
            _logger.LogInformation("Add new user %s", newUser.Name);
            _usersService.addUser(newUser);

            return NoContent();
        }

        [HttpPost, Route("login")]
        public IActionResult Login(AuthUser authUser)
        {
            if (!_usersService.loginUser(authUser))
            {
                return NotFound();
            }
            _logger.LogInformation("Login of {0}", authUser.UserName);
            return NoContent();
        }
    }
}
