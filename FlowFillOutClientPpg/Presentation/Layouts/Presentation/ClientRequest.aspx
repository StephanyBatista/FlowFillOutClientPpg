<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientRequest.aspx.cs" Inherits="Presentation.Layouts.Presentation.ClientRequest" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

    <link href="css/bootstrap.min.css" rel="stylesheet" scoped />
    <link href="css/Site.css" rel="stylesheet" scoped />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.maskedinput.js" type="text/javascript"></script>
    <script src="js/respond.min.js" type="text/javascript"></script>

    <SharePoint:CssLink runat="server" Version="4" />

    <script type="text/javascript">
        $(function () {
            $(".cnpj").mask("99.999.999/9999-99");

            $("#content div:nth-child(1)").show();
            $(".abas li:first div").addClass("selected");		
            $(".aba").click(function(){
                $(".aba").removeClass("selected");
                $(this).addClass("selected");
                var indice = $(this).parent().index();
                indice++;
                $("#content .conteudo").hide();
                $("#content .conteudo:nth-child(" + indice + ")").show();
            });
            		
            $(".aba").hover(
                function(){$(this).addClass("ativa")},
                function(){$(this).removeClass("ativa")}
            );	

        });

    </script>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <h1>Solicitação de item</h1>

    <asp:HiddenField runat="server" ID="hddRequestId" />

    <div class="TabControl">
        <div id="header">
            <ul class="abas">
                <li>
                    <div class="aba">
                        <span>Solicitação</span>
                    </div>
                </li>
                <li>
                    <div class="aba">
                        <span>Customer</span>
                    </div>
                </li>
            </ul>
        </div>
        <div id="content">
            <div class="conteudo">

                <asp:Panel ID="formRequest" runat="server" Enabled="false">

                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-horizontal">
                                <h4>Cadastro de cliente</h4>
                                <hr />

                                <div class="form-group">
                                    <span class="control-label col-md-2">Vendedor/Representante</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtSeller" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="SellerRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">SBU</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlSbu" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">CNPJ/CPF Datasul</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtRegisterId" runat="server" CssClass="form-control cnpj"></asp:TextBox>
                                        <span id="RegisterIdRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Nome Fantasia</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Contato Primeiro Nome</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="ContactFirstNameRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Contato Sobrenome</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Tipo do Telefone</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlPhoneFirstType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Código DDD Contato</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtPhoneFirstAreaCode" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="PhoneFirstAreaCodeRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Telefone do Contato</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactFirstPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Ramal Contato</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactFirstStation" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <!---->

                                <div class="form-group">
                                    <span class="control-label col-md-2">Contato 2</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactSecondName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Tipo do Telefone 2</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlPhoneSecondType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Código DDD Contato 2</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtPhoneSecondAreaCode" runat="server" CssClass="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Telefone do Contato 2</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactSecondPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Ramal Contato 2</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtContactSecondStation" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Banco</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtBank" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="BankRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Número da CC</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCheckingAccountNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="CheckingAccountNumberRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Moeda da CC</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCheckingAccountCurrency" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Agência</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtAgency" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="AgencyCurrency" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Condição de Pagamento</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlPaymentCondition" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">E-mail Boleto</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtEmailBillet" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">E-mail Contato</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtEmailContact" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="EmailContactRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">E-mail Contato</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtEmailReport" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">E-mail XML</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtEmailXml" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="EmailXmlRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Endereço de Entrega Completo(se for diferente do principal)</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtAddressFullDelivery" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Logradouro</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Número</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtStreetNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Complemento</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtStreetComplement" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Bairro</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Cidade</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Estado</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">País</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">CEP</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCep" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Crédito solicitado </span>
                                    <div class="col-md-10">
                                        <asp:FileUpload runat="server" ID="fileCreditRequested" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Contrato social </span>
                                    <div class="col-md-10">
                                        <asp:FileUpload runat="server" ID="fileSocialContract" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Comprovante endereço </span>
                                    <div class="col-md-10">
                                        <asp:FileUpload runat="server" ID="fileAddress" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Balanço </span>
                                    <div class="col-md-10">
                                        <asp:FileUpload runat="server" ID="fileBalance" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Documento conta bancária </span>
                                    <div class="col-md-10">
                                        <asp:FileUpload runat="server" ID="fileBankAccountDocument" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:Button runat="server" ID="Save" Text="Salvar" CssClass="btn btn-default" OnClick="SaveEvent" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </asp:Panel>

            </div>

            <div class="conteudo">
			    <asp:Panel ID="formCustomer" runat="server" Enabled="false">

                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-horizontal">
                                <h4>Fluxo Customer</h4>
                                <hr />

                                <div class="form-group">
                                    <span class="control-label col-md-2">Grupo PBC</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlPbc" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Incrição Municipal</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtMunicipalRegistration" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="MunicipalRegistrationRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Lista Preço</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtPriceList" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="PriceListRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Perfil/Porte</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtPostage" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="PostageRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <span class="control-label col-md-2">Valor mínimo de faturamento</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtMinimumBillingValue" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="MinimumBillingValueRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Volume - Data Vigencia</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtVolumeEffectiveDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="VolumeEffectiveDateRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Volume - Mix de Produtos</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtVolumeProductMix" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="VolumeProductMixRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Volume - Penulltimo Trimestre</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtVolumePenultimateTrimester" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="VolumePenultimateTrimesterRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Volume - Ultimo Trimestre</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtVolumeLastTrimester" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="VolumeLastTrimesterRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Comissão %</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCommission" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="CommissionRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Grupo de Cliente</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlClientGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Grupo de Compras</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlPurchagesGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Perfil Comercial</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCommercialProfile" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Ramo de Atividade</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlBranchActivity" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Sub-ramo atividade</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlSubBranchActivity" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Volume Compras</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtVolumePurchages" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="VolumePurchagesRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Regiao Geográfica</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtGeographicRegion" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span id="GeographicRegionRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <h5>Aprovação</h5>
                                <hr />

                                <div class="form-group">
                                    <span class="control-label col-md-2">Status</span>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCustomerStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                            <asp:ListItem Value="-1" Text="Iniciar"></asp:ListItem>
                                            <asp:ListItem Value="-1" Text="Aprovar"></asp:ListItem>
                                            <asp:ListItem Value="-1" Text="Reprovar"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <span class="control-label col-md-2">Observação</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtCustomerObservation" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                        <span id="CustomerObservationRequired" runat="Server" class="ms-formvalidation" title="Obrigatório">Obrigatório</span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:Button runat="server" ID="btnSaveFlowCustomer" Text="Salvar" CssClass="btn btn-default" OnClick="SaveEvent" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
		    </div>
        </div>

    </div>
    <div runat="server" id="divMessage" visible="False" class="alert alert-success">
        <a class="close" data-dismiss="alert">×</a>
        <asp:Label ID="RequestMessage" runat="server" Style="font-size: 16px;" Text=""></asp:Label>
    </div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Solicitação de Cliente
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Solicitação de Cliente
</asp:Content>
