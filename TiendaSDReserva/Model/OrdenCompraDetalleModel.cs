using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaSDReserva.Model
{
    [Serializable]
    class OrdenCompraDetalleModel
    {
        public int nCodigoProducto;
        public String sNombreProducto;
        public int nCantidadProducto;
        public double nTotalParcial;
        public Boolean bExistencia;
    }
}
