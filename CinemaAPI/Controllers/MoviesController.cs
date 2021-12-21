using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IActionResult Post([FromBody] Movie newMovie)
        {
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie newMovie)
        {
            var currentMovie = _dbContext.Movies.Find(id);

            if (currentMovie == null)
            {
                return NotFound("We could not find this movie...");
            }
            else
            {

                currentMovie.Name = newMovie.Name;
                currentMovie.Language = newMovie.Language;

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
