using Dapper;
using Microsoft.Data.SqlClient;
using ProductsFunctionApp.Dto;
using ProductsFunctionApp.Models;
using System;
using System.Threading.Tasks;

namespace ProductsFunctionApp.Repository
{
    public static class ProductsRepository
    {
        public static async Task<Product> GetById(FindProductDto dto)
        {
            using (var connection = new SqlConnection(
                Environment.GetEnvironmentVariable("ConnectionString")))
            {
                var sql = @"
                    SELECT * FROM dbo.Products
                    WHERE Id = @Id
                      AND CompanyId = @CompanyId
                ";

                return await connection.QueryFirstOrDefaultAsync<Product>(sql, dto);
            }
        }

        public static async Task<int> Add(CreateProductDto dto)
        {
            using (var connection = new SqlConnection(
                Environment.GetEnvironmentVariable("ConnectionString")))
            {
                var sql = @"
                    INSERT INTO dbo.Products
                    (CompanyId, Name, BrandName)
                    OUTPUT INSERTED.[Id]
                    VALUES(@CompanyId, @Name, @BrandName)
                ";

                return await connection.QuerySingleAsync<int>(sql, dto);
            }
        }

        public static async Task<bool> Delete(FindProductDto dto)
        {
            using (var connection = new SqlConnection(
                Environment.GetEnvironmentVariable("ConnectionString")))
            {
                var sql = @"
                    DELETE FROM dbo.Products
                    WHERE Id = @Id
                      AND CompanyId = @CompanyId
                ";

                var rowsAffected = await connection.ExecuteAsync(sql, dto);
                return rowsAffected > 0;
            }
        }

        public static async Task<bool> Update(Product product)
        {
            using (var connection = new SqlConnection(
                Environment.GetEnvironmentVariable("ConnectionString")))
            {
                var sql = @"
                    UPDATE dbo.Products
                    SET Name = @Name, 
                        BrandName = @BrandName
                    WHERE Id = @Id
                      AND CompanyId = @CompanyId
                ";

                var rowsAffected = await connection.ExecuteAsync(sql, product);
                return rowsAffected > 0;
            }
        }
    }

}
