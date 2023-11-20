using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using StarFlux.Models;

namespace StarFlux.DAO
{
    public class ApartamentoDAO : PadraoDAO<ApartamentoViewModel>
    {
        protected override SqlParameter[] CriaParametros(ApartamentoViewModel model)
        {
            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("id", model.ID);
            p[1] = new SqlParameter("nome", model.Nome);
            p[2] = new SqlParameter("dataCadastro", model.DataCadastro);
            p[3] = new SqlParameter("idTorre", model.ID_Torre);
            p[4] = new SqlParameter("idSensor", model.ID_Sensor);
            return p;
        }

        protected override ApartamentoViewModel MontaModel(DataRow registro)
        {
            ApartamentoViewModel u = new ApartamentoViewModel();
            u.ID = Convert.ToInt32(registro["ID"]);
            u.Nome = registro["Nome"].ToString();
            u.DataCadastro = Convert.ToDateTime(registro["dataCadastro"]);
            u.ID_Torre = Convert.ToInt32(registro["ID_Torre"]);
            u.ID_Sensor = Convert.ToInt32(registro["ID_Sensor"]);

            if (registro.Table.Columns.Contains("nomeTorre"))
                u.NomeTorre = registro["nomeTorre"].ToString();

            if (registro.Table.Columns.Contains("nomeSensor"))
                u.NomeSensor = registro["nomeSensor"].ToString();

            if (registro.Table.Columns.Contains("entidadeSensor"))
                u.EntidadeSensor = registro["entidadeSensor"].ToString();

            return u;
        }

        protected override void SetTabela()
        {
            Tabela = "Apartamentos";
            NomeSpListagem = "spListagemApartamentos";
        }

        public List<ApartamentoViewModel> BuscaApartamentosFiltro(
            int codigo,
            string nome,
            DateTime dataInicial,
            DateTime dataFinal,
            int torre,
            int sensor)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", codigo),
                new SqlParameter("nome", nome),
                new SqlParameter("dataInicial", dataInicial),
                new SqlParameter("dataFinal", dataFinal),
                new SqlParameter("idTorre", torre),
                new SqlParameter("idSensor", sensor)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spListagemApartamentosFiltro", p);
            var lista = new List<ApartamentoViewModel>();
            foreach (DataRow item in tabela.Rows)
                lista.Add(MontaModel(item));

            return lista;
        }

        public List<ApartamentoViewModel> ListagemApartamentosSensores()
        {
            SqlParameter[] p = null;

            var tabela = HelperDAO.ExecutaProcSelect("ListagemApartamentosSensores", p);
            List<ApartamentoViewModel> lista = new List<ApartamentoViewModel>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}
