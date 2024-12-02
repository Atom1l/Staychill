using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Staychill.Data;
using Staychill.Models;
using Staychill.Models.ProductModel;
using Staychill.ViewModel;

namespace Staychill.Controllers.ProductController
{
    public class ProductController : Controller
    {
        private readonly StaychillDbContext _db;

        public ProductController(StaychillDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductIndex(string productquery)
        {
            IQueryable<Product> query = _db.ProductDB.Include(p => p.Images); // Start with IQueryable

            if (!string.IsNullOrEmpty(productquery))
            {
                query = query.Where(p => p.Id.ToString().Contains(productquery) || p.ProductName.Contains(productquery)
                                      || p.ProductType.Contains(productquery) || p.Color.Contains(productquery)
                                      || p.Description.Contains(productquery) || p.Price.ToString().Contains(productquery)
                                      || p.Instock.ToString().Contains(productquery));
            }

            var model = await query.ToListAsync(); // Call ToListAsync on IQueryable
            return View(model);
        }


        public IActionResult ProductCreate()
        {
            var product = new Product(); // make a new object name product using attributes from Product.cs //
            return View(product);
        }

        // POST:(CREATE) //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(Product product, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4)
        {
            if (ModelState.IsValid)
            {
                _db.ProductDB.Add(product); // Save the data from product that have Product.cs attributes //
                await _db.SaveChangesAsync();

                var productImages = new ProductImages // Convert IMG to Bytes to save in database //
                {
                    ProductId = product.Id,
                    Image1 = await ConvertToBytes(image1),
                    Image2 = await ConvertToBytes(image2),
                    Image3 = await ConvertToBytes(image3),
                    Image4 = await ConvertToBytes(image4),
                };

                _db.ProductImagesDB.Add(productImages); // Save the byte type of data into database //
                await _db.SaveChangesAsync();

                return RedirectToAction("ProductIndex");
            }
            return View("Index_Product");
        }
        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            // Find ProductDB include ProductImages Id if matching with id //
            var product = await _db.ProductDB.Include(m => m.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _db.Remove(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("ProductIndex", "Product");
            }
        }

        public IActionResult ProductAddCart()
        {
            var cartItems = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) // Load the Product for each CartItem
                .ToList();

            var viewModel = new TrackingViewModel
            {
                CartItemDetails = cartItems,
                TotalAmount = cartItems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice) // Calculate total from all CartItems
            };

            return View(viewModel);
        }


        [HttpPost]
        [Route("ProductAddToCart")]
        public IActionResult ProductAddToCart(int productId, int quantity)
        {
            var product = _db.ProductDB.FirstOrDefault(p => p.Id == productId); // Check if productId matches with ProductDB.Id
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" }); // Return as JSON
            }

            // Find the existing Cart (assuming there's only one cart on the website)
            var cart = _db.CartDB.Include(c => c.CartItems)
                                  .FirstOrDefault(); // Get the first Cart (since there is only one cart)

            // If the cart does not exist, you might want to create one here or handle the scenario
            if (cart == null)
            {
                cart = new Cart(); // Create a new Cart if one doesn't exist
                _db.CartDB.Add(cart); // Add it to the database
                _db.SaveChanges(); // Save changes to generate CartId
            }

            // Check if the product is already in the cart
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // If matching, increase the quantity
            }
            else
            {
                // Create a new CartItem
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price ?? 0, // Use null-coalescing to set to 0 if null
                    CartId = cart.CartId // Associate the CartItem with the existing Cart
                };
                cart.CartItems.Add(newCartItem); // Add new CartItem to the Cart
            }

            _db.SaveChanges(); // Save changes

            return Json(new { success = true, message = "Product added to cart" }); // Return success response as JSON
        }


        // POST:(CART.DELETE) //
        [HttpPost]
        public IActionResult ProductRemoveCart(int RemovecartId, int[] RemoveitemId)
        {
            // Find the cart using the CartId
            var cart = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(p => p.CartId == RemovecartId);

            if (cart != null)
            {
                // Remove each CartItem that matches the provided CartItemIds
                foreach (var itemId in RemoveitemId)
                {
                    var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId);
                    if (itemToRemove != null)
                    {
                        // Remove the item from the cart
                        cart.CartItems.Remove(itemToRemove);
                    }
                }
                _db.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("ProductAddCart"); // Redirect to the appropriate action
        }
        public async Task<IActionResult> ProductDetailsAdmin(int id)
        {
            var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id); // Load Product with Images
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id); // Load Product with Images
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.ProductDB.Update(updatedProduct);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ProductIndex));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(updatedProduct);
        }
        public async Task<IActionResult> ProductMainPage(string[] category, float? minPrice, float? maxPrice)
        {
            // ดึงข้อมูลประเภทสินค้าทั้งหมด (ProductType) สำหรับ filter
            ViewBag.Categories = await _db.ProductDB
                                           .Select(p => p.ProductType)
                                           .Distinct()
                                           .ToListAsync();

            // เริ่มต้นกรองสินค้าตามเงื่อนไข
            var products = _db.ProductDB.AsQueryable();

            // ถ้าเลือกหมวดหมู่
            if (category != null && category.Any())
            {
                products = products.Where(p => category.Contains(p.ProductType));
            }

            // ถ้ามีการกรอกราคาต่ำสุดและสูงสุด
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // เพิ่มเงื่อนไขให้แสดงเฉพาะสินค้าที่มีสี White
            products = products.Where(p => p.Color == "White");

            // ดึงสินค้าทั้งหมดที่ตรงกับตัวกรอง
            var filteredProducts = await products
                .Include(p => p.Images) // โหลดข้อมูล Images ด้วย
                .ToListAsync();

            return View(filteredProducts);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            // โหลดสินค้าตัวปัจจุบันพร้อมข้อมูลรูปภาพ
            var product = await _db.ProductDB
                .Include(p => p.Images) // รวมข้อมูลรูปภาพของสินค้า
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // ค้นหาสินค้าเฉพาะที่ ProductName เดียวกันและสีต่างกัน
            var relatedProducts = await _db.ProductDB
                .Where(p => p.ProductName == product.ProductName && p.Id != id) // เงื่อนไข: ชื่อสินค้าเดียวกันและไม่ใช่สินค้าปัจจุบัน
                .GroupBy(p => p.Color) // จัดกลุ่มตาม Color
                .Select(g => new
                {
                    Id = g.First().Id,   // ดึง Id ของสินค้าตัวแรกในกลุ่ม
                    Color = g.Key        // ดึงสีของกลุ่ม
                })
                .ToListAsync();

            // เก็บรายการสินค้าที่เกี่ยวข้องใน ViewBag เพื่อส่งไปยัง View
            ViewBag.RelatedColors = relatedProducts;

            // ดึงสินค้าจากฐานข้อมูลทั้งหมดและสุ่ม 3 ชิ้น (ยกเว้นสินค้าปัจจุบัน)
            var otherProducts = await _db.ProductDB
                .Include(p => p.Images)
                .Where(p => p.Id != id) // ไม่ให้ดึงสินค้าตัวปัจจุบัน
                .OrderBy(r => Guid.NewGuid()) // ใช้ Guid.NewGuid() เพื่อสุ่ม
                .Take(3) // เลือกแค่ 3 ชิ้น
                .ToListAsync();

            // เก็บสินค้าที่สุ่มใน ViewBag
            ViewBag.OtherProducts = otherProducts;

            return View(product); // ส่งสินค้าตัวปัจจุบันไปยัง View
        }


        // Check if Product exists
        private bool ProductExists(int id)
        {
            return _db.ProductDB.Any(e => e.Id == id);
        }


    }
}