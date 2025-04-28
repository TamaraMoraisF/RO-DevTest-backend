# RO.DevTest
This project is a RESTful API developed in ASP.NET Core, designed to manage customers, products, and sales. It fulfills the requirements of the Rota das Oficinas technical test, including JWT authentication, role-based access control, pagination, sorting, and sales analytics.

## 🧩Features

- **CRUD for Customers, Products, and Sales:** Endpoints to create, read, update, and delete records.
- **Authentication and Authorization:** JWT-based authentication with role-based access (`Admin` and `Customer`).
- **Pagination and Sorting:** Support for pagination and sorting on listing endpoints.
- **Sales Analytics:** Endpoint to retrieve sales analysis for a given period, including total sales, total revenue, and revenue breakdown per product.
- **Validation:** Input validation using FluentValidation.
- **Unit Tests:** Unit test coverage for command handlers and validators.

## ⚙️ Technologies Used

- .NET 8
- Entity Framework Core
- MediatR
- FluentValidation
- xUnit & Moq
- Swagger

## 🔐 Authentication and Authorization

Authentication is done using JWT tokens. There are two types of users:

- **Admin:** Full access to all API resources.
- **Customer:** Limited access to specific endpoints.

### Example of a JWT Token
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "issuedAt": "2025-04-26T04:20:03.3838221Z",
  "expirationDate": "2025-04-26T05:20:03.3424569Z",
  "roles": [
    "Customer"
  ]
}
```

## 📄 API Documentation
Interactive API documentation is available via Swagger:
``` bash
https://localhost:7014/swagger
```
![alt text](demo.gif)

## 🧪 Running Tests
Unit tests are located in the `RO.DevTest.Tests.Unit` project. To run them:

``` bash
dotnet test
```

## 📦 Running the Project

1. Clone the repository:
``` bash
git clone https://github.com/TamaraMoraisF/RO.DevTest.git
```

2. Navigate to the project folder:

``` bash
cd RO.DevTest
```

3. Restore dependencies:

``` bash
dotnet restore
```

4. Apply database migrations:

``` bash
dotnet ef database update
```

5. Start the application:

``` bash
dotnet run --project RO.DevTest.WebApi
```

The application will be available at `https://localhost:7014`.

## 📝 Notes

- Make sure to configure the database connection string in the `appsettings.json` file.
- To authenticate in Swagger, click "Authorize" and insert the JWT token using the format: `Bearer {your_token}`.