using Microsoft.AspNetCore.Http;
using System;

namespace StarFlux.Controllers
{
    public class HelperControllers
    {
        public static Boolean VerificaUsuarioLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return false;
            else
                return true;
        }

        public static Boolean VerificaUsuarioAdministrador(ISession session)
        {
            string administrador = session.GetString("Administrador");
            if (administrador == "true")
                return true;
            else 
                return false;
        }
    }
}
