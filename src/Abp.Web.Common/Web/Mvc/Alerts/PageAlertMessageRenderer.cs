using System.Collections.Generic;
using System.Text;

namespace Abp.Web.Mvc.Alerts
{
    public class PageAlertMessageRenderer : IAlertMessageRenderer
    {
        public string DisplayType { get; } = AlertDisplayType.PageAlert;

        public virtual string Render(List<AlertMessage> alertList)
        {
            var sb = new StringBuilder();
            foreach (var alertMessage in alertList)
            {
                var mType = alertMessage.Type.ToString().ToLowerInvariant();

                if (alertMessage.Dismissible)
                {
                    sb.Append($"<div class=\"alert alert-{mType} alert-dismisable\" role=\"alert\">");
                    sb.Append($"<h4 class=\"alert-heading\">{alertMessage.Title}");
                    sb.Append($"<button type=\"button\" class=\"close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    sb.Append($"</h4>");
                    sb.Append($"<p>{alertMessage.Text}</p>");
                    sb.Append($"</div>");
                }
                else
                {
                    sb.Append($"<div class=\"alert alert-{mType}\" role=\"alert\">");
                    sb.Append($"<h4 class=\"alert-heading\">{alertMessage.Title}");
                    sb.Append($"</h4>");
                    sb.Append($"<p>{alertMessage.Text}</p>");
                    sb.Append($"</div>");
                }
            }

            return sb.ToString();
        }
    }
}
