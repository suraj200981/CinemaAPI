using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public double Rating { get; set; }
        /*
         * It is bad practice to store image in database, which is why
         * we are going to store the images in wwwroot folder and only store the paths in the
         * database. It is good practice to do it this way. 
         */
        [NotMapped] //this data anontation means that this property will not be apart of the movie table
        public IFormFile Image { get; set; } //IFromFile is not a data type which can be recognised by the database
        public string ImageUrl { get; set; }


    }
}
