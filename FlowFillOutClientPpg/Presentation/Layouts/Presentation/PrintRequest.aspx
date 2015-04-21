<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintRequest.aspx.cs" Inherits="Presentation.Layouts.Presentation.PrintRequest" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

    <%--<link href="css/bootstrap.min.css" rel="stylesheet" scoped />
    <link href="css/Site.css" rel="stylesheet" scoped />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="js/respond.min.js" type="text/javascript"></script>--%>

<%--

    <style type='text/css'>
        body {
            font-family: Verdana;
            font-size: 14px;
            color: #333333;
            padding: 10px;
            margin: 0;
            line-height: 16px;
        }

        td {
            border-collapse: collapse;
            padding: 3px 100px 3px 6px;
            font-size: 12px;
        }

        table {
            border-collapse: collapse;
        }
    </style>--%>

    <SharePoint:CssLink runat="server" Version="4" />

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">


    <table style='margin-bottom: 4px; border: none;' cellpadding='0' cellspacing='0' border='0'>
        <tr>
            <td colspan='2' style='border-bottom: 2px solid #0078AA; padding-bottom: 6px; padding-left: 0;'>
                <table cellpadding='0' cellspacing='0' border='0'>
                    <tr>
                        <td style='padding-right: 20px;'>
                            <img src='http://image.rodandcustommagazine.com/f/hotnews/1012rc_ppg_refinish_live_on_social_media_sites/35488911/1012rc_01_z%2Bppg_logo%2B.jpg' width='74' height='56' />
                        </td>
                        <td>
                            <h3>Solicita&ccedil;&atilde;o de cliente</h3>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Raz&atilde;o Social do Cliente</td>
            <td>
                <asp:Label runat="server" ID="CorporateName"></asp:Label></td>
        </tr>
        <tr>
            <td>CNPJ/CPF Datasul</td>
            <td>
                <asp:Label runat="server" ID="Register"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Nome Fantasia</td>
            <td>
                <asp:Label runat="server" ID="Name"></asp:Label></td>
        </tr>
        <tr>
            <td>Logradouro</td>
            <td>
                <asp:Label runat="server" ID="Street"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>N&uacute;mero</td>
            <td>
                <asp:Label runat="server" ID="StreetNumber"></asp:Label></td>
        </tr>
        <tr>
            <td>Complemento</td>
            <td>
                <asp:Label runat="server" ID="StreetComplement"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Bairro</td>
            <td>
                <asp:Label runat="server" ID="District"></asp:Label></td>
        </tr>
        <tr>
            <td>Cidade</td>
            <td>
                <asp:Label runat="server" ID="City"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Estado</td>
            <td>
                <asp:Label runat="server" ID="State"></asp:Label></td>
        </tr>
        <tr>
            <td>Pa&iacute;s</td>
            <td>
                <asp:Label runat="server" ID="Country"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>CEP</td>
            <td>
                <asp:Label runat="server" ID="Cep"></asp:Label></td>
        </tr>
        <tr>
            <td>Banco</td>
            <td>
                <asp:Label runat="server" ID="Bank"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>N&uacute;mero da CC</td>
            <td>
                <asp:Label runat="server" ID="CheckingAccountNumber"></asp:Label></td>
        </tr>
        <tr>
            <td>Ag&ecirc;ncia</td>
            <td>
                <asp:Label runat="server" ID="Agency"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Moeda da CC</td>
            <td>
                <asp:Label runat="server" ID="CheckingAccountCurrency"></asp:Label></td>
        </tr>
        <tr>
            <td>Perfil/Porte</td>
            <td>
                <asp:Label runat="server" ID="Postage"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Grupo de Cliente</td>
            <td>
                <asp:Label runat="server" ID="ClientGroup"></asp:Label></td>
        </tr>
        <tr>
            <td>Grupo PBC</td>
            <td>
                <asp:Label runat="server" ID="PbcGroup"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>N&uacute;mero Contrato Vendor</td>
            <td>
                <asp:Label runat="server" ID="ContractNumber"></asp:Label></td>
        </tr>
        <tr>
            <td>Data Inicio Contrato Vendor</td>
            <td>
                <asp:Label runat="server" ID="ContractStartDate"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>SBU</td>
            <td>
                <asp:Label runat="server" ID="Sbu"></asp:Label></td>
        </tr>
        <tr>
            <td>Regiao Geogr&aacute;fica</td>
            <td>
                <asp:Label runat="server" ID="GeographicRegion"></asp:Label></td>
        </tr>
        <tr>
            <td>C&oacute;digo CNAE</td>
            <td>
                <asp:Label runat="server" ID="Cnae"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Inscri&ccedil;&atilde;o Municipal</td>
            <td>
                <asp:Label runat="server" ID="MunicipalRegistration"></asp:Label></td>
        </tr>
        <tr>
            <td>Tipo Inscri&ccedil;&atilde;o</td>
            <td>
                <asp:Label runat="server" ID="InscriptionType"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Inscri&ccedil;&atilde;o Estadual</td>
            <td>
                <asp:Label runat="server" ID="StateRegistration"></asp:Label></td>
        </tr>
        <tr>
            <td>Tipo Contribuinte</td>
            <td>
                <asp:Label runat="server" ID="ContributorType"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Inscri&ccedil;&atilde;o SUFRAMA</td>
            <td>
                <asp:Label runat="server" ID="Suframa"></asp:Label></td>
        </tr>
        <tr>
            <td>Conta Cliente</td>
            <td>
                <asp:Label runat="server" ID="ClientAccount"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Conta de Receita</td>
            <td>
                <asp:Label runat="server" ID="RevenueAccount"></asp:Label></td>
        </tr>
        <tr>
            <td>Conta imposto</td>
            <td>
                <asp:Label runat="server" ID="TaxAccount"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Prioridade de Entrega</td>
            <td>
                <asp:Label runat="server" ID="DeliveryPriority"></asp:Label></td>
        </tr>
        <tr>
            <td>Valor m&iacute; nimo de faturamento</td>
            <td>
                <asp:Label runat="server" ID="MinimumBillingValue"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Volume - Data Vigencia</td>
            <td>
                <asp:Label runat="server" ID="VolumeEffectiveDate"></asp:Label></td>
        </tr>
        <tr >
            <td>Volume - Mix de Produtos</td>
            <td>
                <asp:Label runat="server" ID="VolumeProductMix"></asp:Label></td>
        </tr>
        <tr>
            <td>Volume - Penultimo Trimestre</td>
            <td>
                <asp:Label runat="server" ID="VolumePenultimateTrimester"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Volume - Ultimo Trimestre</td>
            <td>
                <asp:Label runat="server" ID="VolumeLastTrimester"></asp:Label></td>
        </tr>
        <tr>
            <td>Comiss&atilde;o %</td>
            <td>
                <asp:Label runat="server" ID="Commission"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Ordem de Venda </td>
            <td>
                <asp:Label runat="server" ID="SalesOrder"></asp:Label></td>
        </tr>
        <tr>
            <td>Condi&ccedil;&atilde;o de Pagamento</td>
            <td>
                <asp:Label runat="server" ID="PaymentCondition"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Lista de Pre&ccedil;o</td>
            <td>
                <asp:Label runat="server" ID="PriceList"></asp:Label></td>
        </tr>
        <tr>
            <td>Metodo de Entrega</td>
            <td>
                <asp:Label runat="server" ID="DeliveryMethod"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Condi&ccedil;&atilde;o Frete</td>
            <td>
                <asp:Label runat="server" ID="ShippingCondition"></asp:Label></td>
        </tr>
        <tr>
            <td>Ponto FOB</td>
            <td>
                <asp:Label runat="server" ID="PointFob"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Deposito</td>
            <td>
                <asp:Label runat="server" ID="Deposit"></asp:Label></td>
        </tr>
        <tr>
            <td>Vendedor/Representante</td>
            <td>
                <asp:Label runat="server" ID="Seller"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Endere&ccedil;o de Entrega Completo(se for diferente do principal)</td>
            <td>
                <asp:Label runat="server" ID="AddressFullDelivery"></asp:Label></td>
        </tr>
        <tr>
            <td>Logradouro</td>
            <td>
                <asp:Label runat="server" ID="StreetCredit"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>N&uacute;mero</td>
            <td>
                <asp:Label runat="server" ID="StreetNumberCredit"></asp:Label></td>
        </tr>
        <tr>
            <td>Complemento</td>
            <td>
                <asp:Label runat="server" ID="StreetComplementCredit"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Bairro</td>
            <td>
                <asp:Label runat="server" ID="DistrictCredit"></asp:Label></td>
        </tr>
        <tr>
            <td>Cidade</td>
            <td>
                <asp:Label runat="server" ID="CityCredit"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Estado</td>
            <td>
                <asp:Label runat="server" ID="StateCredit"></asp:Label></td>
        </tr>
        <tr>
            <td>Pa&iacute; s</td>
            <td>
                <asp:Label runat="server" ID="CountryCredit"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>CEP</td>
            <td>
                <asp:Label runat="server" ID="CepCredit"></asp:Label></td>
        </tr>
        <tr>
            <td>M&eacute;todo de Pagamento</td>
            <td>
                <asp:Label runat="server" ID="PaymentMethod"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Contato Primeiro Nome</td>
            <td>
                <asp:Label runat="server" ID="ContactFirstName"></asp:Label></td>
        </tr>
        <tr>
            <td>Contato Sobrenome</td>
            <td>
                <asp:Label runat="server" ID="ContactLastName"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Tipo do Telefone</td>
            <td>
                <asp:Label runat="server" ID="PhoneFirstType"></asp:Label></td>
        </tr>
        <tr>
            <td>C&oacute;digo DDD Contato</td>
            <td>
                <asp:Label runat="server" ID="PhoneFirstAreaCode"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Telefone do Contato</td>
            <td>
                <asp:Label runat="server" ID="ContactFirstPhone"></asp:Label></td>
        </tr>
        <tr>
            <td>Ramal Contato</td>
            <td>
                <asp:Label runat="server" ID="ContactFirstStation"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Contato 2</td>
            <td>
                <asp:Label runat="server" ID="ContactSecondName"></asp:Label></td>
        </tr>
        <tr>
            <td>Tipo do Telefone2</td>
            <td>
                <asp:Label runat="server" ID="PhoneSecondType"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>C&oacute;digo DDD Contato2</td>
            <td>
                <asp:Label runat="server" ID="PhoneSecondAreaCode"></asp:Label></td>
        </tr>
        <tr>
            <td>Telefone do Contato2</td>
            <td>
                <asp:Label runat="server" ID="ContactSeondPhone"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Ramal Contato2</td>
            <td>
                <asp:Label runat="server" ID="ContactSeondStation"></asp:Label></td>
        </tr>
        <tr>
            <td>E-mail Boleto</td>
            <td>
                <asp:Label runat="server" ID="EmailBillet"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>E-mail Contato</td>
            <td>
                <asp:Label runat="server" ID="EmailContact"></asp:Label></td>
        </tr>
        <tr>
            <td>E-mail Laudo</td>
            <td>
                <asp:Label runat="server" ID="EmailReport"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>E-mail XML</td>
            <td>
                <asp:Label runat="server" ID="EmailXml"></asp:Label></td>
        </tr>
        <tr>
            <td>Grupo de Compras</td>
            <td>
                <asp:Label runat="server" ID="PurchagesGroup"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Perfil Comercial</td>
            <td>
                <asp:Label runat="server" ID="CommercialProfile"></asp:Label></td>
        </tr>
        <tr>
            <td>Ramo de Atividade</td>
            <td>
                <asp:Label runat="server" ID="BranchActivity"></asp:Label></td>
        </tr>
        <tr style='background-color: #ECECFB;'>
            <td>Volume Compras</td>
            <td>
                <asp:Label runat="server" ID="VolumePurchages"></asp:Label></td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    P&aacute;gina impress&atilde;o cliente
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    P&aacute;gina Impress&atilde;o cliente
</asp:Content>
