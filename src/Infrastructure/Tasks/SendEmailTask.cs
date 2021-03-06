﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Collections;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.Routing;

namespace Integer.Infrastructure.Tasks
{
    public class SendEmailTask : BackgroundTask
    {
        private string replyTo;
        private readonly string subject;
        private readonly string view;
        private readonly object model;
        private readonly string sendTo;

        public SendEmailTask(
            string subject,
            string view,
            string sendTo,
            object model) : this(null, subject, view, sendTo, model)
        {
        }

        public SendEmailTask(
            string replyTo,
            string subject,
            string view,
            string sendTo,
            object model)
        {
            this.replyTo = replyTo;
            this.subject = subject;
            this.view = view;
            this.model = model;
            this.sendTo = sendTo;
        }

        public override void Execute()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "MailTemplates");
            var controllerContext = new ControllerContext(new HttpContextWrapper(HttpContext.Current), routeData, new MailController());
            var viewEngineResult = ViewEngines.Engines.FindView(controllerContext, view, "_Layout");
            var stringWriter = new StringWriter();
            viewEngineResult.View.Render(
                new ViewContext(controllerContext, viewEngineResult.View, new ViewDataDictionary(model), new TempDataDictionary(),
                                stringWriter), stringWriter);

            if (string.IsNullOrEmpty(replyTo))
                replyTo = ConfigurationManager.AppSettings["EmailSender"].ToString();

            var from = new MailAddress(replyTo, "Calendário Paroquial");
            #if (DEBUG)
                var to = new MailAddress("danielsilva.rj@gmail.com");
            #else
                var to = new MailAddress(sendTo);
            #endif

            var mailMessage = new MailMessage(from, to)
                            {
                                IsBodyHtml = true,
                                Body = stringWriter.GetStringBuilder().ToString(),
                                Subject = subject
                            };

            var smtpClient = new SmtpClient("smtp.gmail.com", 587) { EnableSsl = true };
            using (smtpClient)
            {
                smtpClient.Credentials = new NetworkCredential("contato@calendarioparoquial.com.br", "PlaBeno2");
                smtpClient.Send(mailMessage);
            }
        }

        public class MailController : Controller
        {
        }

        public class MailHttpContext : HttpContextBase
        {
            private readonly IDictionary items = new Hashtable();

            public override IDictionary Items
            {
                get { return items; }
            }

            public override System.Web.Caching.Cache Cache
            {
                get { return HttpRuntime.Cache; }
            }

            public override HttpResponseBase Response
            {
                get { return new MailHttpResponse(); }
            }

            public override HttpRequestBase Request
            {
                get { return new HttpRequestWrapper(new HttpRequest("", HttpContext.Current.Request.Url.Host, "")); }
            }
        }

        public class MailHttpResponse : HttpResponseBase
        {
            public override string ApplyAppPathModifier(string virtualPath)
            {
                return virtualPath;
            }
        }
    }
}
