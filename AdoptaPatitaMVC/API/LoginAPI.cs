using System.ComponentModel.DataAnnotations;
public class LoginAPI{
    
    [Required(ErrorMessage = "Email es obligatorio")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password es obligatoria")]
    public string Password { get; set; }
}