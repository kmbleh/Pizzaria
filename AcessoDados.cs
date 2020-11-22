using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pizzaria
{
    class AcessoDados
    {
        SqlConnection conn = new SqlConnection();

        //Validação de usuário para login
        public bool ValidarUsuario(string nome, string senha)
        {
            bool retorno = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Usuario WHERE nome = @nome AND " +
                    "senha = @senha";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = nome;
                cmdo.Parameters.Add("@senha", SqlDbType.VarChar, 10).Value = senha;

                SqlDataReader dataReader = cmdo.ExecuteReader();
                if (dataReader.HasRows)
                {
                    retorno = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return retorno;
        }


        //Verificação existência de foto de usuário
        public bool VerificarExistencia(string nome)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            cmdo.Connection = conn;
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Foto FROM Usuario WHERE nome = @nome";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = nome;

                SqlDataAdapter da = new SqlDataAdapter(cmdo);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        //Carregar foto de usuário no login
        public MemoryStream CarregarFoto(string nome)
        {
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            MemoryStream ms = new MemoryStream();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Foto FROM Usuario WHERE nome = @nome";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = nome;

                SqlDataAdapter da = new SqlDataAdapter(cmdo);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Foto"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return ms;
        }

        //Carregar tipos de produto
        public List<Produto> CarregarTipo()
        {
            List<Produto> tipos = new List<Produto>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM TipoProduto";
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    tipos.Add(new Produto()
                    {
                        IdTipo = Convert.ToInt32(dataReader["Id"].ToString()),
                        Tipo = dataReader["Descricao"].ToString()
                    }
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return tipos;
        }

        //Carregar opções de tamanho dos produtos
        public List<Produto> CarregarTamanho()
        {
            List<Produto> tamanhos = new List<Produto>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Tamanho";
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    tamanhos.Add(new Produto()
                    {
                        IdTam = Convert.ToInt32(dataReader["Id"].ToString()),
                        Tamanho = dataReader["Tam"].ToString()
                    }
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return tamanhos;
        }

        //Carregar produtos para o datagridview
        public DataTable DgvProdutos()
        {
            //criar lista de Produtos
            List<Produto> produtos = new List<Produto>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT P.Codigo, P.Nome, T.Descricao, TM.Tam, P.Veg, P.Preco FROM " +
                    "Produto P INNER JOIN TipoProduto T ON P.IdTipo = T.Id INNER JOIN Tamanho TM ON P.IdTamanho = TM.Id";

                SqlDataReader dataReader = cmdo.ExecuteReader();
                dt.Load(dataReader);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        //Pesquisar produto por código
        public DataTable PesquisarProdutoPorCod(int codigo)
        {
            DataTable dataTable = new DataTable();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            SqlDataReader sqlDataReader = null;
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT P.Codigo, P.Veg, P.Nome, P.Preco, T.Descricao, TAM.Tam " +
                    "FROM Produto P INNER JOIN TipoProduto T ON P.IdTipo = T.Id INNER JOIN Tamanho TAM ON P.IdTamanho = TAM.Id" +
                    " WHERE P.Codigo = @codigo";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = codigo;
                sqlDataReader = cmdo.ExecuteReader();
                dataTable.Load(sqlDataReader);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return dataTable;
        }

        //Carrega foto do produto pesquisado
        public MemoryStream CarregarFotoProduto(int codigo)
        {
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            MemoryStream ms = new MemoryStream();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Foto FROM Produto WHERE Codigo = @codigo";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = codigo;

                SqlDataAdapter da = new SqlDataAdapter(cmdo);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Foto"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return ms;
        }

        //Inserção de produto
        public bool InserirProduto(Produto produto)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "INSERT INTO Produto " +
                    "SELECT @codigo, @codTipo, @codTam, @vegetariano, @nome, @preco, @imagem";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = produto.Codigo;
                cmdo.Parameters.Add("@codTipo", SqlDbType.Int, 18).Value = produto.IdTipo;
                cmdo.Parameters.Add("@codTam", SqlDbType.Int, 18).Value = produto.IdTam;
                cmdo.Parameters.Add("@vegetariano", SqlDbType.Bit).Value = produto.Vegetariano;
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 50).Value = produto.Nome;
                cmdo.Parameters.Add("@preco", SqlDbType.Decimal, 18).Value = produto.Preco;

                byte[] img = File.ReadAllBytes(produto.Imagem);

                cmdo.Parameters.AddWithValue("@imagem", img);

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Alteração de produto
        public bool AlterarProduto(Produto produto)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            try
            {
                using (SqlCommand cmdo = new SqlCommand())
                {
                    conn.Open();
                    cmdo.Connection = conn;
                    cmdo.CommandType = CommandType.Text;
                    cmdo.CommandText = "UPDATE Produto SET IdTipo = @codTipo, IdTamanho = @codTamanho, " +
                        "Veg = @vegetariano, Nome = @nome, Preco = @preco, Foto = @imagem " +
                        " WHERE Codigo = @codigo";
                    cmdo.Parameters.Add("@codigo", SqlDbType.VarChar, 60).Value = produto.Codigo;
                    cmdo.Parameters.Add("@codTipo", SqlDbType.Int, 18).Value = produto.IdTipo;
                    cmdo.Parameters.Add("@codTamanho", SqlDbType.Int, 18).Value = produto.IdTam;
                    cmdo.Parameters.Add("@vegetariano", SqlDbType.Bit).Value = produto.Vegetariano;
                    cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 50).Value = produto.Nome;
                    cmdo.Parameters.Add("@preco", SqlDbType.Decimal, 18).Value = produto.Preco;

                    byte[] img = File.ReadAllBytes(produto.Imagem);
                    cmdo.Parameters.AddWithValue("@imagem", img);

                    int linhasAfetadas = cmdo.ExecuteNonQuery();
                    if (linhasAfetadas == 1)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Exclusão de produto
        public bool ExcluirProduto(int codigo)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "DELETE FROM Produto WHERE Codigo = @codigo";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = codigo;

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Carregar clientes para o datagridview
        public List<Cliente> CarregarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Cliente";
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        Codigo = Convert.ToInt32(dataReader["Id"].ToString()),
                        Nome = dataReader["Nome"].ToString(),
                        Telefone = dataReader["Telefone"].ToString(),
                        Cep = dataReader["CEP"].ToString(),
                        Rua = dataReader["Rua"].ToString(),
                        Numero = Convert.ToInt32(dataReader["Numero"].ToString()),
                        Bairro = dataReader["Bairro"].ToString(),
                        Cidade = dataReader["Cidade"].ToString(),
                        Uf = dataReader["Estado"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return clientes;
        }

        // Verifica existência de cliente na pesquisa por nome
        public bool VerificarExistenciaClientes(string nome)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            cmdo.Connection = conn;
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Cliente WHERE Nome = @nome";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = nome;

                SqlDataAdapter da = new SqlDataAdapter(cmdo);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Pesquisa de cliente por nome
        public List<Cliente> PesquisarCliente(string nome)
        {
            List<Cliente> clientes = new List<Cliente>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Cliente WHERE Nome = @nome";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 100).Value = nome;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        Codigo = Convert.ToInt32(dataReader["Id"].ToString()),
                        Nome = dataReader["Nome"].ToString(),
                        Telefone = dataReader["Telefone"].ToString(),
                        Cep = dataReader["CEP"].ToString(),
                        Rua = dataReader["Rua"].ToString(),
                        Numero = Convert.ToInt32(dataReader["Numero"].ToString()),
                        Bairro = dataReader["Bairro"].ToString(),
                        Cidade = dataReader["Cidade"].ToString(),
                        Uf = dataReader["Estado"].ToString()
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return clientes;
        }

        //Inserção de clientes
        public bool InserirCliente(Cliente cliente)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "INSERT INTO Cliente VALUES (@codigo, @nome, @telefone, @cep, @rua, @numero," +
                    "@bairro, @cidade, @estado)";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = cliente.Codigo;
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 100).Value = cliente.Nome;
                cmdo.Parameters.Add("@telefone", SqlDbType.VarChar, 20).Value = cliente.Telefone;
                cmdo.Parameters.Add("@cep", SqlDbType.VarChar, 20).Value = cliente.Cep;
                cmdo.Parameters.Add("@rua", SqlDbType.VarChar, 50).Value = cliente.Rua;
                cmdo.Parameters.Add("@numero", SqlDbType.Int, 18).Value = cliente.Numero;
                cmdo.Parameters.Add("@bairro", SqlDbType.VarChar, 50).Value = cliente.Bairro;
                cmdo.Parameters.Add("@cidade", SqlDbType.VarChar, 50).Value = cliente.Cidade;
                cmdo.Parameters.Add("@estado", SqlDbType.VarChar, 50).Value = cliente.Uf;

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Alteração de cliente
        public bool AlterarCliente(Cliente cliente)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            try
            {
                using (SqlCommand cmdo = new SqlCommand())
                {
                    conn.Open();
                    cmdo.Connection = conn;
                    cmdo.CommandType = CommandType.Text;
                    cmdo.CommandText = "UPDATE Cliente SET Nome = @nome, Telefone = @telefone," +
                        " CEP = @cep, Rua = @rua, Numero = @numero, Bairro = @bairro, Cidade = @cidade," +
                        " Estado = @estado WHERE Id = @codigo";
                    cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = cliente.Codigo;
                    cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 100).Value = cliente.Nome;
                    cmdo.Parameters.Add("@telefone", SqlDbType.VarChar, 20).Value = cliente.Telefone;
                    cmdo.Parameters.Add("@cep", SqlDbType.VarChar, 20).Value = cliente.Cep;
                    cmdo.Parameters.Add("@rua", SqlDbType.VarChar, 50).Value = cliente.Rua;
                    cmdo.Parameters.Add("@numero", SqlDbType.Int, 18).Value = cliente.Numero;
                    cmdo.Parameters.Add("@bairro", SqlDbType.VarChar, 50).Value = cliente.Bairro;
                    cmdo.Parameters.Add("@cidade", SqlDbType.VarChar, 50).Value = cliente.Cidade;
                    cmdo.Parameters.Add("@estado", SqlDbType.VarChar, 50).Value = cliente.Uf;

                    int linhasAfetadas = cmdo.ExecuteNonQuery();
                    if (linhasAfetadas == 1)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Exclusão de cliente
        public bool ExcluirCliente(int codigo)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "DELETE FROM Cliente WHERE Id = @codigo";
                cmdo.Parameters.Add("@codigo", SqlDbType.Int, 18).Value = codigo;

                int linhasAfetadas = cmdo.ExecuteNonQuery();

                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Criação de pedido
        public bool CriarPedido(Vendas vendas)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "INSERT INTO Vendas (IdCliente, Data, IdVendedor) " +
                    "SELECT @codigoCliente, @data, (SELECT Id FROM Usuario WHERE Nome = @nome)";
                cmdo.Parameters.Add("@codigoCliente", SqlDbType.Int, 18).Value = vendas.CodCliente;
                cmdo.Parameters.Add("@data", SqlDbType.DateTime).Value = vendas.Hora;
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = vendas.Vendedor;

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Carregar Id de Pedido
        public Vendas CarregarIdPedido(string nome, int codCliente, DateTime hora)
        {
            Vendas vendas = new Vendas();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT V.IdPedido FROM Vendas V INNER JOIN Usuario U ON U.Id = V.IdVendedor " +
                    "WHERE V.IdCliente = @codCliente AND V.Data = @hora " +
                    "AND V.IdVendedor = U.Id";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 60).Value = nome;
                cmdo.Parameters.Add("@codCliente", SqlDbType.Int, 18).Value = codCliente;
                cmdo.Parameters.Add("@hora", SqlDbType.DateTime).Value = hora;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    vendas.CodVendas = Convert.ToInt32(dataReader["IdPedido"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return vendas;
        }

        //Inserção de produto no pedido
        public bool AdicionarProduto(Vendas vendas)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "INSERT INTO PedidoItem VALUES (@codVendas, @codProduto, @quant, @preco)";
                cmdo.Parameters.Add("@codVendas", SqlDbType.Int, 18).Value = vendas.CodVendas;
                cmdo.Parameters.Add("@codProduto", SqlDbType.Int, 18).Value = vendas.CodProd;
                cmdo.Parameters.Add("@quant", SqlDbType.Int, 18).Value = vendas.Quantidade;
                cmdo.Parameters.Add("@preco", SqlDbType.Decimal, 18).Value = vendas.Subtotal;

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Opções disponíveis na combobox com base na seleção de tipo de produto
        public List<Vendas> SelecionarProduto(int codTipo)
        {
            List<Vendas> vendas = new List<Vendas>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Nome FROM Produto" +
                    " WHERE IdTipo = @codTipo GROUP BY Nome";
                cmdo.Parameters.Add("@codTipo", SqlDbType.Int, 18).Value = codTipo;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    vendas.Add(new Vendas()
                    {
                        Produto = dataReader["Nome"].ToString()
                    }
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return vendas;
        }

        //Checa se o produto escolhido é vegetariano para marcar a checkbox
        public Vendas ChecarVeg(string nome)
        {
            Vendas vendas = new Vendas();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Veg FROM Produto WHERE Nome = @nome GROUP BY Nome, Veg";
                cmdo.Parameters.Add("@nome", SqlDbType.VarChar, 50).Value = nome;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    vendas.Vegetariano = Convert.ToBoolean(dataReader["Veg"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return vendas;
        }

        //Lista tamanhos combobox vendas
        public List<Vendas> CmbTamanho(int codTipo)
        {
            List<Vendas> tamanhos = new List<Vendas>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM Tamanho WHERE Id IN (SELECT IdTamanho FROM Produto" +
                    " WHERE IdTipo = @IdTipo)";
                cmdo.Parameters.Add("@IdTipo", SqlDbType.Int, 18).Value = codTipo;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    tamanhos.Add(new Vendas()
                    {
                        CodTam = Convert.ToInt32(dataReader["Id"].ToString()),
                        Tamanho = dataReader["Tam"].ToString()
                    }
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return tamanhos;
        }

        //Carregar Tipos para a combobox
        public List<Vendas> CmbTipo()
        {
            List<Vendas> tipos = new List<Vendas>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT * FROM TipoProduto";
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    tipos.Add(new Vendas()
                    {
                        CodTipo = Convert.ToInt32(dataReader["Id"].ToString()),
                        Tipo = dataReader["Descricao"].ToString()
                    }
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return tipos;
        }

        public List<Vendas> CarregarProdutos()
        {
            List<Vendas> produtos = new List<Vendas>();
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT P.Codigo, P.IdTipo, P.IdTamanho, T.Descricao, TM.Tam, P.Veg, P.Nome, P.Preco FROM " +
                    "Produto P INNER JOIN TipoProduto T ON P.IdTipo = T.Id INNER JOIN Tamanho TM ON P.IdTamanho = TM.Id";
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    produtos.Add(new Vendas()
                    {
                        CodProd = Convert.ToInt32(dataReader["Codigo"].ToString()),
                        CodTipo = Convert.ToInt32(dataReader["IdTipo"].ToString()),
                        CodTam = Convert.ToInt32(dataReader["IdTamanho"].ToString()),
                        Tipo = dataReader["Descricao"].ToString(),
                        Tamanho = dataReader["Tam"].ToString(),
                        Vegetariano = Convert.ToBoolean(dataReader["Veg"].ToString()),
                        Produto = dataReader["Nome"].ToString(),
                        Preco = Convert.ToDecimal(dataReader["Preco"].ToString())
                    }); ; ;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return produtos;
        }

        public DataTable CarregarPedido(int codVendas)
        {
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            DataTable dataTable = new DataTable();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT TP.Descricao, PI.IdProduto, P.Nome, T.Tam, PI.Quantidade, PI.Preco FROM Produto P " +
                    "INNER JOIN Tamanho T ON P.IdTamanho = T.Id INNER JOIN PedidoItem PI ON " +
                    "PI.IdProduto = P.Codigo INNER JOIN TipoProduto TP ON P.IdTipo = TP.Id WHERE PI.IdVendas = @codVendas";
                cmdo.Parameters.Add("@codVendas", SqlDbType.Int, 18).Value = codVendas;

                SqlDataReader dataReader = cmdo.ExecuteReader();
                dataTable.Load(dataReader);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return dataTable;
        }

        //Excluir produto do pedido
        public bool ExcluirProdutoPedido(int id)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "DELETE FROM PedidoItem WHERE Id = @Id";
                cmdo.Parameters.Add("@Id", SqlDbType.Int, 18).Value = id;

                int linhasAfetadas = cmdo.ExecuteNonQuery();

                if (linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int IdPedidoItem(int codVendas, int codProd, int quantidade)
        {
            int id = 0;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT Id FROM PedidoItem WHERE IdVendas = @codVendas AND IdProduto = @codProd AND " +
                    "Quantidade = @qtde";
                cmdo.Parameters.Add("@codVendas", SqlDbType.Int, 18).Value = codVendas;
                cmdo.Parameters.Add("@codProd", SqlDbType.Int, 18).Value = codProd;
                cmdo.Parameters.Add("@qtde", SqlDbType.Int, 18).Value = quantidade;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    id = Convert.ToInt32(dataReader["Id"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return id;
        }

        public decimal ValorTotal(int codVendas)
        {
            decimal total = 0; 
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "SELECT SUM(Preco) AS [Preco] FROM PedidoItem WHERE IdVendas = @codVendas";
                cmdo.Parameters.Add("@codVendas", SqlDbType.Int, 18).Value = codVendas;
                SqlDataReader dataReader = cmdo.ExecuteReader();
                while (dataReader.Read())
                {
                    total = Convert.ToDecimal(dataReader["Preco"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return total;
        }

        public bool AlterarPedidoItem(Vendas vendas)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            try
            {
                using (SqlCommand cmdo = new SqlCommand())
                {
                    conn.Open();
                    cmdo.Connection = conn;
                    cmdo.CommandType = CommandType.Text;
                    cmdo.CommandText = "UPDATE PedidoItem SET IdProduto = @codProd, Quantidade = @quant, Preco = @preco WHERE Id = @id";
                    cmdo.Parameters.Add("@id", SqlDbType.VarChar, 60).Value = vendas.Id;
                    cmdo.Parameters.Add("@codProd", SqlDbType.Int, 18).Value = vendas.CodProd;
                    cmdo.Parameters.Add("@quant", SqlDbType.Int, 18).Value = vendas.Quantidade;
                    cmdo.Parameters.Add("@preco", SqlDbType.Decimal, 18).Value = vendas.Preco;

                    int linhasAfetadas = cmdo.ExecuteNonQuery();
                    if (linhasAfetadas == 1)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool FecharVenda(int codVendas, decimal total)
        {
            bool result = false;
            conn.ConnectionString = Properties.Settings.Default.conexao;
            SqlCommand cmdo = new SqlCommand();
            try
            {
                conn.Open();
                cmdo.Connection = conn;
                cmdo.CommandType = CommandType.Text;
                cmdo.CommandText = "UPDATE Vendas SET Total = @total WHERE IdPedido = @codVendas";
                cmdo.Parameters.Add("@codVendas", SqlDbType.Int, 18).Value = codVendas;
                cmdo.Parameters.Add("@total", SqlDbType.Decimal, 18).Value = total;

                int linhasAfetadas = cmdo.ExecuteNonQuery();
                if(linhasAfetadas == 1)
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
