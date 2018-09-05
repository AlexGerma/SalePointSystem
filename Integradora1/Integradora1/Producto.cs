using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Integradora1
{
    class Producto
    {
        public static bool Cargadatos(DataGridView dv)
        {
            MySqlDataReader dr;
            MySqlCommand cmd;
            MySqlConnection cn = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya");
            bool res = false;
            try
            {
                dv.Rows.Clear();
                cn.Open();
                cmd = new MySqlCommand("SELECT * FROM producto", cn);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr.IsDBNull(4))
                        {
                            dv.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetInt16(2), dr.GetString(3));

                        }
                        else
                        {
                            dv.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetInt16(2), dr.GetString(3), dr.GetString(4), Image.FromFile(dr.GetString(4)));

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
            catch (Exception)
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
