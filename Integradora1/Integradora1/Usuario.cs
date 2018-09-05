using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Integradora1
{
    class Usuarios
    {
        
        public static bool Cargadatos (DataGridView dv)
        {
            MySqlDataReader dr;
            MySqlCommand cmd;
            MySqlConnection cn = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya");
            bool res = false;
            try
            {
                dv.Rows.Clear();
                cn.Open();
                cmd = new MySqlCommand("SELECT * FROM usuarios",cn);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr.IsDBNull(8))
                        {
                            dv.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7));

                        }
                        else
                        {
                            dv.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8), Image.FromFile(dr.GetString(8)));

                        }
                    }
                    
                    res = true;
                }
                else
                {
                    MessageBox.Show("Error al Cargar Los Datos");
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show("imposible llenar la tabla" + ex.Message);
            }
            catch (Exception )
            {

                MessageBox.Show("Ups Algo salio mal");
            }
            finally
            {
                cn.Close();
            }
            return res;
        }

       
    }
}
