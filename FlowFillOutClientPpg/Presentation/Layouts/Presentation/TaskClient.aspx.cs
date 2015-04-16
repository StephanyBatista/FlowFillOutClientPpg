using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;
using System.Web.UI.WebControls;
using Presentation.Util;

namespace Presentation.Layouts.Presentation
{
    public partial class TaskClient : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindTasks(UserGroup());

            
        }

        private RegistrationGroup UserGroup()
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                var query = context.UserOfClientRegistration.Where(c => c.UserId == SPContext.Current.Web.CurrentUser.ID);

                if (!query.Any())
                    return RegistrationGroup.None;

                ViewState["RegistrationGroup"] = query.FirstOrDefault().RegistrationGroup.Value;

                return (RegistrationGroup)ViewState["RegistrationGroup"];
            }
        }

        private void BindTasks(RegistrationGroup group)
        {
            if (group == RegistrationGroup.None)
                return;

            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                var tasks =
                    context.TaskClientRegistration.Where(
                        c =>
                       (c.TaskStatus == TaskStatus.Pendente || c.TaskStatus == TaskStatus.Iniciado) &&
                        c.TaskStep.GetHashCode() == group.GetHashCode());

                gridTasks.DataSource = tasks;
                gridTasks.DataBind();
            }
        }

        protected void TasksRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var task = e.Row.DataItem as TaskClientRegistrationItem;
            if (task == null)
                throw new Exception("Error in method ApproverRowDataBound, cast ApproverTaskItem");

            var linkRequest = e.Row.FindControl("hyperRequest") as HyperLink;
            if (linkRequest == null)
                throw new Exception("Error in method ApproverRowDataBound, cast HyperRequest");

            linkRequest.Text = task.Request.Id.ToString();
            linkRequest.NavigateUrl = string.Format("{0}/_layouts/Presentation/ClientRequest.aspx?RequestId={1}&p={2}",
                SPContext.Current.Web.Url,
                task.Request.Id,
                Server.UrlEncode(Encryption.Encrypt(task.Id.ToString() + "@" + ((RegistrationGroup)ViewState["RegistrationGroup"]).GetHashCode().ToString())));

        }
    }
}
