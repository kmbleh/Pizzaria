using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Correios;

namespace Pizzaria
{
    public partial class FrmCliente : Form
    {
        string modo = "";
        bool flag = true;
        public FrmCliente()
        {
            InitializeComponent();
        }

        public FrmCliente(string usuarioLogado)
        {
            InitializeComponent();
            lblUsuario.Text = usuarioLogado;
        }

        public void CarregarGrid()
        {
            AcessoDados acessoDados = new AcessoDados();
            dgvClientes.DataSource = acessoDados.CarregarClientes();
            dgvClientes.Refresh();
        }

        private void LimparCampos()
        {
            txtCodigo.Clear();
            txtNome.Clear();
            txtTelefone.Clear();
            txtCEP.Clear();
            txtRua.Clear();
            txtNum.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            txtUF.Clear();
            txtNome.Focus();
        }

        private void BloquearCampos()
        {
            txtCodigo.ReadOnly = true;
            txtTelefone.ReadOnly = true;
            txtCEP.ReadOnly = true;
            txtRua.ReadOnly = true;
            txtNum.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtCidade.ReadOnly = true;
            txtUF.ReadOnly = true;
        }

        private void DesbloquearCampos()
        {
            txtTelefone.ReadOnly = false;
            txtCEP.ReadOnly = false;
            txtTelefone.ReadOnly = false;
            txtCEP.ReadOnly = false;
            txtRua.ReadOnly = false;
            txtNum.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtCidade.ReadOnly = false;
            txtUF.ReadOnly = false;
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            BloquearCampos();
            CarregarGrid();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            modo = "inserir";
            btnExcluir.Enabled = false;
            btnAlterar.Enabled = false;
            txtCodigo.ReadOnly = false;
            DesbloquearCampos();
            MessageBox.Show("Preencha os campos e clique no botão GRAVAR para efetivar o cadastro.", "Cadastro",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            modo = "alterar";
            btnNovo.Enabled = false;
            btnExcluir.Enabled = false;
            DesbloquearCampos();
            MessageBox.Show("Altere as informações e clique no botão GRAVAR para efetivar a operação.", "Alteração",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            modo = "excluir";
            btnNovo.Enabled = false;
            btnAlterar.Enabled = false;
            MessageBox.Show("Para efetivar a exclusão clique em GRAVAR.", "Confirmação de exclusão",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            modo = "";
            btnAlterar.Enabled = true;
            btnNovo.Enabled = true;
            btnExcluir.Enabled = true;
            BloquearCampos();
            LimparCampos();
        }

        private void btnPedido_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Você será redirecionado para a tela de pedidos.", "Redirecionamento pedidos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            int codigo = Convert.ToInt32(txtCodigo.Text);
            FrmVendas frmVendas = new FrmVendas(codigo, lblUsuario.Text, DateTime.Now);
            frmVendas.Show();
            this.Hide();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            bool result = false;
            string nome = txtNome.Text;
            AcessoDados acessoDados = new AcessoDados();
            result = acessoDados.VerificarExistenciaClientes(nome);

            if(result == true)
            {
                dgvClientes.DataSource = acessoDados.PesquisarCliente(nome);
            }
            else
            {
                MessageBox.Show("Não foi possível encontrar cliente com o nome fornecido.", "Pesquisa de cliente",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente();
            AcessoDados acessoDados = new AcessoDados();
            if (modo == "inserir")
            {
                cliente.Codigo = Convert.ToInt32(txtCodigo.Text);
                cliente.Nome = txtNome.Text;
                cliente.Telefone = txtTelefone.Text;
                cliente.Cep = txtCEP.Text;
                cliente.Rua = txtRua.Text;
                cliente.Numero = Convert.ToInt32(txtNum.Text);
                cliente.Bairro = txtBairro.Text;
                cliente.Cidade = txtCidade.Text;
                cliente.Uf = txtUF.Text;

                bool result = acessoDados.InserirCliente(cliente);

                if (result == true)
                {
                    MessageBox.Show("Cliente adicionado com sucesso!", "Inserção de cliente",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BloquearCampos();
                    LimparCampos();
                    btnAlterar.Enabled = true;
                    btnExcluir.Enabled = true;
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show("Não foi possível adicionar cliente.", "Erro de inserção",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if(modo == "alterar")
            {
                cliente.Codigo = Convert.ToInt32(txtCodigo.Text);
                cliente.Nome = txtNome.Text;
                cliente.Telefone = txtTelefone.Text;
                cliente.Cep = txtCEP.Text;
                cliente.Rua = txtRua.Text;
                cliente.Numero = Convert.ToInt32(txtNum.Text);
                cliente.Bairro = txtBairro.Text;
                cliente.Cidade = txtCidade.Text;
                cliente.Uf = txtUF.Text;

                bool result = acessoDados.AlterarCliente(cliente);

                if(result == true)
                {
                    MessageBox.Show("Dados de cliente alterado com sucesso!", "Alteração de dados de cliente",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BloquearCampos();
                    LimparCampos();
                    btnNovo.Enabled = true;
                    btnExcluir.Enabled = true;

                    string nome = txtNome.Text;
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show("Não foi possível alterar os dados.", "Erro de alteração de dados de cliente",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if(modo == "excluir")
            {
                int codigo = Convert.ToInt32(txtCodigo.Text);
                bool result = acessoDados.ExcluirCliente(codigo);
                
                if(result == true)
                {
                    MessageBox.Show("Cliente excluído com sucesso!", "Exclusão de cliente",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BloquearCampos();
                    LimparCampos();
                    btnNovo.Enabled = true;
                    btnAlterar.Enabled = true;
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir cliente.", "Erro de exclusão de cliente",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int linhaAtual;
            linhaAtual = dgvClientes.CurrentRow.Index;
            txtCodigo.Text = dgvClientes[0, linhaAtual].Value.ToString();
            txtNome.Text = dgvClientes[1, linhaAtual].Value.ToString();
            txtTelefone.Text = dgvClientes[2, linhaAtual].Value.ToString();
            txtCEP.Text = dgvClientes[3, linhaAtual].Value.ToString();
            txtRua.Text = dgvClientes[4, linhaAtual].Value.ToString();
            txtNum.Text = dgvClientes[5, linhaAtual].Value.ToString();
            txtBairro.Text = dgvClientes[6, linhaAtual].Value.ToString();
            txtCidade.Text = dgvClientes[7, linhaAtual].Value.ToString();
            txtUF.Text = dgvClientes[8, linhaAtual].Value.ToString();
        }

        private void LocalizarCEP()
        {
            if(!string.IsNullOrWhiteSpace(txtCEP.Text))
            {
                CorreiosApi correiosApi = new CorreiosApi();
                var endereco = correiosApi.consultaCEP(txtCEP.Text);

                if(endereco.cep != null)
                {
                    txtRua.Text = endereco.end;
                    txtBairro.Text = endereco.bairro;
                    txtCidade.Text = endereco.cidade;
                    txtUF.Text = endereco.uf;
                }
                else
                {
                    MessageBox.Show("CEP não localizado.", "Erro de localização",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Informe um CEP válido.", "Entrada inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCEP_Leave(object sender, EventArgs e)
        {
            LocalizarCEP();
        }
    }
}
