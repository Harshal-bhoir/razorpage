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
            List<EmployeeModel> result = await _employeeService.Get(sqlCosmosQuery);
            //if(result.Count < 1)
            //{
            //    throw new Exception("Exception thrown on GET method to CosmosDB ");
            //}
            Employees = result;
            return Page();
        }
    }
}
