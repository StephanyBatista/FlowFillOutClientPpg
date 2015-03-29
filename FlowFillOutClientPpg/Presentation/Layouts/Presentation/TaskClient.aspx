<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskClient.aspx.cs" Inherits="Presentation.Layouts.Presentation.TaskClient" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" scoped/>
    <link href="css/Site.css" rel="stylesheet" scoped/>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/respond.min.js" type="text/javascript"></script>
    
    <SharePoint:CssLink runat="server" Version="4"/>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <h1>Tarefas de cadastro fornecedor</h1>

        <h4>Tarefas para você</h4>
        <hr />
        <asp:GridView ID="gridTasks" runat="server" AutoGenerateColumns="False"
            EnableModelValidation="True" Width="80%" OnRowDataBound="TasksRowDataBound" CssClass="table">
            <Columns>
                <asp:BoundField HeaderText="Usuário atual da tarefa" DataField="TaskUser" />
                <asp:BoundField HeaderText="Fase" DataField="TaskStep" />
                <asp:BoundField HeaderText="Data" DataField="Created" />
                <asp:TemplateField HeaderText="Solicitação">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hyperRequest" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Status" DataField="TaskStatus" />
            </Columns>
            <EmptyDataTemplate>
                Nenhuma tarefa está atribuida para você no momento
            </EmptyDataTemplate>
        </asp:GridView>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Tarefa de cadastro de cliente
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Tarefa de cadastro de cliente
</asp:Content>
