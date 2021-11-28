using System;

namespace FatecSP.MonitorNotas.API.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException()
        {

        }

        public LoginException(string msg) : base(msg)
        {

        }
    }
}
