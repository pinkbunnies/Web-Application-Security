using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ShopApp.Models;
using System.Security.Claims;
using System.Collections.Concurrent;

namespace ShopApp.Controllers
{
    // Asumimos que BaseController ya está implementado para mostrar la vista de login si el usuario no está autenticado.
    public class CarritoController : BaseController
    {
        // Diccionario que almacena un carrito por cada usuario (clave = User ID)
        private static ConcurrentDictionary<string, Carrito> userCarritos = new ConcurrentDictionary<string, Carrito>();

        // Método privado (no estático) para obtener o crear el carrito del usuario autenticado
        private Carrito GetCurrentUserCarrito()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userCarritos.GetOrAdd(userId, id =>
            {
                var cart = new Carrito();
                cart.OwnerId = id;
                return cart;
            });
        }

        // Método público estático para obtener el carrito del usuario actual desde cualquier controlador
        public static Carrito GetUserCarrito(ClaimsPrincipal user)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userCarritos.GetOrAdd(userId, id =>
            {
                var cart = new Carrito();
                cart.OwnerId = id;
                return cart;
            });
        }

        // Acción para agregar un producto al carrito
        public IActionResult Agregar(int productoID, int cantidad)
        {
            var carrito = GetCurrentUserCarrito();
            Producto? producto = ObtenerProducto(productoID);
            if (producto != null)
            {
                carrito.AgregarProducto(producto, cantidad);
            }
            return RedirectToAction("Index");
        }

        // Acción para eliminar un producto del carrito
        public IActionResult Eliminar(int productoID)
        {
            var carrito = GetCurrentUserCarrito();
            carrito.EliminarProducto(productoID);
            return RedirectToAction("Index");
        }

        // Acción para actualizar la cantidad de un producto en el carrito
        public IActionResult Actualizar(int productoID, int cantidad)
        {
            var carrito = GetCurrentUserCarrito();
            carrito.ActualizarCantidad(productoID, cantidad);
            return RedirectToAction("Index");
        }

        // Acción para mostrar el contenido del carrito
        public IActionResult Index()
        {
            var carrito = GetCurrentUserCarrito();
            ViewBag.Total = carrito.CalcularTotal();
            return View(carrito.Items);
        }

        // Método auxiliar para obtener un producto desde la tabla Productos
        private Producto? ObtenerProducto(int productoID)
        {
            Producto? producto = null;
            string connectionString = "server=localhost;database=shop;user=root;password=Necrobite32!";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID, Nombre, Precio, Categoria FROM Productos WHERE ProductoID = @ProductoID";
                using (var command = new MySqlCommand(query, connection))
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
