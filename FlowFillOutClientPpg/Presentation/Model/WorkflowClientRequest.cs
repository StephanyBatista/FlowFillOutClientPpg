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
            _request.SbuId = Context.Sbu.Where(c => c.Id == _request.SbuId.Id).FirstOrDefault();
            _request.PhoneFirstTypeId = Context.PhoneType.Where(c => c.Id == _request.PhoneFirstTypeId.Id).FirstOrDefault();
            if (_request.PhoneSecondTypeId != null)
                _request.PhoneSecondTypeId = Context.PhoneType.Where(c => c.Id == _request.PhoneSecondTypeId.Id).FirstOrDefault();
            if (_request.PaymentConditionId != null)
                _request.PaymentConditionId = Context.PaymentCondition.Where(c => c.Id == _request.PaymentConditionId.Id).FirstOrDefault();
        }

        private void RequestBase(SPWeb oWeb)
        {
            NextFlow();
            Context.SubmitChanges();
            _request.SaveAttachments(oWeb);
            ProcessNextFlow();
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
