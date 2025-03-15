using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShopApp.Models
{
    public class Carrito
    {
        public List<LineaCarrito> Items { get; set; }
        private readonly string connectionString = "server=localhost;database=shop;user=root;password=Necrobite32!";

        public Carrito()
        {
            Items = new List<LineaCarrito>();
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que 0");

            // Verificar si ya existe el producto en el carrito
            var itemExistente = Items.Find(i => i.Producto.ProductoID == producto.ProductoID);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                Items.Add(new LineaCarrito(producto, cantidad));
            }

            // Guardar en la base de datos usando ON DUPLICATE KEY UPDATE para actualizar la cantidad
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Carrito (ProductoID, Nombre, Precio, Categoria, Cantidad)
                    VALUES (@ProductoID, @Nombre, @Precio, @Categoria, @Cantidad)
                    ON DUPLICATE KEY UPDATE 
                        Cantidad = Cantidad + @Cantidad";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoID", producto.ProductoID);
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int productoID)
        {
            var item = Items.Find(i => i.Producto.ProductoID == productoID);
            if (item != null)
            {
                Items.Remove(item);
                // Eliminar de la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Carrito WHERE ProductoID = @ProductoID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductoID", productoID);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ActualizarCantidad(int productoID, int nuevaCantidad)
        {
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
                        // Si la cantidad es 0, se elimina el producto
                        string query = "DELETE FROM Carrito WHERE ProductoID = @ProductoID";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductoID", productoID);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string query = "UPDATE Carrito SET Cantidad = @Cantidad WHERE ProductoID = @ProductoID";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cantidad", nuevaCantidad);
                            command.Parameters.AddWithValue("@ProductoID", productoID);
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
