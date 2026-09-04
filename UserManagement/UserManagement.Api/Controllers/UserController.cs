using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Interfaces;
using UserManagement.Domain;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users is null)
            { 
                return NotFound("No users exist in the system");
            }
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0)
            { 
                return BadRequest("User Id not received"); 
            }

            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest("User information missing in the request boxy.");
            }

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest("User Name is mandatory, it is missing in the request boxy.");
            }

            try
            {
                var existingUser = await _userService.GetUserByName(user.Name);

                if (existingUser is not null)
                {
                    return Ok("User already exists in the system");
                }
                else
                {
                    var addedUser = await _userService.AddUser(user);
                    if (addedUser is null && addedUser.Id <= 0)
                    {
                        return StatusCode(500, "Error adding user, Contact support for details");
                    }
                    return Ok(addedUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserById([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest("User information missing in the request body.");
            }
            if (user.Id <= 0)
            {
                return BadRequest("Invalid User Id");
            }

            User updatedUser = new User();
            try
            {
                updatedUser = await _userService.UpdateUserById(user);
            }
            catch (Exception)
            {
                throw;
            }

            if (updatedUser is null)
            {
                return StatusCode(500, "Error adding user, Contact support for details");
            }
            else
            {
                return Ok("User Updated Successfully");
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("User Id missing.");
            
            }

            bool successful;
            try
            {
                successful = await _userService.RemoveUserById(id);
            }
            catch (Exception)
            {
                throw;
            }

            if (successful)
            {
                return Ok("Successfully deleted the user.");
            }
            else
            {
                return StatusCode(500, "Error deleting the user. Contact support for details");
            }
        }

    }
}
