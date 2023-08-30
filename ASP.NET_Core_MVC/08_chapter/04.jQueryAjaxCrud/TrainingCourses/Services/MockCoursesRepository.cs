using TrainingCourses.Models;

namespace TrainingCourses.Services;

public class MockCoursesRepository : ICoursesRepository
{
    private readonly List<Course> _courses;

    public MockCoursesRepository()
    {
        _courses = new List<Course>()
        {
            new Course { Id = 1, Title = "Геометрия", Hours = 30 },
            new Course { Id = 2, Title = "История", Hours = 45 },
            new Course { Id = 3, Title = "Информатика", Hours = 60 },
        };
    }

    public ICollection<Course> GetAll()
    {
        return _courses;
    }

    public Course Get(int id)
    {
        return _courses.FirstOrDefault(c => c.Id.Equals(id)) ?? new Course { Id = -1 };
    }

    public int Add(Course newCourse)
    {
        newCourse.Id = _courses.Max(c => c.Id) + 1;
        _courses.Add(newCourse);
        return newCourse.Id;
    }

    public void Save(Course course)
    {
        _courses
            .Where(c => c.Id.Equals(course.Id))
            .ToList()
            .ForEach(c =>
            {
                c.Title = course.Title;
                c.Hours = course.Hours;
            });
    }

    public void Delete(Course course)
    {
        _courses.Remove(course);
    }
}
