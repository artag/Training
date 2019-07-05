using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestAndApiControllers.Models;

namespace RestAndApiControllers.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IRepository _repository;

        public ReservationController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Reservation> Get() => _repository.Reservations;

        [HttpGet("{id}")]
        public Reservation Get(int id) => _repository[id];

        [HttpPost]
        public Reservation Post([FromBody] Reservation reservation) =>
            _repository.AddReservation(new Reservation
            {
                ClientName = reservation.ClientName,
                Location = reservation.Location
            });

        [HttpPut]
        public Reservation Put([FromBody] Reservation reservation) =>
            _repository.UpdateReservation(reservation);

        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody] JsonPatchDocument<Reservation> patch)
        {
            var reservation = Get(id);
            if (reservation == null)
            {
                return NotFound();
            }

            patch.ApplyTo(reservation);
            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => _repository.DeleteReservation(id);
    }
}
