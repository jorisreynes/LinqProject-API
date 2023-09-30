using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        //formatMoves(filePath);
    }

    //private static void formatMoves(string filePath)
    //{
    //    string rawLine = string.Empty;

    //    using (StreamReader reader = new StreamReader(filePath))
    //    {
    //        string line;

    //        while ((line = reader.ReadLine()) != null)
    //        {
    //            if (line.StartsWith("[")) { }
    //            else if (!string.IsNullOrWhiteSpace(line))
    //            {
    //                rawLine += line + " ";
    //            }
    //        }

    //        // Utiliser une expression régulière pour supprimer le contenu entre les accolades
    //        string cleanedString = Regex.Replace(rawLine, @"\{[^}]+\}", "");

    //        // Utiliser une autre expression régulière pour obtenir uniquement les mouvements d'échecs
    //        string movesOnly = Regex.Replace(cleanedString, @"\d+\.\s*([^0-9]+)\s*", "$1 ");
    //    }
    //}
}