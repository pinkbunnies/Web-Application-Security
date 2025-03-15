using System;

namespace ShopApp.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }

        public Producto(int productoID, string nombre, decimal precio, string categoria)
        {
            ProductoID = productoID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Precio = precio;
            Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
        }

        public void ImprimirDetalles()
        {
            Console.WriteLine($"Producto: {Nombre} | Precio: ${Precio:F2} | Categoría: {Categoria}");
        }
    }
}
