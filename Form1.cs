using Grpc.Core;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Grpc.Core.ServerServiceDefinition;

namespace BackupDeDados
{
    public partial class Form1 : Form
    {
        /* Erbson Santos  29/08/2020 sistema que gera backup 
         *  
         * 
         * 
         */
         

        string Strsql;
        string caminho;
        string barra = "\\";
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        EnderecoDeConexao cadastro = new EnderecoDeConexao();
        Configuracao configuracao = new Configuracao();
        public Form1()
        {
            InitializeComponent();
        }

        public void ChooseFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                caminho = textBox1.Text;

            }

        }

        public void backup()
        {
            try
            {
                Configuracao configuracao = new Configuracao();
                //configuracao.CriaArquivo();


                var lista = configuracao.LerArquivo();

                foreach (Conexao conexao in lista)
                {
                    builder.DataSource = conexao.instancia;

                    builder.UserID = conexao.usuario;

                    builder.Password = conexao.senha;
               

                }
                builder.InitialCatalog = "master";
                caminho = caminho + barra;

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @name VARCHAR(256);");
                sb.Append("DECLARE @nomedobanco VARCHAR(256) ");
                sb.Append("DECLARE @fileName VARCHAR(256) ");
                sb.Append("DECLARE @PATH VARCHAR(256)");
                sb.Append("DECLARE @FILEEXTENSION CHAR(4)");
                sb.Append("DECLARE @DATE VARCHAR(20)");
                sb.Append("DECLARE @FULLPATH CHAR(256)");
                sb.Append("SET @path = '" + caminho + "'  ");
                sb.Append("DECLARE db_cursor CURSOR FOR ");
                sb.Append("   SELECT name ");
                sb.Append(" FROM master.dbo.sysdatabases ");
                sb.Append("   WHERE name NOT IN ('model','msdb','tempdb','master','OEE','Empresa')  "); // informe quais bancos  não desejaram fazer bekup
                sb.Append("OPEN db_cursor   ");
                sb.Append("FETCH NEXT FROM db_cursor INTO @name   ");
                sb.Append("WHILE @@FETCH_STATUS = 0  ");
                sb.Append("BEGIN  ");
                sb.Append("set @nomedobanco = @name");
                sb.Append(" SET @PATH = '" + caminho + "' ");
                sb.Append("SET @DATE = REPLACE((CONVERT(nvarchar(30), GETDATE(), 120)),'','')");
                sb.Append("SET @DATE = REPLACE(@DATE,':','.')");
                sb.Append("SET @FILEEXTENSION = '.bak'");
                sb.Append("SET @FULLPATH = @PATH+@nomedobanco+@DATE+@FILEEXTENSION");
                sb.Append("  BACKUP DATABASE @name TO DISK = @FULLPATH with format");
                sb.Append("   FETCH NEXT FROM db_cursor INTO @name;");
                sb.Append("END ");
                sb.Append("CLOSE db_cursor ");
                sb.Append("DEALLOCATE db_cursor ");

                Strsql = sb.ToString();

                SqlCommand command = new SqlCommand(Strsql, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ChooseFolder();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Configuracao configuracao = new Configuracao();
            if (!String.IsNullOrEmpty(textBox1.Text))
            {

                var lista = configuracao.LerArquivo();

                if (lista != null)
                {

                    label1.Visible = true;
                    this.backup();
                    label1.Visible = false;
                    MessageBox.Show("Backup concluido com sucesso!");
                    label2.Visible = true;

                }
                else
                {

                }

            }
            else
            {
                MessageBox.Show("Por favor escolha onde salvar o backup");
            }

        }

        private void cadastrarConexãoComOBancoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            cadastro.Show();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}

