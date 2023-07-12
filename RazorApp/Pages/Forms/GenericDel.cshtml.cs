using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Pages.Forms;

public class GenericDelModel : PageModel
{
    public readonly IGenericService _genericService;
    public GenericDelModel(IGenericService genericService)
    {
        _genericService = genericService;
    }

    [BindProperty]
    public EmployeeModel Employee { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        //if (!ModelState.IsValid) return Page();
        string query = "select * from c where c.EmpId=";
        try
        {
            await _genericService.Delete<EmployeeModel>(query, Employee.EmpId.ToString());
        }
        catch (Exception e)
        {
            throw new Exception("Exception occured on Delete Employee POST Request " + e.StackTrace);
        }
        return RedirectToPage("/Index");
    }
}
