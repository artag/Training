using Microsoft.AspNetCore.Mvc;

namespace SiteProduct.Controllers;

public class ProductController : Controller
{
    public ContentResult Name()
    {
        return Content("Шилдт Г. C# 4.0: Полное руководство.");
    }
}
