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
        private TaskClientRegistrationItem _task;
        public ListModelDataContext contextModel { get; private set; }
        private WorkflowEmail workflowEmail;
        private TaskStatus _status;
        private string _evaluationObservation;
        
        public bool Request(ClientRequestItem request)
        {
            _request = request;
            _request.RequesterId = SPContext.Current.Web.CurrentUser.ID;
            var run = new RunWithElevatedPrivileges();
            return run.Execute(RequestDelegate);
        }

        private bool RequestDelegate(SPWeb oWeb)
        {
            try
            {
                contextModel = new ListModelDataContext(oWeb.Url);
                if (_request.Id.HasValue)
                    throw new Exception("Request already save. Use method Update");

                LoadListsToRequest();
                _request.RequestStatus = RequestStatus.Pendente;
                _request.Created = _request.Modified = DateTime.Now;
                contextModel.ClientRequest.InsertOnSubmit(_request);
                RequestBase(oWeb);
                return true;
            }catch
            {
                return false;
            }
        }

        private void LoadListsToRequest()
        {
            _request.Sbu = contextModel.Sbu.Where(c => c.Id == _request.Sbu.Id).FirstOrDefault();
            _request.PhoneFirstType = contextModel.PhoneType.Where(c => c.Id == _request.PhoneFirstType.Id).FirstOrDefault();
            if (_request.PhoneSecondType != null)
                _request.PhoneSecondType = contextModel.PhoneType.Where(c => c.Id == _request.PhoneSecondType.Id).FirstOrDefault();
            if (_request.PaymentCondition != null)
                _request.PaymentCondition = contextModel.PaymentCondition.Where(c => c.Id == _request.PaymentCondition.Id).FirstOrDefault();
        }

        private void RequestBase(SPWeb oWeb)
        {
            NextFlow();
            contextModel.SubmitChanges();
            _request.SaveAttachments(oWeb);
            ProcessNextFlow();
        }

        public void MakeEvaluation(ClientRequestItem request, TaskClientRegistrationItem task, TaskStatus status, string evaluationObservation)
        {
            _request = request;
            _task = task;
            _status = status;
            _evaluationObservation = evaluationObservation;

            var run = new RunWithElevatedPrivileges();
            run.Execute(MakeEvaluationDelegate);
        }

        private bool MakeEvaluationDelegate(SPWeb web)
        {
            try
            {
                contextModel = new ListModelDataContext(web.Url);
                _task.TaskUserId = web.CurrentUser.ID;

                SetEvaluationByStatus();
                SaveEvaluation(web);
                ProcessNextFlow();
                return true;
            }catch
            {
                return false;
            }
        }

        private void SaveEvaluation(SPWeb web)
        {
            contextModel.TaskClientRegistration.Attach(_task);
            contextModel.ClientRequest.Attach(_request);
            contextModel.SubmitChanges();
            _request.SaveAttachments(web);
        }

        private void SetEvaluationByStatus()
        {
            if (_status == TaskStatus.Iniciado)
            {
                _task.TaskStatus = TaskStatus.Iniciado;
                if (_request.RequestStatus != RequestStatus.Retorno)
                    _request.RequestStatus = RequestStatus.Iniciado;
            }
            else if (_status == TaskStatus.Aprovado)
            {
                _task.TaskStatus = TaskStatus.Aprovado;
                if (_request.RequestStatus != RequestStatus.Retorno)
                    _request.RequestStatus = RequestStatus.Iniciado;
                NextFlow();
            }
            else if (_status == TaskStatus.Reprovado)
            {
                _task.TaskStatus = TaskStatus.Reprovado;
                _task.Observation = _evaluationObservation;
                _request.RequestStatus = RequestStatus.Reprovado;
                NextFlow();
            }
            else if (_status == TaskStatus.Retorno)
            {
                _task.TaskStatus = TaskStatus.Retorno;
                _task.Observation = _evaluationObservation;
                _request.RequestStatus = RequestStatus.Retorno;
                NextFlow();
            }
        }

        private void NextFlow()
        {
            if (_request.RequestStatus == RequestStatus.Finalizado || _request.RequestStatus == RequestStatus.Reprovado)
                return;
            
            if(_task != null && _task.TaskStatus == TaskStatus.Retorno)
            {
                CreateTask(TaskStep.Customer); 
            }
            else if (_request.RequestStep == null)
            {
                _request.RequestStep = RequestStep.Customer;
                CreateTask(TaskStep.Customer); 
            }
            else if (_request.RequestStep == RequestStep.Customer)
            {
                _request.RequestStep = RequestStep.Fiscal;
                CreateTask(TaskStep.Fiscal);
            }
            else if (_request.RequestStep == RequestStep.Fiscal)
            {
                if(_request.RequestStatus != RequestStatus.Retorno)
                {
                    _request.RequestStep = RequestStep.CAS;
                    CreateTask(TaskStep.CAS);
                }
                else
                {
                    _request.RequestStatus = RequestStatus.Iniciado;
                    CreateTask(TaskStep.Fiscal);
                }
                
            }
            else if (_request.RequestStep == RequestStep.CAS)
            {
                if (_request.RequestStatus != RequestStatus.Retorno)
                {
                    _request.RequestStep = RequestStep.Logistica;
                    CreateTask(TaskStep.Logistica);
                }
                else
                {
                    _request.RequestStatus = RequestStatus.Iniciado;
                    CreateTask(TaskStep.CAS);
                }
                
            }
            else if (_request.RequestStep == RequestStep.Logistica)
            {
                if (_request.RequestStatus != RequestStatus.Retorno)
                {
                    _request.RequestStep = RequestStep.Crédito;
                    CreateTask(TaskStep.Crédito);
                }
                else
                {
                    _request.RequestStatus = RequestStatus.Iniciado;
                    CreateTask(TaskStep.Logistica);
                }
            }
            else if (_request.RequestStep == RequestStep.Crédito)
            {
                if (_request.RequestStatus != RequestStatus.Retorno)
                {
                    _request.RequestStep = RequestStep.Cadastro;
                    CreateTask(TaskStep.Cadastro);
                }
                    
                else
                {
                    _request.RequestStatus = RequestStatus.Iniciado;
                    CreateTask(TaskStep.Crédito);
                }
            }

            else if (_request.RequestStep == RequestStep.Cadastro)
            {
                _request.RequestStatus = RequestStatus.Finalizado;
            }
        }

        private void CreateTask(TaskStep step)
        {
            var task = new TaskClientRegistrationItem();
            task.Created = task.Modified = DateTime.Now;
            task.Request = _request;
            task.TaskStatus = TaskStatus.Pendente;
            task.TaskStep = step;
            contextModel.TaskClientRegistration.InsertOnSubmit(task);
        }

        private void ProcessNextFlow()
        {
            workflowEmail = new WorkflowEmail(_request);

            if (_request.RequestStatus == RequestStatus.Reprovado)
                workflowEmail.SendEmailReproved();

            if (_request.RequestStatus == RequestStatus.Retorno)
                workflowEmail.SendEmailReturn();

            else if (_request.RequestStatus == RequestStatus.Finalizado)
                workflowEmail.SendEmailStepFinish();

            else if (_request.RequestStep != null)
                workflowEmail.SendEmailStepInitialCadastre();
        }
    }
}
