using FatecSP.MonitorNotas.API.Interfaces;
using FatecSP.MonitorNotas.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FatecSP.MonitorNotas.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {
        private IMonitorHandle MonitorHandle { get; set; }

        public MonitorController(IMonitorHandle monitorHandle)
        {
            this.MonitorHandle = monitorHandle;
        }

        [HttpPost]
        public IActionResult Post([FromHeader] long matricula, [FromHeader] string senha, [FromHeader] string emailContato)
        {
            Credenciais credenciais = new()
            {
                Matricula = matricula,
                Senha = senha,
                EmailContato = emailContato
            };

            try
            {
                var notas = this.MonitorHandle.Handle(credenciais);

                return Ok(notas);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }


        }
    }
}
