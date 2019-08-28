using System;
using System.ComponentModel.DataAnnotations;
using System.Security;

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
            // Проблемы.
            // 1. Ловим все исключения, без разбору. Обработка одна на все исключения.
            // 2. Ловятся исключения от нескольких методов. Будет сложно определить источник
            // исключения. 
            try
            {
                Validate(date, customerName);
                _gateway.Reserve(date, customerName);
                var ticket = new Ticket(date, customerName);
                _repository.Save(ticket);

                return View("Success");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        private void Validate(in DateTime date, string customerName)
        {
            // Проблема. Использование исключения для контроля валидации.
            // (не отражено в сигнатуре метода, может привести к пробросу исключения
            // на несколько уровней вверх по стеку).
            if (date.Date < DateTime.Now.Date)
                throw new ValidationException("Cannot reserve on a past date");

            // Проблема. Использование исключения для контроля валидации.
            // (не отражено в сигнатуре метода, может привести к пробросу исключения
            // на несколько уровней вверх по стеку).
            if (string.IsNullOrWhiteSpace(customerName) || customerName.Length > 200)
                throw new VerificationException("Incorrect customer name");
        }
    }

    // Wrapper on 3rd-party library
    public class TheaterGateway
    {
        public void Reserve(DateTime date, string customerName)
        {
            var client = new TheaterApiClient();

            // Проблема. Документация говорит, что метод может выкинуть два исключения:
            // 1. HttpRequestException - if unable to connect to the API.
            // 2. InvalidOperationException - if no ticket are available.
            client.Reserve(date, customerName);
        }
    }
}
