using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using WebApp.Data.Account;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        public LoginModel(SignInManager<User> signInManager) 
        {
            this.signInManager = signInManager;
        }
        [BindProperty] //gives 2-way data binding view
        //...helps communicate between razor page and page model class
        public CredentialViewModel Credential { get; set; } = new CredentialViewModel(); //must initialize
        public SignInManager<User> SignInManager { get; }

        [BindProperty]
        public IEnumerable<AuthenticationScheme> ExternalLoginProviders { get; set; }
        public async Task OnGetAsync()
        {
            this.ExternalLoginProviders = await signInManager.GetExternalAuthenticationSchemesAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await signInManager.PasswordSignInAsync(
                this.Credential.Email,
                this.Credential.Password,
                this.Credential.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else 
            {
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("/Account/LoginTwoFactorWithAuthenticator",
                        new {
                            //Email = this.Credential.Email,
                            this.Credential.RememberMe
                        });
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "You are logged out.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to login.");
                }

                return Page();
            }
        }

        public IActionResult OnPostLoginExternally(string provider)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, null);
            properties.RedirectUri = Url.Action("ExternalLoginCallback", "Account");
            return Challenge(properties, provider);
        }

    }

    public class CredentialViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty; //we assign an initial value here because we will always have values here and they can't be null references
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty; //could have also done "string?"
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
