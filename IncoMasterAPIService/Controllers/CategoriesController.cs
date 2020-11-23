using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IncoMasterAPIService.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IncoMasterAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CategoriesService _categoriesService;

        public CategoriesController(UserService userService, CategoriesService categoriesService)
        {
            _userService = userService;
            _categoriesService = categoriesService;
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoriesModel category)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _categoriesService.CreateAsync(category);
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CategoriesModel categoryToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = await _categoriesService.GetByIdAsync(id);
            if (category == null) return NotFound();

            await _categoriesService.UpdateAsync(categoryToUpdate);

            return NoContent();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoriesService.GetByIdAsync(id);
            
            if (category == null)
                return NotFound();

            await _categoriesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
