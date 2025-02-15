# Basic Authentication API

## Introduction

This project is an **ASP.NET Core Web API** that uses **Basic Authentication** to secure the API. It allows users to **register, log in, and access protected resources** after successful authentication.

## Project Features

- **Uses Basic Authentication** via `Authorization: Basic base64(username:password)`.
- **User registration and login** using a SQL Server database.
- **Entity Framework Core** for database management.
- **Swagger UI integration** for easy API documentation and testing.
- **Securing specific endpoints with `[Authorize]`** to ensure safe access.

## Technologies Used

- **ASP.NET Core 7**
- **Entity Framework Core**
- **SQL Server**
- **Swagger (Swashbuckle)**

## Installation and Setup



### 1. Configure the Database
Update `appsettings.json` and modify the connection details for SQL Server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
```

### 2. Apply Migrations and Update the Database
```bash
dotnet ef database update
```

### 3. Run the Application
```bash
dotnet run
```

## Code and Functionality Details

### **(A) `Program.cs` (Application Setup and Authentication Configuration)**
- **Configures the database connection using `DBcontext`**.
- **Adds authentication services with `BasicAuthenticationHandler`**.
- **Includes `Swagger` for API documentation**.
- **Enables HTTPS and authentication**.

### **(B) `BasicAuthenticationHandler.cs` (Basic Authentication Implementation)**
- **Reads the `Authorization` header** from the request.
- **Decodes the Base64 authentication credentials** to extract the username and password.
- **Validates the user credentials against the database**.
- **Returns an authentication object `ClaimsPrincipal`** upon successful validation.

### **(C) `AuthenticationController.cs` (User Registration and Login)**
Contains two endpoints **(register & login)**:
- `register`: Creates a new user and adds them to the database after verifying that the **username and email are unique**.
- `login`: Verifies user credentials in the database and returns a success or failure message.

### **(D) Securing Endpoints with `[Authorize]`**
```csharp
[Authorize]
[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
```
- This endpoint is protected, meaning **it requires authentication** before access is granted.

## How to Use the API

### **Register a New User**
**Endpoint:** `POST /register`
```json
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123",
  "name": "Test User"
}
```

### **Login**
**Endpoint:** `POST /login`
```json
{
  "username": "testuser",
  "password": "password123"
}
```

### **Accessing Protected Endpoints**
**Endpoint:** `GET /weatherforecast`
**Headers:**
```plaintext
Authorization: Basic base64(username:password)
```

## Notes
- This application uses **Basic Authentication**, which is not fully secure for production without **HTTPS**.
- Passwords are stored as plain text (not encrypted), so **a hashing algorithm like `BCrypt` should be used** for security.
- It is recommended to use **JWT Authentication** for better security and user experience.



