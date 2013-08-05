using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Infrastructure.DocumentModelling;
using Integer.Domain.Paroquia;
using DbC;

namespace Integer.Domain.Agenda
{
    [Serializable]
    public class Reserva
    {
        public Reserva(Local local)
        {
            #region pré-condição
            var localFoiInformado = Assertion.That(local != null)
                                            .WhenNot("Necessário informar local da reserva.");
            #endregion
            localFoiInformado.Validate();

            this.Local = local;
        }

        public DenormalizedReference<Local> Local { get; set; }
    }
}
