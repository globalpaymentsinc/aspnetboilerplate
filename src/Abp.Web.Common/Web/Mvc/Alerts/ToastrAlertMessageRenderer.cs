using System.Collections.Generic;
using System.Text;

namespace Abp.Web.Mvc.Alerts
{
    public class ToastrAlertMessageRenderer : IAlertMessageRenderer
    {
        public string DisplayType { get; } = AlertDisplayType.Toastr;

        public virtual string Render(List<AlertMessage> alertList)
        {
            var sb = new StringBuilder("<script type=\"text/javascript\">");

            foreach (var alertMessage in alertList)
            {
                var mType = alertMessage.Type.ToString().ToLowerInvariant().Replace("warning", "warn").Replace("danger", "error");

                if (alertMessage.Dismissible)
                {
                    sb.AppendLine($"abp.notify.{mType}('{alertMessage.Text}','{alertMessage.Title}', {{ 'closeButton':  true, 'timeOut': 0, 'extendedTimeOut': 0 }});");
                }
                else
                {
                    sb.AppendLine($"abp.notify.{mType}('{alertMessage.Text}','{alertMessage.Title}', {{ 'closeButton':  false, 'timeOut': 5000, 'extendedTimeOut': 1000 }});");
                }
            }

            sb.Append("</script>");
            return sb.ToString();
        }
    }
}
