using System.Data.SqlClient;

namespace StarFlux.DAO
{
    public static class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = "Data Source=DAVI\\SQLEXPRESS;Database=BDN1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
