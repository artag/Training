using Authorization.Filters;
using Authorization.Models;
using Authorization.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers;

public class HomeController : Controller
{
    private readonly IUsersPortalRepository _usersRepository;

    public HomeController(IUsersPortalRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    // Представление. Содержит информацию для авторизованных пользователей.
    [AuthorizationFilter]
    public IActionResult Index()
    {
        ViewData["Title"] = "Домашняя";
        return View();
    }

    // Представление входа в систему.
    public IActionResult Login()
    {
        ViewData["Title"] = "Вход";
        return View();
    }

    // Удаление переменной сессии.
    public IActionResult Logout()
    {
        HttpContext.Session.Remove(Constants.UserId);
        DeleteCookie();
        return RedirectToAction(nameof(Index));
    }

    // ValidateAntiForgeryToken - встроенный фильтр.
    // Используется для предотвращения атак с подделкой межсайтовых запросов.
    // Производит проверку токенов при обращении в методу действия.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginUser login)
    {
        if (!ModelState.IsValid)
            return View(nameof(Login), login);

        try
        {
            var users = _usersRepository.UsersPortal;
            var knownUser = users.FirstOrDefault(
                x => x.UserName == login.User
                && x.Password == login.Password);

            if (knownUser == null)
            {
                ModelState.AddModelError(
                    key: string.Empty,
                    errorMessage: "Пользователь не существует. Вход невозможен!");
                return View("Login", login);
            }

            var userId = knownUser.Id.ToString();
            HttpContext.Session.SetString(Constants.UserId, userId);
            SaveCookie(userId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                key: string.Empty,
                errorMessage: ex.Message);
            return View(nameof(Login), login);
        }
    }

    private void SaveCookie(string userId)
    {
        if (Request.Cookies.ContainsKey(Constants.UserId))
            return;

        try
        {
            var options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddHours(1);
            Response.Cookies.Append(Constants.UserId, userId, options);
        }
        catch (Exception ex)
        {
            // В production так не делать!
            Console.WriteLine(
                "Error on cookies '{0}' creating.\n{1}",
                Constants.UserId, ex.Message);
        }
    }

    private void DeleteCookie()
    {
        try
        {
            Response.Cookies.Delete(Constants.UserId);
        }
        catch (Exception ex)
        {
            // В production так не делать!
            Console.WriteLine(
                "Error on cookies '{0}' deleting.\n{1}",
                 Constants.UserId, ex.Message);
        }
    }
}