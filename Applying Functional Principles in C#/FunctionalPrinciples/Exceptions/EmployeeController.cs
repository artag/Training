using System.ComponentModel.DataAnnotations;
using OperationResult;

namespace Exceptions
{
    public class EmployeeController : Controller
    {
        public ActionResult CreateEmployee(string name)
        {
            try
            {
                var result = ValidateName(name);
                if (result.IsFailure)
                {
                    return View("Error", result.Error);
                }

                // Rest of the method

                return View("Success");
            }
            catch (ValidationException ex)
            {
                return View("Error", ex.Message);
            }
        }

        private Result ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Name cannot be empty");

            if (name.Length > 100)
                return Result.Fail("Name is too long");

            return Result.Ok();
        }
    }
}
