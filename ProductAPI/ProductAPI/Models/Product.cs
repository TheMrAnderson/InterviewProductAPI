using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Models
{
	/// <summary>
	/// Product object
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Product()
		{
			Name = "Unknown";
			Category = "Unknown";
		}

		/// <summary>
		/// Constructor to create a new product when the name, price and category are known
		/// </summary>
		/// <param name="name">Product Name</param>
		/// <param name="price">Product Price</param>
		/// <param name="category">Product Price</param>
		public Product(string name, decimal price, string category)
		{
			Name = name;
			Price = price;
			Category = category;
		}

		/// <summary>
		/// Product Id
		/// </summary>
		[Required]
		public int Id { get; set; }

		/// <summary>
		/// Product Name
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Product Price
		/// </summary>
		[Required]
		public decimal Price { get; set; }

		/// <summary>
		/// Product category
		/// </summary>
		[Required]
		public string Category { get; set; }

		/// <summary>
		/// Override of ToString to list out all the properties
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Price)}: {Price}, {nameof(Category)}: {Category}";
		}
	}
}
