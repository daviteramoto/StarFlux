using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using StarFlux.Models;

namespace StarFlux.DAO
{
    public class HabitanteDAO : PadraoDAO<HabitanteViewModel>
    {
        protected override SqlParameter[] CriaParametros(HabitanteViewModel habitante)
        {
            object anxByte = habitante.FotoByte;
            if (anxByte == null)
                anxByte = DBNull.Value;

            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("id", habitante.ID);
            p[1] = new SqlParameter("nome", habitante.Nome);
            p[2] = new SqlParameter("dataNascimento", habitante.DataNascimento);
            p[3] = new SqlParameter("idApartamento", habitante.ID_Apartamento);
            p[4] = new SqlParameter("foto", anxByte);

            return p;
        }

        protected override HabitanteViewModel MontaModel(DataRow registro)
        {
            HabitanteViewModel u = new HabitanteViewModel();
            u.ID = Convert.ToInt32(registro["ID"]);
            u.Nome = registro["Nome"].ToString();
            u.DataNascimento = Convert.ToDateTime(registro["DataNascimento"]);
            u.ID_Apartamento = Convert.ToInt32(registro["ID_Apartamento"]);

            if (registro.Table.Columns.Contains("nomeApartamento"))
                u.NomeApartamento = registro["nomeApartamento"].ToString();

            if (registro.Table.Columns.Contains("nomeTorre"))
                u.NomeTorre = registro["nomeTorre"].ToString();

            if (registro["Foto"] != DBNull.Value)
                u.FotoByte = registro["Foto"] as byte[];

            return u;
        }

        protected override void SetTabela()
        {
            Tabela = "Habitantes";
            NomeSpListagem = "spListagemHabitantes";
        }

        public List<HabitanteViewModel> BuscaHabitantesFiltro(
            int codigo,
            string nome,
            DateTime dataInicial,
            DateTime dataFinal,
            int apartamento,
            int torre)
        {
            SqlParameter[] p =
            {
                new SqlParameter("id", codigo),
                new SqlParameter("nome", nome),
                new SqlParameter("dataInicial", dataInicial),
                new SqlParameter("dataFinal", dataFinal),
                new SqlParameter("idApartamento", apartamento),
                new SqlParameter("idTorre", torre)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spListagemHabitantesFiltro", p);
            var lista = new List<HabitanteViewModel>();
            foreach (DataRow item in tabela.Rows)
                lista.Add(MontaModel(item));

            return lista;
        }
    }
}
