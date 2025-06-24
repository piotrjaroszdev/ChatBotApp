
# ChatBotApp

## Opis

**ChatBotApp** to aplikacja demonstracyjna typu full-stack, składająca się z:
- Serwera API w .NET 8 (ASP.NET Core + Entity Framework Core)
- Klienta frontendowego w Angularze (TypeScript)
- Testów jednostkowych (xUnit, Moq)

Aplikacja umożliwia prowadzenie rozmowy z prostym botem, ocenianie wiadomości oraz przeglądanie historii czatu.

---

## Struktura projektu

- **ChatBotApp.Server** – ASP.NET Core Web API (kontrolery, konfiguracja, CORS, Swagger)
- **ChatBotApp.Data** – warstwa dostępu do danych (DbContext, modele, migracje)
- **chatbotapp.client** – aplikacja Angular (komponenty, serwisy, translacje)
- **ChatBotApp.Tests** – testy jednostkowe .NET (xUnit, Moq)

---

## Szybki start

### 1. Konfiguracja bazy danych

Upewnij się, że connection string w `ChatBotApp.Server/appsettings.json` i `ChatBotApp.Data/appsettings.json` jest poprawny:

### 2. Migracje bazy danych

W katalogu `ChatBotApp.Data` uruchom: update-database

### 3. Uruchomienie backendu

Ustaw projekt startowy dla `ChatBotApp.Server` i uruchom

## Translacje

Pliki tłumaczeń dla Angulara znajdują się w `chatbotapp.client/src/assets/i18n/`.
