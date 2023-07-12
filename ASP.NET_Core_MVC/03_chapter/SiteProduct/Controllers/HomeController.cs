using Microsoft.AspNetCore.Mvc;
using SiteProduct.Models;
using SiteProduct.Services;
using SiteProduct.ViewModels;

namespace SiteProduct.Controllers;

public class HomeController : Controller
{
    private readonly IProductData _products;
    private readonly IProductTypeData _categories;

    public HomeController(IProductData products, IProductTypeData categories)
    {
        _products = products;
        _categories = categories;
    }

    public IActionResult Index()
    {
        var allCategories = _categories
            .GetAll()
            .ToArray();

        var model = _products
            .GetAll()
            .Select(product => product.MapToViewModel(allCategories));

        return View(model);
    }

    public IActionResult Details(int id)
    {
        var product = _products.Get(id);
        if (product.Id < 1)
            return RedirectToAction(nameof(Index));

        var allCategories = _categories
            .GetAll()
            .ToArray();

        var vm = product.MapToViewModel(allCategories);
        return View(vm);
    }
}