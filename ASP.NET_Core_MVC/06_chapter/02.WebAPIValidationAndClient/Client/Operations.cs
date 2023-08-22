using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace Client;

public static class Operations
{
    private static readonly HttpClient Client = new HttpClient();

    //private static string UrlWebApi = "https://localhost:5001";   // Нет сертификатов безопасности
    private static readonly string UrlWebApi = "http://localhost:5000";
    private static readonly string CoursesUrl = UrlWebApi + "/api/courses";

    public static async Task<ICollection<Course>> GetCourses()
    {
        try
        {
            var response = await Client.GetAsync(CoursesUrl);
            CheckStatusCode(response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var courses = JsonSerializer.Deserialize<ICollection<Course>>(json, options) ?? Array.Empty<Course>();
            return courses;
        }
        catch (Exception ex)
        {
            ShowErrorMessageBox(ex.Message);
        }

        return Array.Empty<Course>();
    }

    public static async Task<Course> GetCourses(int id)
    {
        var url = GetCoursesUrlWithId(id);
        try
        {
            var response = await Client.GetAsync(url);
            CheckStatusCode(response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var course = JsonSerializer.Deserialize<Course>(json, options) ?? new Course { Id = -1 };
            return course;
        }
        catch
        {
            return new Course { Id = -1 };
        }
    }

    public static async Task<bool> UpdateCourse(int id, Course course)
    {
        var url = GetCoursesUrlWithId(id);
        try
        {
            course.Id = id;
            var content = SerializeCourse(course);
            var response = await Client.PutAsync(url, content);
            var jsonContent = await response.Content.ReadAsStringAsync();

            // Проверка и обработка статуса-кода.
            CheckUpdateResponseStatusOrThrow(response, jsonContent);

            return true;
        }
        catch (Exception ex)
        {
            ShowErrorMessageBox(ex.Message);
        }

        return false;
    }

    public static async Task<int> InsertCourse(Course course)
    {
        try
        {
            var content = SerializeCourse(course);
            var response = await Client.PostAsync(CoursesUrl, content);
            var jsonContent = await response.Content.ReadAsStringAsync();

            // Проверка и обработка статуса-кода.
            CheckInsertResponseStatusOrThrow(response, jsonContent);

            var id = Convert.ToInt32(jsonContent);
            return id;
        }
        catch (Exception ex)
        {
            ShowErrorMessageBox(ex.Message);
        }

        return -1;
    }

    public static async Task<bool> DeleteCourse(int id)
    {
        var url = GetCoursesUrlWithId(id);
        try
        {
            var response = await Client.DeleteAsync(url);
            CheckStatusCode(response.StatusCode);
            return true;
        }
        catch (Exception ex)
        {
            ShowErrorMessageBox(ex.Message);
        }

        return false;
    }

    private static string GetCoursesUrlWithId(int id) =>
        string.Format("{0}/{1}", CoursesUrl, id);

    private static StringContent SerializeCourse(Course course)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var json = JsonSerializer.Serialize(course, options);
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        return content;
    }

    private static void ShowErrorMessageBox(string message)
    {
        var mb = MessageBoxManager.GetMessageBoxStandardWindow(
            "Ошибка",
            message,
            ButtonEnum.Ok,
            Icon.Error);
        mb.Show();
    }

    private static void CheckStatusCode(HttpStatusCode statusCode)
    {
        if (statusCode.Equals(HttpStatusCode.OK))
            return;
        throw new Exception($"Метод не выполнен: {statusCode}");
    }

    private static void CheckUpdateResponseStatusOrThrow(HttpResponseMessage response, string jsonContent)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new Exception(jsonContent);

        throw new Exception("Обновление не было произведено по неизвестной причине");
    }

    private static void CheckInsertResponseStatusOrThrow(HttpResponseMessage response, string jsonContent)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new Exception(jsonContent);

        throw new Exception("Добавление не было произведено по неизвестной причине");
    }
}
