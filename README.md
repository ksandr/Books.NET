# Books.NET

Web-клиент для просмотра архивов библиотек на движке Либрусек (Либрусек , Флибуста и т.д.)

[![License](https://img.shields.io/github/license/ksandr/Books.NET.svg)](https://github.com/ksandr/Books.NET/blob/master/LICENSE) [![Build Status](https://img.shields.io/travis/ksandr/Books.NET.svg)](https://travis-ci.org/ksandr/Books.NET) [![Download](https://img.shields.io/github/downloads/ksandr/Books.NET/total.svg)](https://github.com/ksandr/Books.NET/releases)

# Сборка

## Требования

Для сборки приложения требуется установить:

- [.NET Core SDK v2.2](https://dotnet.github.io/)
- [Node.js](https://nodejs.org/)

## Сборка

Скриипты для сборки находятся в папке `.build`.

При запуске скрипта сборки необходимо передать в качестве параметра имя целевой среды исполнения (`runtime`).

Linux:

```
$ ./build.sh <runtime>
```

Windows:

```
> build.bat <runtime>
```

Проверена работа в следующих средах исполнения:

- win-x64
- linux-x64
- linux-arm

По окончании процесса сборки риложение со всеми необходимыми библиотеками будет расположено в папке `publish/<runtime>`.

# Конфигурация

Для работы приложения нужно задать следующие ключи в разделе `AppConfig` файла конфигурации `appsettings.json`:

- `DatabaseFile` - путь к файлу базы данных приложения;
- `GenresFile` - путь к файлу описания жанров книг;
- `LibraryPath` - путь к папке с архивами библиотеки.
- `Languages` - список языков книг, которые будут импортированы из файла описания архива библиотеки. Если не указан или пустой - буту импортированы книги на всех язаках.

Пример файла конфигурации `appsettings.json`:

```json
{
  "AppConfig": {
    "DatabaseFile": "App_Data/librusec.sqlite",
    "GenresFile": "App_Data/genres_fb2.glst",
    "LibraryPath": "App_Data/lib.rus.ec",
    "Languages": ["RU"]
  }
}
```

По умолчанию web-сервер приложения слушает адрес `http://localhost:5000`. Чтобы изменить этот адрес нужно задать значение переменной окружения `ASPNETCORE_URLS`, например:

```
export ASPNETCORE_URLS=http://*:8080
```

# Использование

Перед началом использования приложения нужно импортировать файл описания архива библиотеки (`.inpx`) в базу данных приложения выполнив команду `import`c ключем `--inpx` (или `-i`).

Например, команда:

```
$ ./Books import --inpx App_Data/librusec_local_fb2.inpx
```

импортирует в базу данных приложения файл `App_Data/librusec_local_fb2.inpx`.

**Внимание!** _При импорте описания архива библиотеки существующая база данных будет заменена._

Чтобы пропустить запрос подтверждения передайте в команду ключ `--force` (или `-f`).

Далее можно запустить web-сервер приложения командой:

```
$ ./Books web
```

в этим случае web-сервер будет слушать адрес по умолчанию или адрес, заданный переменной окружения `ASPNETCORE_URLS`.

Так же можно явно передать программе значение переменной окружения `ASPNETCORE_URLS` при запуске:

```
$ export ASPNETCORE_URLS=http://*:8080 && ./Books web
```
