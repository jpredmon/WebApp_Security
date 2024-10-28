using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using WebApp_UnderTheHood.Authorization;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty] //gives 2-way data binding view
        //...helps communicate between razor page and page model class
        public Credential Credential { get; set; } = new Credential(); //must initialize
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Verify the Credential
            if (Credential.UserName == "admin" && Credential.Password == "password")
            {
                //Create the security Context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2024-01-01")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                //the principal contains the security context
                //below serializes the claimsPrincipal into a string
                //then encrypts that string and saves that as a cookie in the HttpContext Object

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                //if everything is good, we redirect to a page:
                return RedirectToPage("/Index");

            }

            return Page();
        }

       
    }
}
