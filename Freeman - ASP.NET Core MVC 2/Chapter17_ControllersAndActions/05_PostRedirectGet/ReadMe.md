# »спользование паттерна Post/Redirect/Get

ѕеренаправление чаще всего используетс€ в методах действий, которые обрабатывают запросы POST.
≈сли после обработки POST пользователь случайно перезагрузит страницу, то может отправить
запрос POST во второй раз.

„тобы избежать подобной проблемы используетс€ паттерн Post/Redirect/Get:
1. ѕолучение запроса Post.
2. ≈го обработка и перенаправление Redirect.
3. ѕолучение Get информации с другого URL.


## –еализаци€

ѕример (см. `Controllers/HomeController`):
```cs
// Post
public ViewResult Index() => View("SimpleForm");

// Redirect
public RedirectToActionResult ReceiveForm(string name, string city)
{
    TempData["name"] = name;
    TempData["city"] = city;

    return RedirectToAction(nameof(Data));
}

// Get
public ViewResult Data()
{
    var name = TempData["name"] as string;
    var city = TempData["city"] as string;

    return View("Result", $"{name} lives in {city}");
}
```

ƒл€ передачи данных между запросами используетс€ средство `TempData`.
`TempData` - похож на данные сеанса `Session` (см. главу 9) кроме этого:
значени€ в `TempData` помечаютс€ дл€ удалени€ когда они прочитаны и удал€ютс€ после обработки
запроса.

ќбъект `TempData` доступен через свойство `TempData` класса `Controller`.

” словар€ `TempData` есть *методы*:
`Peek()` - получение данных не помеча€ на удаление.
`Keep()` - используетс€ дл€ предотвращени€ удалени€ ранее прочитанного значени€. ƒействует до
следующего прочтени€.
