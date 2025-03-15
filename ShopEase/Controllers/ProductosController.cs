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
                string query = "SELECT ProductoID, Nombre, Precio, Categoria FROM Productos";
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
                                reader.GetString("Categoria")
                            ));
                        }
                    }
                }
            }

            return View(productos);
        }
    }
}
