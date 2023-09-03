// Для отправки данных на сервер часто требуется их преобразовывать из объекта в строку.
// Метод $.param принимает данные в виде объекта и преобразует их в строку.

// Из всплывающей документации.
// $.param - Serialize an array of form elements or a set of key/values into a query string.

$(document).ready(function(){
    let form = {
        Land: "Russia",
        Currency: "Ruble",
        Capital: "Moscow"
    }
    $(".result").text($.param(form));
});
