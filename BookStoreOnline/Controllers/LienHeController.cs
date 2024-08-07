using System.Web.Mvc;

namespace BookStoreOnline.Controllers
{
    public class LienHeController : Controller
    {
        // GET: LienHe
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        // POST: Contact/Send
        [HttpPost]
        public ActionResult Send(string name, string email, string message)
        {
            if (ModelState.IsValid)
            {
                // Xử lý gửi email hoặc lưu thông tin vào cơ sở dữ liệu
                // Ví dụ: gửi email đến quản trị viên

                // Thông báo gửi thành công
                ViewBag.Message = "Tin nhắn của bạn đã được gửi thành công!";
                return View("Index");
            }

            // Thông báo lỗi nếu có
            ViewBag.Message = "Có lỗi xảy ra. Vui lòng thử lại!";
            return View("Index");
        }
    }
}