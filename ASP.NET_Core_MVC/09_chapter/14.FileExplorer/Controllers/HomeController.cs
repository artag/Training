using System.Net.Mime;
using FileExplorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Controllers;

public class HomeController : Controller
{
    private const string FilesDirectory = "Files";
    private readonly IWebHostEnvironment _environment;

    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public IActionResult Index()
    {
        var files = GetFiles();
        return View(files);
    }

    public IActionResult DownloadFile(string fileName)
    {
        var file = Path.Combine(_environment.WebRootPath, "Files", fileName);
        var bytes = System.IO.File.ReadAllBytes(file);
        return File(bytes, MediaTypeNames.Application.Octet, fileName);
    }

    // (1) Предельный размер загружаемого файла (1 МБ).
    [HttpPost]
    [RequestSizeLimit(1048576)]     // (1)
    public async Task<IActionResult> AddFile(IFormFile uploadedFile)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var filename = Path.GetFileName(uploadedFile.FileName);
                var fileInfo = new FileInfo(filename);
                var targetDir = Path.Combine(_environment.WebRootPath, FilesDirectory);
                var targetPath = Path.Combine(targetDir, fileInfo.Name);
                await using var fileStream = new FileStream(targetPath, FileMode.Create);
                await uploadedFile.CopyToAsync(fileStream);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(nameof(Index), GetFiles());
            }
        }

        ModelState.AddModelError("", "Модель недействительна");
        return View(nameof(Index), GetFiles());
    }

    private FileModel[] GetFiles()
    {
        var filesPath = Path.Combine(_environment.WebRootPath, FilesDirectory);
        var files = Directory.GetFiles(filesPath);
        return files
            .Select(file => new FileModel { Filename = Path.GetFileName(file) })
            .ToArray();
    }
}
