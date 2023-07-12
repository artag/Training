using System.Text;
using Microsoft.AspNetCore.Mvc;
using SendModelToController.Models;

namespace SendModelToController.Controllers;

[Route("documents")]
public class DocumentsController : Controller
{
    [HttpGet("use-properties")]
    public IActionResult SendUsingProperties()
    {
        return View();
    }

    [HttpGet("use-class")]
    public IActionResult SendUsingClass()
    {
        return View();
    }

    [HttpGet("use-classes")]
    public IActionResult SendUsingClasses()
    {
        return View();
    }

    [Route("acceptresult")]
    public IActionResult AcceptResult(int code, string title, string issuedBy)
    {
        var doc = new Doc
        {
            Code = code,
            Title = title,
            IssuedBy = issuedBy,
        };

        var message = $"Код документа: {doc.Code}\n" +
                      $"Наименование документа: {doc.Title}\n" +
                      $"Кем выдан: {doc.IssuedBy}";

        return Content(message);
    }

    [Route("acceptresultclass")]
    public IActionResult AcceptResultClass(Doc doc)
    {
        var message = $"Код документа: {doc.Code}\n" +
                      $"Наименование документа: {doc.Title}\n" +
                      $"Кем выдан: {doc.IssuedBy}";

        return Content(message);
    }

    [Route("acceptresultclasses")]
    public IActionResult AcceptResultClasses(Doc[] docs)
    {
        var message = new StringBuilder();
        foreach (var doc in docs)
        {
            message.AppendLine($"Код документа: {doc.Code}; " +
                               $"Наименование документа: {doc.Title}; " +
                               $"Кем выдан: {doc.IssuedBy};");
        }

        return Content(message.ToString());
    }
}
