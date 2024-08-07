using PayPal.Api;
using System;
using System.Web.Mvc;

public class PaymentController : Controller
{
    private readonly PayPalService _payPalService;

    public PaymentController()
    {
        _payPalService = new PayPalService();
    }

    [HttpPost]
    public ActionResult CreatePayment(decimal amount)
    {
        var returnUrl = Url.Action("PaymentSuccess", "Payment", null, Request.Url.Scheme);
        var cancelUrl = Url.Action("PaymentCancel", "Payment", null, Request.Url.Scheme);

        var payment = _payPalService.CreatePayment(amount, "USD", returnUrl, cancelUrl);

        var approvalUrl = payment.GetApprovalUrl();

        return Redirect(approvalUrl);
    }

    public ActionResult PaymentSuccess(string paymentId, string token, string PayerID)
    {
        try
        {
            var payment = _payPalService.ExecutePayment(paymentId, PayerID);

            if (payment.state.ToLower() != "approved")
            {
                return View("PaymentFailed");
            }

            // Bạn có thể thêm logic xử lý đơn hàng tại đây
            // Ví dụ: Lưu thông tin thanh toán vào cơ sở dữ liệu

            return View();
        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ (ghi log, thông báo lỗi, vv)
            return View("PaymentFailed");
        }
    }

    public ActionResult PaymentCancel()
    {
        // Xử lý logic khi hủy thanh toán
        // Bạn có thể hiển thị thông báo cho người dùng tại đây
        return View();
    }
}
