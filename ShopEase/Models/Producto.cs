using System;

namespace ShopApp.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string? ImagenUrl { get; set; }  // Nueva propiedad para la imagen

        public Producto(int productoID, string nombre, decimal precio, string categoria, string? imagenUrl = null)
        {
            ProductoID = productoID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Precio = precio;
            Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
            ImagenUrl = imagenUrl;
        }

        public void ImprimirDetalles()
        {
            Console.WriteLine($"Producto: {Nombre} | Precio: ${Precio:F2} | Categoría: {Categoria}");
        }
    }
}
