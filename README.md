## 1. Features

- **User Authentication and Registration:**
  - Uses ASP.NET Identity to manage users.
  - Registration validates email, password (minimum 6 characters with confirmation), and personal data.
  - Login displays temporary error messages for invalid credentials.
  - Custom password hasher using MD5 with SALT "Dificil_" (for academic purposes only).

- **Product Management:**
  - Displays a product list from a MySQL database.
  - Detailed view for each product, including an image (if available), price, and category.
  - Option to add products to the cart from the detail view.

- **Personalized Shopping Cart:**
  - Each logged-in user has their own cart, managed by a concurrent dictionary mapping user IDs to carts.
  - Supports adding, removing, and updating product quantities.
  - Cart data is stored in a MySQL table with an `OwnerId` column to associate each record with the correct user.

- **Modern and Responsive UI:**
  - Custom CSS creates a modern, responsive design.
  - Two layouts: a minimal layout for authentication (no menu or footer) and a full layout for the rest of the application.
  - Supports animations and temporary alert messages for a better user experience.

- **Security:**
  - Validates and sanitizes inputs.
  - Uses parameterized queries to protect against SQL Injection.
  - Includes antiforgery tokens in all forms.
  - Configures ASP.NET Identity for secure password handling (using MD5 here for demonstration).

## 2. Requirements

- [.NET 6 or later](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB for Identity
- [MySQL](https://www.mysql.com/) for product and cart data
- [dotnet-ef](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) command-line tool
- A modern web browser

## 3. Setup & Installation

### 3.1 Clone the Repository

```bash
git clone <repository_URL>
cd ShopApp
```

### 3.2 Configure the Connection String

Edit **appsettings.json** in the project root to set the correct SQL Server connection string for Identity. For example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=ShopEaseIdentity;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3.3 Run EF Core Migrations (ASP.NET Identity)

Install the EF Core tools if needed:

```bash
dotnet tool install --global dotnet-ef
```

Then create the migration and update the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3.4 Create the Product and Cart Tables in MySQL

Run this SQL script in your MySQL server:

```sql
CREATE DATABASE IF NOT EXISTS shop;
USE shop;

DROP TABLE IF EXISTS Productos;
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    ImagenUrl VARCHAR(255)
);

DROP TABLE IF EXISTS Carrito;
CREATE TABLE Carrito (
    ProductoID INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    Cantidad INT NOT NULL,
    OwnerId VARCHAR(450) NOT NULL,
    PRIMARY KEY (ProductoID, OwnerId)
);
```

### 3.5 Build and Run

```bash
dotnet run
```

Open your browser at `http://localhost:<port>` to see the login/registration page. Once authenticated, you can browse products and manage your personalized cart.

## 4. Project Structure

- **Controllers:**
  - `AccountController.cs`: Manages user login and registration.
  - `CarritoController.cs`: Manages each user’s shopping cart via a concurrent dictionary.
  - `ProductosController.cs`: Displays the product list and detailed views, and lets you add products to the cart.

- **Models:**
  - `ApplicationUser.cs`: Extended user model for ASP.NET Identity.
  - `LoginViewModel.cs` & `RegisterViewModel.cs`: Models for login and registration data.
  - `Producto.cs`: Represents a product.
  - `Carrito.cs` & `LineaCarrito.cs`: Handle shopping cart logic.

- **Views:**
  - **Account:** Login and registration views, using the minimal _LayoutAuth.cshtml_ layout.
  - **Productos:** Product list and detail views.
  - **Carrito:** Shopping cart page that shows all items.
  - **Shared:** Common layouts, including _LayoutAuth.cshtml_ and _Layout.cshtml_.

- **Data:**
  - `ApplicationDbContext.cs`: EF Core context for ASP.NET Identity.

- **Security:**
  - `CustomPasswordHasher.cs`: Uses MD5 with SALT "Dificil_" for demonstration.

## 5. Usage

1. **Authentication:** Log in or register. Invalid credentials trigger a temporary error message.
2. **Product Catalog:** Browse the list of products, view details, and add items to your cart.
3. **Shopping Cart:** Each user has a separate cart. You can remove products or update quantities.

## 6. Author

**Esteban M. Cruz Seoane**

## 7. License

This project is licensed under the MIT License.

```
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
