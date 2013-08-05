using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Integer.Domain.Acesso;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Criptografia;
using Integer.Infrastructure.IoC;
using Web.Areas.Api.Security;

namespace Web.Security
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Authenticate(request);
            return base.SendAsync(request, cancellationToken);
        }

        private void Authenticate(HttpRequestMessage request)
        {
            AuthenticationHeaderValue authValue = request.Headers.Authorization;
            if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter))
            {
                var usuarios = (Usuarios)request.GetDependencyScope().GetService(typeof(Usuarios));
                Usuario usuario = usuarios.ComToken(new Guid(authValue.Parameter));
                if (usuario != null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new ApiIdentity(usuario), new string[] { });
                }
            }
        }
    }
}