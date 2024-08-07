using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BookStoreOnline.Areas.Admin.Constants
{
    public static class EHelper
    {
        public static string GetEnumDescription<T>(T value) where T : Enum
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return value.ToString();
        }

        public static string GetEnumDescription<T>(string value) where T : struct, Enum
        {
            if (!Enum.TryParse(value, out T enumValue))
            {
                return "Unknown Role";
            }

            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
            {
                return value;
            }

            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value;
        }

        public static MvcHtmlString GetEnumDescription<T>(this HtmlHelper htmlHelper, string value) where T : struct, Enum
        {
            string description = GetEnumDescription<T>(value);
            return MvcHtmlString.Create(description);
        }
    }
}