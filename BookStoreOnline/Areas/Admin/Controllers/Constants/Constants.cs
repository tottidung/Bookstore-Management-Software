using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BookStoreOnline.Areas.Admin.Constants
{
    public static class Constants
    {
        public enum AdminRole
        {
            [Description("Quản trị viên")]
            Administrator = 1,
            [Description("Quản lý")]
            Manager = 2,
            [Description("Nhân viên bán hàng")]
            Seller = 3
        }
        public enum StatusOrder
        {
            [Description("Chưa xác nhận")]
            NoInform = 0,
            [Description("Đã xác nhận")]
            Informed = 1,
            [Description("Đang giao hàng")]
            Shipping = 2,
            [Description("Đã nhận hàng")]
            Received = 3,
            [Description("Đã hủy")]
            Canceled = 4
        }
        public enum StatusPayment
        {
            [Description("Chưa thanh toán")]
            Unpaid = 0,
            [Description("Đã thanh toán")]
            Paid = 1
        }
        public enum TypePayment
        {
            [Description("Tiền mặt")]
            COD = 1,
            [Description("Ví điện tử")]
            VNPAY = 2,
          /*  [Description("Ngân hàng")]
            Bank = 3*/
        }

    }
}