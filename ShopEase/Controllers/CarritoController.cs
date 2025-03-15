using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class CarritoController : Controller
    {
        // Instancia estática para fines de prueba
        public static Carrito carrito = new Carrito();

        // Acción para agregar producto al carrito
        public IActionResult Agregar(int productoID, int cantidad)
        {
            Producto? producto = ObtenerProducto(productoID);
            if (producto != null)
            {
                carrito.AgregarProducto(producto, cantidad);
            }
            return RedirectToAction("Index");
        }

        // Acción para eliminar producto del carrito
        public IActionResult Eliminar(int productoID)
        {
            carrito.EliminarProducto(productoID);
            return RedirectToAction("Index");
        }

        // Acción para actualizar la cantidad de un producto en el carrito
        public IActionResult Actualizar(int productoID, int cantidad)
        {
            carrito.ActualizarCantidad(productoID, cantidad);
            return RedirectToAction("Index");
        }

        // Acción para mostrar el carrito
        public IActionResult Index()
        {
            ViewBag.Total = carrito.CalcularTotal();
            return View(carrito.Items);
        }

        // Método auxiliar para obtener un producto desde la tabla Productos
        private Producto? ObtenerProducto(int productoID)
        {
            Producto? producto = null;
            string connectionString = "server=localhost;database=shop;user=root;password=Necrobite32!";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID, Nombre, Precio, Categoria FROM Productos WHERE ProductoID = @ProductoID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoID", productoID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto(
                                reader.GetInt32("ProductoID"),
                                reader.GetString("Nombre"),
                                reader.GetDecimal("Precio"),
                                reader.GetString("Categoria")
                            );
                        }
                    }
                }
            }
            return producto;
        }
    }
}
