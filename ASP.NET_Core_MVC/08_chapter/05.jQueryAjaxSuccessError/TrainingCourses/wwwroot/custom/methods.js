$(document).ready(function() {

    // Успещный запрос.
    $(".request").click(function() {
        $.ajax({
            url: "/api/courses/3",
            success: function (result) {
                console.log("Success request: ", result)
            }
        });
    });

    // Запрос с ошибкой.
    $(".broken").click(function() {
        $.ajax({
            url: "pi/courses/3",
            success: function (result) {
                console.log("Failure request 1: ", result)
            },
            error: function (result) {
                console.log("Failure request 2: ", result)
            }
        });
    });

    // Обработчик события. Вызывается если любой из запросов удачно завершился.
    // e - событие, xhr - запрос, opt - параметры.
    $(document).ajaxSuccess(function(e, xhr, opt) {
        alert("completed");
        console.log("ajaxSuccess. e: ", e);
        console.log("ajaxSuccess. xhr: ", xhr);
        console.log("ajaxSuccess. opt: ", opt);
    });

    // Обработчик события. Вызывается если любой из запросов завершился с ошибкой.
    // e - событие, xhr - запрос, opt - параметры.
    $(document).ajaxError(function(e, xhr, opt) {
        alert("error");
        console.log("ajaxERROR. e: ", e);
        console.log("ajaxERROR. xhr: ", xhr);
        console.log("ajaxERROR. opt: ", opt);
    });
});
