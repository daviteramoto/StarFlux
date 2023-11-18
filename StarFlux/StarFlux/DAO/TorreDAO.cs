using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StarFlux.Models;

namespace StarFlux.DAO
{
    public class TorreDAO : PadraoDAO<TorreViewModel>
    {
        protected override SqlParameter[] CriaParametros(TorreViewModel model)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", model.ID),
                new SqlParameter("nome", model.Nome)
            };

            return p;
        }

        protected override TorreViewModel MontaModel(DataRow registro)
        {
            TorreViewModel c = new TorreViewModel()
            {
                ID = Convert.ToInt32(registro["ID"]),
                Nome = registro["Nome"].ToString()
            };

            return c;
        }

        protected override void SetTabela()
        {
            Tabela = "Torres";
            NomeSpListagem = "spListagemTorres";
        }

        public List<TorreViewModel> BuscaTorresFiltro(int codigo, string nome)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", codigo),
                new SqlParameter("nome", nome)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spListagemTorresFiltro", p);
            var lista = new List<TorreViewModel>();
            foreach (DataRow item in tabela.Rows)
                lista.Add(MontaModel(item));

            return lista;
        }
    }
}
