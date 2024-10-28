using System.ComponentModel.DataAnnotations;

namespace WebApp_UnderTheHood.Authorization
{
    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty; //we assign an initial value here because we will always have values here and they can't be null references
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; //could have also done "string?"
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
