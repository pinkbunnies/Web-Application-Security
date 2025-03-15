namespace ShopApp.Models
{
    public class LineaCarrito
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public LineaCarrito(Producto producto, int cantidad)
        {
            Producto = producto;
            Cantidad = cantidad;
        }
    }
}
