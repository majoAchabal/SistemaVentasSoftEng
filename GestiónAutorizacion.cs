using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFacturacion.Models
{
    public class GestionAutorizacion
    {
        //Códigos CIUS y CUFD actuales
        private string cius;
        private string cufd;

        // Fechas de expiración de CIUS y CUFD
        private DateTime ciusExpiry;
        private DateTime cufdExpiry;

        // Conjuntos estáticos para almacenar códigos generados y evitar repeticiones
        private static HashSet<string> generatedCIUS = new HashSet<string>();
        private static HashSet<string> generatedCUFD = new HashSet<string>();

        // Constructor de la clase, genera los códigos iniciales
        public GestionAutorizacion()
        {
            GenerateCIUS();
            GenerateCUFD();
        }

        // Método para generar un CIUS único
        private void GenerateCIUS()
        {
            cius = GenerateUniqueCode(16, generatedCIUS);
            // Definimos la expiración del CIUS en 1 año a partir del momento de generación
            ciusExpiry = DateTime.UtcNow.AddYears(1);
        }

        // Método para generar un CUFD único
        private void GenerateCUFD()
        {
            cufd = GenerateUniqueCode(64, generatedCUFD);
            // Definimos la expiración del CUFD en 24 horas a partir del momento de generación
            cufdExpiry = DateTime.UtcNow.AddHours(24);
        }

        // Método para generar un código único con longitud y evitar duplicados
        private string GenerateUniqueCode(int length, HashSet<string> existingCodes)
        {
            string code;
            do
            {
                code = GenerateRandomString(length);
            }
            while (!existingCodes.Add(code)); // Añade el código al HashSet y verifica si es único
            return code;
        }

        // Método para generar una cadena aleatoria de longitud específica
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            byte[] data = new byte[length];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[data[i] % chars.Length]);
            }
            return result.ToString();
        }

        // Método para verificar la vigencia del CIUS
        public bool IsCIUSValid()
        {
            return DateTime.UtcNow <= ciusExpiry;
        }

        // Método para verificar la vigencia del CUFD
        public bool IsCUFDValid()
        {
            return DateTime.UtcNow <= cufdExpiry;
        }

        // Método para renovar el CIUS si está vencido
        public void RenovarCIUS()
        {
            if (!IsCIUSValid())
            {
                GenerateCIUS();
                Console.WriteLine("El CIUS ha sido renovado exitosamente.");
            }
            else
            {
                Console.WriteLine("El CIUS aún es válido, no se requiere renovación.");
            }
        }

        // Método para renovar el CUFD si está vencido
        public void RenovarCUFD()
        {
            if (!IsCUFDValid())
            {
                GenerateCUFD();
                Console.WriteLine("El CUFD ha sido renovado exitosamente.");
            }
            else
            {
                Console.WriteLine("El CUFD aún es válido, no se requiere renovación.");
            }
        }

        // Método para obtener el CIUS actual
        public string GetCIUS()
        {
            // Regresa el CIUS actual
            return cius;
        }

        // Método para obtener el CUFD actual
        public string GetCUFD()
        {
            // Regresa el CUFD actual
            return cufd;
        }
    }

}