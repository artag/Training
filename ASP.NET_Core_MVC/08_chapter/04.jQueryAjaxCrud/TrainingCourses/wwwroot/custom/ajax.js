// Основные методы AJAX-библиотеки jQuery. CRUD-операции.

$(document).ready(function() {

    // 1. Вызов метода ajax. По умолчанию выполняет GET-запрос.
    $('.ajax').click(function () {
        $.ajax({
            // Адрес ресурса, на который будет отправлен запрос.
            url: "/api/courses/3",
            // Обработчик успешного выполнения запроса.
            success: function (result) {
                console.log(result);
                $("h1").text("Id: " + result.id);
                $("h3").text("Title: " + result.title);
                $("p").text("Hours: " + result.hours);
            }
        });
    });

    // 2. Вызов POST-запроса (быстрый метод).
    $(".post").click(function () {
        $.post({
            // Адрес ресурса, на который будет отправлен запрос.
            url: "/api/courses",
            // Тип передаваемого содержимого в запросе (важный параметр).
            contentType: "application/json;charset=utf-8",
            // Тип данных, которые мы ожидаем получить от сервера.
            dataType: "json",
            // Данные, отправляемые вместе с запросом.
            data: JSON.stringify({
                title: "Химия",
                hours: 60
            }),
            // Обработчик успешного выполнения запроса. data - данные от сервера.
            success: function (data) {
                console.log("POST response: " + data);
            }
        });
    });

    // 3. Вызов GET-запроса (быстрый метод).
    $(".get").click(function () {
        // Первый параметр - адрес ресурса, на который будет отправлен запрос.
        // Второй параметр - функция. Сработает после выполнения запроса.
        //                   data - возвращаемые данные, status - код статуса запроса.
        $.get("/api/courses/4", function (data, status) {
            console.log("Status: " + status);
            $("h1").text("Id: " + data.id);
            $("h3").text("Title: " + data.title);
            $("p").text("Hours: " + data.hours);
        });
    });

    // 4. Вызов PUT-запроса.
    $(".put").click(function () {
        $.ajax({
            // Адрес ресурса, на который будет отправлен запрос.
            url: "/api/courses/4",
            // Тип запроса.
            method: 'PUT',
            // Тип передаваемого содержимого в запросе (важный параметр).
            contentType: "application/json;charset=utf-8",
            // Тип данных, которые мы ожидаем получить от сервера.
            dataType: "json",
            data: JSON.stringify({
                title: "История",
                hours: 70
            }),
            // Обработчик успешного выполнения запроса. result - данные от сервера.
            success: function (result) {
                console.log("PUT result: " + result);
            }
        });
    });

    // 4. Вызов DELETE-запроса.
    $(".delete").click(function () {
        $.ajax({
            // Адрес ресурса, на который будет отправлен запрос.
            url: "/api/courses/4",
            // Тип запроса.
            method: 'DELETE',
            // Обработчик успешного выполнения запроса. result - данные от сервера.
            success: function (result) {
                console.log("DELETE result: " + result);
            }
        });
    });
});