using Pizzaria.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Pizzaria
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();

            string nome = txtUsuario.Text;
            string senha = txtSenha.Text;

            bool retorno = acessoDados.ValidarUsuario(nome, senha);

            if(retorno == true)
            {
                MessageBox.Show("Usuário logado com sucesso!", "Login", MessageBoxButtons.OK);
                FrmOpcao frmOpcao = new FrmOpcao(txtUsuario.Text);
                frmOpcao.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuário e/ou senha inválidos.", "Erro de login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsuario_Validating(object sender, CancelEventArgs e)
        {
            AcessoDados acessoDados = new AcessoDados();
            string nome = txtUsuario.Text;
            bool retorno = acessoDados.VerificarExistencia(nome);

            if(retorno == true)
            {
                picLogin.Image = new Bitmap(acessoDados.CarregarFoto(nome));
            }
            else
            {
                picLogin.Image = Resources.Unicorn;
            }
        }
    }
}
