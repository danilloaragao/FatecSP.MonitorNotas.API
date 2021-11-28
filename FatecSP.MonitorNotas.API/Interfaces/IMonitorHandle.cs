using FatecSP.MonitorNotas.API.Models;
using System.Collections.Generic;

namespace FatecSP.MonitorNotas.API.Interfaces
{
    public interface IMonitorHandle
    {
        List<Materia> Handle(Credenciais credenciais);
    }
}
