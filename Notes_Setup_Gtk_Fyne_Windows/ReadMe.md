# Установка Gtk, Fyne для Windows

Для использования с Go.

## Links

- [gotk3. Installing on Windows](https://github.com/gotk3/gotk3/wiki/Installing-on-Windows)

- [Setting up GTK for Windows](https://www.gtk.org/docs/installations/windows/)

- [Fyne. Getting Started](https://developer.fyne.io/started/)

- [Установка и настройка Msys2 для программирования на C и С++ в Windows](https://librebay.blogspot.com/2018/12/install-msys2-for-windows.html)

- [Unicode (utf-8) with git-bash](https://stackoverflow.com/questions/10651975/unicode-utf-8-with-git-bash)



## Процесс

Установщик MSYS2 скачивается с сайта [http://www.msys2.org](http://www.msys2.org/).

Для 64-битной системы `x86_64`, для 32-битной - `i686`.

### Установка MSYS2

1. Установка как обычно в винде.
Путь к msys2 указать:

- на английском
- короткий
- без пробелов и прочих сомнительных знаков

2. После установки закрыть окно MSYS2

Некоторые источники рекомендуют закрывать.

### Настройка MSYS2 (MINGW64)

Открыть окно `MINGW64`

Команды:

1. Обновить базу данных с информацией об доступных пакетах

```text
pacman -Syu
```

2. Обновить пакеты

```text
pacman -Su
```

3. Поставить пакеты группы,  в которой собраны компиляторы GCC, стандартные библиотеки
и инструменты разработки

```text
pacman -S git mingw-w64-x86_64-toolchain
```

4. Поставить пакеты gtk3 (для fyne не требуется)

```text
pacman -S mingw-w64-x86_64-gtk3 glib2-devel
```

4.1. (Опционально) Поставить Midnight Commander (mc)

5. Модификация файла '~/.bashrc'

Прописывание путей ко внешним (по отношению к msys2) путям в системе

Go (из консоли):

```text
echo 'export PATH=/c/Go/bin:$PATH' >> ~/.bashrc
```

У меня на машине Go здесь (из консоли):

```text
echo 'export PATH=/c/Program\ Files/Go/bin:$PATH' >> ~/.bashrc
```

Зачем-то Git (из консоли):

```text
echo 'export PATH=/c/Program\ Files/Git/bin:$PATH' >> ~/.bashrc
```

Еще можно добавить в файл '~/.bashrc'

Путь к Visual Studio Code:

```text
export PATH=/c/VSCode:$PATH
```

Настроить палитру терминала (чтобы в Midnight Commander можно было поставить нормальную тему):

```text
export TERM=xterm-256color
```

Кодировка UTF-8 в консоли (чтобы не вводить в консоли `chcp 65001`):

```text
export LC_ALL=en_US.UTF-8
export LANG=en_US.UTF-8
export LANGUAGE=en_US.UTF-8
```

Это нужно для запуска gtk приложения - иначе могут быть проблемы.

6. Прочитать настроки файла '~/.bashrc'

```text
source ~/.bashrc
```

Можно не вводить, а просто закрыть/открыть новое окно `MINGW64`.

7. Вылечить баг в `pkgconfig` (для fyne не требуется)

```text
sed -i -e 's/-Wl,-luuid/-luuid/g' /mingw64/lib/pkgconfig/gdk-3.0.pc
```

Вроде для gotk3 не требовался обязательный ввод этой команды, но на всякий пожарный...

### Примечание

1. Запускать Visual Studio Code, сами программы на go из окна `MINGW64`. Если запускать
вне среды `MINGW64`, то приложения с gtk и fyne не заработают.

2. warning'и в консоли при запуске gotk3 это нормально.

3. Поиск пакета: `pacman -Ss имя_пакета`.

4. Windows разделы видны из MSYS2 как `/c/`, `/d/` и так далее.
