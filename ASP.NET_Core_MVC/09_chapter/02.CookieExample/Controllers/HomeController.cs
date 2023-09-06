using Microsoft.AspNetCore.Mvc;

namespace CookieExample.Controllers;

public class HomeController : Controller
{
    private const string CookiesValue = "CookiesValue";
    private const string CookieOptionsDomain = "CookieOptionsDomain";
    private const string CookieOptionsPath = "CookieOptionsPath";
    private const string CookieOptionsExpires = "CookieOptionsExpires";
    private const string CookieOptionsHttpOnly = "CookieOptionsHttpOnly";
    private const string CookieOptionsSecure = "CookieOptionsSecure";
    private const string CookieOptionsMaxAge = "CookieOptionsMaxAge";
    private const string CookieOptionsIsEssential = "CookieOptionsIsEssential";
    private const string CookieOptionsSameSite = "CookieOptionsSameSite";

    public IActionResult Index()
    {
        // Чтение cookie.
        var cookie = Get(key: CookiesValue);
        ViewBag.CookiesValue = cookie;

        // Чтение дополнительных параметров cookie.
        ViewBag.OptionsDomain = Get(CookieOptionsDomain);
        ViewBag.OptionsPath = Get(CookieOptionsPath);
        ViewBag.OptionsExpires = Get(CookieOptionsExpires);
        ViewBag.OptionsHttpOnly = Get(CookieOptionsHttpOnly);
        ViewBag.OptionsSecure = Get(CookieOptionsSecure);
        ViewBag.OptionsMaxAge = Get(CookieOptionsMaxAge);
        ViewBag.OptionsIsEssential = Get(CookieOptionsIsEssential);
        ViewBag.OptionsSameSite = Get(CookieOptionsSameSite);

        return View();
    }

    public IActionResult Delete()
    {
        // Удаление cookie.
        Remove(key: CookiesValue);

        // Удаление дополнительных параметров cookie.
        Remove(CookieOptionsDomain);
        Remove(CookieOptionsPath);
        Remove(CookieOptionsExpires);
        Remove(CookieOptionsHttpOnly);
        Remove(CookieOptionsSecure);
        Remove(CookieOptionsMaxAge);
        Remove(CookieOptionsIsEssential);
        Remove(CookieOptionsSameSite);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        // Установка cookie.
        Set(key: CookiesValue, value: "Bill Gates", expireTime: 10);
        return RedirectToAction(nameof(Index));
    }

    private string Get(string key)
    {
        return Request.Cookies[key] ?? string.Empty;
    }

    private void Set(string key, string value, int? expireTime)
    {
        var options = new CookieOptions();
        options.Expires = expireTime.HasValue
            ? DateTime.Now.AddMinutes(expireTime.Value)
            : DateTime.Now.AddMilliseconds(10);
        Response.Cookies.Append(key, value, options);

        // Дополнительно, опции в отдельных cookie.
        Response.Cookies.Append(CookieOptionsDomain, options.Domain ?? string.Empty, options);
        Response.Cookies.Append(CookieOptionsPath, options.Path ?? string.Empty, options);
        Response.Cookies.Append(CookieOptionsExpires, options.Expires.ToString() ?? string.Empty, options);
        Response.Cookies.Append(CookieOptionsHttpOnly, options.HttpOnly.ToString(), options);
        Response.Cookies.Append(CookieOptionsSecure, options.Secure.ToString(), options);
        Response.Cookies.Append(CookieOptionsMaxAge, options.MaxAge.ToString() ?? string.Empty, options);
        Response.Cookies.Append(CookieOptionsIsEssential, options.IsEssential.ToString(), options);
        Response.Cookies.Append(CookieOptionsSameSite, options.SameSite.ToString(), options);
    }

    private void Remove(string key)
    {
        Response.Cookies.Delete(key);
    }
}
