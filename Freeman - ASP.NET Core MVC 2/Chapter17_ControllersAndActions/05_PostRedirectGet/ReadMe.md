# ������������� �������� Post/Redirect/Get

��������������� ���� ����� ������������ � ������� ��������, ������� ������������ ������� POST.
���� ����� ��������� POST ������������ �������� ������������ ��������, �� ����� ���������
������ POST �� ������ ���.

����� �������� �������� �������� ������������ ������� Post/Redirect/Get:
1. ��������� ������� Post.
2. ��� ��������� � ��������������� Redirect.
3. ��������� Get ���������� � ������� URL.


## ����������

������ (��. `Controllers/HomeController`):
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

��� �������� ������ ����� ��������� ������������ �������� `TempData`.
`TempData` - ����� �� ������ ������ `Session` (��. ����� 9) ����� �����:
�������� � `TempData` ���������� ��� �������� ����� ��� ��������� � ��������� ����� ���������
�������.

������ `TempData` �������� ����� �������� `TempData` ������ `Controller`.

� ������� `TempData` ���� *������*:
`Peek()` - ��������� ������ �� ������� �� ��������.
`Keep()` - ������������ ��� �������������� �������� ����� ������������ ��������. ��������� ��
���������� ���������.
