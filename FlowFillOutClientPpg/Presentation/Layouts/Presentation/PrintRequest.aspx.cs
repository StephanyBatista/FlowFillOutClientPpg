using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Presentation.Model;
using System.Drawing;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;


namespace Presentation.Layouts.Presentation
{
    public partial class PrintRequest : LayoutsPageBase
    {
        private ClientRequestItem _request;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int requestId;
            if (int.TryParse(Request.QueryString["RequestId"], out requestId))
            {
                _request = GetRequestById(requestId);
                if(_request != null)
                    LoadRequestToClient(_request);
            }
        }

        private ClientRequestItem GetRequestById(int requestId)
        {
            using (var context = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                var query = context.ClientRequest.Where(c => c.Id == requestId);
                if (query.Any())
                    return query.First();
                return null;
            }
        }

        private void LoadRequestToClient(ClientRequestItem request)
        {
            Seller.Text = request.Seller;
            Sbu.Text = request.Sbu.Title;
            Register.Text = request.Register;
            Name.Text = request.Name;
            ContactFirstName.Text = request.ContactFirstName;
            ContactLastName.Text = request.ContactLastName;
            PhoneFirstType.Text = request.PhoneFirstType.Title;
            PhoneFirstAreaCode.Text = request.PhoneFirstAreaCode;
            ContactFirstPhone.Text = request.ContactFirstPhone;
            ContactFirstStation.Text = request.ContactFirstStation;
            ContactSecondName.Text = request.ContactSecondName;
            if (request.PhoneSecondType != null)
                PhoneSecondType.Text = request.PhoneSecondType.Title;
            PhoneSecondAreaCode.Text = request.PhoneSecondAreaCode;
            ContactSeondPhone.Text = request.ContactSecondPhone;
            ContactSeondPhone.Text = request.ContactSecondStation;
            Bank.Text = request.Bank;
            CheckingAccountNumber.Text = request.CheckingAccountNumber;
            CheckingAccountCurrency.Text = request.CheckingAccountCurrency;
            Agency.Text = request.Agency;
            if (request.PaymentCondition != null)
                PaymentCondition.Text = request.PaymentCondition.Title;
            EmailBillet.Text = request.EmailBillet;
            EmailContact.Text = request.EmailContact;
            EmailReport.Text = request.EmailReport;
            EmailXml.Text = request.EmailXml;
            AddressFullDelivery.Text = request.AddressFullDelivery;
            Street.Text = request.Street;
            StreetNumber.Text = request.StreetNumber;
            StreetComplement.Text = request.StreetComplement;
            District.Text = request.District;
            City.Text = request.City;
            State.Text = request.State;
            Country.Text = request.Country;
            Cep.Text = request.Cep;
            if (request.PbcGroup != null)
                PbcGroup.Text = request.PbcGroup.Id.Value.ToString();
            MunicipalRegistration.Text = request.MunicipalRegistration;
            PriceList.Text = request.PriceList;
            Postage.Text = request.Postage;
            MinimumBillingValue.Text = request.MinimumBillingValue;
            if (request.VolumeEffectiveDate != null && request.VolumeEffectiveDate.HasValue)
                VolumeEffectiveDate.Text = request.VolumeEffectiveDate.Value.ToString("dd/MM/yyyy");
            VolumeProductMix.Text = request.VolumeProductMix;
            VolumePenultimateTrimester.Text = request.VolumePenultimateTrimester;
            VolumeLastTrimester.Text = request.VolumeLastTrimester;
            Commission.Text = request.Commission;
            if (request.ClientGroup != null)
                ClientGroup.Text = request.ClientGroup.Id.Value.ToString();
            if (request.PurchagesGroup != null)
                PurchagesGroup.Text = request.PurchagesGroup.Id.Value.ToString();
            if (request.CommercialProfile != null)
                CommercialProfile.Text = request.CommercialProfile.Id.Value.ToString();
            if (request.BranchActivity != null)
                BranchActivity.Text = request.BranchActivity.Id.Value.ToString();            
            VolumePurchages.Text = request.VolumePurchages;
            GeographicRegion.Text = request.GeographicRegion;
            Cnae.Text = request.Cnae;
            Suframa.Text = request.Suframa;
            if (request.SalesOrder != null)
                SalesOrder.Text = request.SalesOrder.Id.Value.ToString();
            if (request.ContributorType != null)
                ContributorType.Text = request.SalesOrder.Id.Value.ToString();
            DeliveryPriority.Text = request.DeliveryPriority;
            DeliveryMethod.Text = request.DeliveryMethod;
            ShippingCondition.Text = request.ShippingCondition;
            PointFob.Text = request.PointFob;
            Deposit.Text = request.Deposit;
            CorporateName.Text = request.CorporateName;
            ClientAccount.Text = request.ClientAccount;
            RevenueAccount.Text = request.RevenueAccount;
            TaxAccount.Text = request.TaxAccount;
            if (request.PaymentMethod != null)
                PaymentMethod.Text = request.PaymentMethod.Id.Value.ToString();
            if (request.InscriptionType != null)
                InscriptionType.Text = request.InscriptionType.Id.Value.ToString();
            if (request.ContractStartDate != null)
                ContractStartDate.Text = request.ContractStartDate.Value.ToString("dd/MM/yyyy");
            ContractNumber.Text = request.ContractNumber;
            StateRegistration.Text = request.StateRegistration;
            StreetCredit.Text = request.Street;
            StreetNumberCredit.Text = request.StreetNumber;
            StreetComplementCredit.Text = request.StreetComplement;
            DistrictCredit.Text = request.District;
            CityCredit.Text = request.City;
            StateCredit.Text = request.State;
            CountryCredit.Text = request.Country;
            CepCredit.Text = request.Cep;
        }
    }
}
