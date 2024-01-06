using AppOndeE.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppOndeE.Dao
{
    public class UsuarioClienteDao: Conexao
    {
        public List<UsuarioClienteModel> ListarUsuarios(int idCliente)
        {
            List<UsuarioClienteModel> lista = null;

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT id,idCliente,email,senha,ultimoAcesso from usuariosCliente where idCliente = @idCliente;",
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
                        lista = new List<UsuarioClienteModel>();

                        while (reader.Read())
                        {
                            var UltimoAcesso = string.IsNullOrEmpty(reader["ultimoAcesso"].ToString()) ? "" : 
                                Convert.ToDateTime(reader["ultimoAcesso"].ToString()).ToShortDateString() + " " +
                                Convert.ToDateTime(reader["ultimoAcesso"].ToString()).ToShortTimeString();

                            lista.Add(new UsuarioClienteModel
                            {
                                Email = reader["email"].ToString(),
                                Id = (int)reader["id"],
                                IdCliente = (int)reader["idCliente"],
                                Senha = "",
                                UltimoAcesso = UltimoAcesso.ToString()
                            }); ;
                        }

                        return lista;
                    }
                    else
                    {
                        return lista;
                    }
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

        public UsuarioClienteModel CarregarDadosUsuario(int id)
        {
            UsuarioClienteModel lista = null;
            var key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            CriptoGrafia cripto = new CriptoGrafia(key);

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT id,idCliente,email,senha,ultimoAcesso from usuariosCliente where id = @id;",
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
                            var senhaDesCriptografada = cripto.Descriptografar(reader["senha"].ToString());

                            lista = new UsuarioClienteModel
                            {
                                Email = reader["email"].ToString(),
                                Id = (int)reader["id"],
                                IdCliente = (int)reader["idCliente"],
                                Senha = senhaDesCriptografada,
                            };
                        }

                        return lista;
                    }
                    else
                    {
                        return lista;
                    }
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

        public bool ExisteEmail(string email)
        {
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT count(*) from usuarioscliente where email = @email;"
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@email", email);

                    var retorno = Convert.ToInt32(cmd.ExecuteScalar());

                    return retorno > 0 ? true : false;
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

        public bool UsuarioLogado(int id)
        {
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT count(*) from usuarioscliente WHERE id = @id and usuarioLogado = 1;"
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    var retorno = Convert.ToInt32(cmd.ExecuteScalar());

                    return retorno > 0 ? true : false;
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

        public bool SalvarDadosUsuario(UsuarioClienteModel model)
        {
            string sql = "";
            bool novoRegistro = false;
            var key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            CriptoGrafia cripto = new CriptoGrafia(key);
            var senhaCriptografada = cripto.Criptografar(model.Senha);

            if (model.Id == 0)
            {
                novoRegistro = true;
                sql = "INSERT INTO usuarioscliente (idCliente,email,senha) VALUES (@idCliente,@email,@senha);";
            }
            else
            {
                novoRegistro = false;
                sql = "UPDATE usuarioscliente SET idCliente = @idCliente, email = @email, senha = @senha WHERE id = @id;";
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
                    cmd.Parameters.AddWithValue("@email", model.Email);
                    cmd.Parameters.AddWithValue("@senha", senhaCriptografada);

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

        public bool ExcluirDadosCliente(int id)
        {
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "DELETE FROM usuarioscliente WHERE id = @id;",
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
    }
}
