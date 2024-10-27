using System.Collections.Generic;

namespace SistemaFacturacion
{
    public class EmisorFactura
    {
        private ClaseCuis _claseCuis;
        private ClaseCufd _claseCufd;
        private ClaseGeneradorCuf _claseGeneradorCuf;
        private ClaseGeneradorQr _claseGeneradorQr;

        public EmisorFactura()
        {
            _claseCuis = new ClaseCuis();
            _claseCufd = new ClaseCufd();
            _claseGeneradorCuf = new ClaseGeneradorCuf();
            _claseGeneradorQr = new ClaseGeneradorQr();
        }

        public Factura EmitirFactura(List<Producto> productosSeleccionados)
        {
            // Obtener CUIS desde ClaseCuis
            string cuis = _claseCuis.ObtenerCuis();
            // Obtener CUFD desde ClaseCufd
            string cufd = _claseCufd.ObtenerCufd();

            // Crear la factura con los datos obtenidos
            Factura factura = new Factura
            {
                Productos = productosSeleccionados,
                CUIS = cuis,
                CUFD = cufd
            };

            // Generar CUF utilizando ClaseGeneradorCuf
            factura.CUF = _claseGeneradorCuf.GenerarCuf(cuis, cufd, factura);
            // Generar Código QR utilizando ClaseGeneradorQr
            factura.CodigoQR = _claseGeneradorQr.GenerarCodigoQr(factura.CUF);

            return factura;
        }
    }
}
