using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Model
{
    public class RunWithElevatedPrivileges
    {
        public bool Execute(Func<SPWeb,bool> method)
        {
            var wasExecuted = false;
            
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (var site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb oWeb = site.OpenWeb())
                    {
                        CustomHttpRequest.SetHttpContextWithElevatedPrivileges(oWeb);
                        oWeb.AllowUnsafeUpdates = true;
                        wasExecuted = method(oWeb);
                    }
                }
            });

            return wasExecuted;
        }
    }
}
