﻿using AuthenticationPlugin;
using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UsersController(CinemaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
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


        [HttpPost]
        public IActionResult Login([FromBody] User user) {

            var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userEmail == null) {
                return NotFound();
            }
            //login unsuccessful
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password)) {
                return Unauthorized();
            }
            //when login successfull we will generate JWT Token
            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, userEmail.UserType)
                };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id
            });
        }
    }
}
