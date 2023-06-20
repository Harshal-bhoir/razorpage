using System;
using System.ComponentModel.DataAnnotations;

namespace RazorApp.Models;

public class ImageUploadModel
{
        [Required(ErrorMessage = "Please select file")]
        public IFormFile imageFile { get; set; }
}

