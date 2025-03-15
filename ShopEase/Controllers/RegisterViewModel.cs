using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        public string Apellidos { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
