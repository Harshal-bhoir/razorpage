using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Pages.Forms;

public class GenericUpdateModel : PageModel
{
    public readonly IGenericService _genericService;
    public GenericUpdateModel(IGenericService genericService)
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
        try
        {
            Employee.Id = "";
            await _genericService.Update(Employee, "select * from c where c.EmpId=", Employee.EmpId);
        }
        catch (Exception e)
        {
            throw new Exception("Exception occured on PUT method to CosmosDB " + e.StackTrace);
        }
        return RedirectToPage("/Index");
    }
}
