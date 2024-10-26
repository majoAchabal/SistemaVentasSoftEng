using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SistemaVentas
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }
        public List<OrderItem> DetallesVenta { get; set; } = new List<OrderItem>();
    }

    public class CRUDVenta
    {
        private Conexion conexion = new Conexion();

      
        public bool CrearVenta(Venta venta)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    using (var transaction = conn.BeginTransaction())
                    {
                      
                        string queryVenta = "INSERT INTO ventas (FechaVenta, TotalVenta) VALUES (@FechaVenta, @TotalVenta)";
                        using (var cmdVenta = new MySqlCommand(queryVenta, conn, transaction))
                        {
                            cmdVenta.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                            cmdVenta.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                            cmdVenta.ExecuteNonQuery();

                           
                            venta.IdVenta = (int)cmdVenta.LastInsertedId;
                        }

                  
                        foreach (var item in venta.DetallesVenta)
                        {
                            string queryDetalle = "INSERT INTO detalles_venta (IdVenta, IdProducto, Cantidad, PrecioUnitario) VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario)";
                            using (var cmdDetalle = new MySqlCommand(queryDetalle, conn, transaction))
                            {
                                cmdDetalle.Parameters.AddWithValue("@IdVenta", venta.IdVenta);
                                cmdDetalle.Parameters.AddWithValue("@IdProducto", item.Product.ProductID);
                                cmdDetalle.Parameters.AddWithValue("@Cantidad", item.Quantity);
                                cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", item.Product.Price);
                                cmdDetalle.ExecuteNonQuery();
                            }

                        
                            string queryStock = "UPDATE productos SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto";
                            using (var cmdStock = new MySqlCommand(queryStock, conn, transaction))
                            {
                                cmdStock.Parameters.AddWithValue("@Cantidad", item.Quantity);
                                cmdStock.Parameters.AddWithValue("@IdProducto", item.Product.ProductID);
                                cmdStock.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Venta creada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        
        public List<Venta> ObtenerVentas()
        {
            var ventas = new List<Venta>();

            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    string query = "SELECT * FROM ventas";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var venta = new Venta
                            {
                                IdVenta = reader.GetInt32("IdVenta"),
                                FechaVenta = reader.GetDateTime("FechaVenta"),
                                TotalVenta = reader.GetDecimal("TotalVenta")
                            };
                            ventas.Add(venta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ventas;
        }

   
        public bool ActualizarVenta(Venta venta)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    string query = "UPDATE ventas SET FechaVenta = @FechaVenta, TotalVenta = @TotalVenta WHERE IdVenta = @IdVenta";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                        cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                        cmd.Parameters.AddWithValue("@IdVenta", venta.IdVenta);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Venta actualizada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    
        public bool EliminarVenta(int idVenta)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    string query = "DELETE FROM ventas WHERE IdVenta = @IdVenta";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Venta eliminada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
