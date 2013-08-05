using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using DbC;
using Integer.Infrastructure.Exceptions;
using Web.Controllers;

namespace Web.Infra
{
    public class ExceptionFilter : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string responseMessage = "";
            var ex = actionExecutedContext.Exception;

            if (ex is DbCException || ex is IntegerException)
            {
                responseMessage = ex.Message.Replace(Environment.NewLine, "<br/>");
            }
            else 
            {
                //TODO log the error
                responseMessage = "Ops! Ocorreu um problema :(";
            }
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Error = responseMessage });

            ((IntegerController)actionExecutedContext.ActionContext.ControllerContext.Controller).DoNotCallSaveChanges = true;
        }
    }
}