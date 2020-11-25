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
    public partial class FrmVendas : Form
    {
        string modo = "";
        public FrmVendas()
        {
            InitializeComponent();
        }

        public FrmVendas(int codigoCliente, string usuarioLogado, DateTime dataAtual)
        {
            InitializeComponent();
            lblUsuario.Text = usuarioLogado;
            lblHora.Text = dataAtual.ToString();
            txtCodCliente.Text = codigoCliente.ToString();
        }

        public void CarregarGrid()
        {
            AcessoDados acessoDados = new AcessoDados();
            int codVendas = Convert.ToInt32(txtCodVenda.Text);
            dgvVendas.DataSource = acessoDados.CarregarPedido(codVendas);
            dgvVendas.Refresh();
        }

        private void BloquearCampos()
        {
            cmbTipo.Enabled = false;
            cmbNome.Enabled = false;
            cmbTamanho.Enabled = false;
            txtQtde.ReadOnly = true;
        }
        private void DesbloquearCampos()
        {
            cmbTipo.Enabled = true;
            cmbNome.Enabled = true;
            cmbTamanho.Enabled = true;
            txtQtde.ReadOnly = false;
        }

        private void LimparCampos()
        {
            cmbTipo.Text = "";
            cmbNome.Items.Clear();
            cmbNome.Text = "";
            cmbTamanho.Items.Clear();
            cmbTamanho.Text = "";
            chkVeg.Checked = false;
            txtQtde.Clear();
            txtSub.Clear();
            lblCodPI.Text = "";

            picVendas.Image = Resources.mercado;
        }

        private void FrmVendas_Load(object sender, EventArgs e)
        {
            BloquearCampos();
            Vendas vendas = new Vendas();
            AcessoDados acessoDados = new AcessoDados();

            vendas.CodCliente = Convert.ToInt32(txtCodCliente.Text);
            vendas.Hora = Convert.ToDateTime(lblHora.Text);
            vendas.Vendedor = lblUsuario.Text;

            List<Vendas> tipos = acessoDados.CmbTipo();
            foreach (var item in tipos)
            {
                cmbTipo.ValueMember = item.CodTipo.ToString();
                cmbTipo.Items.Add(item.Tipo);
            }

            bool result = acessoDados.CriarPedido(vendas);

            if (result == true)
            {
                int codigo = Convert.ToInt32(txtCodCliente.Text);
                string vendedor = lblUsuario.Text;
                DateTime hora = Convert.ToDateTime(lblHora.Text);
                vendas = acessoDados.CarregarIdPedido(vendedor, codigo, hora);
                txtCodVenda.Text = vendas.CodVendas.ToString();
            }
        }

        private void Adicionar_Click(object sender, EventArgs e)
        {
            modo = "inserir";
            modo = "inserir";
            btnExcluir.Enabled = false;
            btnAtualizar.Enabled = false;
            MessageBox.Show("Preencha os campos e clique no botão GRAVAR para adicionar o produto ao pedido.", "Adicionar produto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DesbloquearCampos();
        }

        private void txtQtde_Validating(object sender, CancelEventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            List<Vendas> vendas = new List<Vendas>();
            vendas = acessoDados.CarregarProdutos();

            foreach (var item in vendas)
            {
                if (cmbNome.Text == item.Produto & cmbTamanho.Text == item.Tamanho)
                {
                    cmbTipo.ValueMember = item.CodTipo.ToString();
                    cmbNome.ValueMember = item.CodProd.ToString();
                    cmbTamanho.ValueMember = item.CodTam.ToString();

                    try
                    {
                        int qtde = Convert.ToInt32(txtQtde.Text);
                        decimal preco = item.Preco;
                        decimal subtotal = qtde * preco;
                        txtSub.Text = subtotal.ToString();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Quantidade inválida.", "Erro de quantidade",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();

            int codVendas = Convert.ToInt32(txtCodVenda.Text);

            int codProd = (int)dgvVendas.CurrentRow.Cells["IdProduto"].Value;

            int qtde = Convert.ToInt32(txtQtde.Text);

            int id = acessoDados.IdPedidoItem(codVendas, codProd, qtde);

            lblCodPI.Text = id.ToString();

            modo = "excluir";
            btnAtualizar.Enabled = false;
            btnAdicionar.Enabled = false;

            MessageBox.Show("Para efetivar a exclusão clique em GRAVAR.",
                "Confirmação de exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvVendas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            int linhaAtual;
            linhaAtual = dgvVendas.CurrentRow.Index;
            cmbTipo.Text = dgvVendas[0, linhaAtual].Value.ToString();
            cmbNome.Text = dgvVendas[2, linhaAtual].Value.ToString();
            cmbTamanho.Text = dgvVendas[3, linhaAtual].Value.ToString();
            txtQtde.Text = dgvVendas[4, linhaAtual].Value.ToString();
            txtSub.Text = dgvVendas[5, linhaAtual].Value.ToString();

            picVendas.Image = new Bitmap(acessoDados.CarregarFotoProduto(Convert.ToInt32(cmbNome.ValueMember)));
        }

        private void cmbNome_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            Vendas vendas = new Vendas();
            string nome = cmbNome.Text;
            vendas = acessoDados.ChecarVeg(nome);

            if (vendas.Vegetariano == true)
            {
                chkVeg.Checked = true;
            }
            else
            {
                chkVeg.Checked = false;
            }

            List<Vendas> tamanhos = acessoDados.CmbTamanho(nome);

            foreach (var item in tamanhos)
            {
                cmbTamanho.ValueMember = item.CodTam.ToString();
                cmbTamanho.Items.Add(item.Tamanho);
            }
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparCampos();
            AcessoDados acessoDados = new AcessoDados();
            List<Vendas> tipos = new List<Vendas>();
            tipos = acessoDados.CmbTipo();

            foreach (var item in tipos)
            {
                if (cmbTipo.SelectedItem.ToString() == item.Tipo)
                {
                    cmbTipo.ValueMember = item.CodTipo.ToString();
                }
            }

            int codigo = Convert.ToInt32(cmbTipo.ValueMember);
            List<Vendas> produto = acessoDados.SelecionarProduto(codigo);

            foreach (var item in produto)
            {
                cmbNome.Items.Add(item.Produto);
            }
        }

        private void cmbTamanho_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            List<Vendas> produtos = new List<Vendas>();
            produtos = acessoDados.CarregarProdutos();
            foreach (var item in produtos)
            {
                if (cmbNome.SelectedItem.ToString() == item.Produto & cmbTamanho.SelectedItem.ToString() == item.Tamanho)
                {
                    cmbTipo.ValueMember = item.CodTipo.ToString();
                    cmbNome.ValueMember = item.CodProd.ToString();
                    cmbTamanho.ValueMember = item.CodTam.ToString();
                }
            }

            picVendas.Image = new Bitmap(acessoDados.CarregarFotoProduto(Convert.ToInt32(cmbNome.ValueMember)));
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();

            int codVendas = Convert.ToInt32(txtCodVenda.Text);

            int codProd = (int)dgvVendas.CurrentRow.Cells["IdProduto"].Value;

            int qtde = (int)dgvVendas.CurrentRow.Cells["Quantidade"].Value;

            int id = acessoDados.IdPedidoItem(codVendas, codProd, qtde);

            lblCodPI.Text = id.ToString();

            modo = "alterar";
            btnAdicionar.Enabled = false;
            btnExcluir.Enabled = false;
            DesbloquearCampos();
            MessageBox.Show("Altere as informações e clique em GRAVAR para efetivar a operação.", "Alteração de produto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Vendas vendas = new Vendas();
            AcessoDados acessoDados = new AcessoDados();
            if(modo == "inserir")
            {
                vendas.CodVendas = Convert.ToInt32(txtCodVenda.Text);
                vendas.CodTipo = Convert.ToInt32(cmbTipo.ValueMember);
                vendas.CodProd = Convert.ToInt32(cmbNome.ValueMember);
                vendas.Quantidade = Convert.ToInt32(txtQtde.Text);
                vendas.Subtotal = Convert.ToDecimal(txtSub.Text);

                bool result = acessoDados.AdicionarProduto(vendas);

                if (result == true)
                {
                    MessageBox.Show("Produto adicionado com sucesso!", "Inserção de produto no pedido",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                    BloquearCampos();
                    btnAtualizar.Enabled = true;
                    btnExcluir.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível adicionar o produto ao pedido.", "Erro de inserção de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                int codigo = Convert.ToInt32(txtCodVenda.Text);
                decimal total = acessoDados.ValorTotal(codigo);
                txtTotal.Text = total.ToString();
            }
            else if(modo == "alterar")
            {
                vendas.Id = Convert.ToInt32(lblCodPI.Text);
                vendas.CodProd = Convert.ToInt32(cmbNome.ValueMember);
                vendas.Quantidade = Convert.ToInt32(txtQtde.Text);
                vendas.Preco = Convert.ToDecimal(txtSub.Text);

                bool result = acessoDados.AlterarPedidoItem(vendas);

                if (result == true)
                {
                    MessageBox.Show("Produto alterado com sucesso!", "Alteração de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarGrid();
                    BloquearCampos();
                    LimparCampos();
                    btnAdicionar.Enabled = true;
                    btnExcluir.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível alterar o produto.", "Erro de alteração de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                int codigo = Convert.ToInt32(txtCodVenda.Text);
                decimal total = acessoDados.ValorTotal(codigo);
                txtTotal.Text = total.ToString();
            }
            else if(modo == "excluir")
            {
                int id = Convert.ToInt32(lblCodPI.Text);
                bool result = acessoDados.ExcluirProdutoPedido(id);


                if (result == true)
                {
                    MessageBox.Show("Produto excluído com sucesso!", "Exclusão de produto",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                    BloquearCampos();
                    btnAdicionar.Enabled = true;
                    btnAtualizar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir o produto.", "Erro de exclusão",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                int codigo = Convert.ToInt32(txtCodVenda.Text);
                decimal total = acessoDados.ValorTotal(codigo);
                txtTotal.Text = total.ToString();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            modo = "";
            btnAdicionar.Enabled = true;
            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
            BloquearCampos();
            LimparCampos();
        }

        private void Finalizar_Click(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            int codVendas = Convert.ToInt32(txtCodVenda.Text);
            decimal total = Convert.ToDecimal(txtTotal.Text);

            bool result = acessoDados.FecharVenda(codVendas, total);

            if(result == true)
            {
                MessageBox.Show("Pedido realizado com sucesso! Total: R$" + total, "Fechamento de pedido",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                this.Hide();
                FrmCliente frmCliente = new FrmCliente(lblUsuario.Text);
                frmCliente.Show();
            }
            else
            {
                MessageBox.Show("Não foi possível fechar o pedido", "Erro no fechamento de pedido",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
