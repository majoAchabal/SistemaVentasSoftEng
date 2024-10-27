using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class ClienteCRUD
{
    private string connectionString = "Data Source=DB.db";

    // Crear nuevo cliente
    public void CrearCliente(Cliente cliente)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Cliente (nombreCompleto, carnet, celular) VALUES (@nombreCompleto, @carnet, @celular)";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombreCompleto", cliente.NombreCompleto);
                command.Parameters.AddWithValue("@carnet", cliente.Carnet);
                command.Parameters.AddWithValue("@celular", cliente.Celular);
                command.ExecuteNonQuery();
            }
        }
    }

    // Leer todos los clientes
    public List<Cliente> ObtenerClientes()
    {
        var clientes = new List<Cliente>();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM Cliente";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cliente = new Cliente
                        {
                            Id = reader.GetInt32(0),
                            NombreCompleto = reader.GetString(1),
                            Carnet = reader.GetString(2),
                            Celular = reader.GetString(3)
                        };
                        clientes.Add(cliente);
                    }
                }
            }
        }
        return clientes;
    }

    // Actualizar cliente
    public void ActualizarCliente(Cliente cliente)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE Cliente SET nombreCompleto = @nombreCompleto, carnet = @carnet, celular = @celular WHERE id = @id";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombreCompleto", cliente.NombreCompleto);
                command.Parameters.AddWithValue("@carnet", cliente.Carnet);
                command.Parameters.AddWithValue("@celular", cliente.Celular);
                command.Parameters.AddWithValue("@id", cliente.Id);
                command.ExecuteNonQuery();
            }
        }
    }

    // Eliminar cliente
    public void EliminarCliente(int id)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM Cliente WHERE id = @id";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
