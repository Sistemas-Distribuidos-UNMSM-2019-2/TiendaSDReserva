using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaSDReserva.Database;
using TiendaSDReserva.Model;

namespace TiendaSDReserva.ComponenteDatos
{
    class ProductoDAO
    {
        private DataBaseMySQL dataBaseMySQL;
        private MySqlCommand commandDatabase;

        public ProductoDAO()
        {
            dataBaseMySQL = DataBaseMySQL.GetInstancia();
        }

        public void actualizarStock(List<OrdenCompraDetalleModel> lDetalleCompra)
        {
            foreach (OrdenCompraDetalleModel auxiliar in lDetalleCompra)
            {
                dataBaseMySQL.OpenBD();

                commandDatabase = new MySqlCommand();
                commandDatabase.Connection = dataBaseMySQL.GetConnection();
                commandDatabase.CommandType = CommandType.StoredProcedure;
                commandDatabase.CommandText = "actualizarStock";
                commandDatabase.Parameters.Add(new MySqlParameter("d1", auxiliar.nCodigoProducto));
                commandDatabase.Parameters.Add(new MySqlParameter("d2", auxiliar.nCantidadProducto));

                commandDatabase.ExecuteNonQuery();

                dataBaseMySQL.CloseBD();
            }
            
        }
    }
}
