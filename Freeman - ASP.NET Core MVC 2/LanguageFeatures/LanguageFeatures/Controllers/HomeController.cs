using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageFeatures.Models;
using Microsoft.AspNetCore.Mvc;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(new string[] { "C#", "Language", "Features" });
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var results = new List<string>();
            foreach (var product in Product.GetProducts())
            {
                var name = product?.Name ?? "<No Name>";
                var category = product?.Category ?? "<Unknown>";
                var price = product?.Price ?? 0;
                var relatedName = product?.Related?.Name ?? "<None>";
                var stock = product?.InStock ?? false;

                results.Add(
                    $"Name: {name}\t\t" +
                    $"Category: {category}\t\t" +
                    $"Price: {price}\t\t" +
                    $"Related: {relatedName}\t\t" +
                    $"In Stock: {stock}");
            }

            ViewBag.Title = "Products";

            return View("NextPage", results);
        }

        [HttpGet]
        public IActionResult GetNames()
        {
            var names = new [] { "Bob", "Joe", "Alice" };
            ViewBag.Title = "Names";

            return View("NextPage", names);
        }

        [HttpGet]
        public IActionResult GetProductsAsDictionaryKeys()
        {
            var products = new Dictionary<string, Product>
            {
                ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
                ["Lifejacket"] = new Product { Name = "Lifejacket", Price = 48.95M }
            };

            ViewBag.Title = "Product Names";

            return View("NextPage", products.Keys);
        }

        [HttpGet]
        public IActionResult CalculateDecimalNumbers()
        {
            var data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };

            var total = 0M;
            var strings = new List<string>();

            strings.Add("Numbers:");
            foreach (var obj in data)
            {
                if (obj is decimal decimalValue)
                {
                    total += decimalValue;
                    strings.Add($"{decimalValue:C2}");
                }
            }

            strings.Add(string.Empty);
            strings.Add("Total:");
            strings.Add($"{total:C2}");

            ViewBag.Title = "Calculate Decimal Numbers";

            return View("NextPage", strings.ToArray());
        }

        [HttpGet]
        public IActionResult CalculateDecimalNumbers2()
        {
            var data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };

            var total = 0M;
            var strings = new List<string>();

            strings.Add("Numbers:");
            foreach (var obj in data)
            {
                switch (obj)
                {
                    case decimal decimalValue:
                        total += decimalValue;
                        strings.Add($"{decimalValue:C2}");
                        break;
                    case int intValue when intValue > 50:
                        total += intValue;
                        strings.Add($"{intValue:C2}");
                        break;
                }
            }

            strings.Add(string.Empty);
            strings.Add("Total:");
            strings.Add($"{total:C2}");

            ViewBag.Title = "Calculate Decimal Numbers";

            return View("NextPage", strings.ToArray());
        }

        public IActionResult TestExtensionMethods()
        {
            var cart = new ShoppingCart { Products = Product.GetProducts() };

            var productArray = new []
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M }
            };

            var cartTotal = cart.TotalPrices();
            var arrayTotal = productArray.TotalPrices();

            ViewBag.Title = "Extension Methods";

            var message = new[]
            {
                $"Cart Total: {cartTotal:C2}",
                $"Array Total: {arrayTotal:C2}",
            };

            return View("NextPage", message);
        }

        public IActionResult EnumerableFilter1()
        {
            var products = new[]
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M },
                new Product { Name = "Soccer ball", Price = 19.50M },
                new Product { Name = "Corner flag", Price = 34.95M}
            };

            var priceFilterTotal = products.FilterByPrice(20).TotalPrices();
            var nameFilterTotal = products.FilterByName('S').TotalPrices();

            ViewBag.Title = "Filters 1";

            var message = new string[]
            {
                $"Filter by price. Price total: {priceFilterTotal:C2}",
                $"Filter by first letter. Price total: {nameFilterTotal:C2}"
            };

            return View("NextPage", message);
        }

        public IActionResult EnumerableFilter2()
        {
            var products = new[]
            {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "Lifejacket", Price = 48.95M },
                new Product { Name = "Soccer ball", Price = 19.50M },
                new Product { Name = "Corner flag", Price = 34.95M}
            };

            var priceFilterTotal1 = products.Filter(FilterByPrice).TotalPrices();

            Func<Product, bool> nameFilter = delegate(Product product)
            {
                return product?.Name?[0] == 'S';
            };

            var nameFilterTotal1 = products.Filter(nameFilter).TotalPrices();

            var priceFilterTotal2 = products.Filter(p => (p?.Price ?? 0) >= 20).TotalPrices();
            var nameFilterTotal2 = products.Filter(p => p?.Name?[0] == 'S').TotalPrices();

            ViewBag.Title = "Filters 2";

            var message = new string[]
            {
                $"Filter by price. Use method. Price total: {priceFilterTotal1:C2}",
                $"Filter by first letter. Use delegate. Price total: {nameFilterTotal1:C2}",
                $"Filter by price. Use lambda. Price total: {priceFilterTotal2:C2}",
                $"Filter by first letter. Use lambda. Price total: {nameFilterTotal2:C2}"
            };

            return View("NextPage", message);
        }

        private bool FilterByPrice(Product product) =>
            (product?.Price ?? 0) >= 20;

        public IActionResult LinqExample()
        {
            ViewBag.Title = "Simple LINQ Example";

            return View("NextPage", Product.GetProducts().Select(p => p?.Name));
        }

        public IActionResult AnonymousTypes()
        {
            var products = new[]
            {
                new { Name = "Kayak", Price = 275M },
                new { Name = "Lifejacket", Price = 48.95M },
                new { Name = "Soccer ball", Price = 19.50M },
                new { Name = "Corner flag", Price = 34.05M }
            };

            ViewBag.Title = "Anonymous Types";

            var message = products.Select(
                p => $"{nameof(p.Name)}: {p.Name}, {nameof(p.Price)}: {p.Price:C2}");

            return View("NextPage", message);
        }

        public async Task<IActionResult> GetPageLengthAsync()
        {
            var length1 = await AsyncMethods.GetPageLength();
            var length2 = await AsyncMethods.GetPageLengthAsync();

            ViewBag.Title = "Async Demo";

            var message = new[]
            {
                $"Page length. First method: {length1}",
                $"Page length. Second method: {length2}",
            };

            return View("NextPage", message);
        }
    }
}
