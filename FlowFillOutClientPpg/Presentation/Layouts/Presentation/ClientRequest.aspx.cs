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

        protected void SaveEvent(object sender, EventArgs e)
        {
            if (!ValidateRequest())
                return;

            var request = LoadRequestFromClient();
        }

        private bool ValidateRequest()
        {
            var validated = true;
            if (string.IsNullOrEmpty(txtSeller.Text))
            {
                validated = false;
                SellerRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtRegisterId.Text))
            {
                validated = false;
                RegisterIdRequired.Visible = true;
            }
            
            if (string.IsNullOrEmpty(txtContactFirstName.Text))
            {
                validated = false;
                ContactFirstNameRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtPhoneFirstAreaCode.Text))
            {
                validated = false;
                PhoneFirstAreaCodeRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtBank.Text))
            {
                validated = false;
                BankRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtCheckingAccountNumber.Text))
            {
                validated = false;
                CheckingAccountNumberRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtAgency.Text))
            {
                validated = false;
                AgencyCurrency.Visible = true;
            }

            if (string.IsNullOrEmpty(txtEmailContact.Text))
            {
                validated = false;
                EmailContactRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtEmailXml.Text))
            {
                validated = false;
                EmailXmlRequired.Visible = true;
            }

            if (string.IsNullOrEmpty(txtEmailXml.Text))
            {
                validated = false;
                EmailXmlRequired.Visible = true;
            }
            return validated;
        }

        private ClientRequestItem LoadRequestFromClient()
        {
            var request = new ClientRequestItem();
            request.Seller = txtSeller.Text;
            request.SbuId = new Item { Id = int.Parse(ddlSbu.SelectedValue) };
            request.RegisterId = txtRegisterId.Text;
            request.Name = txtName.Text;
            request.ContactFirstName = txtContactFirstName.Text;
            request.ContactLastName = txtContactLastName.Text;
            request.PhoneFirstTypeId = new Item { Id = int.Parse(ddlPhoneFirstType.SelectedValue) };
            request.PhoneFirstAreaCode = txtPhoneFirstAreaCode.Text;
            request.ContactFirstPhone = txtContactFirstPhone.Text;
            request.ContactFirstStation = txtContactFirstStation.Text;
            request.ContactSecondName = txtContactSecondName.Text;
            request.PhoneSecondTypeId = new Item { Id = int.Parse(ddlPhoneSecondType.SelectedValue) };
            request.PhoneSecondAreaCode = txtPhoneSecondAreaCode.Text;
            request.ContactSecondPhone = txtContactSecondPhone.Text;
            request.ContactSecondStation = txtContactSecondStation.Text;
            request.Bank = txtBank.Text;
            request.CheckingAccountNumber = txtCheckingAccountNumber.Text;
            request.CheckingAccountCurrency = txtCheckingAccountCurrency.Text;
            request.Agency = txtAgency.Text;
            request.PaymentConditionId = new Item { Id = int.Parse(ddlPaymentCondition.SelectedValue) };
            request.EmailBillet = txtEmailBillet.Text;
            request.EmailContact = txtEmailContact.Text;
            request.EmailReport = txtEmailReport.Text;
            request.EmailXml = txtEmailXml.Text;
            request.AddressFullDelivery = txtAddressFullDelivery.Text;
            request.Street = txtStreet.Text;
            request.StreetNumber = txtStreetNumber.Text;
            request.StreetComplement = txtStreetComplement.Text;
            request.District = txtDistrict.Text;
            request.City = txtCity.Text;
            request.State = txtState.Text;
            request.Country = txtCountry.Text;
            request.CEP = txtCep.Text;

            return request;
        }

    }
}
