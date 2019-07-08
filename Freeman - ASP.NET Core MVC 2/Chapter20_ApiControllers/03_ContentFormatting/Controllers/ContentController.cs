using ContentFormatting.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContentFormatting.Controllers
{
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        [HttpGet("string")]
        public string GetString() => "This is a string response";

        [HttpGet("object")]
        public Reservation GetObject() => new Reservation
        {
            ReservationId = 100,
            ClientName = "Joe",
            Location = "Board Room"
        };

        [HttpGet("jsonobject")]
        [Produces("application/json")]
        public Reservation GetJsonObject() => new Reservation
        {
            ReservationId = 100,
            ClientName = "Joe",
            Location = "Board Room"
        };

        [HttpGet("routeobject/{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
        public Reservation GetRouteObject() => new Reservation
        {
            ReservationId = 100,
            ClientName = "Joe",
            Location = "Board Room"
        };

        [HttpPost]
        [Consumes("application/json")]
        public Reservation ReceiveJson([FromBody] Reservation reservation)
        {
            reservation.ClientName = "Json";
            return reservation;
        }

        [HttpPost]
        [Consumes("application/xml")]
        public Reservation ReceiveXml([FromBody] Reservation reservation)
        {
            reservation.ClientName = "Xml";
            return reservation;
        }
    }
}
