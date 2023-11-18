using System.Data;
using System.Data.SqlClient;

namespace StarFlux.DAO
{
    public class HelperDAO
    {
        public static void ExecutaProc(string nomeProc, SqlParameter[] p)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (p != null)
                        comando.Parameters.AddRange(p);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] p)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    if (p != null)
                        adapter.SelectCommand.Parameters.AddRange(p);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }
        }
    }
}
