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
                // Валидация ввода.
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

        public ActionResult UpdateEmployee(int employeeId, string name)
        {
            // Валидация ввода.
            var result = ValidateName(name);
            if (result.IsFailure)
            {
                return View("Error", result.Error);
            }

            // Все нормально, запуск рабочего кода.
            Employee employee = GetEmployee(employeeId);
            employee.UpdateName(name);

            return View("Success");
        }

        private Result ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Name cannot be empty");

            if (name.Length > 100)
                return Result.Fail("Name is too long");

            return Result.Ok();
        }

        // Mock method
        private Employee GetEmployee(int employeeId)
        {
            return null;
        }
    }
}
