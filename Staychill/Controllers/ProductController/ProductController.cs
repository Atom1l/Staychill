using Microsoft.AspNetCore.Mvc;
using Staychill.Data;
using Microsoft.EntityFrameworkCore;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel.TrackingModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Staychill.ViewModel;

namespace Staychill.Controllers
{
    public class ProductController : Controller
    {
        private readonly StaychillDbContext _db;

        public ProductController(StaychillDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var model = await _db.ProductDB.Include(p => p.Images).ToListAsync(); // ดึงข้อมูลผลิตภัณฑ์และรูปภาพ
            return View(model);
        }

        public IActionResult ProductCreate()
        {
            var product = new Product(); // สร้างอ็อบเจ็กต์ product
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(Product product, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4)
        {
            if (ModelState.IsValid)
            {
                _db.ProductDB.Add(product); // บันทึกข้อมูลผลิตภัณฑ์
                await _db.SaveChangesAsync();

                var productImages = new ProductImages // แปลงรูปภาพเป็น byte
                {
                    ProductId = product.Id,
                    Image1 = await ConvertToBytes(image1),
                    Image2 = await ConvertToBytes(image2),
                    Image3 = await ConvertToBytes(image3),
                    Image4 = await ConvertToBytes(image4),
                };

                _db.ProductImagesDB.Add(productImages); // บันทึกรูปภาพในฐานข้อมูล
                await _db.SaveChangesAsync();

                return RedirectToAction("ProductIndex");
            }
            return View("Index_Product");
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _db.ProductDB.Include(m => m.Images).FirstOrDefaultAsync(p => p.Id == id); 
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("ProductDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDeleteConfirmed(int id)
{
    var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id); 
    if (product != null)
    {
        _db.ProductImagesDB.Remove(product.Images); // ลบรูปภาพที่เกี่ยวข้อง
        _db.ProductDB.Remove(product); // ลบข้อมูลผลิตภัณฑ์
        await _db.SaveChangesAsync();
        return RedirectToAction("ProductIndex");
    }
    return NotFound();
}


        public IActionResult ProductAddCart()
        {
            var cartItems = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ToList();

            var viewModel = new TrackingViewModel
            {
                CartItemDetails = cartItems,
                TotalAmount = cartItems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("ProductAddToCart")]
        public IActionResult ProductAddToCart(int productId, int quantity)
        {
            var product = _db.ProductDB.FirstOrDefault(p => p.Id == productId); 
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            var cart = _db.CartDB.Include(c => c.CartItems).FirstOrDefault(); 
            if (cart == null)
            {
                cart = new Cart();
                _db.CartDB.Add(cart);
                _db.SaveChanges();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId); 
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price ?? 0,
                    CartId = cart.CartId
                };
                cart.CartItems.Add(newCartItem);
            }

            _db.SaveChanges();

            return Json(new { success = true, message = "Product added to cart" });
        }

        [HttpPost]
        public IActionResult ProductRemoveCart(int RemovecartId, int[] RemoveitemId)
        {
            var cart = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(p => p.CartId == RemovecartId);

            if (cart != null)
            {
                foreach (var itemId in RemoveitemId)
                {
                    var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId);
                    if (itemToRemove != null)
                    {
                        cart.CartItems.Remove(itemToRemove);
                    }
                }
                _db.SaveChanges();
            }

            return RedirectToAction("ProductAddCart");
        }

        private async Task<byte[]> ConvertToBytes(IFormFile image)
        {
            if (image != null)
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        public async Task<IActionResult> ProductEdit(int id)
{
    var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
    if (product == null)
    {
        return NotFound();
    }
    return View(product);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ProductEdit(int id, Product product, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4)
{
    if (id != product.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _db.Update(product);
            await _db.SaveChangesAsync();

            // Handle image update if new images are provided
            var productImages = await _db.ProductImagesDB.FirstOrDefaultAsync(pi => pi.ProductId == product.Id);
            if (productImages != null)
            {
                if (image1 != null) productImages.Image1 = await ConvertToBytes(image1);
                if (image2 != null) productImages.Image2 = await ConvertToBytes(image2);
                if (image3 != null) productImages.Image3 = await ConvertToBytes(image3);
                if (image4 != null) productImages.Image4 = await ConvertToBytes(image4);

                _db.Update(productImages);
                await _db.SaveChangesAsync();
            }
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

private bool ProductExists(int id)
{
    return _db.ProductDB.Any(e => e.Id == id);
}

    }
}
