using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Epam.InventoryManagement.Application.DTOs;

class Program
{
    private const string API_BASE_URL = "https://localhost:5001/";

    static async Task Main()
    {
        using var client = new HttpClient { BaseAddress = new Uri(API_BASE_URL) };

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("        INVENTORY MANAGEMENT SYSTEM");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. View Product by ID");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Delete Product");
            Console.WriteLine("6. Search Products");
            Console.WriteLine("7. Exit");
            Console.Write("\nEnter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                continue;

            switch (choice)
            {
                case 1: await AddProduct(client); break;
                case 2: await ViewAllProducts(client); break;
                case 3: await ViewProductById(client); break;
                case 4: await UpdateProduct(client); break;
                case 5: await DeleteProduct(client); break;
                case 6: await SearchProducts(client); break;
                case 7: return;
            }
        }
    }

    // ---------------- UI HELPERS ----------------
    static void Header(string title)
    {
        Console.Clear();
        Console.WriteLine("==============================================");
        Console.WriteLine("        INVENTORY MANAGEMENT SYSTEM");
        Console.WriteLine("==============================================\n");
        Console.WriteLine($"------------ {title} ------------\n");
    }

    static void Line() => Console.WriteLine("----------------------------------------------");

    static void Pause()
    {
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ReadKey();
    }

    // ---------------- ADD PRODUCT ----------------
    static async Task AddProduct(HttpClient client)
    {
        Header("Add New Product");

        var dto = new ProductCreateDto();

        Console.Write("Enter Product Name      : ");
        dto.Name = Console.ReadLine();

        Console.Write("Enter Product Category  : ");
        dto.Category = Console.ReadLine();

        Console.Write("Enter Product Price     : ");
        dto.Price = decimal.Parse(Console.ReadLine()!);

        Console.Write("Enter Product Quantity  : ");
        dto.Quantity = int.Parse(Console.ReadLine()!);

        Line();
        Console.WriteLine("1. Save Product");
        Console.WriteLine("2. Cancel");
        Console.Write("\nEnter your choice: ");

        if (Console.ReadLine() != "1")
        {
            Console.WriteLine("Operation cancelled.");
            Pause();
            return;
        }

        var response = await client.PostAsJsonAsync("api/products", dto);

        Line();
        Console.WriteLine(response.IsSuccessStatusCode
            ? "Product added successfully."
            : "Error adding product.");

        Pause();
    }

    // ---------------- VIEW ALL ----------------
    static async Task ViewAllProducts(HttpClient client)
    {
        Header("Product List");

        var products = await client.GetFromJsonAsync<ProductReadDto[]>("api/products");

        Console.WriteLine("ID   Name        Category        Price     Quantity");
        Line();

        foreach (var p in products!)
            Console.WriteLine($"{p.ProductId,-4} {p.Name,-10} {p.Category,-15} {p.Price,-9} {p.Quantity}");

        Line();
        Console.WriteLine($"Total Products: {products.Length}");

        Pause();
    }

    // ---------------- VIEW BY ID ----------------
    static async Task ViewProductById(HttpClient client)
    {
        Header("View Product");

        Console.Write("Enter Product ID: ");
        int id = int.Parse(Console.ReadLine()!);

        var response = await client.GetAsync($"api/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Product not found.");
            Pause();
            return;
        }

        var p = await response.Content.ReadFromJsonAsync<ProductReadDto>();

        Console.WriteLine("\nID   Name        Category        Price     Quantity");
        Line();
        Console.WriteLine($"{p!.ProductId,-4} {p.Name,-10} {p.Category,-15} {p.Price,-9} {p.Quantity}");

        Pause();
    }

    // ---------------- UPDATE ----------------
    static async Task UpdateProduct(HttpClient client)
    {
        Header("Update Product");

        Console.Write("Enter Product ID: ");
        int id = int.Parse(Console.ReadLine()!);

        var dto = new ProductUpdateDto { ProductId = id };

        Console.Write("New Name      : ");
        dto.Name = Console.ReadLine();

        Console.Write("New Category  : ");
        dto.Category = Console.ReadLine();

        Console.Write("New Price     : ");
        dto.Price = decimal.Parse(Console.ReadLine()!);

        Console.Write("New Quantity  : ");
        dto.Quantity = int.Parse(Console.ReadLine()!);

        var response = await client.PutAsJsonAsync($"api/products/{id}", dto);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "\nProduct updated successfully."
            : "\nError updating product.");

        Pause();
    }

    // ---------------- DELETE ----------------
    static async Task DeleteProduct(HttpClient client)
    {
        Header("Delete Product");

        Console.Write("Enter Product ID: ");
        int id = int.Parse(Console.ReadLine()!);

        var response = await client.DeleteAsync($"api/products/{id}");

        Console.WriteLine(response.IsSuccessStatusCode
            ? "\nProduct deleted successfully."
            : "\nError deleting product.");

        Pause();
    }

    // ---------------- SEARCH ----------------
    static async Task SearchProducts(HttpClient client)
    {
        Header("Search Products");

        Console.WriteLine("1. Search by Name");
        Console.WriteLine("2. Search by Category");
        Line();
        Console.Write("Enter your choice: ");
        Console.ReadLine();

        Console.Write("Enter search text (partial allowed): ");
        var query = Console.ReadLine()!.ToLower();

        var products = await client.GetFromJsonAsync<ProductReadDto[]>("api/products");

        var results = products!
            .Where(p => p.Name!.ToLower().Contains(query) ||
                        p.Category!.ToLower().Contains(query))
            .ToList();

        Console.WriteLine("\nID   Name        Category        Price     Quantity");
        Line();

        foreach (var p in results)
            Console.WriteLine($"{p.ProductId,-4} {p.Name,-10} {p.Category,-15} {p.Price,-9} {p.Quantity}");

        Line();
        Console.WriteLine($"Total Results: {results.Count}");

        Pause();
    }
}
