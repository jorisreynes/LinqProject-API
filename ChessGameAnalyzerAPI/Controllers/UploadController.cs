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
    string folderPath = "../../DataSource/Text";
    
    // function to save a file uploaded by the front in angular
    [HttpPost]
    [Route("api/upload")]
    public async Task UploadFile(IFormFile file)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderPath, file.FileName);

        // If the file already exists, we delete it
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }  
    }
}