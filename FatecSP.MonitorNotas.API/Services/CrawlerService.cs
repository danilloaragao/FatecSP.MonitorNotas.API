using FatecSP.MonitorNotas.API.Exceptions;
using FatecSP.MonitorNotas.API.Interfaces;
using FatecSP.MonitorNotas.API.Models;
using HtmlAgilityPack;
using ScrapySharp.Html.Forms;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FatecSP.MonitorNotas.API.Services
{
    class CrawlerService : ICrawlerService
    {
        public Credenciais Credenciais { get; set; }
        private readonly ScrapingBrowser Browser = new();

        public void SetCredenciais(Credenciais credenciais)
        {
            this.Credenciais = credenciais;
        }

        public List<Materia> PegarNotas()
        {
            List<Materia> materias = new();

            WebPage pageResult = this.Browser.NavigateToPage(new Uri("http://san.fatecsp.br/"));

            HtmlWeb web = new();
            HtmlDocument doc = new();

            PageWebForm form = pageResult.FindFormById("login");

            if (form != null)
            {
                form["userid"] = this.Credenciais.Matricula.ToString();
                form["password"] = this.Credenciais.Senha;

                form.Method = HttpVerb.Post;
                pageResult = form.Submit();
            }

            pageResult = this.Browser.NavigateToPage(new Uri("http://san.fatecsp.br/index.php?task=conceitos_finais"));

            if (pageResult.Html.InnerHtml.Contains("<input type=\"password\" name=\"password\" id=\"password\">"))
                throw new LoginException("RA ou Senha incorretos");

            doc.LoadHtml(pageResult.Html.InnerHtml);

            List<HtmlNode> trNodes = doc.GetElementbyId("disciplinas").Descendants("tr").Where(n => !string.IsNullOrWhiteSpace(n.InnerText)
                                                                                                && !n.InnerText.Contains("Nome da Disciplina")
                                                                                                && !n.InnerHtml.Contains("<td align=\"left\"><font size=\"1\">")).ToList();

            foreach (HtmlNode node in trNodes)
            {
                Materia materia = new();
                materia.Nome = node.SelectSingleNode("td[2]").InnerText.Trim();
                materia.Nota = node.SelectSingleNode("td[5]").InnerText.Replace("&atilde;", "ã")
                                                                       .Replace("&ccedil;", "ç")
                                                                       .Replace("\n", "")
                                                                       .Replace("\n", "")
                                                                       .Trim();
                materias.Add(materia);
            }

            return materias;
        }
    }
}
