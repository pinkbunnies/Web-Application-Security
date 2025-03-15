using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ShopApp.Models;
using System.Collections.Generic;

namespace ShopApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string connectionString = "server=localhost;database=shop;user=root;password=Necrobite32!";

        public IActionResult Index()
        {
            List<Producto> productos = new List<Producto>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID, Nombre, Precio, Categoria, ImagenUrl FROM Productos";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto(
                                reader.GetInt32("ProductoID"),
                                reader.GetString("Nombre"),
                                reader.GetDecimal("Precio"),
                                reader.GetString("Categoria"),
                                reader.IsDBNull(reader.GetOrdinal("ImagenUrl")) ? null : reader.GetString("ImagenUrl")
                            ));
                        }
                    }
                }
            }

            return View(productos);
        }

        // Acción para mostrar la información detallada de un producto
        public IActionResult Detalle(int id)
        {
            Producto? producto = ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // Acción para agregar el producto al carrito desde la vista de detalle
        public IActionResult AgregarDetalle(int id)
        {
            Producto? producto = ObtenerProducto(id);
            if (producto != null)
            {
                // Se utiliza la instancia estática del carrito del CarritoController
                CarritoController.carrito.AgregarProducto(producto, 1);
                TempData["Alert"] = "Producto añadido al carrito.";
            }
            return RedirectToAction("Detalle", new { id = id });
        }

        // Método auxiliar para obtener un producto por ID desde la base de datos
        private Producto? ObtenerProducto(int id)
        {
            Producto? producto = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID, Nombre, Precio, Categoria, ImagenUrl FROM Productos WHERE ProductoID = @ProductoID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto(
                                reader.GetInt32("ProductoID"),
                                reader.GetString("Nombre"),
                                reader.GetDecimal("Precio"),
                                reader.GetString("Categoria"),
                                reader.IsDBNull(reader.GetOrdinal("ImagenUrl")) ? null : reader.GetString("ImagenUrl")
                            );
                        }
                    }
                }
            }
            return producto;
        }
    }
}
