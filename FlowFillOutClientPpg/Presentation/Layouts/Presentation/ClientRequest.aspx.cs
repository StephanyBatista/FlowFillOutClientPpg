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
                BindDatasOfChoise();
                EnableForm();
            }
        }

        private void HideAllRequired()
        {
            SellerRequired.Visible = RegisterIdRequired.Visible = ContactFirstNameRequired.Visible =
            PhoneFirstAreaCodeRequired.Visible = BankRequired.Visible = CheckingAccountNumberRequired.Visible =
            AgencyCurrency.Visible = EmailContactRequired.Visible = EmailXmlRequired.Visible = MunicipalRegistrationRequired.Visible =
            PriceListRequired.Visible = PostageRequired.Visible = MinimumBillingValueRequired.Visible = VolumeEffectiveDateRequired.Visible =
            VolumeProductMixRequired.Visible = VolumePenultimateTrimesterRequired.Visible = VolumeLastTrimesterRequired.Visible =
            CommissionRequired.Visible = VolumePurchagesRequired.Visible = GeographicRegionRequired.Visible = CustomerObservationRequired.Visible =
            CnaeRequired.Visible = SuframaRequired.Visible = FiscalObservationRequired.Visible = DateAsignatureCasRequired.Visible = 
            DateExpirationCasRequired.Visible = CasObservationRequired.Visible = DeliveryPriorityRequired.Visible = DeliveryMethodRequired.Visible = 
            ShippingConditionRequired.Visible = PointFobRequired.Visible = DepositRequired.Visible = DemandClassRequired.Visible =
            LogisticsObservationRequired.Visible = CorporateNameRequired.Visible = ClientAccountRequired.Visible = RevenueAccountRequired.Visible =
            AccountTaxRequired.Visible = ContractStartDateRequired.Visible = ContractNumberRequired.Visible = StateRegistrationRequired.Visible = 
            StreetForCreditRequired.Visible = StreetNumberForCreditRequired.Visible = DistrinctForCreditRequired.Visible =
            CityForCreditRequired.Visible = StateForCreditRequired.Visible = CountryForCreditRequired.Visible = CepForCreditRequired.Visible =
            CreditObservationRequired.Visible = false;
        }

        private void EnableForm()
        {
            if(Request.QueryString["RequestId"] == null)
            {
                formRequest.Enabled = true;
                BindDatasOfChoise();
            }
            else
            {
                int requestId;

                if(int.TryParse(Request.QueryString["RequestId"], out requestId))
                {
                    var request = GetRequestById(requestId);
                    LoadRequestToClient(request);

                    //TODO: When o group Customer start the flow, the request status must be "Started"
                    if (request.RequestStatus == RequestStatus.Iniciado || request.RequestStatus == RequestStatus.Pendente)
                    {
                        var task = GetLastTaskOpen(request.Id.Value);
                        if (task.TaskStep == TaskStep.Customer)
                            formCustomer.Enabled = true;
                    }
                }
            }
        }

        private ClientRequestItem GetRequestById(int requestId)
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                return context.ClientRequest.Where(c => c.Id == requestId).FirstOrDefault();
            }
        }

        private void LoadRequestToClient(ClientRequestItem request)
        {
            txtSeller.Text = request.Seller;
            ddlSbu.SelectedValue = request.SbuId.Id.ToString();
            txtRegisterId.Text = request.RegisterId;
            txtName.Text = request.Name;
            txtContactFirstName.Text = request.ContactFirstName;
            txtContactLastName.Text = request.ContactLastName;
            ddlPhoneFirstType.SelectedValue = request.PhoneFirstTypeId.Id.ToString();
            txtPhoneFirstAreaCode.Text = request.PhoneFirstAreaCode;
            txtContactFirstPhone.Text = request.ContactFirstPhone;
            txtContactFirstStation.Text = request.ContactFirstStation;
            txtContactSecondName.Text = request.ContactSecondName;
            if (request.PhoneSecondTypeId != null)
                ddlPhoneSecondType.SelectedValue = request.PhoneSecondTypeId.Id.ToString();
            txtPhoneSecondAreaCode.Text = request.PhoneSecondAreaCode;
            txtContactSecondPhone.Text = request.ContactSecondPhone;
            txtContactSecondStation.Text = request.ContactSecondStation;
            txtBank.Text = request.Bank;
            txtCheckingAccountNumber.Text = request.CheckingAccountNumber;
            txtCheckingAccountCurrency.Text = request.CheckingAccountCurrency;
            txtAgency.Text = request.Agency;
            if (request.PaymentConditionId != null)
                ddlPaymentCondition.SelectedValue = request.PaymentConditionId.Id.ToString();
            txtEmailBillet.Text = request.EmailBillet;
            txtEmailContact.Text = request.EmailContact;
            txtEmailReport.Text = request.EmailReport;
            txtEmailXml.Text = request.EmailXml;
            txtAddressFullDelivery.Text = request.AddressFullDelivery;
            txtStreet.Text = request.Street;
            txtStreetNumber.Text = request.StreetNumber;
            txtStreetComplement.Text = request.StreetComplement;
            txtDistrict.Text = request.District;
            txtCity.Text = request.City;
            txtState.Text = request.State;
            txtCountry.Text = request.Country;
            txtCep.Text = request.CEP;
        }

        private TaskClientRegistrationItem GetLastTaskOpen(int requestId)
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                return context.TaskClientRegistration.Where(c => 
                    c.Request.Id == requestId && 
                    (c.TaskStatus == TaskStatus.Pendente || c.TaskStatus == TaskStatus.Iniciado))
                    .LastOrDefault();

            }
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

        #region Binds
        private void BindDatasOfChoise()
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                BindSbuData(context);
                BindPaymentConditionsData(context);
                BindPhoneTypesData(context);
                BindPbcData(context);
                BindClientData(context);
                BindPurchagesData(context);
                BindCommercialProfileData(context);
                BindBranchActivityData(context);
                BindSubBranchActivityData(context);
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

        private void BindPbcData(ListModelDataContext context)
        {
            var items = context.PbcGroup.ToList();
            ddlPbc.DataValueField = "ID";
            ddlPbc.DataTextField = "Title";
            ddlPbc.DataSource = items;
            ddlPbc.DataBind();
        }

        private void BindClientData(ListModelDataContext context)
        {
            var items = context.ClientGroup.ToList();
            ddlClientGroup.DataValueField = "ID";
            ddlClientGroup.DataTextField = "Title";
            ddlClientGroup.DataSource = items;
            ddlClientGroup.DataBind();
        }

        private void BindPurchagesData(ListModelDataContext context)
        {
            var items = context.PurchagesGroup.ToList();
            ddlPurchagesGroup.DataValueField = "ID";
            ddlPurchagesGroup.DataTextField = "Title";
            ddlPurchagesGroup.DataSource = items;
            ddlPurchagesGroup.DataBind();
        }

        private void BindCommercialProfileData(ListModelDataContext context)
        {
            var items = context.CommercialProfile.ToList();
            ddlCommercialProfile.DataValueField = "ID";
            ddlCommercialProfile.DataTextField = "Title";
            ddlCommercialProfile.DataSource = items;
            ddlCommercialProfile.DataBind();
        }

        private void BindBranchActivityData(ListModelDataContext context)
        {
            var items = context.BranchActivity.ToList();
            ddlBranchActivity.DataValueField = "ID";
            ddlBranchActivity.DataTextField = "Title";
            ddlBranchActivity.DataSource = items;
            ddlBranchActivity.DataBind();
        }

        private void BindSubBranchActivityData(ListModelDataContext context)
        {
            var items = context.CommercialProfile.ToList();
            ddlSubBranchActivity.DataValueField = "ID";
            ddlSubBranchActivity.DataTextField = "Title";
            ddlSubBranchActivity.DataSource = items;
            ddlSubBranchActivity.DataBind();
        }
        #endregion

        #region FlowRequest
        protected void SaveFlowRequestEvent(object sender, EventArgs e)
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

        private void SetRequestMessageForSuccess(int requestId)
        {
            RequestMessage.Text = string.Format("Solicitação N° {0} salva e enviada para o fluxo do processo", requestId);
            RequestMessage.ForeColor = Color.Black;
            divMessage.Visible = true;
            formRequest.Enabled = false;
        }

        private void SetRequestMessageForError()
        {
            RequestMessage.Text = "Erro ao salvar a solicitação. Contate o administrador";
            RequestMessage.ForeColor = System.Drawing.Color.Red;
            divMessage.Visible = true;
        }
        #endregion

        #region FlowCustomer
        protected void SaveFlowCustomerEvent(object sender, EventArgs e)
        {
            HideAllRequired();

            try
            {
                if (!ValidateCustomer())
                    return;

                var request = LoadFlowCustomerFromClient();
                var workflow = new WorkflowClientRequest();
                if (workflow.Request(request))
                    SetRequestMessageForSuccess(request.Id.Value);
                else
                    SetRequestMessageForError();
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private bool ValidateCustomer()
        {
            var validated = true;

            if (string.IsNullOrEmpty(txtMunicipalRegistration.Text))
            {
                MunicipalRegistrationRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtPriceList.Text))
            {
                PriceListRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtPostage.Text))
            {
                PostageRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtMinimumBillingValue.Text))
            {
                MinimumBillingValueRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtVolumeEffectiveDate.Text))
            {
                VolumeEffectiveDateRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtVolumeProductMix.Text))
            {
                VolumeProductMixRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtVolumePenultimateTrimester.Text))
            {
                VolumePenultimateTrimesterRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtVolumeLastTrimester.Text))
            {
                VolumeLastTrimesterRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtCommission.Text))
            {
                CommissionRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtVolumePurchages.Text))
            {
                VolumePurchagesRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtGeographicRegion.Text))
            {
                GeographicRegionRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtCustomerObservation.Text))
            {
                CustomerObservationRequired.Visible = true;
                validated = false;
            }

            if (ddlCustomerStatus.SelectedValue == "-1")
            {
                CustomerStatusRequired.Visible = true;
                validated = false;
            }

            return validated;
        }

        private ClientRequestItem LoadFlowCustomerFromClient()
        {
            var request = GetRequestById((int)Request.QueryString["RequestId"]);
            request.PbcGroupId.Id = int.Parse(ddlPbc.SelectedValue);
            request.MunicipalRegistration = txtMunicipalRegistration.Text;
            request.PriceList = txtPriceList.Text;
            request.Postage = txtPostage.Text;
            request.MinimumBillingValue = txtMinimumBillingValue.Text;
            request.VolumeEffectiveDate = DateTime.Parse(txtVolumeEffectiveDate.Text);
            request.VolumeProductMix = txtVolumeProductMix.Text;
            request.VolumePenultimateTrimester = txtVolumePenultimateTrimester.Text;
            request.VolumeLastTrimester = txtVolumeLastTrimester.Text;
            request.Commission = txtCommission.Text;
            request.ClientGroupId.Id = int.Parse(ddlClientGroup.SelectedValue);
            request.PurchagesGroupId.Id = int.Parse(ddlPurchagesGroup.SelectedValue);
            request.CommercialProfileId.Id = int.Parse(ddlCommercialProfile.SelectedValue);
            request.BranchActivityId.Id = int.Parse(ddlBranchActivity.SelectedValue);
            request.SubBranchActivityId.Id = int.Parse(ddlSubBranchActivity.SelectedValue);
            request.VolumePurchages = txtVolumePurchages.Text;
            request.GeographicRegion = txtGeographicRegion.Text;
            return request;
        }
        #endregion
    }
}
