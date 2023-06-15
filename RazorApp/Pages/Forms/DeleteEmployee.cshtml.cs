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
    public class DeleteEmployeeModel : PageModel
    {
        public readonly IEmployeeService _employeeService;
        public DeleteEmployeeModel(IEmployeeService employeeService)
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
            await _employeeService.Delete(Employee.EmpId.ToString());
            return RedirectToPage("/Index");
        }
    }
}
