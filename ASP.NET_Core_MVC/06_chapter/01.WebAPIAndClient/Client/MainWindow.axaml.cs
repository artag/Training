using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace Client;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataGridCourses.Items = Array.Empty<Course>();
        SetTextToStatusBar("Курс не выбран");
    }

    private async void ButtonReload_OnClick(object? sender, RoutedEventArgs e)
    {
        await GetCoursesAndRefreshTable();
    }

    private async void ButtonAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        var (title, hours) = await ValidateInputData();
        if (string.IsNullOrEmpty(title) || hours < 0)
        {
            return;
        }

        var course = new Course { Title = title, Hours = hours };
        var newId = await Operations.InsertCourse(course);
        if (newId < 1)
            return;

        await GetCoursesAndRefreshTable();
        ShowInformationMessageBox("Добавление курса успешно произведено");
    }

    private async void ButtonEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        var course = SelectCourse();
        if (course.Id < 1)
        {
            ShowErrorMessageBox("Записи для изменения не найдены.");
            return;
        }

        var (title, hours) = await ValidateInputData();
        if (string.IsNullOrEmpty(title) || hours < 0)
        {
            return;
        }

        course.Title = title;
        course.Hours = hours;

        var updated = await Operations.UpdateCourse(course.Id ,course);
        if (!updated)
            return;

        await GetCoursesAndRefreshTable();
        ShowInformationMessageBox($"Курс {course.Id} успешно изменен.");
    }

    private async void ButtonDelete_OnClick(object? sender, RoutedEventArgs e)
    {
        var course = SelectCourse();
        if (course.Id < 1)
        {
            ShowErrorMessageBox("Записи для удаления не найдены.");
            return;
        }

        var id = course.Id;
        var deleted = await Operations.DeleteCourse(id);
        if (!deleted)
            return;

        await GetCoursesAndRefreshTable();
        ShowInformationMessageBox($"Запись {id} успешно удалена.");
    }

    private async Task GetCoursesAndRefreshTable()
    {
        var data = await Operations.GetCourses();
        DataGridCourses.Items = data;
    }

    private void DataGridCourses_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var course = SelectCourse();
        if (course.Id < 1)
        {
            SetTextToTextBox(string.Empty, string.Empty);
            SetTextToStatusBar("Курс не выбран");
            return;
        }

        SetTextToTextBox(course.Title, course.Hours.ToString());
        SetTextToStatusBar(course.Id, course.Title);
    }

    private Course SelectCourse()
    {
        var items = DataGridCourses.Items as IEnumerable<Course>;
        if (items == null || !items.Any())
            return new Course { Id = -1 };

        var item = DataGridCourses.SelectedItem;
        if (item == null)
            return new Course { Id = -1 };

        var course = item as Course;
        if (course == null)
            return new Course { Id = -1 };

        return course;
    }

    private Task<(string title, int hours)> ValidateInputData()
    {
        if (string.IsNullOrWhiteSpace(TextBoxTitle.Text)
            || TextBoxTitle.Text.Trim().Length == 0)
        {
            ShowErrorMessageBox("Введите наименование курса");
            return Task.FromResult((string.Empty, -1));
        }

        if (string.IsNullOrWhiteSpace(TextBoxHours.Text)
            || TextBoxHours.Text.Trim().Length == 0)
        {
            ShowErrorMessageBox("Введите количество часов");
            return Task.FromResult((string.Empty, -1));
        }

        var hoursStr = TextBoxHours.Text.Trim();
        if (!int.TryParse(hoursStr, out var hours) || hours < 1)
        {
            ShowErrorMessageBox(
                $"Формат введенного количества часов '{hoursStr}' неверен.\n" +
                "Введите положительное число.");
            return Task.FromResult((string.Empty, -1));
        }

        var title = TextBoxTitle.Text.Trim();
        return Task.FromResult((title, hours));
    }

    private void SetTextToTextBox(string title, string hours)
    {
        TextBoxTitle.Text = title;
        TextBoxHours.Text = hours;
    }

    private void SetTextToStatusBar(string text) =>
        StatusBar.Text = text;

    private void SetTextToStatusBar(int courseId, string courseTitle) =>
        StatusBar.Text = string.Format("Id = {0}, Запись = {1}", courseId, courseTitle);

    private static void ShowErrorMessageBox(string message)
    {
        var mb = MessageBoxManager.GetMessageBoxStandardWindow(
            "Ошибка",
            message,
            ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon.Error);
        mb.Show();
    }

    private static void ShowInformationMessageBox(string message)
    {
        var mb = MessageBoxManager.GetMessageBoxStandardWindow(
            "Информация",
            message,
            ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon.Success);
        mb.Show();
    }
}
