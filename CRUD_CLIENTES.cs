using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SistemaFacturacion.Models
{
    public class Cliente
    {
        public string Nit { get; private set; }
        public string Nombre { get; private set; }
        public string Celular { get; private set; }

        // Constructor para inicializar un cliente
        public Cliente(string nombre, string nit, string celular)
        {
            Create(nombre, nit, celular);
        }

        // Método para crear un nuevo cliente
        public void Create(string nombre, string nit, string celular)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(nit) || string.IsNullOrWhiteSpace(celular))
                throw new ArgumentException("Los campos no pueden estar vacíos.");

            Nombre = nombre;
            Nit = nit;
            Celular = celular;

            Console.WriteLine("Cliente creado con éxito.");
        }

        // Método para actualizar los datos de un cliente
        public void Update(string nombre, string celular)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(celular))
                throw new ArgumentException("Los campos no pueden estar vacíos.");

            Nombre = nombre;
            Celular = celular;

            Console.WriteLine("Cliente actualizado con éxito.");
        }

        // Método para eliminar un cliente por NIT
        public void Delete(string nit)
        {
            if (Nit == nit)
            {
                Nit = string.Empty;
                Nombre = string.Empty;
                Celular = string.Empty;

                Console.WriteLine("Cliente eliminado con éxito.");
            }
            else
            {
                Console.WriteLine("Cliente no encontrado.");
            }
        }

        // Método para buscar un cliente por NIT
        public static Cliente Search(string nit, List<Cliente> clientes)
        {
            return clientes.FirstOrDefault(c => c.Nit == nit);
        }
    }
}

