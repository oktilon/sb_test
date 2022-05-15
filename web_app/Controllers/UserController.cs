using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        public ActionResult<List<User>> Get()
        {
            var uid = HttpContext.Items["UserId"];
            if (uid == null)
            {
                return Unauthorized();
            }
            _logger.LogInformation("Acquired users list");
            return _usersService.GetUsers();
        }

        [HttpPost]
        public IActionResult Post(UserDTO newUser)
        {
            _logger.LogInformation("Add new user %s", newUser.Name);
            _usersService.addUser(newUser);

            return NoContent();
        }

        [HttpPost, Route("login")]
        public ActionResult<AuthUserDTO> Login([FromBody] AuthUser authUser)
        {
            var auth = _usersService.loginUser(authUser);
            if (auth == null)
            {
                return BadRequest("Invalid user or password");
            }
            _logger.LogInformation("Login of {0}", authUser.Username);
            return auth;
        }
    }
}
