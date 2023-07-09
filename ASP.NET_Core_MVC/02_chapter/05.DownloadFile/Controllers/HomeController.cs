using Microsoft.AspNetCore.Mvc;

namespace DownloadFile.Controllers;

public class HomeController : Controller
{
    public ContentResult Index()
    {
        var message =
            "Адреса:\n" +
            "https://localhost:5001/file/virtual         - Загрузка файла по виртуальному пути (VirtualFileResult)\n" +
            "https://localhost:5001/file/physical        - Загрузка файла по физическому пути (PhysicalFileResult)\n\n" +
            "https://localhost:5001/file/virtual/array   - Загрузка файла в виде массива байтов (FileContentResult)\n" +
            "https://localhost:5001/file/physical/array  - Загрузка файла в виде массива байтов (FileContentResult)\n\n" +
            "https://localhost:5001/file/virtual/stream  - Загрузка файла в виде потока байтов (FileStreamResult)\n" +
            "https://localhost:5001/file/physical/stream - Загрузка файла в виде потока байтов (FileStreamResult)";

        return Content(message);
    }
}
