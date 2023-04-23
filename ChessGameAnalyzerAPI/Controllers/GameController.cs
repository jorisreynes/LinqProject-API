using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;


namespace ChessGame_AnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        public string pseudo { get; set; }
        
        [HttpGet]
        public GamesResult GetGames()
        {
            // A Faire: créer une fonction qui trouve elle meme le pseudo en fonction de ce qui revient le plus souvent 
            pseudo = "BleepBleepBlop";

            // Chemin du fichier exporté de Chess.com
            string filePathTxt = $@"../DataSource/Text/data.txt";
            
            // A faire: mettre les chemins dans des variables
            string filePathXML = $@"../DataSource/XML/data.txt";


            // On créé une liste avec les parties du fichier texte de chesscom
            List<ChessGame> games = new List<ChessGame>();
            using (StreamReader reader = new StreamReader(filePathTxt))
            {
                string line;
                ChessGame currentGame = null;
                while ((line = reader.ReadLine()) != null)
                {
                    // Si la ligne commence par event on créé un nouvel objet
                    if (line.StartsWith("[Event "))
                    {
                        currentGame = new ChessGame();
                        games.Add(currentGame);
                    }

                    // on alimente les propriétés de l'objet
                    if (line.StartsWith("["))
                    {
                        string key = line.Substring(1, line.IndexOf(' ') - 1);
                        string value = line.Substring(line.IndexOf('"') + 1,
                            line.LastIndexOf('"') - line.IndexOf('"') - 1);

                        switch (key)
                        {
                            case "Event":
                                currentGame.Event = value;
                                break;
                            case "Site":
                                currentGame.Site = value;
                                break;
                            case "Date":
                                currentGame.Date = value;
                                break;
                            case "Round":
                                currentGame.Round = value;
                                break;
                            case "White":
                                currentGame.White = value;
                                break;
                            case "Black":
                                currentGame.Black = value;
                                break;
                            case "Result":
                                currentGame.Result = value;
                                break;
                            case "WhiteElo":
                                currentGame.WhiteElo = int.Parse(value);
                                break;
                            case "BlackElo":
                                currentGame.BlackElo = int.Parse(value);
                                break;
                            case "TimeControl":
                                currentGame.TimeControl = value;
                                break;
                            case "EndTime":
                                currentGame.EndTime = value;
                                break;
                            case "Termination":
                                currentGame.Termination = value;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        currentGame.Moves = line.Split(' ').ToList();
                    }
                }
            }
            
           // on appelle la fonction printxml pour enregistrer la collection games en XML
            printXML(games);
            
            // on appelle la fonction printjson pour enregistrer la collection games en JSON
            printJSON(games);
            
            //on créé une nouvelle instance de GamesResult
            GamesResult gamesResult = new GamesResult();
            
            // On compte les parties gagnees perdues ou nulles
            foreach (ChessGame game in games)
            {
                if (game.Result == "1-0" && game.White == pseudo)
                {
                    gamesResult.numberOgGamesWonWithWhite++;
                }
                else if (game.Result == "1/2-1/2" && game.White == pseudo)
                {
                    gamesResult.numberOfGamesDrawnWithWhite++;
                }
                else if (game.Result == "0-1" && game.White == pseudo)
                {
                    gamesResult.numberOfGamesLostWithWhite++;
                }
                else if (game.Result == "1-0" && game.Black == pseudo)
                {
                    gamesResult.numberOfGamesWonWithBlack++;
                }
                else if (game.Result == "1/2-1/2" && game.Black == pseudo)
                {
                    gamesResult.numberOfGamesDrawnWithBlack++;
                }
                else if (game.Result == "0-1" && game.Black == pseudo)
                {
                    gamesResult.numberOfGamesLostWithBlack++;
                }
            }
            return gamesResult;
        }
        
        // Fonction pour enregistrer la collection games en XML
        static void printXML(List<ChessGame> games)
        {
            var XML = new XElement("ChessGame",
                from game in games
                select new XElement("Game",
                    new XElement("Event", game.Event),
                    new XElement("Site", game.Site),
                    new XElement("Date", game.Date),
                    new XElement("Round", game.Round),
                    new XElement("White", game.White),
                    new XElement("Black", game.Black),
                    new XElement("Result", game.Result),
                    new XElement("WhiteElo", game.WhiteElo),
                    new XElement("BlackElo", game.BlackElo),
                    new XElement("TimeControl", game.TimeControl),
                    new XElement("EndTime", game.EndTime),
                    new XElement("Termination", game.Termination),
                    new XElement("Moves", game.Moves)
                    )
                );
            System.IO.File.WriteAllText(@"../DataSource/XML/data.xml", XML.ToString());
        }
        
        // Fonction pour enregistrer la collection games en JSON
        static void printJSON(List<ChessGame> games)
        {
            string json = JsonSerializer.Serialize(games);
            System.IO.File.WriteAllText(@"../DataSource/JSON/data.json", json);
        }
    }
}