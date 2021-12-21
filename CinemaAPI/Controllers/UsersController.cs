using AuthenticationPlugin;
using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDbContext _dbContext;
        public UsersController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user) {

           var userWithSameEmail= _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if (userWithSameEmail!=null) {
                return BadRequest("User with same email address already exists");
            }

            var regularUserReg = new User
            {
                Name = user.Name,
                Email = user.Email,
                //Secure password haser helper hashes the password in the sql DB
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                UserType = "Regular"

            };

            _dbContext.Users.Add(regularUserReg);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
