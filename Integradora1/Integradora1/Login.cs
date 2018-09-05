using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Integradora1
{
    public partial class Login : Form
    {
        private MySqlConnection cn;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtusuario.Text.Trim()== "")
            {
                MessageBox.Show("Eaaa te falto el usuairo");
                txtusuario.Clear();
                txtusuario.Focus();
                
            }
            else
            {
                if (txtPass.Text.Trim() == "")
                {
                    MessageBox.Show("Eaaa te falto el password");
                    txtPass.Clear();
                    txtPass.Focus();
                }
                else
                {
                    try
                    {
                        cn = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya;");
                        cn.Open();
                        String Query = "SELECT * FROM usuarios WHERE Usuario='" + txtusuario.Text + "' AND Password='" + txtPass.Text + "';";
                        cmd = new MySqlCommand(Query, cn);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        if (!dr.HasRows)
                        {
                            MessageBox.Show("El usuario no existe");
                        }
                        else
                        {
                            this.Hide();
                            if (dr[4].ToString()=="1")
                            {

                                new Operador().ShowDialog();
                            }
                            if (dr[4].ToString() == "2")
                            {
                                new Gerente(dr[3].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString()).ShowDialog();
                            }
                            this.Show();
                            txtusuario.Clear();
                            txtPass.Clear();
                            txtusuario.Focus();
                        }
                        cn.Close();

                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                   
                }
                
            }
           
                
            
           
        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
