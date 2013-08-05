using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Integer.Domain.Paroquia;
using Raven.Client;
using Integer.Domain.Acesso;
using System.Web.Http.Controllers;
using System.Threading;
using Integer.Infrastructure.Repository;
using System.Threading.Tasks;
using Integer.Infrastructure.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class IntegerController : ApiController
    {        
        public IDocumentSession RavenSession { get; set; }

        public bool DoNotCallSaveChanges { get; set; }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            // TODO multi-tenancy (wait RavenHQ response)
            //var host = controllerContext.Request.Headers.Host;
            //string subdomain = "";

            //var index = host.IndexOf(".");
            //if (index >= 0)
            //{
            //    subdomain = host.Substring(0, index);
            //}
            //RavenSession = DocumentStoreHolder.DocumentStore.OpenSession(subdomain);
            RavenSession = IntegerConfig.CurrentSession;

            return base.ExecuteAsync(controllerContext, cancellationToken)
                .ContinueWith(task =>
                {
                    using (RavenSession) 
                    {
                        if (task.Status != TaskStatus.Faulted && RavenSession != null && DoNotCallSaveChanges == false)
                        {
                            RavenSession.SaveChanges();
                            TaskExecutor.StartExecuting();
                        }
                    }
                    return task;
                }).Unwrap();
        }
    }
}
