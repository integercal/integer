using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace site.Controllers
{
    public class SiteController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Versoes()
        {
            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        [HttpPost]
        public void Contato(Contato contato)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("contato@calendarioparoquial.com.br", "PlaBeno2"),
                EnableSsl = true
            };
            string msg = string.Format("Nome: {0} --- E-mail: {1} --- Paróquia: {2} --- Comentário: {3}", contato.Nome, contato.Email, contato.Paroquia, contato.Comentario);
            client.SendAsync(contato.Email, "contato@calendarioparoquial.com.br", "[contratação]", msg, null);
        }
    }

    public class Contato 
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Paroquia { get; set; }
        public string Comentario { get; set; }
    }
}
