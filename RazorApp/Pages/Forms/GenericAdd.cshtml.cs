using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Services;

using RazorApp.Models;

namespace RazorApp.Pages.Forms;

public class GenericAddModel : PageModel
{
    public readonly IGenericService _genericService;
    public GenericAddModel(IGenericService genericService)
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
        if (!ModelState.IsValid) RedirectToPage("/Index");
        Employee.Id = Guid.NewGuid().ToString();
        try
        {
            var result = await _genericService.AddAsync(Employee, Employee.EmpId);
        }
        catch (Exception e)
        {
            throw new Exception("Exception occured at POST method to CosmosDB " + e.StackTrace);
        }
        return RedirectToPage("/Index");
    }
}
