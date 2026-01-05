using Epam.InventoryManagement.ConsoleClient.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

// ---------------- CONFIG ----------------
const string API_BASE_URL = "https://localhost:5001/"; // change port if needed

HttpClient client = new()
{
    BaseAddress = new Uri(API_BASE_URL)
};

// ----------------------------------------

Console.WriteLine("===== EPAM Inventory Management System =====");

while (true)
{
    try
    {
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Add Product");
        Console.WriteLine("2. View All Products");
        Console.WriteLine("3. View Product by ID");
        Console.WriteLine("4. Update Product");
        Console.WriteLine("5. Delete Product");
        Console.WriteLine("6. Search Products");
        Console.WriteLine("7. Exit");

        Console.Write("Enter choice: ");
        int choice = int.Parse(Console.ReadLine()!);

        switch (choice)
        {
            case 1:
                await AddProduct();
                break;

            case 2:
                await ViewAllProducts();
                break;

            case 3:
                await ViewProductById();
                break;

            case 4:
                await UpdateProduct();
                break;

            case 5:
                await DeleteProduct();
                break;

            case 6:
                await SearchProducts();
                break;

            case 7:
                Console.WriteLine("Exiting application...");
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// ---------------- METHODS ----------------

async Task AddProduct()
{
    Console.Write("Name: ");
    string name = Console.ReadLine()!;

    Console.Write("Category: ");
    string category = Console.ReadLine()!;

    Console.Write("Price: ");
    decimal price = decimal.Parse(Console.ReadLine()!);

    Console.Write("Quantity: ");
    int quantity = int.Parse(Console.ReadLine()!);

    var product = new
    {
        Name = name,
        Category = category,
        Price = price,
        Quantity = quantity
    };

    var response = await client.PostAsJsonAsync("api/products", product);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Product added successfully.");
    else
        Console.WriteLine("Failed to add product.");
}

async Task ViewAllProducts()
{
    var products = await client.GetFromJsonAsync<List<ProductDto>>("api/products");

    if (products == null || products.Count == 0)
    {
        Console.WriteLine("No products found.");
        return;
    }

    Console.WriteLine("\n--- Product List ---");
    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Category} | ₹{p.Price} | Qty: {p.Quantity}");
    }
}

async Task ViewProductById()
{
    Console.Write("Enter Product ID: ");
    int id = int.Parse(Console.ReadLine()!);

    var response = await client.GetAsync($"api/products/{id}");

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    var product = await response.Content.ReadFromJsonAsync<ProductDto>();

    Console.WriteLine("\n--- Product Details ---");
    Console.WriteLine($"ID       : {product!.ProductId}");
    Console.WriteLine($"Name     : {product.Name}");
    Console.WriteLine($"Category : {product.Category}");
    Console.WriteLine($"Price    : ₹{product.Price}");
    Console.WriteLine($"Quantity : {product.Quantity}");
}

async Task UpdateProduct()
{
    Console.Write("Product ID: ");
    int id = int.Parse(Console.ReadLine()!);

    Console.Write("New Name: ");
    string name = Console.ReadLine()!;

    Console.Write("New Category: ");
    string category = Console.ReadLine()!;

    Console.Write("New Price: ");
    decimal price = decimal.Parse(Console.ReadLine()!);

    Console.Write("New Quantity: ");
    int quantity = int.Parse(Console.ReadLine()!);

    var product = new
    {
        ProductId = id,
        Name = name,
        Category = category,
        Price = price,
        Quantity = quantity
    };

    var response = await client.PutAsJsonAsync("api/products", product);

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Product updated successfully.");
    else
        Console.WriteLine("Update failed.");
}

async Task DeleteProduct()
{
    Console.Write("Enter Product ID to delete: ");
    int id = int.Parse(Console.ReadLine()!);

    var response = await client.DeleteAsync($"api/products/{id}");

    if (response.IsSuccessStatusCode)
        Console.WriteLine("Product deleted successfully.");
    else
        Console.WriteLine("Delete failed.");
}

async Task SearchProducts()
{
    Console.Write("Enter name or category: ");
    string keyword = Console.ReadLine()!;

    var products = await client.GetFromJsonAsync<List<ProductDto>>(
        $"api/products/search?keyword={keyword}");

    if (products == null || products.Count == 0)
    {
        Console.WriteLine("No matching products found.");
        return;
    }

    Console.WriteLine("\n--- Search Results ---");
    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Category}");
    }
}
