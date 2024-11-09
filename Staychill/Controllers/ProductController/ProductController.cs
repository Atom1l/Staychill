using Microsoft.AspNetCore.Mvc;
using Staychill.Data;
using Staychill.Models.ProductModel;
using Microsoft.EntityFrameworkCore;

namespace Staychill.Controllers.ProductController
{
    public class ProductController : Controller
    {
        private readonly StaychillDbContext _context;

        public ProductController(StaychillDbContext context)
        {
            _context = context;
        }

        // Display all products
        public async Task<IActionResult> ProductIndex()
        {
            var products = await _context.ProductDB.Include(p => p.Images).ToListAsync();
            return View(products);
        }

        // Display details of a specific product
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _context.ProductDB
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Create product view (already exists in your project)
        public IActionResult ProductCreate()
        {
            return View();
        }

        // Edit product view
        public async Task<IActionResult> ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductDB.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Edit product (POST method)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(int id, [Bind("Id,ProductName,ProductType,Color,Description,Price,Instock")] Product product, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Update product details
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ProductType = product.ProductType;
                    existingProduct.Color = product.Color;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Instock = product.Instock;

                    // Update images if new ones are uploaded
                    if (image1 != null) existingProduct.Images.Image1 = await ConvertToBytes(image1);
                    if (image2 != null) existingProduct.Images.Image2 = await ConvertToBytes(image2);
                    if (image3 != null) existingProduct.Images.Image3 = await ConvertToBytes(image3);
                    if (image4 != null) existingProduct.Images.Image4 = await ConvertToBytes(image4);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ProductIndex));
            }
            return View(product);
        }

        // Delete product view (already exists in your project)
        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _context.ProductDB
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Confirm product deletion
        [HttpPost, ActionName("ProductDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
        {
            var product = await _context.ProductDB.FindAsync(id);
            _context.ProductDB.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductIndex));
        }

        // Helper function to convert image to byte array
        private async Task<byte[]> ConvertToBytes(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private bool ProductExists(int id)
        {
            return _context.ProductDB.Any(e => e.Id == id);
        }
    }
}
