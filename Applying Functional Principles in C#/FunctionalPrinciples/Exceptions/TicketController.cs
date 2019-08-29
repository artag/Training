using System;
using OperationResult;

namespace Exceptions
{
    public class TicketController : Controller
    {
        private readonly TicketRepository _repository;
        private readonly TheaterGateway _gateway;

        public TicketController(TicketRepository repository, TheaterGateway gateway)
        {
            _repository = repository;
            _gateway = gateway;
        }

        public ActionResult BuyTicket(DateTime date, string customerName)
        {
            Result validationResult = Validate(date, customerName);
            if (validationResult.IsFailure)
                return View("Error", validationResult.Error);

            Result reserveResult = _gateway.Reserve(date, customerName);
            if (reserveResult.IsFailure)
                return View("Error", reserveResult.Error);

            var ticket = new Ticket(date, customerName);
            _repository.Save(ticket);

            return View("Success");
        }

        private Result Validate(in DateTime date, string customerName)
        {
            if (date.Date < DateTime.Now.Date)
                return Result.Fail("Cannot reserve on a past date");

            if (string.IsNullOrWhiteSpace(customerName) || customerName.Length > 200)
                return Result.Fail("Incorrect customer name");

            return Result.Ok();
        }
    }

    // Wrapper on 3rd-party library
    public class TheaterGateway
    {
        public Result Reserve(DateTime date, string customerName)
        {
            try
            {
                var client = new TheaterApiClient();
                client.Reserve(date, customerName);

                return Result.Ok();
            }
            catch (HttpRequestException ex)
            {
                return Result.Fail("Unable to connect to the theater. Please try again later.");
            }
            catch (InvalidOperationException ex)
            {
                return Result.Fail("Sorry, tickets on this date are no longer available.");
            }
        }
    }
}
