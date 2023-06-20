using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;

using RazorApp.Models;
using RazorApp.Services;

namespace RazorApp.Pages.Forms;

public class UploadImageModel : PageModel
{
    public readonly IAzBlobService _service;

    public UploadImageModel(IAzBlobService service)
    {
        _service = service;
    }

    public void OnGet()
    {

    }

    [BindProperty]
    public ImageUploadModel file { get; set; }

    public async Task<IActionResult> OnPost()
    {
        var response = await _service.uploadFile(file);
        return RedirectToPage("/Index");
    }
}
