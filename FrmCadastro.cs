using Pizzaria.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pizzaria
{
    public partial class FrmCadastro : Form
    {
        string modo = "";
        public FrmCadastro()
        {
            InitializeComponent();
        }

        public FrmCadastro(string usuarioLogado)
        {
            InitializeComponent();
            lblUsuario.Text = usuarioLogado;
        }

        public void CarregarGrid()
        {
            AcessoDados acessoDados = new AcessoDados();
            dgvProdutos.DataSource = acessoDados.DgvProdutos();
            dgvProdutos.Refresh();
        }

        private void LimparCampos()
        {
            txtCodigo.Clear();
            txtProduto.Clear();
            txtPreco.Clear();
            txtImagem.Clear();
            cmbTipo.Text = "";
            cmbTamanho.Text = "";
            chkVegetariano.Checked = false;
            picProduto.Image = Resources.PizzaCad;
            txtCodigo.Focus();
        }

        private void BloquearCampos()
        {
            txtProduto.ReadOnly = true;
            txtPreco.ReadOnly = true;
            txtImagem.ReadOnly = true;
            cmbTipo.Enabled = false;
            cmbTamanho.Enabled = false;
            chkVegetariano.Enabled = false;
            btnPesqImagem.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtCodigo.ReadOnly = false;
            txtProduto.ReadOnly = false;
            txtPreco.ReadOnly = false;
            cmbTipo.Enabled = true;
            cmbTamanho.Enabled = true;
            chkVegetariano.Enabled = true;
            btnPesqImagem.Enabled = true;
        }

        private void FrmCadastro_Load(object sender, EventArgs e)
        {
            BloquearCampos();
            CarregarGrid();

            AcessoDados acessoDados = new AcessoDados();
            List<Produto> tipos = acessoDados.CarregarTipo();

            foreach(var item in tipos)
            {
                cmbTipo.ValueMember = item.IdTipo.ToString();
                cmbTipo.Items.Add(item.Tipo);
            }

            List<Produto> tamanhos = acessoDados.CarregarTamanho();

            foreach(var item in tamanhos)
            {
                cmbTamanho.ValueMember = item.IdTam.ToString();
                cmbTamanho.Items.Add(item.Tamanho);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            modo = "inserir";
            btnExcluir.Enabled = false;
            btnAlterar.Enabled = false;
            MessageBox.Show("Preencha os campos e clique no botão GRAVAR para efetivar o cadastro.", "Cadastro",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DesbloquearCampos();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            modo = "alterar";
            txtCodigo.ReadOnly = true;
            btnNovo.Enabled = false;
            btnExcluir.Enabled = false;
            DesbloquearCampos();
            MessageBox.Show("Altere as informações e clique em GRAVAR para efetivar a operação.", "Alteração",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            modo = "excluir";
            btnAlterar.Enabled = false;
            btnNovo.Enabled = false;

            MessageBox.Show("Para efetivar a exclusão clique em GRAVAR.",
                "Confirmação de exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPesqCodigo_Click(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(txtCodigo.Text);
            AcessoDados acessoDados = new AcessoDados();

            DataTable dt = acessoDados.PesquisarProdutoPorCod(codigo);
            if(dt.Rows.Count >= 1)
            {
                foreach(DataRow row in dt.Rows)
                {
                    txtCodigo.Text = row["Codigo"].ToString();
                    txtProduto.Text = row["Nome"].ToString();
                    txtPreco.Text = row["Preco"].ToString();
                    cmbTipo.Text = row["Descricao"].ToString();
                    cmbTamanho.Text = row["Tam"].ToString();

                    if(row["Veg"].ToString() == "true")
                    {
                        chkVegetariano.Checked = true;
                    }
                    else
                    {
                        chkVegetariano.Checked = false;
                    }

                    int cod = Convert.ToInt32(txtCodigo.Text);

                    picProduto.Image = new Bitmap(acessoDados.CarregarFotoProduto(cod));
                }
            }
            else
            {
                MessageBox.Show("Não foi possível encontrar produto pelo código fornecido.",
                    "Pesquisa de produto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

        private void btnVendas_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Você será redirecionado para a tela de clientes.", "Redirecionamento vendas",
                MessageBoxButtons.OK);
            FrmCliente frmCliente = new FrmCliente(lblUsuario.Text);
            frmCliente.Show();
            this.Hide();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Produto produto = new Produto();
            AcessoDados acessoDados = new AcessoDados();
            if (modo == "inserir")
            {
                produto.Codigo = Convert.ToInt32(txtCodigo.Text);
                produto.IdTipo = Convert.ToInt32(cmbTipo.ValueMember);
                produto.IdTam = Convert.ToInt32(cmbTamanho.ValueMember);
                produto.Nome = txtProduto.Text;
                produto.Preco = Convert.ToDecimal(txtPreco.Text);
                produto.Imagem = txtImagem.Text;
                if (chkVegetariano.Checked == true)
                {
                    produto.Vegetariano = true;
                }
                else
                {
                    chkVegetariano.Checked = false;
                }

                bool result = acessoDados.InserirProduto(produto);

                if (result == true)
                {
                    MessageBox.Show("Produto adicionado com sucesso!", "Produto adicionado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarGrid();
                    LimparCampos();
                    BloquearCampos();
                    btnAlterar.Enabled = true;
                    btnExcluir.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível adicionar o produto.", "Erro de cadastro de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (modo == "alterar")
            {
                produto.Codigo = Convert.ToInt32(txtCodigo.Text);
                produto.IdTipo = Convert.ToInt32(cmbTipo.ValueMember);
                produto.IdTam = Convert.ToInt32(cmbTamanho.ValueMember);
                produto.Nome = txtProduto.Text;
                produto.Preco = Convert.ToDecimal(txtPreco.Text);
                produto.Imagem = txtImagem.Text;
                if (chkVegetariano.Checked == true)
                {
                    produto.Vegetariano = true;
                }
                else
                {
                    produto.Vegetariano = false;
                }

                bool result = acessoDados.AlterarProduto(produto);

                if (result == true)
                {
                    MessageBox.Show("Produto alterado com sucesso!", "Alteração de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarGrid();
                    LimparCampos();
                    BloquearCampos();
                    btnNovo.Enabled = true;
                    btnExcluir.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível alterar o produto.", "Erro de alteração de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (modo == "excluir")
            {
                int codigo = Convert.ToInt32(txtCodigo.Text);
                bool result = acessoDados.ExcluirProduto(codigo);

                if (result == true)
                {
                    MessageBox.Show("Produto excluído com sucesso!", "Exclusão de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarGrid();
                    BloquearCampos();
                    LimparCampos();
                    btnNovo.Enabled = true;
                    btnAlterar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir o produto.", "Erro de exclusão de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPesqImagem_Click(object sender, EventArgs e)
        {
            string strFilePath = "";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\USER\source\repos\Pizzaria\Resources";
                openFileDialog.Filter = "Images(*.jpg,*.png)|*.jpg;*.jpg";

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    strFilePath = openFileDialog.FileName;
                    txtImagem.Text = strFilePath.ToString();
                    picProduto.Image = new Bitmap(strFilePath);
                }
            }
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            List<Produto> tipos = new List<Produto>();
            tipos = acessoDados.CarregarTipo();

            foreach (var item in tipos)
            {
                if(cmbTipo.SelectedItem.ToString() == item.Tipo)
                {
                    cmbTipo.ValueMember = item.IdTipo.ToString();
                    break;
                }
            }
        }

        private void cmbTamanho_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            List<Produto> tamanhos = new List<Produto>();
            tamanhos = acessoDados.CarregarTamanho();

            foreach (var item in tamanhos)
            {
                if(cmbTamanho.SelectedItem.ToString() == item.Tamanho)
                {
                    cmbTamanho.ValueMember = item.IdTam.ToString();
                    break;
                }
            }
        }
    }
}
