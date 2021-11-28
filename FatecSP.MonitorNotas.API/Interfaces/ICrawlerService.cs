using FatecSP.MonitorNotas.API.Models;
using System.Collections.Generic;

namespace FatecSP.MonitorNotas.API.Interfaces
{
    public interface ICrawlerService
    {
        List<Materia> PegarNotas();
        void SetCredenciais(Credenciais credenciais);
    }
}
