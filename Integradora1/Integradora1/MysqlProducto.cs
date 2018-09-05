using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Integradora1
{
    class MysqlProducto
    {
        MySqlConnection con = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya;");
        public bool actualizar(long id, string producto, int Precio, string descripcion, string foto)
        {

            con.Open();
            bool respuesta = false;
            MySqlCommand cmd = new MySqlCommand("update producto set producto=@producto, precio=@precio, descripcion=@descripcion, foto=@foto WHERE id_producto=@id_producto;", con);
            cmd.Parameters.AddWithValue("@id_producto", id);
            cmd.Parameters.AddWithValue("@producto", producto);
            cmd.Parameters.AddWithValue("@precio", Precio);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@foto", foto);
            respuesta = (cmd.ExecuteNonQuery() > 0);
            con.Close();
            return respuesta;
        }
        public bool delet(long id_producto)
        {
            bool res = false;
            try
            {
                MySqlCommand cmd;

                con.Open();
                cmd = new MySqlCommand("DELETE FROM producto WHERE id_producto=" + id_producto, con);
                cmd.ExecuteNonQuery();
                res = true;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return res;




        }
        public bool add(string producto, int Precio, string descripcion, string foto)
        {
            bool respuesta = false;

            try
            {
                MySqlCommand cmd;
                con.Open();
                cmd = new MySqlCommand("INSERT INTO producto (id_producto, producto, precio, descripcion, foto) VALUES " +
                    "(NULL, @producto, @precio, @descripcion, @foto)", con);
                cmd.Parameters.AddWithValue("@producto", producto);
                cmd.Parameters.AddWithValue("@precio", Precio);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@foto", foto);
                cmd.ExecuteNonQuery();
                respuesta = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return respuesta;
        }
    }
}
