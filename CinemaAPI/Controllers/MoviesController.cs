using CinemaAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private static List<Movie> movies = new List<Movie>
        {
            new Movie(){ Id = 0, Name= "Mission Impossible 7", Language="Spanish"},
             new Movie(){ Id = 1, Name= "James Bond", Language="English"}
        };

        [HttpGet]
        public IEnumerable<Movie> Get() {

            return movies;
        }


        [HttpPost]
        public void Post([FromBody]Movie newMovie) {

            movies.Add(newMovie);

        }
      
    }
}
