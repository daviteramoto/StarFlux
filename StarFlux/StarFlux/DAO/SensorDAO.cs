using StarFlux.Models;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace StarFlux.DAO
{
    public class SensorDAO : PadraoDAO<SensorViewModel>
    {
        protected override SqlParameter[] CriaParametros(SensorViewModel model)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", model.ID),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("entidade", model.Entidade)
            };

            return p;
        }

        protected override SensorViewModel MontaModel(DataRow registro)
        {
            SensorViewModel c = new SensorViewModel()
            {
                ID = Convert.ToInt32(registro["ID"]),
                Nome = registro["Nome"].ToString(),
                Entidade = registro["Entidade"].ToString()
            };

            return c;
        }

        protected override void SetTabela()
        {
            Tabela = "Sensores";
            NomeSpListagem = "spListagemSensores";
        }

        public List<SensorViewModel> BuscaSensoresFiltro(int codigo, string nome, string entidade)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", codigo),
                new SqlParameter("nome", nome),
                new SqlParameter("entidade", entidade)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spListagemSensoresFiltro", p);
            var lista = new List<SensorViewModel>();
            foreach (DataRow item in tabela.Rows)
                lista.Add(MontaModel(item));

            return lista;
        }
    }
}
