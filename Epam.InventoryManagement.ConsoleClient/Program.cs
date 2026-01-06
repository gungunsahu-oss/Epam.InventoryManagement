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
            Console.WriteLine("====================================================");
            Console.WriteLine("              INVENTORY MANAGEMENT SYSTEM");
            Console.WriteLine("====================================================");
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
        Console.WriteLine("====================================================");
        Console.WriteLine("              INVENTORY MANAGEMENT SYSTEM");
        Console.WriteLine("====================================================");
        Console.WriteLine($" {title}");
        Console.WriteLine("----------------------------------------------------");
    }

    static void TableHeader()
    {
        Console.WriteLine(
            $"{"ID",-4} {"Name",-15} {"Category",-15} {"Price",12} {"Qty",8}");
        Console.WriteLine(new string('-', 58));
    }

    static string Cut(string text, int max)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Length <= max ? text : text.Substring(0, max - 3) + "...";
    }

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

        Console.Write("Name      : ");
        dto.Name = Console.ReadLine();

        Console.Write("Category  : ");
        dto.Category = Console.ReadLine();

        Console.Write("Price     : ");
        dto.Price = decimal.Parse(Console.ReadLine()!);

        Console.Write("Quantity  : ");
        dto.Quantity = int.Parse(Console.ReadLine()!);

        Console.Write("\nSave product? (Y/N): ");
        if (!Console.ReadLine()!.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            Pause();
            return;
        }

        var response = await client.PostAsJsonAsync("api/products", dto);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "\nProduct added successfully."
            : "\nError adding product.");

        Pause();
    }

    // ---------------- VIEW ALL ----------------
    static async Task ViewAllProducts(HttpClient client)
    {
        Header("Product List");

        var products = await client.GetFromJsonAsync<ProductReadDto[]>("api/products");

        if (products == null || products.Length == 0)
        {
            Console.WriteLine("No products found.");
            Pause();
            return;
        }

        TableHeader();

        foreach (var p in products)
        {
            Console.WriteLine(
                $"{p.ProductId,-4} " +
                $"{Cut(p.Name, 15),-15} " +
                $"{Cut(p.Category, 15),-15} " +
                $"{p.Price,12:F2} " +
                $"{p.Quantity,8}");
        }

        Console.WriteLine(new string('-', 58));
        Console.WriteLine($"Total Products: {products.Length}");

        Pause();
    }

    // ---------------- VIEW BY ID ----------------
    static async Task ViewProductById(HttpClient client)
    {
        Header("View Product by ID");

        Console.Write("Enter Product ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            Pause();
            return;
        }

        var response = await client.GetAsync($"api/products/{id}");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Product not found.");
            Pause();
            return;
        }

        var p = await response.Content.ReadFromJsonAsync<ProductReadDto>();

        TableHeader();
        Console.WriteLine(
            $"{p!.ProductId,-4} " +
            $"{Cut(p.Name, 15),-15} " +
            $"{Cut(p.Category, 15),-15} " +
            $"{p.Price,12:F2} " +
            $"{p.Quantity,8}");

        Pause();
    }

    // ---------------- UPDATE ----------------
    static async Task UpdateProduct(HttpClient client)
    {
        Header("Update Product");

        Console.Write("Enter Product ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            Pause();
            return;
        }

        var dto = new ProductUpdateDto();

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
            : "\nUpdate failed.");

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

        Console.Write("Enter search text (name/category): ");
        var query = Console.ReadLine()!.ToLower();

        var products = await client.GetFromJsonAsync<ProductReadDto[]>("api/products");

        var results = products!
            .Where(p =>
                p.Name!.ToLower().Contains(query) ||
                p.Category!.ToLower().Contains(query))
            .ToList();

        if (!results.Any())
        {
            Console.WriteLine("No matching products found.");
            Pause();
            return;
        }

        TableHeader();

        foreach (var p in results)
        {
            Console.WriteLine(
                $"{p.ProductId,-4} " +
                $"{Cut(p.Name, 15),-15} " +
                $"{Cut(p.Category, 15),-15} " +
                $"{p.Price,12:F2} " +
                $"{p.Quantity,8}");
        }

        Console.WriteLine(new string('-', 58));
        Console.WriteLine($"Total Results: {results.Count}");

        Pause();
    }
}

