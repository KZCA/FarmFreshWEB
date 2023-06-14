using FarmFreshWEB.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmFreshWEB.Models
{
	public class ProductModel
	{
		public long Id { get; set; }
        [Required(ErrorMessage = "Please Enter a value")]
        public string Name { get; set; }
		public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Please Enter a value")]
        public string Description { get; set; }
		public string Country { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "You must choose a category")]
        public long CategoryId { get; set; }		
		public CategoryModel Category { get; set; }
		public string Image { get; set; }
		[NotMapped]
		[FileExtension]
        public IFormFile ImageUpload { get; set; }
        public ProductModel()
		{
			Category = new CategoryModel();
		}
	}

	public class ProductPaginationModel
	{
		public List<ProductModel> data { get; set; }
		public int pageIndex { get; set; }
        public int totalData { get; set; }
        public int totalPages { get; set; }
    }
}
