# Демо проект к курсу "Domain Driven Design и Clean Architecture на языке C#"

📚 Подробнее о курсе: [microarch.ru/courses/ddd/languages/csharp](https://microarch.ru/courses/ddd/languages/csharp?utm_source=gitlab&utm_medium=repository)

---

## Условия использования

Вы можете использовать и модифицировать данный код **в образовательных целях**, при условии сохранения ссылки на курс и оригинального источника.

---

# OpenApi
```
openapi-generator generate \
  -i https://gitlab.com/microarch-ru/microservices/dotnet/system-design/-/raw/main/services/delivery/contracts/openapi.yml \
  -g aspnetcore \
  -o DeliveryApp.Api/Adapters/Http/Generated \
  --package-name OpenApi \
  --additional-properties sourceFolder=. \
  --additional-properties classModifier=abstract \
  --additional-properties operationResultTask=true \
  --additional-properties generateFilters=false
```

# БД
```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```
[Подробнее про dotnet cli](https://learn.microsoft.com/ru-ru/ef/core/cli/dotnet)

# Миграции
```
dotnet ef migrations add Init --startup-project ./DeliveryApp.Api --project ./DeliveryApp.Infrastructure --output-dir ./Adapters/Postgres/Migrations
dotnet ef database update --startup-project ./DeliveryApp.Api --connection "Server=localhost;Port=5432;User Id=username;Password=secret;Database=delivery;"
```

# Запросы к БД
```
SELECT * public.assignments;
SELECT * FROM public.couriers;
SELECT * FROM public.orders;
SELECT * public.outbox;
```

# Очистка БД (все кроме справочников)
```
DELETE FROM public.assignments;
DELETE FROM public.couriers;
DELETE FROM public.orders;

DELETE FROM public.outbox;
```

# Лицензия

Код распространяется под лицензией [MIT](./LICENSE).  
© 2025 microarch.ru