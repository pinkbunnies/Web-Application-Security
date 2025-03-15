using Microsoft.AspNetCore.Mvc;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            // Crear dos productos
            Producto producto1 = new Producto(1, "Portátil", 999.99m, "Electrónica");
            Producto producto2 = new Producto(2, "Smartphone", 599.99m, "Electrónica");

            // Crear el carrito y agregar productos con cantidad 1 por defecto
            Carrito carrito = new Carrito();
            carrito.AgregarProducto(producto1, 1);
            carrito.AgregarProducto(producto2, 1);

            // Eliminar el primer producto
            carrito.EliminarProducto(1);

            // Calcular total y obtener los items del carrito (usando Items en lugar de ListaProductos)
            var total = carrito.CalcularTotal();
            var items = carrito.Items;

            ViewBag.Total = total;
            return View(items);
        }
    }
}
