using System.ComponentModel.DataAnnotations;
using OperationResult;

namespace PrimitiveObsession
{
    public class CustomerController : Controller
    {
        private readonly IDatabase _database;

        public CustomerController(IDatabase database)
        {
            _database = database;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCustomer(CustomerModel customerModel)
        {
            // Конвертация (и валидация) strings в ValueObjects. 
            Result<CustomerName> customerNameResult = CustomerName.Create(customerModel.Name);
            Result<Email> emailResult = Email.Create(customerModel.Email);

            // Информирование пользователя о возможных ошибках в конвертации (и валидации).
            if (customerNameResult.IsFailure)
                ModelState.AddModelError("Name", customerNameResult.Error);
            if (emailResult.IsFailure)
                ModelState.AddModelError("Email", emailResult.Error);

            if (!ModelState.IsValid)
                return View("Error");

            var customer = new Customer(customerNameResult.Value, emailResult.Value);
            _database.Save(customer);

            return RedirectToAction("Index");
        }
    }

    public class CustomerModel
    {
        // Все проверки сожержатся в классах CustomerName и Email.
        // Проверки отсюда можно удалить.

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
