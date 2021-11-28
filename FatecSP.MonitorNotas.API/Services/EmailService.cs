using FatecSP.MonitorNotas.API.Interfaces;
using FatecSP.MonitorNotas.API.Models;
using FatecSP.MonitorNotas.API.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace FatecSP.MonitorNotas.API.Services
{
    public class EmailService : IEmailService
    {
        private string Usuario { get; set; }
        private string Senha { get; set; }

        public EmailService()
        {
            this.Usuario = Environment.GetEnvironmentVariable("UsuarioEmail");
            this.Senha = Environment.GetEnvironmentVariable("SenhaEmail");
        }
        public void EnviarEmail(List<Materia> materias, string enderecoDestino)
        {
            try
            {
                var fromAddress = new MailAddress(this.Usuario, "Envio Automático");
                var toAddress = new MailAddress(enderecoDestino);
                string fromPassword = this.Senha;
                string subject = "Atualização das notas no portal";
                string body = "";

                foreach (Materia materia in materias)
                {
                    body += $"{materia.Nome} -> {materia.Nota.LimparNota()}\n";
                }

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível enviar o e-mail: {ex.Message}");
            }
        }
    }
}
