using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using VirtualGroupEx.Server.Services;

namespace VirtualGroupEx.Server.Pages
{
    public class DownloadModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly CurrentUserService currentUser;
        private readonly UploadService uploadService;

        public DownloadModel(IConfiguration configuration, CurrentUserService currentUser, UploadService uploadService)
        {
            this.configuration = configuration;
            this.currentUser = currentUser;
            this.uploadService = uploadService;
        }

        public IActionResult OnGet(string hash, string name, string mid)
        {

            if (string.IsNullOrEmpty(hash) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(mid) && currentUser.User.IsOperator)
            {
                return Content(uploadService.GetFileListJson());
            }

            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mid))
            {
                return BadRequest();
            }

            var filePath = Path.Combine(configuration.GetValue<string>("UploadRootPath"), mid, hash);

            try
            {
                //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                //return File(fileBytes, "application/force-download", name);

                FileStream fs = new(filePath, FileMode.Open);

                return File(fs, "application/octet-stream", name);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
