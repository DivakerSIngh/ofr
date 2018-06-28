using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using System.ComponentModel.DataAnnotations;

namespace OneFineRateWeb.Handlers
{
    /// <summary>
    /// This is a class-level attribute, doesn't make sense at the property level
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var typeInfo = value.GetType();

            var propertyInfo = typeInfo.GetProperties();

            foreach (var property in propertyInfo)
            {
                if (null != property.GetValue(value, null))
                {
                    return true;
                }
            }

            // All properties were null.
            return false;
        }
    }


    public static class HtmlHelperExtensions
    {
        /// <summary>
        ///     Returns an unordered list (ul element) of validation messages that utilizes bootstrap markup and styling.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="alertType">The alert type styling rule to apply to the summary element.</param>
        /// <param name="heading">The optional value for the heading of the summary element.</param>
        /// <returns></returns>
        public static HtmlString ValidationBootstrap(this HtmlHelper htmlHelper, string alertType = "danger",
            string heading = "")
        {
            if (htmlHelper.ViewData.ModelState.IsValid)
                return new HtmlString(string.Empty);

            var sb = new StringBuilder();

            sb.AppendFormat("<div class=\"alert alert-{0} alert-block\">", alertType);
            sb.Append("<button class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");

            if (!heading.IsNullOrWhiteSpace())
            {
                sb.AppendFormat("<h4 class=\"alert-heading\">{0}</h4>", heading);
            }

            sb.Append(htmlHelper.ValidationSummary());
            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }
    }


}