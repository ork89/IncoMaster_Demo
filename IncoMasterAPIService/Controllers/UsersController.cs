using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using HelperClasses;
using IncoMasterAPIService.Interfaces;
using IncoMasterAPIService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace IncoMasterAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IAuthenticationService _authService;
        readonly SecureStringConverter _converter;

        public UsersController(UserService service, IAuthenticationService authService)
        {
            _userService = service;
            _authService = authService;
        }

        //TODO: add [Authorize] to the rest of the methods.

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel user)
        {
            var token = await _authService.JwtAuthenticate(user.Email, _converter.ConvertToSecureString(user.Password));

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string email, SecureString password)
        {
            var accountExists = _userService.AuthenticateUser(email, password);
            var user = new UserModel();
            if (accountExists)
                user = _userService.GetAllAsync().Result.SingleOrDefault(e => e.Email == email && e.Password == _converter.ConvertToString(password));

            if (user != null)
                return NotFound();
            var userId = int.Parse(HttpContext.User.Claims.SingleOrDefault(x => x.Type == "Email").Value.ToString());
            return Ok(userId);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>>LogIn(string email, string password)
        {
            var user = await _userService.LoginUserAsync(email, password);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.CreateAsync(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, UserModel updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedUser = await _userService.GetByIdAsync(id);
            if (queriedUser == null)
            {
                return NotFound();
            }

            await _userService.UpdateAsync(id, updatedUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
