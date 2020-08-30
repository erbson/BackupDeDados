using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupDeDados
{
    class Configuracao
    {



        List<Conexao> cadastro = new List<Conexao>();

        public void CriaArquivo(string endereco, string usuario, string senha)
        {
            Criptografia criptografia = new Criptografia(CryptProvider.RC2);  // escolhe o tipo de criptografia, neste caso escolhemos o RC2
            criptografia.Key = "Bkap2020";

            using (StreamWriter writer = new StreamWriter("C:\\dados\\config.config", true))
            {
                writer.WriteLine(endereco);
            }
            // 2: Anexa uma linha ao arquivo
            using (StreamWriter writer = new StreamWriter(@"C:\dados\config.config", true))
            {
                writer.WriteLine(usuario);
            }

            using (StreamWriter writer = new StreamWriter(@"C:\dados\config.config", true))
            {

                writer.WriteLine(criptografia.Encrypt(senha));
            }


        }


   public List<Conexao> LerArquivo() 
 
        {

            Criptografia criptografia = new Criptografia(CryptProvider.RC2);
            Conexao con = new Conexao();
            string arquivo = @"C:\dados\config.config";

            if (File.Exists(arquivo))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(arquivo))
                    {
                        String linha;

                        // Lê linha por linha até o final do arquivo
                        while ((linha = sr.ReadLine()) != null)
                        {

                            if (con.instancia == null)
                            {
                                con.instancia = linha;
                               
                              continue;
                            }
                            if (con.usuario == null)
                            {
                                con.usuario = linha;
                                continue;

                            }

                        
                            criptografia.Key = "Bkap2020";
                            con.senha = criptografia.Decrypt(linha);
                            

                            cadastro.Add(con);

                        }
                        return cadastro;

                    }
                }

                
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
            }
            
            else
            {
                MessageBox.Show("O Arquivo de configuração com conexão com o banco de dados não foi localizado, por favor cadastre a configuração no menu configuração!");
          
            }
            return null;

        }

        public  string CalculaHash(string Senha)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Senha);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString(); // Retorna senha criptografada 
            }
            catch (Exception)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }


     


    }
}
