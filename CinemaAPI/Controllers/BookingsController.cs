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


            var bookingData = from booking in _dbContext.Bookings
                              join user in _dbContext.Users on booking.UserId equals user.Id
                              join movie in _dbContext.Movies on booking.MovieId equals movie.Id
                              select new
                              {
                                  Id = booking.Id,
                                  BookingTime = booking.ReservationTime,
                                  CustomerName = user.Id,
                                  MovieName = movie.Name
                              };

            return Ok(bookingData);
        
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var bookingData = from booking in _dbContext.Bookings
                              join user in _dbContext.Users on booking.UserId equals user.Id
                              join movie in _dbContext.Movies on booking.MovieId equals movie.Id
                              where booking.Id == id
                              select new
                              {
                                  Id = booking.Id,
                                  BookingTime = booking.ReservationTime,
                                  CustomerName = user.Id,
                                  MovieName = movie.Name,
                                  Email = user.Email,
                                  Qty = booking.Qty,
                                  Price = booking.Price,
                                  Phone = booking.Phone,
                                  PlayingDate = movie.PlayingDate,
                                  PlayingTime = movie.PlayingTime

                              };

            return Ok(bookingData);

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


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bookingToRemove = _dbContext.Bookings.Find(id);


            if (bookingToRemove == null)
            {
                return NotFound("We could not find this reservation...");
            }
            else
            {

                _dbContext.Bookings.Remove(bookingToRemove);

                _dbContext.SaveChanges();

                return Ok("Reservation has been successfully deleted");
            }
        }
    }
}
