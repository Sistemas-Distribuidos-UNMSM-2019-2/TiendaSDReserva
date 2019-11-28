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
    class OrdenCompraDAO
    {
        private DataBaseMySQL dataBaseMySQL;
        private MySqlCommand commandDatabase;

        public OrdenCompraDAO()
        {
            dataBaseMySQL = DataBaseMySQL.GetInstancia();
        }

        public void registrarOrdenCompra(OrdenCompraModel ordenCompra)
        {
            dataBaseMySQL.OpenBD();

            commandDatabase = new MySqlCommand();
            commandDatabase.Connection = dataBaseMySQL.GetConnection();
            commandDatabase.CommandType = CommandType.StoredProcedure;
            commandDatabase.CommandText = "registrarOrdenCompra";
            commandDatabase.Parameters.Add(new MySqlParameter("d1", ordenCompra.sRucCliente));
            commandDatabase.Parameters.Add(new MySqlParameter("d2", ordenCompra.nPrecioTotal));
            commandDatabase.Parameters.Add(new MySqlParameter("d3", ordenCompra.dFechaCompra));
            commandDatabase.Parameters.Add(new MySqlParameter("d4", ordenCompra.sEstado));

            commandDatabase.ExecuteNonQuery();

            dataBaseMySQL.CloseBD();

            ordenCompra.nCodigoOrden = retornarCodigoOrdenCompra();

            registrarDetalleOrdenCompra(ordenCompra.lDetalleCompra, ordenCompra.nCodigoOrden);
        }

        private int retornarCodigoOrdenCompra()
        {
            dataBaseMySQL.OpenBD();

            int nCodigo = 0;

            commandDatabase = new MySqlCommand();
            commandDatabase.Connection = dataBaseMySQL.GetConnection();
            commandDatabase.CommandType = CommandType.StoredProcedure;
            commandDatabase.CommandText = "devolverCodigoOrdenCompra";

            MySqlDataReader lector = commandDatabase.ExecuteReader();

            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    nCodigo = lector.GetInt32(0);
                }
            }
            else
            {
                Console.WriteLine("No se encontraron datos.");
            }

            dataBaseMySQL.CloseBD();

            return nCodigo;
        }

        private void registrarDetalleOrdenCompra(List<OrdenCompraDetalleModel> lDetalleCompra, int nCodigoOrden)
        {
            foreach (OrdenCompraDetalleModel auxiliar in lDetalleCompra)
            {
                dataBaseMySQL.OpenBD();

                commandDatabase = new MySqlCommand();
                commandDatabase.Connection = dataBaseMySQL.GetConnection();
                commandDatabase.CommandType = CommandType.StoredProcedure;
                commandDatabase.CommandText = "registrarDetalleOrdenCompra";
                commandDatabase.Parameters.Add(new MySqlParameter("d1", auxiliar.nCodigoProducto));
                commandDatabase.Parameters.Add(new MySqlParameter("d2", auxiliar.nCantidadProducto));
                commandDatabase.Parameters.Add(new MySqlParameter("d3", auxiliar.nTotalParcial));
                commandDatabase.Parameters.Add(new MySqlParameter("d4", nCodigoOrden));

                commandDatabase.ExecuteNonQuery();

                dataBaseMySQL.CloseBD();
            }
        }


    }
}
