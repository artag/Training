using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace DownloadFile.Controllers;

[Route("file")]
public class FileController : Controller
{
    private const string VirtualFilesDirectory = "virtual_files";
    private const string PhysicalFilesDirectory = "physical_files";
    private const string ContentType = MediaTypeNames.Text.Plain;    // text/plain

    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly string _appRootPath;

    public FileController(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _appRootPath = hostEnvironment.ContentRootPath;
    }

    [Route("virtual")]
    public IActionResult GetVirtual()
    {
        var filepath = Path.Combine(VirtualFilesDirectory, "virtual_hello.txt");
        var fileDownloadName = "hello_from_virtual.txt";
        return File(filepath, ContentType, fileDownloadName);
    }

    [Route("physical")]
    public IActionResult GetPhysical()
    {
        var filepath = Path.Combine(_appRootPath, PhysicalFilesDirectory, "physical_hello.txt");
        var fileDownloadName = "hello_from_physical.txt";
        return PhysicalFile(filepath, ContentType, fileDownloadName);
    }

    [Route("virtual/array")]
    public IActionResult GetVirtualAsBytesArray()
    {
        var filepath = GetWebRootFilepath(filename: "virtual_bytes_array.txt");
        var bytes = System.IO.File.ReadAllBytes(filepath);
        var fileDownloadName = "bytes_array_from_virtual.txt";
        return File(bytes, ContentType, fileDownloadName);
    }

    [Route("physical/array")]
    public IActionResult GetPhysicalAsBytesArray()
    {
        var filepath = Path.Combine(_appRootPath, PhysicalFilesDirectory, "physical_bytes_array.txt");
        var bytes = System.IO.File.ReadAllBytes(filepath);
        var fileDownloadName = "bytes_array_from_physical.txt";
        return File(bytes, ContentType, fileDownloadName);
    }

    [Route("virtual/stream")]
    public IActionResult GetVirtualAsStream()
    {
        var filepath = GetWebRootFilepath(filename: "virtual_stream.txt");
        var stream = new FileStream(filepath, FileMode.Open);
        var fileDownloadName = "stream_from_virtual.txt";
        return File(stream, ContentType, fileDownloadName);
    }

    [Route("physical/stream")]
    public IActionResult GetPhysicalAsStream()
    {
        var filepath = Path.Combine(_appRootPath, PhysicalFilesDirectory, "physical_bytes_array.txt");
        var stream = new FileStream(filepath, FileMode.Open);
        var fileDownloadName = "stream_from_physical.txt";
        return File(stream, ContentType, fileDownloadName);
    }

    private string GetWebRootFilepath(string filename)
    {
        var files = _hostEnvironment.WebRootFileProvider.GetDirectoryContents(VirtualFilesDirectory);
        var file = files.First(f => string.Equals(f.Name, filename, StringComparison.OrdinalIgnoreCase));
        var filepath = file.PhysicalPath;
        return filepath;
    }
}
