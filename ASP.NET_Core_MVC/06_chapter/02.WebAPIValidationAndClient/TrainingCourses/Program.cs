using Microsoft.AspNetCore.Mvc;
using TrainingCourses.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Используются контроллеры.
// Опция отключает validation для модели Course перед вызовом метода контроллера.
// (Иначе, сразу возвращается Bad Request с невнятным сообщением об ошибке).
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddSingleton<ICoursesRepository, MockCoursesRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Перенаправление HTTP к HTTPS запросам.
//app.UseHttpsRedirection();

// Маршрутизация контроллеров на основе атрибутов.
// (Настройка маршрутизации непосредственно в параметрах контроллера).
app.MapControllers();

app.Run();
