using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public MoviesController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET all movies
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Movies);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromForm] Movie newMovie)
        {
            var uniqueNameForImage = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", uniqueNameForImage + ".jpg");

            if (newMovie.Image != null)
            {

                var fileStream = new FileStream(filePath, FileMode.Create);
                newMovie.Image.CopyTo(fileStream);
            }

            newMovie.ImageUrl = filePath.Remove(0, 7);
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
