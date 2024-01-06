using System;
using System.Collections.Generic;
using System.Text;
using AppOndeE.Model;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AppOndeE.Dao
{
    public class ClienteDao : Conexao
    {     
        public Dictionary<string,string> DadosLogin(LoginCliente loginCliente)
        {
            var key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            CriptoGrafia cripto = new CriptoGrafia(key);
            var senhaCriptografada = cripto.Criptografar(loginCliente.Senha);

            Dictionary<string, string> dados = null;

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT id,idcliente FROM usuarioscliente WHERE email = @email AND senha = @senha;",
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@email", loginCliente.Login);
                    cmd.Parameters.AddWithValue("@senha", senhaCriptografada);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            dados = new Dictionary<string, string>
                            {
                                { "IdUsuario", reader["id"].ToString() },
                                { "IdCliente", reader["idCliente"].ToString() }
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                return dados;
            }

            finally
            {
                cmd.Connection.Close();
            }

            if (dados != null)
            {
                MySqlCommand cmd2 = new MySqlCommand
                {
                    CommandText = "UPDATE usuarioscliente SET ultimoAcesso = now(),usuarioLogado = 1 WHERE id = @id AND idCliente = @idCliente;",
                    CommandType = System.Data.CommandType.Text
                };

                try
                {
                    using (cmd2.Connection = CriarConexao())
                    {
                        cmd2.Connection.Open();
                        cmd2.Parameters.AddWithValue("@id", dados["IdUsuario"].ToString());
                        cmd2.Parameters.AddWithValue("@idCliente", dados["IdCliente"].ToString());

                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    return dados;
                }
                finally
                {
                    cmd2.Connection.Close();
                }
            }

            return dados;
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

        public bool ExisteCnpj(string cnpj)
        {
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT count(*) from clientes where cnpj = @cnpj;"
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@cnpj", cnpj);

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

        public int CadastrarNovoCliente(CadastroNovoCli model)
        {
            int id = 0;
            string sql = "";

            sql += "INSERT INTO clientes(nomeFantasia,razaoSocial,cnpj,nomeResponsavel,telefone,cep,logradouro,complemento,bairro,cidade,estado,dataCadastro,email)";
            sql += " VALUES(@nomeFantasia,@razaoSocial,@cnpj,@nomeResponsavel,@telefone,@cep,@logradouro,@complemento,@bairro,@cidade,@estado,now(),@email);";
            sql += "SELECT LAST_INSERT_ID();";

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
                    cmd.Parameters.AddWithValue("@nomeFantasia", model.NomeFantasia);
                    cmd.Parameters.AddWithValue("@razaoSocial", model.RazaoSocial);
                    cmd.Parameters.AddWithValue("@cnpj", model.Cnpj);
                    cmd.Parameters.AddWithValue("@nomeResponsavel", model.NomeResponsavel);
                    cmd.Parameters.AddWithValue("@telefone", model.Telefone);
                    cmd.Parameters.AddWithValue("@cep", model.Cep);
                    cmd.Parameters.AddWithValue("@logradouro", model.Logradouro);
                    cmd.Parameters.AddWithValue("@complemento", model.Complemento);
                    cmd.Parameters.AddWithValue("@bairro", model.Bairro);
                    cmd.Parameters.AddWithValue("@cidade", model.Cidade);
                    cmd.Parameters.AddWithValue("@estado", model.Estado);
                    cmd.Parameters.AddWithValue("@email", model.Email);

                    id = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception)
            {
                return 0;
            }

            finally
            {
                cmd.Connection.Close();
            }
            

            return id;
        }

        public bool CadastrarNovoUsuarioCliente(UsuarioCliente model)
        {
            var key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            CriptoGrafia cripto = new CriptoGrafia(key);
            var senhaCriptografada = cripto.Criptografar(model.Senha);

            string sql = "";

            sql += "INSERT INTO usuarioscliente (idCliente,email,senha)";
            sql += " values(@idCliente,@email,@senha);";

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

        public string RecuperarSerial(string email)
        {
            string serialRecovery = "";
            int id = 0;

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "select id FROM usuarioscliente WHERE email = @email;",
                CommandType = System.Data.CommandType.Text
            };

            #region Existe email
            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@email", email);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            id = (int)reader["id"];
                            serialRecovery = Guid.NewGuid().ToString("N").Substring(0, 24);
                        }
                        else
                        {
                            return serialRecovery;
                        }
                    }
                    else
                    {
                        return serialRecovery;
                    }
                }
            }
            catch (Exception)
            {
                return serialRecovery;
            }

            finally
            {
                cmd.Connection.Close();
            }

            #endregion

            #region Generate serial Recovery
            
            MySqlCommand cmd2 = new MySqlCommand
            {
                CommandText = "UPDATE usuarioscliente SET serialRecovery = @serialRecovery where id = @id;",
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd2.Connection = CriarConexao())
                {
                    cmd2.Connection.Open();
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.Parameters.AddWithValue("@serialRecovery", serialRecovery);

                    cmd2.ExecuteNonQuery();
                    return serialRecovery;
                }
            }
            catch (Exception)
            {
                return serialRecovery;
            }

            finally
            {
                cmd2.Connection.Close();
            }
            #endregion
        }

        public UsuarioCliente DadosRecuperacaoSenha(string serialRecovery)
        {
            UsuarioCliente dados = null;

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "SELECT id,idcliente FROM usuarioscliente WHERE serialRecovery = @serialRecovery;",
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd.Connection = CriarConexao())
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@serialRecovery", serialRecovery);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            dados = new UsuarioCliente
                            {
                                Id = (int)reader["id"],
                                IdCliente =(int)reader["idCliente"]
                            };

                            return dados;
                        }
                        else
                        {
                            return dados;
                        }
                    }
                    else
                    {
                        return dados;
                    }
                }
            }
            catch (Exception)
            {
                return dados;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public string AlterarSenha(UsuariosClienteModel model)
        {
            var key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            CriptoGrafia cripto = new CriptoGrafia(key);
            var senhaCriptografada = cripto.Criptografar(model.Senha);

            string sql = "";

            sql += "UPDATE usuarioscliente SET senha = @senha, serialRecovery = @serialRecovery" +
                   " WHERE id = @id AND idCliente = @idCliente;";

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
                    cmd.Parameters.AddWithValue("@id", model.Id);
                    cmd.Parameters.AddWithValue("@idCliente", model.IdCliente);
                    cmd.Parameters.AddWithValue("@senha", senhaCriptografada);
                    cmd.Parameters.AddWithValue("@serialRecovery", "");

                    cmd.ExecuteNonQuery();

                    return "Sua senha foi alterada com sucesso!";
                }
            }
            catch (Exception)
            {
                return "Não foi possível realizar a alteração da senha! Tente novamente mais tarde.";
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public Dictionary<string,string> CarregarDadosClienteLogado(UsuariosClienteModel model)
        {
            Dictionary<string, string> dados = null;
            string sql = "";
            sql += "SELECT u.email AS Usuario, c.Id AS CodigoCliente, c.RazaoSocial AS NomeCliente" +
                   " FROM clientes c INNER JOIN usuarioscliente u ON c.Id = u.IdCliente " +
                   " WHERE c.Id = @IdCliente AND u.Id = @Id;";

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
                    cmd.Parameters.AddWithValue("@IdCliente", model.IdCliente);
                    cmd.Parameters.AddWithValue("@Id", model.Id);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            dados = new Dictionary<string, string>();
                            dados.Add("Usuario", reader["Usuario"].ToString());
                            dados.Add("CodigoCliente", reader["CodigoCliente"].ToString());
                            dados.Add("NomeCliente", reader["NomeCliente"].ToString());

                            return dados;
                        }
                        else
                        {
                            return dados;
                        }
                    }
                    else
                    {
                        return dados;
                    }
                }
            }
            catch (Exception)
            {
                return dados;
            }

            finally
            {
                cmd.Connection.Close();
            }
        }

        public ClienteModel CarregarDadosConfiguracoesCliente(int id)
        {
            ClienteModel clienteModel = null;
            string sql = "";

            sql += "SELECT 	id, nomeFantasia,razaoSocial,cnpj,nomeResponsavel,telefone,cep, logradouro,complemento,bairro,cidade,estado,email";
            sql += " from clientes where id = @id;";

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
                    cmd.Parameters.AddWithValue("@id",id);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            clienteModel = new ClienteModel
                            {
                                Bairro = reader["bairro"].ToString(),
                                Cep = reader["cep"].ToString(),
                                Cidade = reader["cidade"].ToString(),
                                Cnpj = reader["cnpj"].ToString(),
                                Complemento = reader["complemento"].ToString(),
                                Email = reader["email"].ToString(),
                                Estado = reader["estado"].ToString(),
                                Id = (int)reader["id"],
                                Logradouro = reader["logradouro"].ToString(),
                                NomeFantasia = reader["nomeFantasia"].ToString(),
                                NomeResponsavel = reader["nomeResponsavel"].ToString(),
                                RazaoSocial = reader["razaoSocial"].ToString(),
                                Telefone = reader["telefone"].ToString()
                            };

                            return clienteModel;
                        }
                        else
                        {
                            return clienteModel;
                        }
                    }
                    else
                    {
                        return clienteModel;
                    }
                }
            }
            catch (Exception)
            {
                return clienteModel;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public string SalvarDadosConfiguracoesCliente(ClienteModel model)
        {
            string sql = "";

            sql += "UPDATE clientes SET nomeFantasia = @nomeFantasia, razaoSocial = @razaoSocial, cnpj = @cnpj, nomeResponsavel = @nomeResponsavel,";
            sql += "telefone = @telefone,cep = @cep,logradouro = @logradouro,complemento = @complemento,bairro = @bairro,cidade = @cidade,";
            sql += "estado = @estado, email = @email WHERE id = @id;";

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

                    cmd.Parameters.AddWithValue("@id", model.Id); 
                    cmd.Parameters.AddWithValue("@nomeFantasia", model.NomeFantasia);
                    cmd.Parameters.AddWithValue("@razaoSocial", model.RazaoSocial);
                    cmd.Parameters.AddWithValue("@cnpj", model.Cnpj);
                    cmd.Parameters.AddWithValue("@nomeResponsavel", model.NomeResponsavel);
                    cmd.Parameters.AddWithValue("@telefone", model.Telefone);
                    cmd.Parameters.AddWithValue("@cep", model.Cep);
                    cmd.Parameters.AddWithValue("@logradouro", model.Logradouro);
                    cmd.Parameters.AddWithValue("@complemento", model.Complemento);
                    cmd.Parameters.AddWithValue("@bairro", model.Bairro);
                    cmd.Parameters.AddWithValue("@cidade", model.Cidade);
                    cmd.Parameters.AddWithValue("@estado", model.Estado);
                    cmd.Parameters.AddWithValue("@email", model.Email);

                    cmd.ExecuteNonQuery();

                    return "Dados salvos com sucesso!";
                }
            }
            catch (Exception)
            {
                return "Não foi possível salvar os dados! Tente novamente mais tarde.";
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public void Logoff(UsuariosClienteModel model)
        {
            MySqlCommand cmd2 = new MySqlCommand
            {
                CommandText = "UPDATE usuarioscliente SET usuarioLogado = 0 WHERE id = @id AND idCliente = @idCliente;",
                CommandType = System.Data.CommandType.Text
            };

            try
            {
                using (cmd2.Connection = CriarConexao())
                {
                    cmd2.Connection.Open();
                    cmd2.Parameters.AddWithValue("@id",model.Id);
                    cmd2.Parameters.AddWithValue("@idCliente", model.IdCliente);

                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd2.Connection.Close();
            }
        }
    }
}
