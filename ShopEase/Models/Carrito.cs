using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShopApp.Models
{
    public class Carrito
    {
        // Propiedad para asociar el carrito al usuario autenticado
        public string? OwnerId { get; set; }
        public List<LineaCarrito> Items { get; set; }

        // Cadena de conexión a MySQL (ajústala según tu entorno)
        private readonly string connectionString = "server=localhost;database=shop;user=root;password=Necrobite32!";

        // Variable estática para asegurar que se verifique el esquema sólo una vez
        private static bool schemaChecked = false;

        public Carrito()
        {
            Items = new List<LineaCarrito>();
        }

        /// <summary>
        /// Verifica que la tabla Carrito tenga la columna OwnerId.
        /// Si no existe, la agrega mediante ALTER TABLE.
        /// </summary>
        private void EnsureSchemaChecked()
        {
            if (!schemaChecked)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Usamos DATABASE() para obtener el esquema actual
                    string checkQuery = @"SELECT COUNT(*) 
                                          FROM INFORMATION_SCHEMA.COLUMNS 
                                          WHERE TABLE_SCHEMA = DATABASE() 
                                          AND TABLE_NAME = 'Carrito' 
                                          AND COLUMN_NAME = 'OwnerId'";
                    using (var command = new MySqlCommand(checkQuery, connection))
                    {
                        long count = (long)command.ExecuteScalar();
                        if (count == 0)
                        {
                            // La columna no existe, se agrega OwnerId
                            string alterQuery = "ALTER TABLE Carrito ADD COLUMN OwnerId VARCHAR(450) NOT NULL DEFAULT ''";
                            using (var alterCommand = new MySqlCommand(alterQuery, connection))
                            {
                                alterCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                schemaChecked = true;
            }
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            EnsureSchemaChecked();

            if (producto == null)
                throw new ArgumentNullException(nameof(producto));
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que 0");

            // Verificar si el producto ya existe en el carrito (en memoria)
            var itemExistente = Items.Find(i => i.Producto.ProductoID == producto.ProductoID);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                Items.Add(new LineaCarrito(producto, cantidad));
            }

            // Insertar o actualizar en la base de datos
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Carrito (ProductoID, Nombre, Precio, Categoria, Cantidad, OwnerId)
                    VALUES (@ProductoID, @Nombre, @Precio, @Categoria, @Cantidad, @OwnerId)
                    ON DUPLICATE KEY UPDATE 
                        Cantidad = Cantidad + @Cantidad";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoID", producto.ProductoID);
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@OwnerId", OwnerId ?? string.Empty);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int productoID)
        {
            EnsureSchemaChecked();

            var item = Items.Find(i => i.Producto.ProductoID == productoID);
            if (item != null)
            {
                Items.Remove(item);
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Carrito WHERE ProductoID = @ProductoID AND OwnerId = @OwnerId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductoID", productoID);
                        command.Parameters.AddWithValue("@OwnerId", OwnerId ?? string.Empty);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ActualizarCantidad(int productoID, int nuevaCantidad)
        {
            EnsureSchemaChecked();

            if (nuevaCantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa");

            var item = Items.Find(i => i.Producto.ProductoID == productoID);
            if (item != null)
            {
                item.Cantidad = nuevaCantidad;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (nuevaCantidad == 0)
                    {
                        string query = "DELETE FROM Carrito WHERE ProductoID = @ProductoID AND OwnerId = @OwnerId";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductoID", productoID);
                            command.Parameters.AddWithValue("@OwnerId", OwnerId ?? string.Empty);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string query = "UPDATE Carrito SET Cantidad = @Cantidad WHERE ProductoID = @ProductoID AND OwnerId = @OwnerId";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cantidad", nuevaCantidad);
                            command.Parameters.AddWithValue("@ProductoID", productoID);
                            command.Parameters.AddWithValue("@OwnerId", OwnerId ?? string.Empty);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public decimal CalcularTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Producto.Precio * item.Cantidad;
            }
            return total;
        }
    }
}
