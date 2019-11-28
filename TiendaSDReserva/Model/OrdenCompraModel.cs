using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaSDReserva.Model
{
    [Serializable]
    class OrdenCompraModel
    {
        public int nCodigoOrden;
        public String sRucCliente;
        public double nPrecioTotal;
        public DateTime dFechaCompra;
        public DateTime dFechaPago;
        public string sEstado;
        public List<OrdenCompraDetalleModel> lDetalleCompra;
    }
}
