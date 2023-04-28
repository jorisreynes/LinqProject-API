using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ChessGame_AnalyzerAPI.Controllers;

public class UploadController : Controller
{
    // function to save a file uploaded by the front in angular
    [HttpPost]
    [Route("api/upload")]
    public void UploadFile(IFormFile file)
    {
            

        var path = Path.Combine(
            Directory.GetCurrentDirectory(), "../../DataSource/Text",
            file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyToAsync(stream);
        }

            
    }
}