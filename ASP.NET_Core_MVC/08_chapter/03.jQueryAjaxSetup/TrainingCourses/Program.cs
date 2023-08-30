using TrainingCourses.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

// (1) Используются контроллеры.
// (2) Опция отключает validation для модели Course перед вызовом метода контроллера.
// (Иначе, сразу возвращается Bad Request с невнятным сообщением об ошибке).
builder.Services
    .AddControllers()           // (1)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;     // (2)
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

app.UseStaticFiles();

// Перенаправление HTTP к HTTPS запросам.
//app.UseHttpsRedirection();

// Маршрутизация контроллеров на основе атрибутов.
// (Настройка маршрутизации непосредственно в параметрах контроллера).
app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");

app.Run();
