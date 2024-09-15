
# AspRestApiTest

## Описание

Тестовое приложения для демонстрации навыков работы с ASP.NET Core

## Запуск через Docker

1. Убедитесь, что Docker установлен и запущен.
2. В корневой папке проекта выполните команду:

   ```bash
   docker compose up --build
   ```

3. Приложение будет доступно по адресу `http://localhost:5000/swagger`.

## Локальный запуск

1. Настройте PostgreSQL и обновите строку подключения в `appsettings.json`:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=AspRestApiTestDb;Username=postgres;Password=123"
   }
   ```

2. В корневой папке выполните команду:

   ```bash
   dotnet run
   ```

3. Перейдите по адресу `http://localhost:5000/swagger` для использования API.

