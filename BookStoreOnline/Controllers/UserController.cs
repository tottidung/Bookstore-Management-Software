using System;
using System.Linq;
using System.Web.Mvc;
using BookStoreOnline.Models;
using System.Web.Security; // Thêm namespace cho bảo mật
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Security;

public class UserController : Controller
{
    private NhaSachEntities1 db = new NhaSachEntities1();

    // GET: User/Login
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    // POST: User/Login
    [HttpPost]
    public ActionResult Login(KHACHHANG taikhoan)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra tài khoản quản trị
            var taikhoanAdmin = db.NHANVIEN
                .FirstOrDefault(k => k.Email == taikhoan.Email && k.MatKhau == taikhoan.MatKhau);

            if (taikhoanAdmin != null)
            {
                if (!taikhoanAdmin.TrangThai) // Kiểm tra trạng thái tài khoản
                {
                    ViewBag.ThongBao = "Tài khoản đã bị khóa";
                    return View();
                }

                // Đăng nhập thành công
                Session["TaiKhoan"] = taikhoanAdmin;
                return RedirectToAction("Index", "Home_Page", new { area = "Admin" });
            }

            // Kiểm tra tài khoản khách hàng
            var account = db.KHACHHANG
                .FirstOrDefault(k => k.Email == taikhoan.Email && k.MatKhau == taikhoan.MatKhau);

            if (account != null)
            {
                if (!account.TrangThai) // Kiểm tra trạng thái tài khoản khách hàng
                {
                    ViewBag.ThongBao = "Tài khoản đã bị khóa";
                    return View();
                }

                // Đăng nhập thành công
                Session["TaiKhoan"] = account;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Email hoặc mật khẩu không chính xác";
            }
        }
        return View();
    }

    // GET: User/SignUp
    [HttpGet]
    public ActionResult SignUp()
    {
        return View();
    }

    // POST: User/SignUp
    [HttpPost]
    public ActionResult SignUp(KHACHHANG cus, string rePass)
    {
        if (ModelState.IsValid)
        {
            var checkEmail = db.KHACHHANG.FirstOrDefault(c => c.Email == cus.Email);
            if (checkEmail != null)
            {
                ViewBag.ThongBaoEmail = "Đã có tài khoản đăng nhập bằng Email này";
                return View();
            }
            if (cus.MatKhau == rePass)
            {
                // Không mã hóa mật khẩu
                cus.TrangThai = true; // Mặc định khi đăng ký thì tài khoản sẽ được kích hoạt
                cus.NgayTao = DateTime.Now; // Gán thời gian hiện tại cho thuộc tính NgayTao

                db.KHACHHANG.Add(cus);
                db.SaveChanges();

                // Thêm thông báo đăng ký thành công
                ViewBag.ThongBao = "Đăng ký thành công!";

                // Giữ người dùng lại trên trang đăng ký
                return View();
            }
            else
            {
                ViewBag.ThongBao = "Mật khẩu xác nhận không chính xác";
                return View();
            }
        }
        return View();
    }

    // GET: User/ChangePassword
    [HttpGet]
    public ActionResult ChangePassword()
    {
        return View();
    }

    // POST: User/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
    {
        if (ModelState.IsValid)
        {
            var currentUser = Session["TaiKhoan"] as KHACHHANG;

            if (currentUser != null)
            {
                var user = db.KHACHHANG.FirstOrDefault(u => u.MaKH == currentUser.MaKH);

                if (user != null)
                {
                    if (user.MatKhau == oldPassword)
                    {
                        if (newPassword == confirmPassword)
                        {
                            // Mã hóa mật khẩu mới (khuyến nghị)
                            user.MatKhau = newPassword; // Nên mã hóa mật khẩu trước khi lưu
                            db.SaveChanges();
                            ViewBag.ThongBao = "Mật khẩu đã được thay đổi thành công!";
                        }
                        else
                        {
                            ViewBag.ThongBao = "Mật khẩu xác nhận không chính xác";
                        }
                    }
                    else
                    {
                        ViewBag.ThongBao = "Mật khẩu cũ không chính xác";
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Người dùng không tồn tại";
                }
            }
            else
            {
                ViewBag.ThongBao = "Người dùng chưa đăng nhập";
            }
        }
        return View();
    }

    // GET: User/UpdateInfo
    [HttpGet]
    public ActionResult UpdateInfo()
    {
        var currentUser = Session["TaiKhoan"] as KHACHHANG;

        if (currentUser != null)
        {
            // Lấy thông tin người dùng hiện tại
            var user = db.KHACHHANG.FirstOrDefault(u => u.MaKH == currentUser.MaKH);
            if (user != null)
            {
                return View(user);
            }
        }
        return RedirectToAction("Login", "User");
    }

    // POST: User/UpdateInfo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateInfo(KHACHHANG updatedUser)
    {
        if (ModelState.IsValid)
        {
            var currentUser = Session["TaiKhoan"] as KHACHHANG;

            if (currentUser != null)
            {
                var user = db.KHACHHANG.FirstOrDefault(u => u.MaKH == currentUser.MaKH);
                if (user != null)
                {
                    // Cập nhật thông tin người dùng
                    user.Ten = updatedUser.Ten;
                    user.Email = updatedUser.Email;
                    user.DiaChi = updatedUser.DiaChi;
                    user.SoDienThoai = updatedUser.SoDienThoai;

                    db.SaveChanges();

                    // Cập nhật thông tin người dùng trong session
                    Session["TaiKhoan"] = user;

                    ViewBag.ThongBao = "Thông tin đã được cập nhật thành công!";
                }
                else
                {
                    ViewBag.ThongBao = "Người dùng không tồn tại";
                }
            }
            else
            {
                ViewBag.ThongBao = "Người dùng chưa đăng nhập";
            }
        }
        return View(updatedUser);
    }

    // GET: User/LogOut
    public ActionResult LogOut()
    {
        Session["TaiKhoan"] = null;
        Session["GioHang"] = null;

        return RedirectToAction("Login", "User");
    }
}