# Этап 1: Сборка приложения
# Используем образ SDK версии 10.0
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY DDMdiplom.csproj .
RUN dotnet restore "DDMdiplom.csproj"

# Копируем весь остальной код и собираем приложение
COPY . .
RUN dotnet publish "DDMdiplom.csproj" -c Release -o /app/publish

# Этап 2: Запуск приложения
# Используем образ ASP.NET Runtime версии 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Копируем собранные файлы из первого этапа
COPY --from=build /app/publish .

# Указываем порт, который будет слушать приложение
EXPOSE 8080

# Переменная окружения для порта (на случай, если в коде используется PORT)
ENV ASPNETCORE_URLS=http://+:8080

# Точка входа - запуск вашего приложения
# ⚠️ ВАЖНО: замените "DDMdiplom.dll" на имя вашего файла .dll
ENTRYPOINT ["dotnet", "DDMdiplom.dll"]