using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Integradora1
{
    public class DatosBtn
    {
        private MySqlCommand cmd;
        private MySqlConnection con = new MySqlConnection("host=localhost;uid=root;pwd=0420;database=mysqlya;");
        private MySqlDataReader dr;
        public  int cod;
        public  string pro;
        public  double pre;
        public void btnCardatos(int num)
        {
            string query = "SELECT * FROM producto WHERE id_producto ="+ num +";";
           con.Open();
            cmd = new MySqlCommand(query,con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cod = dr.GetInt16(0);
                pro = dr.GetString(1);
                pre = dr.GetInt16(2);

            }
            con.Close();
        }
    }
}
