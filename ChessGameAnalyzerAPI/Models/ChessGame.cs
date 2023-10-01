﻿namespace ChessGame_AnalyzerAPI.Models;

public class ChessGame
{
    public string Event { get; set; }
    public string Site { get; set; }
    public string Date { get; set; }
    public string Round { get; set; }
    public string White { get; set; }
    public string Black { get; set; }
    public string Result { get; set; }
    public int WhiteElo { get; set; }
    public int BlackElo { get; set; }
    public string TimeControl { get; set; }
    public string EndTime { get; set; }
    public string Termination { get; set; }
    public string Moves { get; set; }

}

