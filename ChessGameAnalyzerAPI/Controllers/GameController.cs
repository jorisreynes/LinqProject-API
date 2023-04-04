using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace ChessGame_AnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        [HttpGet]
        //public async Task<ActionResult<List<Game>>> GetGames()
        public string GetGames()
        
        {
            
           
            string fileName = $@"../DataSource/Text/data.txt";

          
            
            using(StreamReader file = new StreamReader(fileName)) {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null) {
                    Console.WriteLine(ln);
                    counter++;
                }
                file.Close();
                return $"File has {counter} lines.";
                
            }
            
            
            
            
            
            
            
            // return new List<Game>
            // {
            //     new Game
            //     {
            //         WhitePlayer = "whiteplayer",
            //         BlackPlayer = "blackplayer",
            //         Result = "1-0",
            //         Moves = "1. e4"
            //
            //     }
            // };
        }
    }
}

