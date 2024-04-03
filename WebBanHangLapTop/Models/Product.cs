using System.ComponentModel.DataAnnotations;
//thể hiện không gian chứa các lớp.Quy định các lớp nhìn thấy nhau trong cùng1  không gian
namespace WebBanHangLapTop.Models
{
    //Product.cs
    public class Product
    {

		public int Id { get; set; }
		[Required, StringLength(100)]
		public string Name { get; set; }
		[Range(0.01, 10000.00)]
		public decimal Price { get; set; }
		public string Description { get; set; }
		public string? ImageUrl { get; set; }
		public List<ProductImage>? Images { get; set; }
		public int  CategoryId { get; set; }
		public   Category ?   Category { get; set; }

	}
}
