using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SistemaVentas
{
    internal class Carrito
    {
        private List<Products> productosEnCarrito;
        private Conexion conexion = new Conexion(); //falta

        public Carrito()
        {
            productosEnCarrito = new List<Products>();
        }

        public void AgregarAlCarrito(Products producto, int cantidad)
        {
            if (producto.CantidadDisponible < cantidad)
            {
                MessageBox.Show("No hay suficiente stock disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Verificar si el producto ya esta en el carrito
            var productoExistente = productosEnCarrito.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
            if (productoExistente != null)
            {
                //Si el producto ya esta en el carrito actualiza la cantidad
                productoExistente.CantidadDisponible += cantidad;
            }
            else
            {
                //Si no, agrega el producto al carrito con la cantidad seleccionada
                productosEnCarrito.Add(new Products
                {
                    IdProducto = producto.IdProducto,
                    NombreProducto = producto.NombreProducto,
                    PrecioProducto = producto.PrecioProducto,
                    CategoriaProducto = producto.CategoriaProducto,
                    CantidadDisponible = cantidad
                });
            }

            MessageBox.Show("Producto agregado al carrito.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void EliminarDelCarrito(int idProducto)
        {
            var producto = productosEnCarrito.FirstOrDefault(p => p.IdProducto == idProducto);
            if (producto != null)
            {
                productosEnCarrito.Remove(producto);
                MessageBox.Show("Producto eliminado del carrito.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("El producto no esta en el carrito.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public decimal CalcularTotal()
        {
            return productosEnCarrito.Sum(p => p.PrecioProducto * p.CantidadDisponible);
        }

        public void VaciarCarrito()
        {
            productosEnCarrito.Clear();
            MessageBox.Show("El carrito ha sido vaciado.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void FinalizarCompra()
        {
            try
            {
                //falta
                using (var conn = conexion.AbrirConexion())
                {
                    //Iniciar una transaccinn
                    using (var transaction = conn.BeginTransaction())
                    {
                        foreach (var producto in productosEnCarrito)
                        {
                            #Actualizar la cantidad disponible en la base de datos
                            string query = "UPDATE producto SET cantidadDisponible = cantidadDisponible - @Cantidad WHERE idProducto = @IdProducto";
                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Cantidad", producto.CantidadDisponible);
                                cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }

                VaciarCarrito();
                MessageBox.Show("Compra realizada.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar la compra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Products> MostrarProductosEnCarrito()
        {
            return productosEnCarrito;
        }
    }
}
