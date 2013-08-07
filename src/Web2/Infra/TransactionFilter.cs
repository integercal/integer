using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using DbC;
using Integer.Infrastructure.Exceptions;
using Integer.Infrastructure.Tasks;
using Web.Controllers;

namespace Web.Infra
{
    public class TransactionFilter : ActionFilterAttribute 
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            bool doNotCallSaveChanges = ((IntegerController)actionExecutedContext.ActionContext.ControllerContext.Controller).DoNotCallSaveChanges;

            using (var session = IntegerConfig.CurrentSession)
            {
                if (session == null || actionExecutedContext.Exception != null || doNotCallSaveChanges)
                    return;

                session.SaveChanges();
                TaskExecutor.StartExecuting();                
            }
        }
    }
}