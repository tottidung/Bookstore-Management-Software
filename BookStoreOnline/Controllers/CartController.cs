using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;
using PayPal.Api;

namespace BookStoreOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly NhaSachEntities1 db = new NhaSachEntities1();

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        // Get the current cart from session or create a new one if it doesn't exist
        private List<CartItem> GetCart()
        {
            if (Session["GioHang"] is List<CartItem> cart)
            {
                return cart;
            }

            cart = new List<CartItem>();
            Session["GioHang"] = cart;
            return cart;
        }

        // Save the cart to the session
        private void SaveCart(List<CartItem> cart)
        {
            Session["GioHang"] = cart;
        }

        // Add product to cart
        [HttpPost]
        public ActionResult AddToCart(FormCollection product)
        {
            var cart = GetCart();

            if (!int.TryParse(product["ProductID"], out var productId) ||
                !int.TryParse(product["Quantity"], out var quantity))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid input");
            }

            var productInDb = db.SANPHAM.Find(productId);
            if (productInDb == null)
            {
                return HttpNotFound("Product not found");
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductID == productId);
            if (cartItem == null)
            {
                if (quantity > productInDb.SoLuong)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
                }

                cartItem = new CartItem(productId)
                {
                    Number = quantity
                };
                cart.Add(cartItem);
            }
            else
            {
                if (cartItem.Number + quantity > productInDb.SoLuong)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
                }

                cartItem.Number += quantity;
            }
            SaveCart(cart);
            return RedirectToAction("GetCartInfo");
        }

        // Add a single product to the cart
        public ActionResult AddSingleProduct(int id)
        {
            var cart = GetCart();
            const int quantity = 1;

            var productInDb = db.SANPHAM.Find(id);
            if (productInDb == null)
            {
                return HttpNotFound("Product not found");
            }

            var cartItem = cart.FirstOrDefault(p => p.ProductID == id);
            if (cartItem == null)
            {
                if (quantity > productInDb.SoLuong)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
                }

                cartItem = new CartItem(id)
                {
                    Number = quantity
                };
                cart.Add(cartItem);
            }
            else
            {
                if (cartItem.Number + quantity > productInDb.SoLuong)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
                }

                cartItem.Number += quantity;
            }
            SaveCart(cart);
            return RedirectToAction("GetCartInfo");
        }

        // Remove a product from the cart
        public ActionResult Remove(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(p => p.ProductID == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }
            return RedirectToAction("GetCartInfo");
        }

        // Get total number of items in the cart
        private int GetTotalNumber()
        {
            var cart = GetCart();
            return cart.Sum(sp => sp.Number);
        }

        // Get total price of items in the cart
        private decimal GetTotalPrice()
        {
            var cart = GetCart();
            return cart.Sum(sp => sp.FinalPrice());
        }

        // Display cart information
        public ActionResult GetCartInfo()
        {
            var cart = GetCart();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("NullCart");
            }

            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(cart);
        }

        // Update the quantity of a product in the cart
        [HttpPost]
        public ActionResult Update(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid quantity.");
            }

            var product = db.SANPHAM.Find(productId);
            if (product == null)
            {
                return HttpNotFound("Product not found.");
            }

            if (quantity > product.SoLuong)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Number = quantity;
                SaveCart(cart);
            }

            return Json(new { success = true });
        }

        // Partial view for cart summary
        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            return PartialView();
        }

        // View for empty cart
        public ActionResult NullCart()
        {
            return View();
        }


        // Insert order and clear cart
        [HttpPost]
        public ActionResult InsertOrder(string address, int paymentMethod)
        {
            var cartItems = GetCart();
            if (cartItems == null || !cartItems.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Empty cart.");
            }

            var customer = Session["TaiKhoan"] as KHACHHANG;
            if (customer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Not logged in.");
            }

            // Retrieve and apply discount
            var discountAmount = Session["DiscountAmount"] as decimal? ?? 0;
            var totalPrice = GetTotalPrice();
            var finalPrice = totalPrice - discountAmount;
            var roundedFinalPrice = (int)Math.Round(finalPrice, MidpointRounding.AwayFromZero);

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new DONHANG
                    {
                        ID = customer.MaKH,
                        NgayDat = DateTime.Now,
                        DiaChi = address,
                        TrangThai = 0, // Not confirmed
                        PhuongThucThanhToan = paymentMethod,
                        TongTien = roundedFinalPrice,
                        MaKM = discountAmount > 0 ? Session["DiscountCode"]?.ToString() : null // Store discount code in order
                    };

                    db.DONHANG.Add(order);
                    db.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var product = db.SANPHAM.Find(item.ProductID);
                        if (product == null)
                        {
                            return HttpNotFound("Product not found.");
                        }

                        if (item.Number > product.SoLuong)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Quantity exceeds stock.");
                        }

                        var orderDetail = new CHITIETDONHANG
                        {
                            MaDonHang = order.MaDonHang,
                            MaSanPham = item.ProductID,
                            SoLuong = item.Number
                        };
                        db.CHITIETDONHANG.Add(orderDetail);

                       /* product.SoLuong == item.Number;*/
                        product.SoLuongBan += item.Number;
                        db.Entry(product).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    Session["GioHang"] = null;
                    Session["DiscountAmount"] = null;
                    Session["DiscountCode"] = null; // Clear discount code
                    transaction.Commit();

                    return RedirectToAction("Index", "Order", new { id = customer.MaKH });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Log exception here if needed
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Order processing error.");
                }
            }
        }
        // Apply discount to cart
        [HttpPost]
        public JsonResult ApplyDiscount(string discountCode)
        {
            var discount = db.KHUYENMAI
                .FirstOrDefault(d => d.MaKM == discountCode && d.KichHoat && d.NgayBatDau <= DateTime.Now && d.NgayKetThuc >= DateTime.Now);

            if (discount == null)
            {
                return Json(new { success = false, message = "Mã khuyến mãi không hợp lệ hoặc đã hết hạn." });
            }

            var discountAmount = discount.SoTienKM;
            Session["DiscountAmount"] = discountAmount;
            Session["DiscountCode"] = discountCode; // Lưu mã khuyến mãi

            return Json(new { success = true, discountAmount });
        }
        // Check the stock before updating the cart
        [HttpPost]
        public ActionResult CheckStock(int productId, int quantity)
        {
            var product = db.SANPHAM.Find(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            if (quantity > product.SoLuong)
            {
                return Json(new { success = false, message = "Quantity exceeds stock." });
            }

            return Json(new { success = true });
        }
    }
}
