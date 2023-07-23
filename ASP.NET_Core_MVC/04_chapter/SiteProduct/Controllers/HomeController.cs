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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
            return View();

        var newId = _products.Add(product);
        return RedirectToAction(nameof(Details), new { id = newId });
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _products.Get(id);
        if (product.Id < 1)
            return RedirectToAction("Index");
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product model)
    {
        var product = _products.Get(model.Id);
        if (product.Id < 1 || !ModelState.IsValid)
            return View(model);

        var changedProduct = product.WithProduct(model);
        _products.Save(changedProduct);
        return RedirectToAction("Details", new { id = product.Id });
    }
}
