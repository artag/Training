using System.ComponentModel.DataAnnotations;

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
            if (!ModelState.IsValid)
                return View("Error");

            var customer = new Customer(customerModel.Name, customerModel.Email);
            _database.Save(customer);

            return RedirectToAction("Index");
        }
    }

    public class CustomerModel
    {
        // Проблема. Дублирование проверок в CustomerModel и Customer классах.

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Invalid e-mail address")]
        public string Email { get; set; }
    }
}
