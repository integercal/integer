using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.DocumentModelling;

namespace Integer.Domain.Agenda
{
    [Serializable]
    public class TipoEvento : INamedDocument
    {
        protected TipoEvento() { }

        public TipoEvento(string nome, int nivelPrioridade)
        {
            Nome = nome;
            NivelPrioridade = nivelPrioridade;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public int NivelPrioridade { get; set; }
        public bool IndependenteLocal { get; set; }
    }
}
