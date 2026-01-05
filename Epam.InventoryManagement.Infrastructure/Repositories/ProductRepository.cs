using Epam.InventoryManagement.Domain;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Epam.InventoryManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _conn;

        public ProductRepository(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> AddAsync(Product product)
        {
            using SqlConnection conn = new(_conn);
            string sql = @"INSERT INTO Products
                           (Name,Category,Price,Quantity,IsActive,CreatedDate)
                           VALUES(@Name,@Category,@Price,@Quantity,1,GETDATE());
                           SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

            await conn.OpenAsync();
            int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            Log.Information("Product added: {Id}", id);
            return id;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(
                "UPDATE Products SET Name=@Name,Category=@Category,Price=@Price,Quantity=@Quantity WHERE ProductId=@Id",
                conn);

            cmd.Parameters.AddWithValue("@Id", product.ProductId);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            Log.Information("Product updated: {Id}", product.ProductId);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(
                "UPDATE Products SET IsActive=0 WHERE ProductId=@Id",
                conn);

            cmd.Parameters.AddWithValue("@Id", productId);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            Log.Information("Product deleted: {Id}", productId);
            return rows > 0;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(
                "SELECT * FROM Products WHERE ProductId=@Id AND IsActive=1",
                conn);

            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            List<Product> list = new();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM Products WHERE IsActive=1", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
                list.Add(Map(reader));

            return list;
        }

        public async Task<List<Product>> SearchAsync(string keyword)
        {
            List<Product> list = new();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(
                "SELECT * FROM Products WHERE IsActive=1 AND (Name LIKE @k OR Category LIKE @k)",
                conn);

            cmd.Parameters.AddWithValue("@k", $"%{keyword}%");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
                list.Add(Map(reader));

            Log.Information("Search executed: {Keyword}", keyword);
            return list;
        }

        private Product Map(SqlDataReader r) => new()
        {
            ProductId = (int)r["ProductId"],
            Name = r["Name"].ToString()!,
            Category = r["Category"].ToString()!,
            Price = (decimal)r["Price"],
            Quantity = (int)r["Quantity"],
            IsActive = (bool)r["IsActive"],
            CreatedDate = (DateTime)r["CreatedDate"]
        };
    }
}
