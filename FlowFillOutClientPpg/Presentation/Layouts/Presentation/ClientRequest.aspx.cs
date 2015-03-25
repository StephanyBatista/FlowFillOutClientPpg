using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Layouts.Presentation
{
    public partial class ClientRequest : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindDatas();
        }

        private void BindDatas()
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                BindSbuData(context);
                BindPaymentConditionsData(context);
                BindPhoneTypesData(context);
            }
        }

        private void BindPhoneTypesData(ListModelDataContext context)
        {
            var phoneTypes = context.PhoneType.ToList();
            ddlPhoneFirstType.DataValueField = ddlPhoneSecondType.DataValueField = "ID";
            ddlPhoneFirstType.DataTextField = ddlPhoneSecondType.DataTextField = "Title";
            ddlPhoneFirstType.DataSource = ddlPhoneSecondType.DataSource = phoneTypes;
            ddlPhoneFirstType.DataBind();
            ddlPhoneSecondType.DataBind();
        }

        private void BindPaymentConditionsData(ListModelDataContext context)
        {
            var paymentConditions = context.PaymentCondition.ToList();
            ddlPaymentCondition.DataValueField = "ID";
            ddlPaymentCondition.DataTextField = "Title";
            ddlPaymentCondition.DataSource = paymentConditions;
            ddlPaymentCondition.DataBind();
        }

        private void BindSbuData(ListModelDataContext context)
        {
            var sbus = context.Sbu.ToList();
            ddlSbu.DataValueField = "ID";
            ddlSbu.DataTextField = "Title";
            ddlSbu.DataSource = sbus;
            ddlSbu.DataBind();
        }
    }
}
