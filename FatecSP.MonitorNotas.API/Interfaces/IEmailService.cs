using FatecSP.MonitorNotas.API.Models;
using System.Collections.Generic;

namespace FatecSP.MonitorNotas.API.Interfaces
{
    public interface IEmailService
    {
        void EnviarEmail(List<Materia> materias, string enderecoDestino);
    }
}
