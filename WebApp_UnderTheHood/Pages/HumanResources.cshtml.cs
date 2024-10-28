using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "MustBelongToHRDepartment")]
    public class HumanResourcesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
