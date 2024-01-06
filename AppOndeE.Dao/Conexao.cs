using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AppOndeE.Dao
{
    public abstract class Conexao
    {
        protected MySqlConnection CriarConexao()
        {
            return new MySqlConnection
            {
                ConnectionString = "Server=15.235.55.95;DataBase=appondee;Uid=appondee;Pwd=kE@laqJLl#1DM"
            };
        }
    }
}
