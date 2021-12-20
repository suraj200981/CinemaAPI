using CinemaAPI.Data;
using CinemaAPI.Entities;
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
        public IEnumerable<Movie> Get()
        {
            return _dbContext.Movies;
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public Movie Get(int id)
        {

            //will return the movie which matches this id only
            return _dbContext.Movies.Find(id);
        }

        // POST api/<MoviesController>
        [HttpPost]
        public void Post([FromBody] Movie newMovie)
        {
            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Movie newMovie)
        {
            var currentMovie = _dbContext.Movies.Find(id);

            currentMovie.Name = newMovie.Name;
            currentMovie.Language = newMovie.Language;

            _dbContext.SaveChanges();



        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           var movieToRemove = _dbContext.Movies.Find(id);

            _dbContext.Movies.Remove(movieToRemove);

            _dbContext.SaveChanges();


        }
    }
}
