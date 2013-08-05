using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Raven.Client.Indexes;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class TipoEvento_NextPriority : AbstractIndexCreationTask<TipoEvento, TipoEvento_NextPriority.Result>
    {
        public class Result
        {
            public int Number { get; set; }
        }

        public TipoEvento_NextPriority()
        {
            Map = tipos => from tipo in tipos
                           select new
                           {
                               Number = tipo.NivelPrioridade
                           };

            Reduce = results => from result in results
                                group result by 0 into g
                                select new
                                {
                                    Number = g.Max(x => x.Number)
                                };
        }
    }
}
