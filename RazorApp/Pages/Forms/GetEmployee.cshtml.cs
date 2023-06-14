using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RazorApp.Models;

namespace RazorApp.Pages.Forms
{
    public class GetEmployeeModel : PageModel
    {
        [BindProperty]
        public EmployeeModel Employee { get; set; }

        public void OnGet()
        {

        }
    }
}
