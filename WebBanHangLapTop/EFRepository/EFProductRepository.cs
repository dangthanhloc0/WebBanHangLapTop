using Microsoft.EntityFrameworkCore;
using WebBanHangLapTop.Models;
using WebBanHangLapTop.Repository;

public class EFProductRepository : IProductRepository
{
	private readonly ApplicationDbContext _context;
	public EFProductRepository(ApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<IEnumerable<Product>> GetAllAsync()
	{
		return await _context.Products.ToListAsync();
	}


	public async Task<IEnumerable<ProductImage>> GetAllImagesById(int id)
	{
		return await _context.ProductImages.Where(p=>p.ProductId==id).ToListAsync();
	}
	public async Task<Product> GetByIdAsync(int id)
	{
		return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id) ;
	}



	public async Task AddAsync(Product product,List<IFormFile> Images)
	{

		_context.Products.Add(product);    	 
        await _context.SaveChangesAsync();


		if (Images != null)
		{

			foreach (var file in Images)
			{
				int id = product.Id;
				ProductImage pd = new ProductImage();
				pd.ProductId = id;
				pd.Url = await SaveImage(file);
				_context.ProductImages.Add(pd);
				await _context.SaveChangesAsync();

			}
		}

	}

	private async Task<string> SaveImage(IFormFile image)
	{
		var savePath = Path.Combine("wwwroot/Image", image.FileName);
		using (var fileStream = new FileStream(savePath, FileMode.Create))
		{
			await image.CopyToAsync(fileStream);
		}
		return "/Image/" + image.FileName;
	}
	public async Task UpdateAsync(Product product)
	{
		_context.Products.Update(product);
		await _context.SaveChangesAsync();
	}
	public async Task DeleteAsync(int id)
	{
		var product = await _context.Products.FindAsync(id);
		_context.Products.Remove(product);
		await _context.SaveChangesAsync();
	}


    public async Task<int> CountCategoryByIDAsync(int id)
    {
        int count = _context.Products.Where(p => p.CategoryId==id).Count();
        return count;
    }

	public async Task<IEnumerable<int>> GetListCountCategory()
	{
        List<int> ints = new List<int>();
        foreach (var category in _context.Categories)
        {
            ints.Add(await CountCategoryByIDAsync(category.Id));
        }

		return ints;
    }
}