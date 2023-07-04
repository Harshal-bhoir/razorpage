using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.EnvConfig;

namespace RazorApp.Pages.Forms
{
    public class EnvDisplayPageModel : PageModel
    {
        private readonly IAppConfig _appconfig;
        public string Message { get; set; }

        public EnvDisplayPageModel(IAppConfig appconfig)
        {
            _appconfig = appconfig;
        }
        public void OnGet()
        {
            var result = _appconfig.GetTestValue();
            Message = result.ToString();
        }
    }
}
