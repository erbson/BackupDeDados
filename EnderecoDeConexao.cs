using BackupDeDados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupDeDados
{
    public partial class EnderecoDeConexao : Form
    {
        public EnderecoDeConexao()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuracao config = new Configuracao();
            try
            {

                config.CriaArquivo(textendereco.Text, textusuario.Text, textsenha.Text);

                MessageBox.Show("configuração cadastrada  com sucesso!");
                
            }
            catch (Exception erro)
            {
                MessageBox.Show("erro"+erro);

            }




        }
    }
}
