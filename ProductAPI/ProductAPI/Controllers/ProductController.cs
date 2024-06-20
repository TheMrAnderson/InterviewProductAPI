using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
	/// <summary>
	/// Handles the product CRUD operations
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		/// <summary>
		/// Returns a list of products
		/// </summary>
		/// <param name="searchName">Optional: Search name field</param>
		/// <param name="searchCategory">Optional: Search category field</param>
		/// <param name="searchPriceLow">Optional: Search price using this as the lowest value</param>
		/// <param name="searchPriceHigh">Optional: Search price using this as the highest value</param>
		/// <param name="sortOrder">Optional: Defaults to name,category</param>
		/// <param name="page">Optional: Page Number</param>
		/// <param name="limit">Optional: Record limit to return. Defaults to 50</param>
		/// <returns></returns>
		[Authorize(Roles = "SuperAdmin,Admin,User")]
		[HttpGet]
		[ProducesResponseType<List<Product>>(StatusCodes.Status200OK)]
		[ProducesResponseType<ApiError>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get([Optional][FromQuery] string? searchName,
			[Optional][FromQuery] string? searchCategory,
			[Optional][FromQuery] decimal? searchPriceLow,
			[Optional][FromQuery] decimal? searchPriceHigh,
			[Optional][FromQuery] string? sortOrder,
			[Optional][FromQuery] uint? page,
			[Optional][FromQuery] uint? limit)
		{
			try
			{
				var products = await ProductRepository.GetProducts(searchName, searchCategory, searchPriceLow, searchPriceHigh, sortOrder, page, limit);
				if (products == null || products.Count() == 0)
					return NotFound();
				return Ok(products);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error in ProductController Get",
					searchName, searchCategory, searchPriceLow.ToString(), searchPriceHigh.ToString(), sortOrder, page.ToString(), limit.ToString());
				return new ApiError(StatusCodes.Status500InternalServerError, "Error getting products");
			}
		}

		/// <summary>
		/// Create a new product
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		[Authorize(Roles = "SuperAdmin,Admin")]
		[HttpPost]
		[ProducesResponseType<Product>(StatusCodes.Status201Created)]
		[ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create(Product product)
		{
			try
			{
				var result = await ProductRepository.CreateProduct(product);
				return Created("", product);
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error in ProductController Create", product.ToString());
				return new ApiError(StatusCodes.Status500InternalServerError, "Error creating products");
			}
		}

		/// <summary>
		/// Update a product
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		[Authorize(Roles = "SuperAdmin,Admin")]
		[HttpPut]
		public async Task<IActionResult> Update(Product product)
		{
			try
			{
				var result = await ProductRepository.UpdateProduct(product);
				return result != null ? Ok(result) : BadRequest();
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error in ProductController Update", product.ToString());
				return new ApiError(StatusCodes.Status500InternalServerError, "Error updating product");
			}
		}

		/// <summary>
		/// Delete a product
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Authorize(Roles = "SuperAdmin")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var result = await ProductRepository.DeleteProduct(id);
				return result ? Ok() : NotFound();
			}
			catch (Exception ex)
			{
				Logger.Exception(ex, "Error in ProductController Delete", id.ToString());
				return new ApiError(StatusCodes.Status500InternalServerError, "Error deleting product");
			}
		}
	}
}
