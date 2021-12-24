using CinemaAPI.Data;
using CinemaAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public BookingsController(CinemaDbContext dbContext)
        {
            _dbContext  = dbContext;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get() {

            return Ok(_dbContext.Bookings);
        
        }


        [Authorize]
        [HttpPost]
        public IActionResult Post([FromForm] Booking booking) {

            //gets the exact date and time of booking so it does not need to be passed in the form
            booking.ReservationTime = DateTime.Now;

            _dbContext.Bookings.Add(booking);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
