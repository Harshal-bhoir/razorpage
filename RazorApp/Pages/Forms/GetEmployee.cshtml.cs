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
    public class GetEmployeeModel : PageModel
    {
        public readonly IEmployeeService _employeeService;
        public GetEmployeeModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public List<EmployeeModel> Employees { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var sqlCosmosQuery = "Select * from c";
            var result = await _employeeService.Get(sqlCosmosQuery);
            Employees = result;
            return Page();
        }
    }
}
