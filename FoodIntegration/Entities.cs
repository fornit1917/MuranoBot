using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodIntegration
{
	[Table("Menu")]
	public class Menu
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public string Weight { get; set; }
		public int SupplierId { get; set; }
	}

	[Table("Orders")]
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int MenuId { get; set; }
		public int Count { get; set; }
		public int Confirmed { get; set; }
		public int Ordered { get; set; }
	}

	[Table("User")]
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsDomain { get; set; }
		public bool IsAdmin { get; set; }
		public int CompanyId { get; set; }
		public string DomainIdentityName { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public bool IsDisabled { get; set; }
		public string Login { get; set; }
		public string FullNameRus { get; set; }
	}
}