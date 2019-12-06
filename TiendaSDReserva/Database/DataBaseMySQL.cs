using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaSDReserva.Database
{
    public class DataBaseMySQL
    {
        public static DataBaseMySQL instancia;

        private MySqlConnection con;
        private String servidor = "192.168.4.7";
        private String puerto = "3306";
        private String usuario = "root";
        private String password = "root";
        private String database = "sdtienda";

        private DataBaseMySQL()
        {

        }

        public static DataBaseMySQL GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new DataBaseMySQL();
            }

            return instancia;
        }

        public MySqlConnection GetConnection()
        {
            return con;
        }

        public void OpenBD()
        {
            string sConexion = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", servidor, puerto, usuario, password, database);

            con = new MySqlConnection(sConexion);
            con.Open();
        }

        public void CloseBD()
        {
            con.Close();
        }
    }

}
