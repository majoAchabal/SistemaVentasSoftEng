using System;
using System.IO;
using System.Xml;

namespace Facturacion
{
    public class GestorFacturacion
    {
        private readonly string directorioArchivos;

        // Constructor que establece el directorio de almacenamiento y respaldo
        public GestorFacturacion(string directorioArchivos)
        {
            this.directorioArchivos = directorioArchivos;
            CrearDirectorioSiNoExiste(directorioArchivos);
        }

        // Método para generar y almacenar la factura
        public void GenerarYAlmacenarFactura(Factura factura)
        {
            // Generar el XML de la factura
            string xmlFactura = factura.GenerarXMLFactura();
            string nombreArchivo = $"Factura_{factura.CUF}.xml";
            string rutaArchivo = Path.Combine(directorioArchivos, nombreArchivo);

            // Guardar la factura en el archivo
            File.WriteAllText(rutaArchivo, xmlFactura);
            Console.WriteLine($"Factura almacenada exitosamente en {rutaArchivo}");

            // Llamar al método para generar respaldo
            GenerarRespaldo(rutaArchivo);
        }

        // Método para generar respaldo de la factura
        private void GenerarRespaldo(string rutaArchivo)
        {
            string directorioRespaldo = Path.Combine(directorioArchivos, "Respaldo", DateTime.Now.ToString("yyyy-MM"));
            CrearDirectorioSiNoExiste(directorioRespaldo);

            string nombreRespaldo = Path.GetFileName(rutaArchivo);
            string rutaRespaldo = Path.Combine(directorioRespaldo, nombreRespaldo);

            // Copiar la factura al directorio de respaldo
            File.Copy(rutaArchivo, rutaRespaldo, overwrite: true);
            Console.WriteLine($"Respaldo generado exitosamente en {rutaRespaldo}");
        }

        // Método para crear un directorio si no existe
        private void CrearDirectorioSiNoExiste(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }

    // Clase Factura con generación automática de códigos y sin datos fijos
    public class Factura
    {
        public string CUIS { get; private set; }
        public string CUFD { get; private set; }
        public string CUF { get; private set; }
        public DateTime FechaEmision { get; private set; }
        public decimal Total { get; private set; }

        // Constructor que inicializa una factura y recibe el total como parámetro
        public Factura(decimal total)
        {
            FechaEmision = DateTime.Now;
            Total = total;
            CUIS = GenerarCUIS();
            CUFD = GenerarCUFD();
            CUF = GenerarCUF();
        }

        // Generación del XML de la factura
        public string GenerarXMLFactura()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Factura");
            root.AppendChild(CrearElemento(doc, "CUIS", CUIS));
            root.AppendChild(CrearElemento(doc, "CUFD", CUFD));
            root.AppendChild(CrearElemento(doc, "CUF", CUF));
            root.AppendChild(CrearElemento(doc, "FechaEmision", FechaEmision.ToString("yyyy-MM-ddTHH:mm:ss")));
            root.AppendChild(CrearElemento(doc, "Total", Total.ToString("F2")));
            doc.AppendChild(root);

            return doc.OuterXml;
        }

        // Métodos para generar códigos únicos y datos de la factura
        private string GenerarCUIS() => $"CUIS-{Guid.NewGuid()}";
        private string GenerarCUFD() => $"CUFD-{DateTime.Now:yyyyMMddHHmmss}";
        private string GenerarCUF() => $"CUF-{Guid.NewGuid()}";

        // Método auxiliar para crear elementos XML
        private XmlElement CrearElemento(XmlDocument doc, string nombre, string valor)
        {
            XmlElement elem = doc.CreateElement(nombre);
            elem.InnerText = valor;
            return elem;
        }
    }
}
