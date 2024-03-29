﻿using FatecSP.MonitorNotas.API.Interfaces;
using FatecSP.MonitorNotas.API.Models;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FatecSP.MonitorNotas.API.Handles
{
    public class MonitorHandle : IMonitorHandle
    {
        private Credenciais Credenciais { get; set; }
        private List<Materia> Notas = new();
        private ICrawlerService Crawler { get; set; }
        private IEmailService EmailService { get; set; }

        public MonitorHandle(ICrawlerService crawler, IEmailService emailService)
        {
            this.Crawler = crawler;
            this.EmailService = emailService;
        }

        public List<Materia> Handle(Credenciais credenciais)
        {
            this.Credenciais = credenciais;
            this.Crawler.SetCredenciais(credenciais);
            this.Notas = this.Crawler.PegarNotas();

            MonitorarNotas();

            return Notas;
        }

        private async void MonitorarNotas()
        {
            await Task.Delay(1);
            int contagemFalhas = 0;

            while (!VerificaNotasLancadas())
            {
                try
                {
                    List<Materia> notasAtuais = Crawler.PegarNotas();
                    bool atualizado = false;

                    foreach (Materia materia in notasAtuais)
                    {
                        if (!materia.Nota.Equals(Notas.FirstOrDefault(m => m.Nome.Equals(materia.Nome)).Nota))
                        {
                            atualizado = true;
                            break;
                        }
                    }

                    if (atualizado || (DateTime.Now.Hour >= 0 && DateTime.Now.Minute <= 31))
                    {
                        Notas = notasAtuais;
                        EmailService.EnviarEmail(Notas, Credenciais.EmailContato);
                    }
                }
                catch (Exception)
                {
                    contagemFalhas++;
                    if (contagemFalhas > 10)
                        throw;
                }
                int intervaloVerificacao = 30;
                await Task.Delay(intervaloVerificacao * 60 * 1000);
            }
        }

        private bool VerificaNotasLancadas()
        {
            foreach (Materia materia in this.Notas)
            {
                if (materia.Nota.Contains("lan"))
                    return false;
            }
            return true;
        }
    }
}
