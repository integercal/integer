﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;
using DbC;
using Integer.Domain.Agenda.Exceptions;
using System.EnterpriseServices;

namespace Integer.Domain.Agenda
{
    public class AgendaEventoService
    {
        private Eventos eventos;

        public AgendaEventoService(Eventos eventos)
        {
            this.eventos = eventos;
        }

        public void Agendar(Evento novoEvento)
        {
            var dataAberta = new DateTime(2013, 01, 01);
            var dataAbertaParaAgendamento = Assertion.That(!String.IsNullOrEmpty(novoEvento.Id) || novoEvento.DataInicio > new DateTime(2013, 01, 01))
                                                        .WhenNot(String.Format("Só é permitido agendar eventos para datas posteriores a {0}", dataAberta.ToString("dd/MM/yyyy")));
            dataAbertaParaAgendamento.Validate();

            VerificaSeConflitaComEventoParoquial(novoEvento);
            if (novoEvento.Tipo == TipoEventoEnum.Paroquial)
                DesmarcaEventosNaoParoquiaisDaMesmaData(novoEvento);
            
            VerificaDisponibilidadeDeLocais(novoEvento);

            eventos.Salvar(novoEvento);
        }

        private void VerificaSeConflitaComEventoParoquial(Evento novoEvento)
        {
            IEnumerable<Evento> eventosParoquiaisEmConflito = eventos.Todos(Evento.MontarVerificacaoDeConflitoDeHorario(novoEvento.DataInicio, novoEvento.DataFim))
                .Where(e => e.Tipo == TipoEventoEnum.Paroquial && e.Id != novoEvento.Id).ToList();

            if (eventosParoquiaisEmConflito.Count() > 0)
                throw new EventoPrioritarioException(eventosParoquiaisEmConflito);
        }

        private void DesmarcaEventosNaoParoquiaisDaMesmaData(Evento novoEvento)
        {
            var eventosNaoParoquiaisEmConflito = eventos.Todos(Evento.MontarVerificacaoDeConflitoDeHorario(novoEvento.DataInicio, novoEvento.DataFim))
                .Where(e => e.Tipo != TipoEventoEnum.Paroquial && e.Id != novoEvento.Id).ToList();
            foreach (Evento eventoNaoParoquial in eventosNaoParoquiaisEmConflito)
            {
                eventoNaoParoquial.AdicionarConflito(novoEvento, MotivoConflitoEnum.ExisteEventoPrioritarioNaData);
            }
        }

        private void VerificaDisponibilidadeDeLocais(Evento novoEvento)
        {
            IEnumerable<Evento> eventosQueReservaramLocalNoMesmoHorario = eventos.QueReservaramOMesmoLocal(novoEvento);

            IList<Evento> eventosPrioritarios = new List<Evento>();
            foreach (Evento eventoQueReservouLocal in eventosQueReservaramLocalNoMesmoHorario)
            {
                if (eventoQueReservouLocal.PossuiPrioridadeSobre(novoEvento))
                    eventosPrioritarios.Add(eventoQueReservouLocal);
                else
                    eventoQueReservouLocal.AdicionarConflito(novoEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            }

            if (eventosPrioritarios.Count > 0)
                throw new LocalReservadoException(eventosPrioritarios, null);
        }
    }
}
