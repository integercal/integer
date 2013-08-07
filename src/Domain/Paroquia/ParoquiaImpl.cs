using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integer.Domain.Paroquia
{
    public class ParoquiaConfig
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Paroco { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public Configuracao Configuracao { get; set; }
        public Plano Plano { get; set; }
    }
}
