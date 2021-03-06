﻿using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Presentation.Model;
using System.Drawing;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using Presentation.Util;

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
            CreditObservationRequired.Visible = CasStatusRequired.Visible = CreditStatusRequired.Visible = CustomerStatusRequired.Visible =
            FiscalStatusRequired.Visible = LogisticsStatusRequired.Visible = CadastreStatusRequired.Visible = false;
        }

        private void DecryptParameter()
        {
            var text = Server.UrlDecode(Encryption.Decrypt(Request.QueryString["p"]));
            
            var taskId = int.Parse(text.Split('@')[0]);
            ViewState["TaskId"] = taskId;

            var groupId = int.Parse(text.Split('@')[1]);
            ViewState["Group"] = (RegistrationGroup)groupId;
        }

        private void EnableForm()
        {
            if (Request.QueryString["RequestId"] == null || Request.QueryString["p"] == null)
                formRequest.Enabled = true;
            else
            {
                DecryptParameter();
                int requestId;

                if(int.TryParse(Request.QueryString["RequestId"], out requestId))
                {
                    var request = GetRequestById(requestId);
                    var task = GetTaskById((int)ViewState["TaskId"]);
                    LoadRequestToClient(request);

                    if (HasPermissionToEvaluation(request, task))
                        EnableEvaluationForm(task);
                }

                SetLinksOfRequest();
            }
        }

        private void SetLinksOfRequest()
        {
            linkRequest.Visible = true;
            linkRequest.Enabled = true;
            linkRequest.NavigateUrl = string.Format("{0}/Lists/Solicita Cliente/DispForm.aspx?ID={1}", SPContext.Current.Web.Url, Request.QueryString["RequestId"]);
            linkPrintRequest.Enabled = true;
            linkPrintRequest.NavigateUrl = string.Format("{0}/_layouts/Presentation/PrintRequest.aspx?RequestId={1}", SPContext.Current.Web.Url, Request.QueryString["RequestId"]);
        }

        private void EnableEvaluationForm(TaskClientRegistrationItem task)
        {
            if (task.TaskStep == TaskStep.Customer)
            {
                formCustomer.Enabled = true;
                ddlCustomerStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
            else if (task.TaskStep == TaskStep.Fiscal)
            {
                formFiscal.Enabled = true;
                ddlFiscalStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
            else if (task.TaskStep == TaskStep.CAS)
            {
                formCas.Enabled = true;
                ddlCasStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
            else if (task.TaskStep == TaskStep.Logistica)
            {
                formLogistics.Enabled = true;
                ddlLogisticsStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
            else if (task.TaskStep == TaskStep.Crédito)
            {
                formCredit.Enabled = true;
                ddlCreditStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
            else if (task.TaskStep == TaskStep.Cadastro)
            {
                formCadastre.Enabled = true;
                ddlCadastreStatus.SelectedValue = GetCodeFromTaskStatus(task.TaskStatus.Value);
            }
        }

        private static bool HasPermissionToEvaluation(ClientRequestItem request, TaskClientRegistrationItem task)
        {
            return (request.RequestStatus == RequestStatus.Iniciado ||
                                    request.RequestStatus == RequestStatus.Pendente ||
                                    request.RequestStatus == RequestStatus.Retorno) &&
                                    (task.TaskStatus == TaskStatus.Iniciado || task.TaskStatus == TaskStatus.Pendente);
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
            ddlSbu.SelectedValue = request.Sbu.Id.ToString();
            txtRegisterId.Text = request.Register;
            txtName.Text = request.Name;
            txtContactFirstName.Text = request.ContactFirstName;
            txtContactLastName.Text = request.ContactLastName;
            ddlPhoneFirstType.SelectedValue = request.PhoneFirstType.Id.ToString();
            txtPhoneFirstAreaCode.Text = request.PhoneFirstAreaCode;
            txtContactFirstPhone.Text = request.ContactFirstPhone;
            txtContactFirstStation.Text = request.ContactFirstStation;
            txtContactSecondName.Text = request.ContactSecondName;
            if (request.PhoneSecondType != null)
                ddlPhoneSecondType.SelectedValue = request.PhoneSecondType.Id.ToString();
            txtPhoneSecondAreaCode.Text = request.PhoneSecondAreaCode;
            txtContactSecondPhone.Text = request.ContactSecondPhone;
            txtContactSecondStation.Text = request.ContactSecondStation;
            txtBank.Text = request.Bank;
            txtCheckingAccountNumber.Text = request.CheckingAccountNumber;
            txtCheckingAccountCurrency.Text = request.CheckingAccountCurrency;
            txtAgency.Text = request.Agency;
            if (request.PaymentCondition != null)
                ddlPaymentCondition.SelectedValue = request.PaymentCondition.Id.ToString();
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
            txtCep.Text = request.Cep;
            if (request.PbcGroup != null)
                ddlPbc.SelectedValue = request.PbcGroup.Id.Value.ToString();
            txtMunicipalRegistration.Text = request.MunicipalRegistration;
            txtPriceList.Text = request.PriceList;
            txtPostage.Text = request.Postage;
            txtMinimumBillingValue.Text = request.MinimumBillingValue;
            if (request.VolumeEffectiveDate != null && request.VolumeEffectiveDate.HasValue)
                txtVolumeEffectiveDate.SelectedDate = request.VolumeEffectiveDate.Value;
            txtVolumeProductMix.Text = request.VolumeProductMix;
            txtVolumePenultimateTrimester.Text = request.VolumePenultimateTrimester;
            txtVolumeLastTrimester.Text = request.VolumeLastTrimester;
            txtCommission.Text = request.Commission;
            if(request.ClientGroup != null)
                ddlClientGroup.SelectedValue = request.ClientGroup.Id.Value.ToString();
            if(request.PurchagesGroup != null)
                ddlPurchagesGroup.SelectedValue = request.PurchagesGroup.Id.Value.ToString();
            if(request.CommercialProfile != null)
                ddlCommercialProfile.SelectedValue = request.CommercialProfile.Id.Value.ToString();
            if (request.BranchActivity != null)
                ddlBranchActivity.SelectedValue = request.BranchActivity.Id.Value.ToString();
            if (request.SubBranchActivity != null)
                ddlSubBranchActivity.SelectedValue = request.SubBranchActivity.Id.Value.ToString();
            txtVolumePurchages.Text = request.VolumePurchages;
            txtGeographicRegion.Text = request.GeographicRegion;
            txtCnae.Text = request.Cnae;
            txtSuframa.Text = request.Suframa;
            if (request.SalesOrder != null)
                ddlSalesOrder.SelectedValue = request.SalesOrder.Id.Value.ToString();
            if (request.ContributorType != null)
                ddlContributorType.SelectedValue = request.SalesOrder.Id.Value.ToString();
            if (request.DateAsignatureCas != null)
                txtDateAsignatureCas.SelectedDate = request.DateAsignatureCas.Value;
            if (request.DateExpirationCas != null)
                txtDateExpirationCas.SelectedDate = request.DateExpirationCas.Value;
            txtDeliveryPriority.Text = request.DeliveryPriority;
            txtDeliveryMethod.Text = request.DeliveryMethod;
            txtShippingCondition.Text = request.ShippingCondition;
            txtPointFob.Text = request.PointFob;
            txtDeposit.Text = request.Deposit;
            txtDemandClass.Text = request.DemandClass;
            txtCorporateName.Text = request.CorporateName;
            txtClientAccount.Text = request.ClientAccount;
            txtRevenueAccount.Text = request.RevenueAccount;
            txtAccountTax.Text = request.TaxAccount;
            if (request.PaymentMethod != null)
                ddlPaymentMethod.SelectedValue = request.PaymentMethod.Id.Value.ToString();
            if (request.InscriptionType != null)
                ddlInscriptionType.SelectedValue = request.InscriptionType.Id.Value.ToString();
            if (request.ContractStartDate != null)
                txtContractStartDate.SelectedDate = request.ContractStartDate.Value;
            txtContractNumber.Text = request.ContractNumber;
            txtStateRegistration.Text = request.StateRegistration;
            txtStreetForCredit.Text = request.Street;
            txtStreetNumberForCredit.Text = request.StreetNumber;
            txtStreetComplementForCredit.Text = request.StreetComplement;
            txtDistrinctForCredit.Text = request.District;
            txtCityForCredit.Text = request.City;
            txtStateForCredit.Text = request.State;
            txtCountryForCredit.Text = request.Country;
            txtCepForCredit.Text = request.Cep;

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

        private TaskClientRegistrationItem GetTaskById(int id)
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                return context.TaskClientRegistration.Where(c => c.Id == id)
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

        private TaskStatus GetTaskStatusFromApprover(string statusCodeFromControl)
        {
            switch(statusCodeFromControl)
            {
                case "1":
                    return TaskStatus.Iniciado;
                case "2":
                    return TaskStatus.Aprovado;
                case "3":
                    return TaskStatus.Reprovado;
                case "4":
                    return TaskStatus.Retorno;
                default:
                    return TaskStatus.None;
            }
        }

        private string GetCodeFromTaskStatus(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Iniciado:
                    return "1";
                case TaskStatus.Aprovado:
                    return "2";
                case TaskStatus.Reprovado:
                    return "3";
                case TaskStatus.Retorno:
                    return "4";
                default:
                    return "0";
            }
        }

        private bool ValidateApproval(TaskStatus taskStatus, string approvalObservation, Control controlObservationRequired, Control controlStatusRequired)
        {
            var validated = true;

            if (taskStatus == TaskStatus.None)
            {
                controlStatusRequired.Visible = true;
                validated = false;
            }

            else if ((taskStatus == TaskStatus.Reprovado ||
                taskStatus == TaskStatus.Retorno) &&
                string.IsNullOrEmpty(approvalObservation))
            {
                controlObservationRequired.Visible = true;
                validated = false;
            }

            return validated;
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
                BindSalesOrderData(context);
                BindContributorTypeData(context);
                BindPaymentMethodData(context);
                BindInscriptionTypeData(context);
            }
        }

        private void BindPhoneTypesData(ListModelDataContext context)
        {
            var phoneTypes = context.PhoneType.ToList();
            ddlPhoneFirstType.DataValueField = ddlPhoneSecondType.DataValueField = "ID";
            ddlPhoneFirstType.DataTextField = ddlPhoneSecondType.DataTextField = "Title";
            ddlPhoneFirstType.DataSource = phoneTypes;
            ddlPhoneSecondType.DataSource = phoneTypes;
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

        private void BindSalesOrderData(ListModelDataContext context)
        {
            var items = context.SaleOrder.ToList();
            ddlSalesOrder.DataValueField = "ID";
            ddlSalesOrder.DataTextField = "Title";
            ddlSalesOrder.DataSource = items;
            ddlSalesOrder.DataBind();
        }

        private void BindContributorTypeData(ListModelDataContext context)
        {
            var items = context.ContributorType.ToList();
            ddlContributorType.DataValueField = "ID";
            ddlContributorType.DataTextField = "Title";
            ddlContributorType.DataSource = items;
            ddlContributorType.DataBind();
        }

        private void BindPaymentMethodData(ListModelDataContext context)
        {
            var items = context.PaymentMethod.ToList();
            ddlPaymentMethod.DataValueField = "ID";
            ddlPaymentMethod.DataTextField = "Title";
            ddlPaymentMethod.DataSource = items;
            ddlPaymentMethod.DataBind();
        }

        private void BindInscriptionTypeData(ListModelDataContext context)
        {
            var items = context.InscriptionType.ToList();
            ddlInscriptionType.DataValueField = "ID";
            ddlInscriptionType.DataTextField = "Title";
            ddlInscriptionType.DataSource = items;
            ddlInscriptionType.DataBind();
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
                if (workflow.Request(request))
                {
                    var msg = string.Format("Solicitação N° {0} salva e enviada para o fluxo do processo", request.Id.Value);
                    SetRequestMessageForSuccess(msg);
                    formRequest.Enabled = false;
                }
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
            request.Sbu = new Item { Id = int.Parse(ddlSbu.SelectedValue) };
            request.Register = txtRegisterId.Text;
            request.Name = txtName.Text;
            request.ContactFirstName = txtContactFirstName.Text;
            request.ContactLastName = txtContactLastName.Text;
            if (ddlPhoneFirstType.SelectedValue != "-1")
                request.PhoneFirstType = new Item { Id = int.Parse(ddlPhoneFirstType.SelectedValue) };
            request.PhoneFirstAreaCode = txtPhoneFirstAreaCode.Text;
            request.ContactFirstPhone = txtContactFirstPhone.Text;
            request.ContactFirstStation = txtContactFirstStation.Text;
            request.ContactSecondName = txtContactSecondName.Text;
            if (ddlPhoneSecondType.SelectedValue != "-1")
                request.PhoneSecondType = new Item { Id = int.Parse(ddlPhoneSecondType.SelectedValue) };
            request.PhoneSecondAreaCode = txtPhoneSecondAreaCode.Text;
            request.ContactSecondPhone = txtContactSecondPhone.Text;
            request.ContactSecondStation = txtContactSecondStation.Text;
            request.Bank = txtBank.Text;
            request.CheckingAccountNumber = txtCheckingAccountNumber.Text;
            request.CheckingAccountCurrency = txtCheckingAccountCurrency.Text;
            request.Agency = txtAgency.Text;
            if (ddlPaymentCondition.SelectedValue != "-1")
                request.PaymentCondition = new Item { Id = int.Parse(ddlPaymentCondition.SelectedValue) };
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
            request.Cep = txtCep.Text;
            LoadAllAttachments(request);

            return request;
        }        

        private void SetRequestMessageForSuccess(string msg)
        {
            RequestMessage.Text = msg;
            RequestMessage.ForeColor = Color.Black;
            divMessage.Visible = true;            
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
            hddScript.Value = "$('#abaCustomer').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlCustomerStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, txtCustomerObservation.Text, CustomerObservationRequired, CustomerStatusRequired) ||
                    (taskStatus == TaskStatus.Aprovado && !ValidateCustomer()))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);

                if (taskStatus == TaskStatus.Aprovado || taskStatus == TaskStatus.Iniciado)
                    request = LoadFlowCustomerFromClient(request);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, txtCustomerObservation.Text);
                SetRequestMessageForSuccess("Fluxo salvo");
                formCustomer.Enabled = false;
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

            if (txtVolumeEffectiveDate.SelectedDate == null)
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

            return validated;
        }

        private ClientRequestItem LoadFlowCustomerFromClient(ClientRequestItem request)
        {
            request.PbcGroup = new Item { Id = int.Parse(ddlPbc.SelectedValue) };
            request.MunicipalRegistration = txtMunicipalRegistration.Text;
            request.PriceList = txtPriceList.Text;
            request.Postage = txtPostage.Text;
            request.MinimumBillingValue = txtMinimumBillingValue.Text;
            request.VolumeEffectiveDate =txtVolumeEffectiveDate.SelectedDate;
            request.VolumeProductMix = txtVolumeProductMix.Text;
            request.VolumePenultimateTrimester = txtVolumePenultimateTrimester.Text;
            request.VolumeLastTrimester = txtVolumeLastTrimester.Text;
            request.Commission = txtCommission.Text;
            request.ClientGroup = new Item { Id = int.Parse(ddlClientGroup.SelectedValue) };
            request.PurchagesGroup = new Item { Id = int.Parse(ddlPurchagesGroup.SelectedValue) };
            request.CommercialProfile = new Item { Id = int.Parse(ddlCommercialProfile.SelectedValue) };
            request.BranchActivity = new Item { Id = int.Parse(ddlBranchActivity.SelectedValue) };
            request.SubBranchActivity = new Item { Id = int.Parse(ddlSubBranchActivity.SelectedValue) };
            request.VolumePurchages = txtVolumePurchages.Text;
            request.GeographicRegion = txtGeographicRegion.Text;
            if (fileSintegra.HasFile)
                LoadAttachment(fileSintegra, request, "sintegra");
            return request;
        }

        protected void VolumeEffectiveDateEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCustomer').click();";
        }

        protected void VolumeEffectiveDateMonthChangedEvent(object sender, MonthChangedEventArgs e)
        {
            hddScript.Value = "$('#abaCustomer').click();";
        }
        #endregion

        #region FlowFiscal
        protected void SaveFlowFiscalEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaFiscal').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlFiscalStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, txtFiscalObservation.Text, FiscalObservationRequired, FiscalStatusRequired) ||
                    (taskStatus == TaskStatus.Aprovado && !ValidateFiscal()))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);
                
                if (taskStatus == TaskStatus.Aprovado || taskStatus == TaskStatus.Iniciado)
                    request = LoadFlowFiscalFromClient(request);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, txtCustomerObservation.Text);
                SetRequestMessageForSuccess("Fluxo salvo");
                formFiscal.Enabled = false;
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private bool ValidateFiscal()
        {
            var validated = true;

            if (string.IsNullOrEmpty(txtCnae.Text))
            {
                CnaeRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtSuframa.Text))
            {
                SuframaRequired.Visible = true;
                validated = false;
            }

            return validated;
        }

        private ClientRequestItem LoadFlowFiscalFromClient(ClientRequestItem request)
        {
            request.Cnae = txtCnae.Text;
            request.Suframa = txtSuframa.Text;
            request.SalesOrder = new Item { Id = int.Parse(ddlSalesOrder.SelectedValue) };
            request.ContributorType = new Item { Id = int.Parse(ddlContributorType.SelectedValue) };
            return request;
        }
        #endregion

        #region FlowCAS
        protected void SaveFlowCasEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCas').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlCasStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, txtCasObservation.Text, CasObservationRequired, CasStatusRequired) ||
                    (taskStatus == TaskStatus.Aprovado && !ValidateCas()))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);

                if (taskStatus == TaskStatus.Aprovado || taskStatus == TaskStatus.Iniciado)
                    request = LoadFlowCasFromClient(request);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, txtCustomerObservation.Text);
                SetRequestMessageForSuccess("Fluxo salvo");
                formCas.Enabled = false;
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private bool ValidateCas()
        {
            var validated = true;

            if (txtDateAsignatureCas.SelectedDate == null)
            {
                DateAsignatureCasRequired.Visible = true;
                validated = false;
            }

            if (txtDateExpirationCas.SelectedDate == null)
            {
                DateExpirationCasRequired.Visible = true;
                validated = false;
            }

            return validated;
        }

        private ClientRequestItem LoadFlowCasFromClient(ClientRequestItem request)
        {
            request.DateAsignatureCas = txtDateAsignatureCas.SelectedDate;
            request.DateExpirationCas = txtDateExpirationCas.SelectedDate;
            return request;
        }

        protected void DateAsignatureCasEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCas').click();";
        }

        protected void DateAsignatureCasMonthChangedEvent(object sender, MonthChangedEventArgs e)
        {
            hddScript.Value = "$('#abaCas').click();";
        }

        protected void DateExpirationCasEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCas').click();";
        }

        protected void DateExpirationCasMonthChangedEvent(object sender, MonthChangedEventArgs e)
        {
            hddScript.Value = "$('#abaCas').click();";
        }
        #endregion

        #region FlowLogistics
        protected void SaveFlowLogisticsEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaLogistic').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlLogisticsStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, txtLogisticsObservation.Text, LogisticsObservationRequired, LogisticsStatusRequired) ||
                    (taskStatus == TaskStatus.Aprovado && !ValidateLogistics()))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);

                if (taskStatus == TaskStatus.Aprovado || taskStatus == TaskStatus.Iniciado)
                    request = LoadFlowLogisticsFromClient(request);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, txtCustomerObservation.Text);
                SetRequestMessageForSuccess("Fluxo salvo");
                formLogistics.Enabled = false;
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private bool ValidateLogistics()
        {
            var validated = true;

            if (string.IsNullOrEmpty(txtDeliveryPriority.Text))
            {
                DeliveryPriorityRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtDeliveryMethod.Text))
            {
                DeliveryMethodRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtShippingCondition.Text))
            {
                ShippingConditionRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtPointFob.Text))
            {
                PointFobRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtDeposit.Text))
            {
                DepositRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtDemandClass.Text))
            {
                DemandClassRequired.Visible = true;
                validated = false;
            }

            return validated;
        }

        private ClientRequestItem LoadFlowLogisticsFromClient(ClientRequestItem request)
        {
            request.DeliveryPriority = txtDeliveryPriority.Text;
            request.DeliveryMethod = txtDeliveryMethod.Text;
            request.ShippingCondition = txtShippingCondition.Text;
            request.PointFob = txtPointFob.Text;
            request.Deposit = txtDeposit.Text;
            request.DemandClass = txtDemandClass.Text;
            return request;
        }
        #endregion

        #region FlowCredit
        protected void SaveFlowCreditEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCredit').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlCreditStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, txtCreditObservation.Text, CreditObservationRequired, CreditStatusRequired) ||
                    (taskStatus == TaskStatus.Aprovado && !ValidateCredit()))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);

                if (taskStatus == TaskStatus.Aprovado || taskStatus == TaskStatus.Iniciado)
                    request = LoadFlowCreditFromClient(request);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, txtCustomerObservation.Text);
                SetRequestMessageForSuccess("Fluxo salvo");
                formCredit.Enabled = false;
            }
            catch
            {
                SetRequestMessageForError();
            }
        }

        private bool ValidateCredit()
        {
            var validated = true;

            if (string.IsNullOrEmpty(txtCorporateName.Text))
            {
                CorporateNameRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtClientAccount.Text))
            {
                ClientAccountRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtRevenueAccount.Text))
            {
                RevenueAccountRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtAccountTax.Text))
            {
                AccountTaxRequired.Visible = true;
                validated = false;
            }

            if (txtContractStartDate.SelectedDate == null)
            {
                ContractStartDateRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtContractNumber.Text))
            {
                ContractNumberRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtStateRegistration.Text))
            {
                StateRegistrationRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtStreetForCredit.Text))
            {
                StreetForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtStreetNumberForCredit.Text))
            {
                StreetNumberForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtDistrinctForCredit.Text))
            {
                DistrinctForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtCityForCredit.Text))
            {
                CityForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtStateForCredit.Text))
            {
                StateForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtCountryForCredit.Text))
            {
                CountryForCreditRequired.Visible = true;
                validated = false;
            }

            if (string.IsNullOrEmpty(txtCepForCredit.Text))
            {
                CepForCreditRequired.Visible = true;
                validated = false;
            }

            return validated;
        }

        private ClientRequestItem LoadFlowCreditFromClient(ClientRequestItem request)
        {
            request.CorporateName = txtCorporateName.Text;
            request.ClientAccount = txtClientAccount.Text;
            request.RevenueAccount = txtRevenueAccount.Text;
            request.TaxAccount = txtAccountTax.Text;
            request.PaymentMethod = new Item { Id = int.Parse(ddlPaymentMethod.SelectedValue) };
            request.InscriptionType = new Item { Id = int.Parse(ddlInscriptionType.SelectedValue) };
            request.ContractStartDate = txtContractStartDate.SelectedDate;
            request.ContractNumber = txtContractNumber.Text;
            request.StateRegistration = txtStateRegistration.Text;
            request.Street = txtStreetForCredit.Text;
            request.StreetNumber = txtStreetNumberForCredit.Text;
            request.StreetComplement = txtStreetComplementForCredit.Text;
            request.District = txtDistrinctForCredit.Text;
            request.State = txtStateForCredit.Text;
            request.Country = txtCountryForCredit.Text;
            request.Cep = txtCepForCredit.Text;
            if (fileSerasa.HasFile)
                LoadAttachment(fileSerasa, request, "serasa");
            return request;
        }

        protected void ContractStartDateEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCredit').click();";
        }

        protected void ContractStartDateMonthChangedEvent(object sender, MonthChangedEventArgs e)
        {
            hddScript.Value = "$('#abaCredit').click();";
        }
        #endregion

        #region FlowCadastre
        protected void SaveFlowCadastreEvent(object sender, EventArgs e)
        {
            hddScript.Value = "$('#abaCadastre').click();";
            HideAllRequired();

            try
            {
                var statusCodeFromApprover = ddlCadastreStatus.SelectedValue;
                var taskStatus = GetTaskStatusFromApprover(statusCodeFromApprover);

                if (!ValidateApproval(taskStatus, null, null, CadastreStatusRequired))
                    return;

                var request = GetRequestById(int.Parse(Request.QueryString["RequestId"]));
                var task = GetTaskById((int)ViewState["TaskId"]);

                var workflow = new WorkflowClientRequest();
                workflow.MakeEvaluation(request, task, taskStatus, null);
                SetRequestMessageForSuccess("Fluxo salvo");
                formCadastre.Enabled = false;
            }
            catch
            {
                SetRequestMessageForError();
            }
        }
        #endregion

    }
}
