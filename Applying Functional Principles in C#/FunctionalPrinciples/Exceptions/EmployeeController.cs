using System.ComponentModel.DataAnnotations;

namespace Exceptions
{
    public class EmployeeController : Controller
    {
        public ActionResult CreateEmployee(string name)
        {
            try
            {
                string error = ValidateName(name);
                if (error != string.Empty)
                {
                    return View("Error", error);
                }

                // Rest of the method

                return View("Success");
            }
            catch (ValidationException ex)
            {
                return View("Error", ex.Message);
            }
        }

        private string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name cannot be empty";

            if (name.Length > 100)
                return "Name is too long";

            return string.Empty;
        }
    }
}
