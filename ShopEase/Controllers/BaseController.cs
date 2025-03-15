using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Obtenemos el nombre del controlador desde la ruta
            var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "";
            
            // Si no es el AccountController (que maneja login/registro)
            if (!controllerName.Equals("Account", System.StringComparison.OrdinalIgnoreCase))
            {
                // Si el usuario no está autenticado...
                if (User.Identity is null || !User.Identity.IsAuthenticated)
                {
                    // Creamos un ViewDataDictionary para el LoginViewModel
                    var viewData = new ViewDataDictionary<LoginViewModel>(this.ViewData, new LoginViewModel());
                    
                    // Asignamos el resultado a la vista de Login especificando la ruta completa
                    context.Result = new ViewResult
                    {
                        ViewName = "~/Views/Account/Login.cshtml",
                        ViewData = viewData
                    };
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
