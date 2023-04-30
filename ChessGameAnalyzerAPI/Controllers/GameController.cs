﻿using System;
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
        public GamesResult GetGames(string opening)
        {
            // To do: create a function to find automatically the nickname of the player
            pseudo = "BleepBleepBlop";

            // Path of file Chess.com
            string filePathTxt = $@"../../DataSource/Text/data.txt";
            
            // To Do : Use variables to store the path of the files
            //string filePathXML = $@"../DataSource/XML/data.txt";
            //string filePathJSON = $@"../DataSource/JSON/data.txt";

            string firstMoves = "";

            switch (opening)
            {
                case "Scotch":
                    firstMoves = "1. e4 e5 2. Nf3 Nc6 3. d4";
                    break;
                case "Spanish":
                    firstMoves = "1. e4 e5 2. Nf3 Nc6 3. Bb5";
                    break;
                case "Italian":
                    firstMoves = "1. e4 e5 2. Nf3 Nc6 3. Bc4";
                    break;
                case "Alekhine":
                    firstMoves = "1. e4 Cf6";
                    break;
                case "Sicilian":
                    firstMoves = "1. e4 c5";
                    break;
                case "Caro-kann":
                    firstMoves = "1. e4 c6";
                    break;
                case "Slav":
                    firstMoves = "1. d4 d5 2. c4 c6";
                    break;
                case"Petrov":
                    firstMoves = "1. e4 e5 2. Nf3 Nc6";
                    break;
                case "Scandinavian":
                    firstMoves = "1. e4 d5";
                    break;
                case "e4":
                    firstMoves = "1. e4";
                    break;
                case "d4":
                    firstMoves = "1. d4";
                    break;
            }
            
            // We create a list avec all the games in the file from Chess.com
            List<ChessGame> games = new List<ChessGame>();
            using (StreamReader reader = new StreamReader(filePathTxt))
            {
                string line;
                ChessGame currentGame = null;
                while ((line = reader.ReadLine()) != null)
                {
                    // If the line starts with Event we create a new object ChessGame
                    if (line.StartsWith("[Event "))
                    {
                        currentGame = new ChessGame();
                        games.Add(currentGame);
                    }

                    // We store the data in the object
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
                        currentGame.Moves += line + " ";
                    }
                }
            }

            // We create a list with all the games that contains the opening using LINQ
            List<ChessGame> filteredGames = games.Where(g => g.Moves.Contains(firstMoves)).ToList();
            


            // We save the games in a new XML file
            printXML(games);
            
            // We save the games in a new JSON file
            printJSON(games);
            
            // We create a new GamesResult
            GamesResult gamesResult = new GamesResult();
            
            // We score the games
            foreach (ChessGame game in filteredGames)
            {
                if (game.Result == "1-0" && game.White == pseudo)
                {
                    gamesResult.numberOfGamesWon++;
                }
                else if (game.Result == "1/2-1/2")
                {
                    gamesResult.numberOfGamesDrawn++;
                }
                else if (game.Result == "0-1" && game.Black == pseudo)
                {
                    gamesResult.numberOfGamesLost++;
                }
            }
            return gamesResult;
        }
        
        
        
       
        
        // Save the collection games in XML
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
            System.IO.File.WriteAllText(@"../../DataSource/XML/data.xml", XML.ToString());
        }
        
        // Function to save the collection games in JSON
        static void printJSON(List<ChessGame> games)
        {
            string json = JsonSerializer.Serialize(games);
            System.IO.File.WriteAllText(@"../../DataSource/JSON/data.json", json);
        }
    }
}