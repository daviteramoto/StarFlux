using System;
using System.Data;
using System.Data.SqlClient;
using StarFlux.Models;

namespace StarFlux.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuarioViewModel usuario)
        {
            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("id", usuario.ID);
            p[1] = new SqlParameter("login", usuario.Login);
            p[2] = new SqlParameter("senha", usuario.Senha);
            p[3] = new SqlParameter("administrador", usuario.Administrador);

            return p;
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel u = new UsuarioViewModel();
            u.ID = Convert.ToInt32(registro["ID"]);
            u.Login = registro["Login"].ToString();
            u.Senha = registro["Senha"].ToString();
            if (Convert.ToInt32(registro["Administrador"]) == 1)
                u.Administrador = true;
            else
                u.Administrador = false;

            return u;
        }

        public UsuarioViewModel VerificaUsuario(string login, string senha)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login", login),
                new SqlParameter("senha", senha)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spVerificaUsuario", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public bool VerificaLoginExistente(string login)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login", login)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spVerificaLoginExistente", p);
            if (tabela.Rows.Count == 0)
                return false;
            else
                return true;
        }

        protected override void SetTabela()
        {
            Tabela = "Usuarios";
            NomeSpListagem = "spListagemUsuarios";
        }
    }
}
