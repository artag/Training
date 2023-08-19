using TrainingCourses.Models;

namespace TrainingCourses.Services;

public interface ICoursesRepository
{
    ICollection<Course> GetAll();
    Course Get(int id);
    int Add(Course newCourse);
    void Save(Course course);
    void Delete(Course course);
}
