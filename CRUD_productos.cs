using System;
using System.Data.SqlClient;

public class Class1
{
    int id_producto;
    string nombre;
    string descripcion;
    float precio;
    string categoria;
    int stock;

    private string connectionString = "Server=myServerAddress;Database=mibasededatos;User Id=myUsername;Password=myPassword;";

    public void Actualizar_Producto(int id_producto, string nombre, string descripcion, float precio, string categoria, int stock)
    {
        this.id_producto = id_producto;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
        this.categoria = categoria;
        this.stock = stock;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, Categoria = @categoria, Stock = @stock WHERE Id_Producto = @id_producto";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_producto", id_producto);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@precio", precio);
                command.Parameters.AddWithValue("@categoria", categoria);
                command.Parameters.AddWithValue("@stock", stock);
                command.ExecuteNonQuery();
            }
        }
    }

    public void agregar_producto()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Productos (Id_Producto, Nombre, Descripcion, Precio, Categoria, Stock) VALUES (@id_producto, @nombre, @descripcion, @precio, @categoria, @stock)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_producto", id_producto);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@precio", precio);
                command.Parameters.AddWithValue("@categoria", categoria);
                command.Parameters.AddWithValue("@stock", stock);
                command.ExecuteNonQuery();
            }
        }
    }

    public void modificar_producto()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, Categoria = @categoria, Stock = @stock WHERE Id_Producto = @id_producto";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_producto", id_producto);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@descripcion", descripcion);
                command.Parameters.AddWithValue("@precio", precio);
                command.Parameters.AddWithValue("@categoria", categoria);
                command.Parameters.AddWithValue("@stock", stock);
                command.ExecuteNonQuery();
            }
        }
    }

    public void eliminar_producto()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Productos WHERE Id_Producto = @id_producto";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_producto", id_producto);
                command.ExecuteNonQuery();
            }
        }
    }
}
