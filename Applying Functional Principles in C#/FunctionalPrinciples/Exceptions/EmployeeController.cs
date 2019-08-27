using System.ComponentModel.DataAnnotations;

namespace Exceptions
{
    public class EmployeeController : Controller
    {
        public ActionResult CreateEmployee(string name)
        {
            try
            {
                ValidateName(name);
                // Rest of the method

                return View("Success");
            }
            catch (ValidationException ex)
            {
                return View("Error", ex.Message);
            }
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Name cannot be empty");

            if (name.Length > 100)
                throw new ValidationException("Name is too long");
        }
    }
}
