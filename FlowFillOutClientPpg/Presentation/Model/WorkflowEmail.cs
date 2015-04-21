using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Model
{
    public class WorkflowEmail
    {
        private ClientRequestItem _request;

        public WorkflowEmail(ClientRequestItem request)
        {
            _request = request;
        }
        
        private string TemplateEmail(ClientRequestItem request, string message)
        {
            return string.Format(
                @"<html>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                    <title></title>
                    <style type='text/css'>
                        body {{
                            font-family: Verdana;
                            font-size: 14px;
                            color: #333333;
                            padding: 10px;
                            margin: 0;
                            line-height: 16px;
                        }}

                        td {{
                            border-collapse: collapse;
                            padding: 3px 100px 3px 6px;
                            font-size: 12px;
                        }}

                        table {{
                            border-collapse: collapse;
                        }}
                    </style>
                </head>
                <body style='font-family: Verdana; font-size: 12px; color: #333333; padding: 10px; margin: 0; line-height: 18px;'>
                    <table style='margin-bottom:4px; border:none;' cellpadding='0' cellspacing='0' border='0'>
                        <tr>
                            <td colspan='2' style='border-bottom:2px solid #0078AA; padding-bottom:6px; padding-left:0;'>
                                <table cellpadding='0' cellspacing='0' border='0'>
                                    <tr>
                                        <td style='padding-right:20px;'>
                                            <img src='http://image.rodandcustommagazine.com/f/hotnews/1012rc_ppg_refinish_live_on_social_media_sites/35488911/1012rc_01_z%2Bppg_logo%2B.jpg' width='74' height='56' />
                                        </td>
                                        <td>
                                            <h3>Solicitação de cliente</h3>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>Id</td>
                            <td>{0}</td>
                        </tr>
                        <tr>
                            <td>Vendedor/Representante</td>
                            <td>{1}</td>
                        </tr>
                        <tr>
                            <td>Nome</td>
                            <td>{2}</td>
                        </tr>
                        <tr style='background-color:#ECECFB;'>
                            <td>CNPJ</td>
                            <td>{3}</td>
                        </tr>
                        <tr>
                            <td>Status</td>
                            <td>{4}</td>
                        </tr>
                        <tr style='background-color:#ECECFB;'>
                            <td>Solicitante</td>
                            <td>{5}</td>
                        </tr>
                        <tr>
                            <td>Observação aprovador</td>
                            <td>{6}</td>
                        </tr>
                    </table>

                    <br/><br/>
                    Mensagem:
                    <br/><br/>
                    {7}
                </body>
                </html>",
                request.Id,
                request.Seller,
                request.Name,
                request.Register,
                request.RequestStatus == RequestStatus.Pendente ? "Pendente" : request.RequestStatus == RequestStatus.Iniciado ? "Iniciado" : request.RequestStatus == RequestStatus.Finalizado ? "Finalizado" : "Reprovado",
                request.Requester,                
                !string.IsNullOrEmpty(request.ApproverObservation) ? request.ApproverObservation : string.Empty,
                message);
        }

        public void SendEmailStepInitialCadastre()
        {
            var body = string.Empty;
            body += string.Format("Existe tarefa de <a href='{0}/Lists/Solicita%20Cliente/DispForm.aspx?ID={1}'>solicitação de cadastro de cliente</a> aguardando<br /> <br />",
                SPContext.Current.Web.Url, _request.Id);
            body += string.Format("Segue <a href='{0}/_layouts/Presentation/TaskClient.aspx'>itens pendente</a>",
                SPContext.Current.Web.Url);

            body = TemplateEmail(_request, body);

            var email = string.Empty;

            var filterGroup = _request.RequestStep == RequestStep.Customer
                ? RegistrationGroup.Customer
                : _request.RequestStep == RequestStep.Fiscal ? RegistrationGroup.Fiscal
                : _request.RequestStep == RequestStep.CAS ? RegistrationGroup.CAS
                : _request.RequestStep == RequestStep.Logistica ? RegistrationGroup.Logistica
                : _request.RequestStep == RequestStep.Crédito ? RegistrationGroup.Crédito
                : RegistrationGroup.Cadastro;

            using(var contextModel = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                foreach (var user in contextModel.UserOfClientRegistration.Where(c => c.RegistrationGroup == filterGroup))
                {
                    var userEmail = SPContext.Current.Web.AllUsers.GetByID(user.UserId.Value).Email;
                    email += userEmail + ";";
                }
            }

            SPUtility.SendEmail(SPContext.Current.Web, true, false, email, "Tarefa de cadastro cliente", body);
        }

        public void SendEmailStepFinish()
        {
            var body = string.Empty;
            body += string.Format("Sua <a href='{0}/_layouts/Presentation/ClientRequest.aspx?Id={1}'>solicitação de cadastro de cliente</a> foi finalizada<br /> <br />",
                SPContext.Current.Web.Url, _request.Id);

            body = TemplateEmail(_request, body);

            var email = SPContext.Current.Web.AllUsers.GetByID(_request.RequesterId.Value).Email;

            SPUtility.SendEmail(SPContext.Current.Web, true, false, email, "Finalizado tarefa de cadastro cliente", body);
        }

        public void SendEmailReproved()
        {
            var body = string.Empty;
            body += string.Format("Sua <a href='{0}/_layouts/Presentation/ClientRequest.aspx?RequestId={1}'>solicitação de cadastro de cliente</a> foi reprovada<br /> <br />",
                SPContext.Current.Web.Url, _request.Id);

            body = TemplateEmail(_request, body);

            var email = SPContext.Current.Web.AllUsers.GetByID(_request.RequesterId.Value).Email;

            SPUtility.SendEmail(SPContext.Current.Web, true, false, email, "Solicitação reprovada", body);
        }

        public void SendEmailReturn()
        {
            var body = string.Empty;
            body += string.Format("A solicitação {1} foi retornada. <a href='{0}/_layouts/Presentation/TaskClient.aspx'>Acesse tela de tarefa para visualizar</a><br /> <br />",
                SPContext.Current.Web.Url, _request.Id);

            body = TemplateEmail(_request, body);

            var email = string.Empty;
            using (var contextModel = new ListModelDataContext(SPContext.Current.Web.Url))
            {
                foreach (var user in contextModel.UserOfClientRegistration.Where(c => c.RegistrationGroup == RegistrationGroup.Customer))
                {
                    var userEmail = SPContext.Current.Web.AllUsers.GetByID(user.UserId.Value).Email;
                    email += userEmail + ";";
                }
            }

            SPUtility.SendEmail(SPContext.Current.Web, true, false, email, "Solicitação retornada", body);
        }
    }
}
