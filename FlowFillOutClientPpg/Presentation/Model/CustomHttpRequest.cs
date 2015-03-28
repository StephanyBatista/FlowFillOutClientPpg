using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Presentation.Model
{
    public static class CustomHttpRequest
    {
        public static void SetHttpContextWithElevatedPrivileges(SPWeb oWeb)
        {
            var httpRequest = new HttpRequest("", oWeb.Url, "");
            HttpContext.Current = new HttpContext(httpRequest, new HttpResponse(new System.IO.StringWriter()));
            SPControl.SetContextWeb(HttpContext.Current, oWeb);
        }
    }
}
