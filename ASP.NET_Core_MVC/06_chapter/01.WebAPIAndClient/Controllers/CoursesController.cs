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
    public Course Get(int id)
    {
        return _repository.Get(id);
    }

    // POST api/courses
    [HttpPost]
    public int Post([FromBody] Course course)
    {
        return _repository.Add(course);
    }

    // PUT api/courses/3
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Course newCourse)
    {
        newCourse.Id = id;
        _repository.Save(newCourse);
    }

    // DELETE api/courses/3
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var course = _repository.Get(id);
        _repository.Delete(course);
    }
}
