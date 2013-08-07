using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Integer.Infrastructure.Criptografia;

namespace Integer.Infrastructure.Email.TemplateModels
{
    public class NovoUsuarioModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
