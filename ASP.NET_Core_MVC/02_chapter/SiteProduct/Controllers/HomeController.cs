using Microsoft.AspNetCore.Mvc;
using SiteProduct.Services;

namespace SiteProduct.Controllers;

public class HomeController : Controller
{
    private readonly IProductData _products;

    public HomeController(IProductData products)
    {
        _products = products;
    }

    public ViewResult Index()
    {
        var model = _products.GetAll();
        return View(model);
    }
}
