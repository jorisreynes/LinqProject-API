using System;
namespace ChessGame_AnalyzerAPI
{
	public class Game
	{
		public int Id {get; set;}
		public string WhitePlayer {get; set;} = string.Empty;
		public string BlackPlayer {get; set;} = string.Empty;
		public string Result {get; set;} = string.Empty;
		
		public string Moves {get; set;} = string.Empty;


	}
}

