using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Presentation.Model
{
    public class WorkflowClientRequest
    {
        private ClientRequestItem _request;
        public ListModelDataContext Context { get; private set; }
        
        public bool Request(ClientRequestItem request)
        {
            _request = request;
            var run = new RunWithElevatedPrivileges();
            return run.Execute(RequestDelegate);
        }

        private bool RequestDelegate(SPWeb oWeb)
        {
            try
            {
                Context = new ListModelDataContext(oWeb.Url);
                if (_request.Id.HasValue)
                    throw new Exception("Request already save. Use method Update");

                LoadListsToRequest();
                _request.RequestStatus = RequestStatus.Pendente;
                _request.Created = _request.Modified = DateTime.Now;
                Context.ClientRequest.InsertOnSubmit(_request);
                RequestBase(oWeb);
                return true;
            }catch
            {
                return false;
            }
        }

        private void LoadListsToRequest()
        {
            _request.Sbu = Context.Sbu.Where(c => c.Id == _request.Sbu.Id).FirstOrDefault();
            _request.PhoneFirstType = Context.PhoneType.Where(c => c.Id == _request.PhoneFirstType.Id).FirstOrDefault();
            if (_request.PhoneSecondType != null)
                _request.PhoneSecondType = Context.PhoneType.Where(c => c.Id == _request.PhoneSecondType.Id).FirstOrDefault();
            if (_request.PaymentCondition != null)
                _request.PaymentCondition = Context.PaymentCondition.Where(c => c.Id == _request.PaymentCondition.Id).FirstOrDefault();
        }

        private void RequestBase(SPWeb oWeb)
        {
            NextFlow();
            Context.SubmitChanges();
            _request.SaveAttachments(oWeb);
            ProcessNextFlow();
        }

        private void MakeEvaluation(ClientRequestItem request, TaskClientRegistrationItem task, TaskStatus status, string observation)
        {
            _request = request;
            Context = new ListModelDataContext(SPContext.Url);
            
            if(status == TaskStatus.Iniciado)
            {
                //TODO: Atualiza a tarefa com o responsável
            }
            else if(status == TaskStatus.Aprovado)
            {
                //TODO: Atualizar a solicitação
                //TODO: Fechar a tarefa atual;
                NextFlow();    
            }
            else if(status == TaskStatus.Reprovado)
            {
                //TODO: Atualizar a solicitação
                //TODO: Fechar a tarefa atual;
                _request.RequestStatus = RequestStatus.Reprovado;
                //Finalizar solicitação;
                NextFlow();
            }
        }

        private void NextFlow()
        {
            if (_request.RequestStatus == RequestStatus.Finalizado)
                return;

            if (_request.RequestStep == null)
            {
                _request.RequestStep = RequestStep.Customer;
                CreateTask(TaskStep.Customer); 
            }
        }

        private void CreateTask(TaskStep step)
        {
            var task = new TaskClientRegistrationItem();
            task.Created = task.Modified = DateTime.Now;
            task.Request = _request;
            task.TaskStatus = TaskStatus.Pendente;
            task.TaskStep = step;
            Context.TaskClientRegistration.InsertOnSubmit(task);
        }

        private void ProcessNextFlow()
        {
            //if (_request.RequestStatus == RequestStatus.Reprovado)
            //    SendEmailReprovedToRequester();

            //if (_request.RequestStatus == RequestStatus.Retorno)
            //    SendEmailReturnToRequester();

            //else if (_request.RequestStatus == RequestStatus.Finalizado)
            //    SendEmailStepFinishToRequester();

            //else if (_request.RequestStep != null)
            //    SendEmailStepInitialCadastre();
        }
    }
}
