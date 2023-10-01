using ChessGame_AnalyzerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace ChessGame_AnalyzerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : Controller
{
    // Files paths 
    private const string FilePathTxt = $@"../../DataSource/Text/data.txt";
    //private const string FilePathXml = $@"../../ChessGameAnalyzer.UI/src/assets/data.xml";
    //private const string FilePathJson = $@"../../ChessGameAnalyzer.UI/src/assets/data.json";

    [HttpGet]
    public GamesResult GetGames(string? opening = "All openings", string? color = "All colors", string? endgame = "All end of game")
    {
        var emptyResult = new GamesResult();

        // Vérifier si le répertoire existe, sinon le créer
        if (!System.IO.File.Exists(FilePathTxt))
        {
            //System.IO.File.Create(FilePathTxt);
            return emptyResult;
        }
        // We create a list avec all the games in the file from Chess.com
        List<ChessGame> games = CreateGamesList();

        // We find the pseudo of the player
        string pseudo = FindPseudo(games);

        // We find the first moves of the game with the opening asked from the front
        string firstMoves = FindFirstMoves(opening);

        // We find the termination from the file with the endgame asked from the front
        string endgameExtractFromFile = FindEndgame(endgame);

        // We filter the games with the pseudo and the parameters from the front
        List<ChessGame> filteredGames = CreateFilteredGames(pseudo, opening, endgame, color, games);

        // We return the result to the front
        return ScoreGames(filteredGames, pseudo);
    }

    private static List<ChessGame> CreateGamesList()
    {
        // We create a list avec all the games in the file from Chess.com
        List<ChessGame> games = new List<ChessGame>();

        using (StreamReader reader = new StreamReader(FilePathTxt))
        {
            string line;
            ChessGame currentGame = null;
            while ((line = reader.ReadLine()) != null)
            {
                // If the line starts with Event we create a new object ChessGame
                if (line.StartsWith("[Event "))
                {
                    // Si ce n'est pas le premier game, ajoute-le à la liste
                    if (currentGame != null)
                    {
                        games.Add(currentGame);
                    }

                    currentGame = new ChessGame();
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

            // Ajoute le dernier jeu à la liste
            if (currentGame != null)
            {
                games.Add(currentGame);
            }
        }

        // Formate tous les jeux après la boucle
        foreach (var game in games)
        {
            game.Moves = formatMoves(game.Moves);
        }

        return games;
    }


    private static string formatMoves(string moves)
    {
        // Regex to delete what is inside {}
        string cleanedString = Regex.Replace(moves, @"\{[^}]+\}", "");

        string[] movesArray = cleanedString.Split(new string[] { " " }, StringSplitOptions.None);

        List<string> filteredMovesList = new List<string>();

        // We want to delete 1... 2... etc
        foreach (var move in movesArray)
        {
            if (!move.Contains("..."))
            {
                filteredMovesList.Add(move);
            }
        }

        string filteredMoves = string.Join(" ", filteredMovesList);

        string result = filteredMoves.Replace("  ", " ");

        return result;
    }

    // The function receive the name of the opening selected in the front, we search for the first moves to filter the games
    private static string FindFirstMoves(string opening)
    {
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
                firstMoves = "1. e4 Nf6";
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
            case "Petrov":
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
            case "All openings":
                firstMoves = "1.";
                break;
        }
        return firstMoves;
    }

    private static string FindPseudo(List<ChessGame> games)
    {
        // function to find the pseudo of the player we have the most in white and black
        List<string> pseudoList = new List<string>();
        foreach (ChessGame game in games)
        {
            pseudoList.Add(game.White);
            pseudoList.Add(game.Black);
        }
        // we find the pseudo that appears the most
        string pseudo = pseudoList.GroupBy(i => i).OrderByDescending(grp => grp.Count())
            .Select(grp => grp.Key).First();
        return pseudo;
    }

    private static string FindEndgame(string endgame)
    {
        string endgameExtractFromFile = string.Empty;
        switch (endgame)
        {
            case "Checkmate":
                endgameExtractFromFile = "échec et mat";
                break;
            case "Draw":
                endgameExtractFromFile = "nulle";
                break;
            case "Resignation":
                endgameExtractFromFile = "abandon";
                break;
            case "Time":
                endgameExtractFromFile = "temps";
                break;
            case "All end of game":
                endgameExtractFromFile = "";
                break;
        }
        return endgameExtractFromFile;
    }

    private static List<ChessGame> CreateFilteredGames(string pseudo, string opening, string endgame, string color, List<ChessGame> games)
    {
        // We create a list of games with the filters
        List<ChessGame> filteredGames = new List<ChessGame>();
        foreach (ChessGame game in games)
        {
            if (game.White == pseudo || game.Black == pseudo)
            {
                if (game.Moves.StartsWith(FindFirstMoves(opening)) && game.Termination.Contains(FindEndgame(endgame)))
                {
                    if (color == "All colors")
                    {
                        filteredGames.Add(game);
                    }
                    else if (color == "White" && game.White == pseudo)
                    {
                        filteredGames.Add(game);
                    }
                    else if (color == "Black" && game.Black == pseudo)
                    {
                        filteredGames.Add(game);
                    }
                }
            }
        }
        return filteredGames;
    }

    private static GamesResult ScoreGames(List<ChessGame> filteredGames, string pseudo)
    {
        GamesResult gamesResult = new GamesResult();
        // We score the games
        foreach (ChessGame game in filteredGames)
        {
            if (game.Result == "1-0" && game.White == pseudo)
            {
                gamesResult.NumberOfGamesWon++;
            }
            else if (game.Result == "1-0" && game.Black == pseudo)
            {
                gamesResult.NumberOfGamesLost++;
            }
            else if (game.Result == "1/2-1/2")
            {
                gamesResult.NumberOfGamesDrawn++;
            }
            else if (game.Result == "0-1" && game.Black == pseudo)
            {
                gamesResult.NumberOfGamesWon++;
            }
            else if (game.Result == "0-1" && game.White == pseudo)
            {
                gamesResult.NumberOfGamesLost++;
            }
        }
        return gamesResult;
    }
}