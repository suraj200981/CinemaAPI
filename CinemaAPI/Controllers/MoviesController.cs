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
        //?pageNumber=1&pageSize=2 for each page to have 2 movies displaying
        //api/movies/?sort=asc or api/movies/?sort=desc for sorting if
        [Authorize]
        [HttpGet]
        public IActionResult Get(string sort, int pageNumber, int pageSize)
        {

            if (string.IsNullOrEmpty(sort))
            {
                return Ok(_dbContext.Movies.Skip((pageNumber-1) * pageSize).Take(pageSize));

            }
            else if (sort.Equals("desc"))
            {
                return Ok(_dbContext.Movies.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(m => m.Rating));

            }
            else if (sort.Equals("asc"))
            {
                return Ok(_dbContext.Movies.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(m => m.Rating));

            }

            return Ok(_dbContext.Movies);

        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var movieFound = _dbContext.Movies.Find(id);

            if (movieFound == null)
            {
                return NotFound("We could not find this movie...");
            }
            else
            {
                //will return the movie which matches this id only
                return Ok(movieFound);
            }
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

        [Authorize(Roles = "Admin")]
        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie newMovie)
        {
            var currentMovie = _dbContext.Movies.Find(id);

            if (currentMovie == null)
            {
                return NotFound("We could not find this movie...");
            }
            else
            {
                var uniqueNameForImage = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", uniqueNameForImage + ".jpg");
                if (newMovie.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    newMovie.Image.CopyTo(fileStream);
                    currentMovie.ImageUrl = filePath.Remove(0, 7);

                }
                currentMovie.Name = newMovie.Name;
                currentMovie.Description = newMovie.Description;
                currentMovie.Language = newMovie.Language;
                currentMovie.Duration = newMovie.Duration;
                currentMovie.PlayingDate = newMovie.PlayingDate;
                currentMovie.PlayingTime = newMovie.PlayingTime;
                currentMovie.Rating = newMovie.Rating;
                currentMovie.Genre = newMovie.Genre;
                currentMovie.TrailerUrl = newMovie.TrailerUrl;
                currentMovie.TicketPrice = newMovie.TicketPrice;

                _dbContext.SaveChanges();

                return Ok("Record successfully updated");
            }
        }

        // DELETE api/<MoviesController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movieToRemove = _dbContext.Movies.Find(id);


            if (movieToRemove == null)
            {
                return NotFound("We could not find this movie...");
            }
            else
            {

                _dbContext.Movies.Remove(movieToRemove);

                _dbContext.SaveChanges();

                return Ok("Movie has been successfully deleted");
            }
        }

    }
}
