using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Services;

using RazorApp.Models;

namespace RazorApp.Pages.Forms;

public class AddEmployeeModel : PageModel
{
    public readonly IEmployeeService _employeeService;
    public AddEmployeeModel(IEmployeeService employeeService)
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
        if (!ModelState.IsValid) RedirectToPage("/Index");
        Employee.Id = Guid.NewGuid().ToString();
        var result = await _employeeService.AddAsync(Employee);
        return RedirectToPage("/Index");
    }
}
