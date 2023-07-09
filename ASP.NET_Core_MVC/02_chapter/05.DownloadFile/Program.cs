var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

/*
 Адреса:
 https://localhost:5001/file/virtual         - Загрузка файла по виртуальному пути (VirtualFileResult)
 https://localhost:5001/file/physical        - Загрузка файла по физическому пути (PhysicalFileResult)
 https://localhost:5001/file/virtual/array   - Загрузка файла в виде массива байтов (FileContentResult)
 https://localhost:5001/file/physical/array  - Загрузка файла в виде массива байтов (FileContentResult)
 https://localhost:5001/file/virtual/stream  - Загрузка файла в виде потока байтов (FileStreamResult)
 https://localhost:5001/file/physical/stream - Загрузка файла в виде потока байтов (FileStreamResult)
*/
