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
        private readonly CategoriesService _categoriesService;
        private readonly IAuthenticationService _authService;
        readonly SecureStringConverter _converter;

        public UsersController(UserService service, CategoriesService categoriesService, IAuthenticationService authService)
        {
            _userService = service;
            _categoriesService = categoriesService;
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
                return NotFound();

            InitUsersCategoriesLists(user);

            return Ok(user);
        }

        private async void InitUsersCategoriesLists(UserModel user)
        {
            var tempList = new List<CategoriesModel>();

            if (user.Income.Count > 0)
            {
                foreach (var incomeId in user.Income)
                {
                    var income = await _categoriesService.GetByIdAsync(incomeId);
                    if (income != null)
                        tempList.Add(income);
                }

                user.IncomeList = tempList;
                tempList.Clear();
            }

            if(user.Expenses.Count > 0)
            {
                foreach (var expensId in user.Expenses)
                {
                    var expense = await _categoriesService.GetByIdAsync(expensId);
                    if (expense != null)
                        tempList.Add(expense);
                }

                user.ExpensesList = tempList;
                tempList.Clear();
            }

            if (user.Savings.Count > 0)
            {
                foreach (var savingsId in user.Savings)
                {
                    var savings = await _categoriesService.GetByIdAsync(savingsId);
                    if (savings != null)
                        tempList.Add(savings);
                }

                user.SavingsList = tempList;
                tempList.Clear();
            }

            if (user.Loans.Count > 0)
            {
                foreach (var loanId in user.Loans)
                {
                    var loan = await _categoriesService.GetByIdAsync(loanId);
                    if (loan != null)
                        tempList.Add(loan);
                }

                user.LoansList = tempList;
                tempList.Clear();
            }
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
                return BadRequest();

            var queriedUser = await _userService.GetByIdAsync(id);
            
            if (queriedUser == null)            
                return NotFound();

            await _userService.UpdateAsync(id, updatedUser);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            
            if (user == null)
                return NotFound();
            
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> InsertCategory(string categoryId, string categoryType, string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.InsertCategoryAsync(categoryId, categoryType, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string categoryId, string categoryType, string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            
            if (user == null)
                return NotFound();

            var result = await _userService.DeleteCategoryAsync(categoryId, categoryType, userId);
            
            if(result == false)
                return NoContent();

            return Ok(result);
        }
    }
}
