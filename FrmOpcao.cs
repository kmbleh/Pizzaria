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
    public partial class FrmOpcao : Form
    {
        public FrmOpcao()
        {
            InitializeComponent();
        }

        public FrmOpcao(string usuarioLogado)
        {
            InitializeComponent();
            lblUsuario.Text = usuarioLogado;
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Você será redirecionado para a tela de clientes.",
                "Redirecionamento Vendas", MessageBoxButtons.OK);
            FrmCliente frmCliente = new FrmCliente(lblUsuario.Text);
            frmCliente.Show();
            this.Hide();
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Você será redirecionado para a tela de cadastro.",
                "Redirecionamento Cadastro", MessageBoxButtons.OK);
            FrmCadastro frmCadastro = new FrmCadastro(lblUsuario.Text);
            frmCadastro.Show();
            this.Hide();
        }
    }
}
