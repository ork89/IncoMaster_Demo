using System.Collections.Generic;
using System.Threading.Tasks;
using IncoMasterAPIService.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace IncoMasterAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService service)
        {
            _userService = service;
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

        [HttpDelete]
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
