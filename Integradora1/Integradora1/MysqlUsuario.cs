using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Integradora1
{
    class MysqlUsuario
    {
        public MySqlConnection con = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya;");
        public  bool actualizar(int id,string ap1,string ap2,string nombre,int nivel, string usuario, string pass, string mail, string img)
        {

            con.Open();
            bool respuesta = false;
            MySqlCommand cmd = new MySqlCommand("update usuarios set Ap1=@Ap1, Ap2=@Ap2, Nombre=@Nombre, Nivel=@Nivel, Usuario=@Usuario, Password=@Password, mail=@mail, Foto=@Foto WHERE Id_usuario=@Id_usuario;", con);

            cmd.Parameters.AddWithValue("@Id_usuario", id);
            cmd.Parameters.AddWithValue("@Ap1", ap1);
            cmd.Parameters.AddWithValue("@Ap2", ap2);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Nivel", nivel);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@Password", pass);
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@Foto", img);


            respuesta = (cmd.ExecuteNonQuery() > 0);
            con.Close();
            return respuesta;
        }
        public bool delet(int id_usuario)
        {
            bool res = false;
           try
            {
                MySqlCommand cmd;
                
                con.Open();
                cmd = new MySqlCommand("DELETE FROM usuarios WHERE Id_usuario=" + id_usuario, con);
                cmd.ExecuteNonQuery();
                res = true;
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return res;


           

        }
        public  bool add(string ap1, string ap2, string nombre, int nivel, string usuario, string pass, string mail,string img)
        {
            bool respuesta = false;

            try
            {
                MySqlCommand cmd;
                con.Open();
                cmd = new MySqlCommand("INSERT INTO usuarios (Id_usuario, Ap1 , Ap2, Nombre, Nivel, Usuario, Password, mail, Foto) VALUES " +
                    "(NULL, @Ap1, @Ap2, @Nombre, @Nivel, @Usuario, @Password, @mail, @Foto)", con);
                cmd.Parameters.AddWithValue("@Ap1", ap1);
                cmd.Parameters.AddWithValue("@Ap2", ap2);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Nivel", nivel);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Password", pass);
                cmd.Parameters.AddWithValue("@mail", mail);
                cmd.Parameters.AddWithValue("@Foto", img);
                cmd.ExecuteNonQuery();
                respuesta = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return respuesta;
        }
        

       
    }
}
