using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;

var builder = WebApplication.CreateBuilder(args);

// Установка отображения русских символов в сообщении alert:
// изменение параметров кодировщика.
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

builder.Services.AddMvc();

var app = builder.Build();

// Подключение возможности использовать статические файлы на случай,
// если потребуется обращаться к клиентским библиотекам,
// расположенным в одном из каталогов приложения.
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
