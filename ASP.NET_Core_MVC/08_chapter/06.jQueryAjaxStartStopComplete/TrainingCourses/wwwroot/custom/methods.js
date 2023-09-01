$(document).ready(function() {

    // Запрос всех курсов. (В API сделана эмуляция задержки получения данных).
    $(".start").click(function() {
        $.ajax({
            url: "/api/courses",
            success: function (result) {
                console.log("Success request: ", result)
            }
        });
    });

    // Срабатывает при возникновении события запуска AJAX запроса.
    // Включение анимации загрузки данных.
    $(document).ajaxStart(function() {
        $(".loading").show(700)
    });
    // $(document).ajaxStart(function() {
    //     alert("Старт")   // Показ всплывающего сообщения
    // });

    // Срабатывает после завершения всех AJAX запросов на странице.
    // Выключение анимации загрузки данных (завершение загрузки).
    $(document).ajaxStop(function() {
        $(".loading").hide(700)
    });
    // $(document).ajaxStop(function() {
    //     alert("Стоп")   // Показ всплывающего сообщения
    // });

    // Срабатывает после методов ajaxSuccess/ajaxError и перед методом ajaxStop.
    $(document).ajaxComplete(function() {
        alert("Завершено");
    });
});
