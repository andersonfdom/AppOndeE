using System;
using System.Collections.Generic;
using System.Text;
using AppOndeE.Model;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AppOndeE.Dao
{
    public class AnuncioDao : Conexao
    {
        public List<AnuncioModel> Listar(int idCliente)
        {
            List<AnuncioModel> lista = null;

            string sql = "SELECT id,idCliente,tipoAnuncio,dataCadastroAnuncio,tituloAnuncio,descricaoAnuncio,midia";
            sql += " FROM anunciosclientes WHERE idCliente = @idCliente";

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        lista = new List<AnuncioModel>();

                        while (reader.Read())
                        {
                            var dataAnuncio = reader["dataCadastroAnuncio"] == null ? "" : Convert.ToDateTime(reader["dataCadastroAnuncio"].ToString()).ToShortDateString() + " " +
                                Convert.ToDateTime(reader["dataCadastroAnuncio"].ToString()).ToShortTimeString();

                            lista.Add(new AnuncioModel
                            {
                                DataAnuncio = dataAnuncio,
                                DescricaoAnuncio = reader["descricaoAnuncio"].ToString(),
                                Id = (int) reader["id"],
                                IdCliente = (int)reader["idCliente"],
                                TipoAnuncio = (int)reader["tipoAnuncio"],
                                TituloAnuncio = reader["tituloAnuncio"].ToString()
                            });
                        }
                    }

                    return lista;
                }
            }
            catch (Exception)
            {

                return lista;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public AnuncioModel CarregarDadosAnuncio(int id)
        {
            AnuncioModel model = null;

            string sql = "SELECT id,idCliente,tipoAnuncio,dataCadastroAnuncio,tituloAnuncio,descricaoAnuncio,midia";
            sql += " FROM anunciosclientes WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new AnuncioModel
                            {
                                DescricaoAnuncio = reader["descricaoAnuncio"].ToString(),
                                Id = (int)reader["id"],
                                IdCliente = (int)reader["idCliente"],
                                Midia = (byte[])reader["midia"],
                                TipoAnuncio = (int)reader["tipoAnuncio"],
                                TituloAnuncio = reader["tituloAnuncio"].ToString()
                            };
                        }
                    }

                    return model;
                }
            }
            catch (Exception)
            {
                return model;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public bool Gravar(AnuncioModel model)
        {
            bool novoRegistro = false;
            string sql = "";

            if (model.Id == 0)
            {
                novoRegistro = true;
                sql += "INSERT INTO anunciosclientes (idCliente,tipoAnuncio,dataCadastroAnuncio,tituloAnuncio,descricaoAnuncio,midia)";
                sql += " VALUES (@idCliente,@tipoAnuncio,NOW(),@tituloAnuncio,@descricaoAnuncio,@midia);";

            }
            else
            {
                novoRegistro = false;
                sql += "UPDATE anunciosclientes SET idCliente = @idCliente,tipoAnuncio = @tipoAnuncio,tituloAnuncio = @tituloAnuncio,";
                sql += "descricaoAnuncio = @descricaoAnuncio, midia = @midia WHERE id = @id;";
            }

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    
                    if (novoRegistro == false)
                    {
                        cmd.Parameters.AddWithValue("@id", model.Id);
                    }

                    cmd.Parameters.AddWithValue("@idCliente", model.IdCliente);
                    cmd.Parameters.AddWithValue("@tipoAnuncio", model.TipoAnuncio);
                    cmd.Parameters.AddWithValue("@tituloAnuncio", model.TituloAnuncio);
                    cmd.Parameters.AddWithValue("@descricaoAnuncio", model.DescricaoAnuncio);
                    cmd.Parameters.AddWithValue("@midia", model.Midia);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public bool Excluir(int id)
        {
            string sql = "DELETE FROM anunciosclientes WHERE id = @id;";

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {


                return false;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public int QtdeAnuncios(int idCliente)
        {
            int qtde = 0;
            string sql = "SELECT count(*) FROM anunciosclientes WHERE idCliente = @idCliente;";

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = sql,
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);

                    qtde = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    return qtde;
                }
            }
            catch (Exception)
            {


                return qtde;
            }

            finally
            {
                cmd.Connection.Close();
            }

        }
    }
}
