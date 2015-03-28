using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Presentation.Model;
using System.Drawing;

namespace Presentation.Layouts.Presentation
{
    public partial class ClientRequest : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HideAllRequired();
                BindDatas();
            }

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
            HideAllRequired();

            try {

                if (!ValidateRequest())
                    return;

                var request = LoadRequestFromClient();
                var workflow = new WorkflowClientRequest();
                if(workflow.Request(request))
                    SetRequestMessageForSuccess(request.Id.Value);
                else
                    SetRequestMessageForError();
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private void HideAllRequired()
        {
            SellerRequired.Visible = RegisterIdRequired.Visible = ContactFirstNameRequired.Visible =
            PhoneFirstAreaCodeRequired.Visible = BankRequired.Visible = CheckingAccountNumberRequired.Visible =
            AgencyCurrency.Visible = EmailContactRequired.Visible = EmailXmlRequired.Visible = false;
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
            if (ddlPhoneFirstType.SelectedValue != "-1")
                request.PhoneFirstTypeId = new Item { Id = int.Parse(ddlPhoneFirstType.SelectedValue) };
            request.PhoneFirstAreaCode = txtPhoneFirstAreaCode.Text;
            request.ContactFirstPhone = txtContactFirstPhone.Text;
            request.ContactFirstStation = txtContactFirstStation.Text;
            request.ContactSecondName = txtContactSecondName.Text;
            if (ddlPhoneSecondType.SelectedValue != "-1")
                request.PhoneSecondTypeId = new Item { Id = int.Parse(ddlPhoneSecondType.SelectedValue) };
            request.PhoneSecondAreaCode = txtPhoneSecondAreaCode.Text;
            request.ContactSecondPhone = txtContactSecondPhone.Text;
            request.ContactSecondStation = txtContactSecondStation.Text;
            request.Bank = txtBank.Text;
            request.CheckingAccountNumber = txtCheckingAccountNumber.Text;
            request.CheckingAccountCurrency = txtCheckingAccountCurrency.Text;
            request.Agency = txtAgency.Text;
            if (ddlPaymentCondition.SelectedValue != "-1")
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
            LoadAllAttachments(request);

            return request;
        }

        private void LoadAllAttachments(ClientRequestItem request)
        {
            if (fileCreditRequested.HasFile)
                LoadAttachment(fileCreditRequested, request, "credito_solicitado");

            if (fileSocialContract.HasFile)
                LoadAttachment(fileSocialContract, request, "contrato_social");

            if (fileAddress.HasFile)
                LoadAttachment(fileAddress, request, "comprovante_endereço");

            if (fileBalance.HasFile)
                LoadAttachment(fileBalance, request, "balanco");

            if (fileBankAccountDocument.HasFile)
                LoadAttachment(fileBankAccountDocument, request, "documento_conta_bancaria");
        }

        private void LoadAttachment(FileUpload attachment, ClientRequestItem request, string name)
        {
            var arraySplit = attachment.FileName.Split('.');
            var fileName = string.Format("{0}.{1}", name, arraySplit[arraySplit.Length - 1]);
            request.AddAttachment(fileName, attachment.FileBytes);
        }

        private void SetRequestMessageForSuccess(int requestId)
        {
            RequestMessage.Text = string.Format("Solicitação N° {0} salva e enviada para o fluxo do processo", requestId);
            RequestMessage.ForeColor = Color.Black;
            divMessage.Visible = true;
        }

        private void SetRequestMessageForError()
        {
            RequestMessage.Text = "Erro ao salvar a solicitação. Contate o administrador";
            RequestMessage.ForeColor = System.Drawing.Color.Red;
            divMessage.Visible = true;
        }

    }
}
