using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client;

public static class Operations
{
    private static HttpClient client = new HttpClient();
    private static string UrlWebApi = "https://localhost:5001";

    public static async Task<ICollection<Course>> GetCourses()
    {
        var response = await client.GetAsync(UrlWebApi + "/api/courses");
        CheckStatusCode(response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var courses = JsonSerializer.Deserialize<ICollection<Course>>(json, options) ?? Array.Empty<Course>();
        return courses;
    }

    private static void CheckStatusCode(HttpStatusCode statusCode)
    {
        if (statusCode.Equals(HttpStatusCode.OK))
            return;
        throw new Exception($"Метод не выполнен: {statusCode}");
    }
}
