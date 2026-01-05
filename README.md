
🏢 Epam Inventory Management System



Epam Inventory Management System** is a **console-based inventory management application** that allows efficient tracking and management of warehouse products. The system supports CRUD operations, searching, logging, and communicates with a Web API backend using JSON.


 🌟 Features

* **Add Products** – Easily add new products with details like Name, Category, Price, Quantity.
* **View Products** – Retrieve all products or a product by its ID.
* **Update Products** – Update existing product details.
* **Delete Products** – Soft delete products from inventory.
* **Search Products** – Search products by name or category.
* **Logging** – Logs all operations and errors using **Serilog**.
* **User-Friendly Console UI** – Menu-driven interface for smooth navigation.

---
🛠️ Tech Stack

| Layer               | Technology / Tool              |
| ------------------- | ------------------------------ |
| **Backend / API**   | ASP.NET Core Web API (.NET 10) |
| **Client**          | Console Application (.NET 10)  |
| **Data Access**     | ADO.NET                        |
| **Database**        | SQL Server                     |
| **Logging**         | Serilog (file-based)           |
| **Serialization**   | System.Text.Json               |
| **Testing / Tools** | Swagger, Postman               |

---

## 📂 Project Structure

```
Epam.InventoryManagement
│
├── Epam.InventoryManagement.API          # Web API Layer
├── Epam.InventoryManagement.Application  # Business logic, DTOs, Services
├── Epam.InventoryManagement.Domain       # Entities & Repository interfaces
├── Epam.InventoryManagement.Infrastructure # ADO.NET Repositories, Logging
├── Epam.InventoryManagement.Client       # Console Application (Menu-driven)
├── Epam.InventoryManagement.Tests        # Unit & Integration Tests
└── README.md
```

---

## ⚙️ Setup & Installation

1. **Clone the repository**

```bash
git clone https://github.com/gungunsahu-oss/Epam.InventoryManagement.git
cd Epam.InventoryManagement
```

2. **Configure the database**

* Create a database named `EpamInventoryDB`.
* Run the following SQL script to create the **Products** table:

```sql
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

3. **Update `appsettings.json` in API Layer**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EpamInventoryDB;Trusted_Connection=True;"
  },
  "Logging": {
    "Serilog": {
      "MinimumLevel": "Information",
      "WriteTo": [
        { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
      ]
    }
  }
}
```

4. **Build and run**

* **API Layer:** Run the Web API (`dotnet run`)
* **Client Layer:** Run the Console Application (`dotnet run`)

---

## 📋 Usage (Console Menu)

```
================= Inventory Management System =================
1. Add Product
2. View All Products
3. View Product by ID
4. Update Product
5. Delete Product
6. Search Products
7. Exit Application
==============================================================
```

* **Input Validation**: Ensures all fields are filled, numeric fields are valid, and invalid menu selections are handled gracefully.
* **Error Handling**: Handles both client-side (invalid input/menu) and API errors (API unreachable, failure responses).

---

## 🔄 CRUD Workflows

* **Add Product:** User inputs product info → Console sends JSON to API → API saves product in DB → Logs operation.
* **View Products:** API fetches data from DB → Returns JSON → Console displays list.
* **Update Product:** User selects product → Updates fields → API updates DB → Logs operation.
* **Delete Product:** Soft delete by setting `IsActive = 0` → Logs operation.
* **Search Product:** Console sends query → API returns filtered results → Display in console.

---

## 🖥️ Sample Console Output


===== EPAM Inventory Management System =====
1. Add Product
2. View All Products
3. View Product by ID
4. Update Product
5. Delete Product
6. Search Products
7. Exit
Enter choice: 1

Name: IPhone
Category: Electronics
Price: 1234567
Quantity: 67
✔ Product added successfully.

===== EPAM Inventory Management System =====
Enter choice: 2

ID: 1, Name: Laptop, Category: Electronics, Price: 87.00, Qty: 87
ID: 4, Name: PEN, Category: Stationary, Price: 5678.00, Qty: 678
ID: 5, Name: Charger, Category: Electronics, Price: 6.00, Qty: 8
ID: 6, Name: IPhone, Category: Electronics, Price: 1234567.00, Qty: 67


## 🧩 Logging

* All operations (Add, Update, Delete, Search) and exceptions are logged using **Serilog**.
* Logs stored in **log files** for auditing.

---

## 🔬 API Testing

* **Swagger**: Interactive endpoint testing.
* **Postman**: Manual and automated API testing.
* Validation: CRUD operations, search functionality, and error scenarios.






