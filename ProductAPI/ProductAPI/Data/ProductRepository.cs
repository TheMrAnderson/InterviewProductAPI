using System.Data.SQLite;
using System.Reflection;
using System.Text;
using Dapper;
using ProductAPI.Models;
using Z.Dapper.Plus;

namespace ProductAPI.Data
{
	/// <summary>
	/// Handles the product data
	/// </summary>
	public static class ProductRepository
	{
		private static readonly string _connectionString = "Data Source=DB.db";

		private static string GetSortClause(string? sortOrder)
		{
			const string defaultOrderByClause = "ORDER BY name,category";
			if (sortOrder == null)
				return defaultOrderByClause;

			var orderParams = sortOrder.Trim().Split(',');
			var propertyInfos = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var sb = new StringBuilder();

			foreach (var param in orderParams)
			{
				if (string.IsNullOrWhiteSpace(param))
					continue;

				var propertyFromQueryName = param.Split(" ")[0];
				var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
				if (objectProperty == null)
					continue;

				var sortingOrder = param.EndsWith("desc") ? "DESC" : "";
				sb.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
			}

			var orderByClause = sb.ToString().TrimEnd(',', ' ');

			if (string.IsNullOrWhiteSpace(orderByClause))
				return defaultOrderByClause;
			return $"ORDER BY {orderByClause}";
		}

		private static string GetLimitClause(uint? page, uint? limit)
		{
			limit ??= 50;
			if (limit > 50)
				limit = 50;

			var pageClause = $"LIMIT {limit}";
			if (page != null)
				pageClause += $" OFFSET {page * limit}";
			return pageClause;
		}

		/// <summary>
		/// Return a list of products based on provided data
		/// </summary>
		/// <param name="searchName">Optional: Search name field</param>
		/// <param name="searchCategory">Optional: Search category field</param>
		/// <param name="searchPriceLow">Optional: Search price using this as the lowest value</param>
		/// <param name="searchPriceHigh">Optional: Search price using this as the highest value</param>
		/// <param name="sortOrder">Optional: Defaults to name,category</param>
		/// <param name="page">Optional: Page Number</param>
		/// <param name="limit">Optional: Record limit to return. Defaults to 50</param>
		/// <returns></returns>
		/// <returns></returns>
		public static async Task<IEnumerable<Product>> GetProducts(
			string? searchName, string? searchCategory, decimal? searchPriceLow, decimal? searchPriceHigh, string? sortOrder, uint? page, uint? limit)
		{
			try
			{

				var limitClause = GetLimitClause(page, limit);
				var sortClause = GetSortClause(sortOrder);

				using (var conn = new SQLiteConnection(_connectionString))
				{
					var sql = $"SELECT * FROM Product " +
						"WHERE (name like @searchName OR @searchName IS NULL) " +
						"AND (category LIKE @searchCategory OR @searchCategory IS NULL) " +
						"AND (price >= @searchPriceLow OR @searchPriceLow IS NULL) " +
						"AND (price <= @searchPriceHigh OR @searchPriceHigh IS NULL) " +
						$"{sortClause} {limitClause};";
					var products = await conn.QueryAsync<Product>(sql, new
					{
						searchName = $"%{searchName}%",
						searchCategory = $"%{searchCategory}%",
						searchPriceLow,
						searchPriceHigh
					});
					return products;
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error getting product records",
					searchName, searchCategory, searchPriceLow.ToString(), searchPriceHigh.ToString(), sortOrder, page.ToString(), limit.ToString());
				return Enumerable.Empty<Product>();
			}
		}

		/// <summary>
		/// Create new product record
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		public static async Task<Product?> CreateProduct(Product product)
		{
			try
			{
				DapperPlusManager.Entity<Product>().Identity(i => i.Id);
				using (var conn = new SQLiteConnection(_connectionString))
				{
					var result = await conn.BulkInsertAsync(product);
					return product;
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error creating product record", product.ToString());
				return null;
			}
		}

		/// <summary>
		/// Delete product record
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static async Task<bool> DeleteProduct(int id)
		{
			try
			{
				using (var conn = new SQLiteConnection(_connectionString))
				{
					var sql = "DELETE FROM Product WHERE id=@id";
					var affectedRows = await conn.ExecuteAsync(sql, new { id });
					return affectedRows > 0;
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error deleting product record", id.ToString());
				return false;
			}
		}

		/// <summary>
		/// Update product record
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		public static async Task<Product?> UpdateProduct(Product product)
		{
			try
			{
				DapperPlusManager.Entity<Product>().Identity(i => i.Id);
				using (var conn = new SQLiteConnection(_connectionString))
				{
					var result = await conn.BulkUpdateAsync(product);
					return product;
				}
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error updating product record", product.ToString());
				return null;
			}
		}
	}
}
