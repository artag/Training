using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using TrainingCourses.Models;
using TrainingCourses.Services;

namespace TrainingCourses.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICoursesRepository _repository;

    public CoursesController(ICoursesRepository repository)
    {
        _repository = repository;
    }

    // GET api/courses
    [HttpGet]
    public ICollection<Course> Get()
    {
        return _repository.GetAll();
    }

    // GET api/courses/3
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var course = _repository.Get(id);
        if (course == null || course.Id < 0)
            return NotFound();

        return new ObjectResult(course);
    }

    // POST api/courses
    [HttpPost]
    public IActionResult Post([FromBody] Course course)
    {
        // Обработка частных случаев валидации.
        ProcessSpecialCasesOfValidation(course);
        // Если есть ошибки - возвращаем ошибку 400.
        var (isValid, errorMessage) = IsModelStateValid();
        if (!isValid)
            return BadRequest(errorMessage);

        // Если ошибок нет, сохраняем элемент.
        var newId = 0;
        try
        {
            newId = _repository.Add(course);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(newId);
    }

    // PUT api/courses/3
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Course newCourse)
    {
        // Обработка частных случаев валидации.
        ProcessSpecialCasesOfValidation(newCourse);
        // Если есть ошибки - возвращаем ошибку 400.
        var (isValid, errorMessage) = IsModelStateValid();
        if (!isValid)
            return BadRequest(errorMessage);

        // Если ошибок нет, сохраняем элемент.
        try
        {
            newCourse.Id = id;
            _repository.Save(newCourse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    // DELETE api/courses/3
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var course = _repository.Get(id);
        _repository.Delete(course);
    }

    /// <summary>
    /// Обрабатывает частные случаи валидации курса.
    /// </summary>
    /// <param name="course">Курс.</param>
    private void ProcessSpecialCasesOfValidation(Course course)
    {
        if (course.Hours == 3)
            ModelState.AddModelError(
                key: nameof(course.Hours),
                errorMessage: "Количество часов не должно быть равно 3");

        if (course.Title.ToLower().IndexOf("нумерология") > -1)
            ModelState.AddModelError(
                key: nameof(course.Title),
                errorMessage: $"Недопустимое наименование курса - '{course.Title}'");
    }

    /// <summary>
    /// Проверяет модель на наличие ошибок.
    /// </summary>
    private (bool IsValid, string ErrorMessage) IsModelStateValid()
    {
        if (ModelState.IsValid)
            return (true, string.Empty);

        var errors = ModelState.Values
            .Where(stateEntry => stateEntry.Errors.Count > 0)
            .SelectMany(stateEntry => stateEntry.Errors)
            .Select(modelError => modelError.ErrorMessage)
            .ToArray();

        var validationErrors = string.Join("\n", errors);
        return (false, validationErrors);
    }
}
