using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Pages.Forms
{
    public class UpdateEmployeeModel : PageModel
    {
        public readonly IEmployeeService _employeeService;
        public UpdateEmployeeModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            //if (!ModelState.IsValid) return Page();
            try
            {
                await _employeeService.Update(Employee);
            }
            catch (Exception e)
            {
                throw new Exception("Exception occured on PUT method to CosmosDB " + e.StackTrace);
            }
            return RedirectToPage("/Index");
        }
    }
}
