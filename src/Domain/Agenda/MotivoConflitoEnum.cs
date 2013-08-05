using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Integer.Domain.Agenda
{
    public enum MotivoConflitoEnum
    {
        [Description("Existe evento prioritário para a data.")]
        ExisteEventoPrioritarioNaData = 1,
        [Description("Local foi reservado para outro evento de maior prioridade.")]
        LocalReservadoParaEventoDeMaiorPrioridade = 2
    }
}
