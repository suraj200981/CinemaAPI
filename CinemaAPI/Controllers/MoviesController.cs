using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private CinemaDbContext _dbContext;



        //this enables the moviescontroller to interate with the dbcontext class in Data
        public MoviesController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: api/<MoviesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Movies);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var movieFound = _dbContext.Movies.Find(id);

            if (movieFound == null) {
                return NotFound("We could not find this movie...");
            }
            else {
                //will return the movie which matches this id only
                return Ok(movieFound);
            }
        }


        // POST api/<MoviesController>
        [HttpPost]
        public IActionResult Post([FromForm] Movie newMovie)
        {
            var uniqueNameForImage = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", uniqueNameForImage + ".jpg");

            if (newMovie.Image!=null) {

                var fileStream = new FileStream(filePath, FileMode.Create);
                newMovie.Image.CopyTo(fileStream);
            }

            newMovie.ImageUrl = filePath.Remove(0,7);
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

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
                currentMovie.Language = newMovie.Language;
                currentMovie.Rating = newMovie.Rating;

                _dbContext.SaveChanges();

                return Ok("Record successfully updated");
            }
        }

        // DELETE api/<MoviesController>/5
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
