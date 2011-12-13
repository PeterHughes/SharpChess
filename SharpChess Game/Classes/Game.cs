using System;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

namespace SharpChess
{
	public class Game
	{
		public enum enmStage
		{
			Opening
				,	Middle
						,	End
		}

		public delegate void delegatetypeGameEvent();

		public static event delegatetypeGameEvent BoardPositionChanged;
		public static event delegatetypeGameEvent GameSaved;
		public static event delegatetypeGameEvent GamePaused;
		public static event delegatetypeGameEvent GameResumed;
		public static event delegatetypeGameEvent SettingsUpdated;

		private static Player m_playerWhite;
		private static Player m_playerBlack;
		private static Player m_playerToPlay;
		private static int m_intTurnNo = 0;
		private static Moves m_movesHistory = new Moves();
		private static Moves m_movesRedoList = new Moves();
		private static Moves m_movesAnalysis = new Moves();
		private static string m_strFileName = "";
		private static string m_strBackupGamePath;
		private static bool m_blnShowThinking = false;
		private static bool m_blnEnablePondering = false;
		private static bool m_blnUseRandomOpeningMoves = true;
		private static bool m_blnCaptureMoveAnalysisData = false;
		private static int m_intDifficultyLevel = 1;
		private static int m_intMaximumSearchDepth = 1;
		private static int m_intClockMoves   = 40;
		private static TimeSpan m_tsnClockTime = new TimeSpan(0,5,0);
		private static TimeSpan m_tsnClockIncrementPerMove = new TimeSpan(0,0,0);
		private static TimeSpan m_tsnClockFixedTimePerMove = new TimeSpan(0,0,0);
		protected static string m_strFENStartPosition = "";
		private static int m_intFiftyMoveDrawBase = 0;
		private static int m_intThreadCounter = 0;
		private static bool m_blnIsInAnalyseMode = false;
        private static bool m_blnEditModeActive = false;
        public static Random random = new Random();


		static void Player_ReadyToMakeMove()
		{
			Move move = null;
			if (Game.PlayerToPlay.PrincipalVariation.Count>0)
			{
				move = Game.PlayerToPlay.PrincipalVariation[0];
			}
			else
			{
				throw new ApplicationException("Player_ReadToMakeMove: Principal Variation is empty.");
			}
			Game.MakeAMove_Internal(move.Name, move.Piece, move.To);
			SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		static Game()
		{
			HashTable.Initialise();
			HashTablePawnKing.Initialise();
			HashTableCheck.Initialise();

			m_playerWhite = new PlayerWhite();
			m_playerBlack = new PlayerBlack();
			m_playerToPlay = m_playerWhite;
			Board.EstablishHashKey();
			OpeningBookSimple.Initialise();

			Game.PlayerWhite.ReadyToMakeMove += new Player.delegatetypePlayerEvent(Player_ReadyToMakeMove);
			Game.PlayerBlack.ReadyToMakeMove += new Player.delegatetypePlayerEvent(Player_ReadyToMakeMove);

			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			if (registryKeySharpChess.GetValue("FileName")==null)
			{
				m_strFileName = "";
			}
			else
			{
				m_strFileName = registryKeySharpChess.GetValue("FileName").ToString();
			}

			if (registryKeySharpChess.GetValue("ShowThinking")==null)
			{
				m_blnShowThinking = true;
			}
			else
			{
				m_blnShowThinking = (registryKeySharpChess.GetValue("ShowThinking").ToString()=="1");
			}

			// Delete deprecated values
			if (registryKeySharpChess.GetValue("EnablePondering")!=null)
			{
				registryKeySharpChess.DeleteValue("EnablePondering");
			}
			if (registryKeySharpChess.GetValue("DisplayMoveAnalysisTree")!=null)
			{
				registryKeySharpChess.DeleteValue("DisplayMoveAnalysisTree");
			}
			if (registryKeySharpChess.GetValue("ClockMoves")!=null)
			{
				registryKeySharpChess.DeleteValue("ClockMoves");
			}
			if (registryKeySharpChess.GetValue("ClockMinutes")!=null)
			{
				registryKeySharpChess.DeleteValue("ClockMinutes");
			}

//			OpeningBook.BookConvert(Game.PlayerWhite);
		}

		public static void StartNormalGame()
		{
			Game.PlayerToPlay.Clock.Start();
			Game.ResumePondering();
		}

		public static void TerminateGame()
		{
			WinBoard.StopListener();

			SuspendPondering();
			Game.PlayerWhite.AbortThinking();
			Game.PlayerBlack.AbortThinking();

			RegistryKey registryKeySoftware =Registry.CurrentUser.OpenSubKey("Software",true);
			RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

			registryKeySharpChess.SetValue("FileName", m_strFileName);
			registryKeySharpChess.SetValue("ShowThinking", m_blnShowThinking ? "1" : "0");
		}

		public static string FileName
		{
			get { return m_strFileName=="" ? "New Game" : m_strFileName; }
		} 

		public static Moves MoveHistory
		{
			get { return m_movesHistory; }
		} 

		public static Moves MoveRedoList
		{
			get { return m_movesRedoList; }
		}

        public static bool EditModeActive
		{
            get { return m_blnEditModeActive; }
		} 

		public static Moves MoveAnalysis
		{
			get { return m_movesAnalysis; }
			set { m_movesAnalysis = value; }
		} 

		public static int TurnNo 
		{
			get	{ return m_intTurnNo; }
			set { m_intTurnNo = value; }
		}

		public static int MoveNo 
		{
			get	{ return m_intTurnNo>>1; }
		}

		public static int MaxMaterialCount
		{
			get { return 7; }
		}

		public static int LowestMaterialCount
		{
			get
			{
				int intWhiteMaterialCount = PlayerWhite.MaterialCount;
				int intBlackMaterialCount = PlayerBlack.MaterialCount;
				return intWhiteMaterialCount<intBlackMaterialCount ? intWhiteMaterialCount : intBlackMaterialCount;
			}
		}

		public static Game.enmStage Stage
		{
			get
			{
				if (LowestMaterialCount >= MaxMaterialCount)
				{
					return Game.enmStage.Opening;
				}
				else if (LowestMaterialCount <= 3)
				{
					return Game.enmStage.End;
				}
				return Game.enmStage.Middle;
			}
		}

		public static Player PlayerWhite
		{
			get	{ return m_playerWhite;	}
		}

		public static Player PlayerBlack
		{
			get	{ return m_playerBlack;	}
		}

		public static Player PlayerToPlay
		{
			get	{ return m_playerToPlay;	}
			set	{ m_playerToPlay = value;	}
		}

		public static uint AvailableMegaBytes
		{
			get
			{
				try 
				{
					//					PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"); 
					//					return ((uint)Math.Max(Convert.ToInt32(ramCounter.NextValue()) - 25, 16));
					return 16;
				}
				catch
				{
					return 16;
				}
			}
		}

		public static bool IsPaused
		{
			get {return !m_playerToPlay.Clock.IsTicking;}
		}

		public static bool ShowThinking 
		{
			get	{ return m_blnShowThinking;}
			set { m_blnShowThinking = value; }
		}

		public static bool UseRandomOpeningMoves
		{
			get { return m_blnUseRandomOpeningMoves; }
			set { m_blnUseRandomOpeningMoves = value; }
		}

		public static bool EnablePondering
		{
			get { return m_blnEnablePondering; }
			set { m_blnEnablePondering = value; }
		}

		public static bool CaptureMoveAnalysisData
		{
			get { return m_blnCaptureMoveAnalysisData; }
			set { m_blnCaptureMoveAnalysisData = value; }
		}

		public static int ClockMoves
		{
			get	{ return m_intClockMoves;}
			set { m_intClockMoves = value; }
		}

		public static TimeSpan ClockTime
		{
			get	{ return m_tsnClockTime;}
			set { m_tsnClockTime = value; }
		}

		public static TimeSpan ClockIncrementPerMove
		{
			get	{ return m_tsnClockIncrementPerMove;}
			set { m_tsnClockIncrementPerMove = value; }
		}

		public static TimeSpan ClockFixedTimePerMove
		{
			get	{ return m_tsnClockFixedTimePerMove;}
			set { m_tsnClockFixedTimePerMove = value; }
		}

		public static int DifficultyLevel
		{
			get	{ return m_intDifficultyLevel;}
			set { m_intDifficultyLevel = value; }
		}

		public static int MaximumSearchDepth
		{
			get	{ return m_intMaximumSearchDepth;}
			set { m_intMaximumSearchDepth = value; }
		}

		public static string BackupGamePath
		{
			get { return m_strBackupGamePath; }
			set { m_strBackupGamePath = value; }
		}

		public static void New()
		{
			New("");
		}

		public static void New(string strFEN)
		{
			SuspendPondering();

			New_Internal(strFEN);
			Game.SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		private static void New_Internal()
		{
			New_Internal("");
		}

		private static void New_Internal(string strFEN)
		{
			if (strFEN == "")
			{
				strFEN = FEN.GameStartPosition;
			}
            random = new Random(DateTime.Now.Millisecond);
			FEN.Validate(strFEN);
			HashTable.Clear();
			HashTablePawnKing.Clear();
			HashTableCheck.Clear();
			UndoAllMoves_Internal();
			m_movesRedoList.Clear();
			m_strFileName = "";
			FEN.SetBoardPosition(strFEN);
			m_playerWhite.Clock.Reset();
			m_playerBlack.Clock.Reset();
		}

		public static bool Load(string FileName)
		{
			SuspendPondering();

			bool blnSuccess = false;
			New_Internal();
			m_strFileName = FileName;
			blnSuccess = LoadGame(FileName);
			if (blnSuccess)
			{
				SaveBackup();
				SendBoardPositionChangeEvent();
				if (Game.IsPaused)
				{
					Game.ResumePlay();
				}
			}

			ResumePondering();

			return blnSuccess;
		}

		public static bool LoadBackup()
		{
			return LoadGame(m_strBackupGamePath);
		}

		private static bool LoadGame(string strFileName)
		{
			m_movesRedoList.Clear();
			XmlDocument xmldoc = new XmlDocument();
			try
			{
				xmldoc.Load(strFileName);
			}
			catch
			{
				return false;
			}

			XmlElement xmlnodeGame = ((XmlElement)xmldoc.SelectSingleNode("/Game"));

			if (xmlnodeGame.GetAttribute("FEN")!="")
			{
				New_Internal(xmlnodeGame.GetAttribute("FEN"));
			}
			if (xmlnodeGame.GetAttribute("WhitePlayer")!="")
			{
				Game.PlayerWhite.Intellegence = (xmlnodeGame.GetAttribute("WhitePlayer")=="Human" ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer ) ;
			}
			if (xmlnodeGame.GetAttribute("BlackPlayer")!="")
			{
				Game.PlayerBlack.Intellegence = (xmlnodeGame.GetAttribute("BlackPlayer")=="Human" ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer ) ;
			}
			if (xmlnodeGame.GetAttribute("BoardOrientation")!="")
			{
				Board.Orientation = ( xmlnodeGame.GetAttribute("BoardOrientation")=="White" ? Board.enmOrientation.White : Board.enmOrientation.Black );
			}
			if (xmlnodeGame.GetAttribute("DifficultyLevel")!="")
			{
				Game.DifficultyLevel = int.Parse(xmlnodeGame.GetAttribute("DifficultyLevel"));
			}
			if (xmlnodeGame.GetAttribute("ClockMoves")!="")
			{
				Game.ClockMoves = int.Parse(xmlnodeGame.GetAttribute("ClockMoves"));
			}
			if (xmlnodeGame.GetAttribute("ClockMinutes")!="")
			{
				Game.ClockTime = new TimeSpan(0, int.Parse(xmlnodeGame.GetAttribute("ClockMinutes")), 0);
			}
			if (xmlnodeGame.GetAttribute("ClockSeconds")!="")
			{
				Game.ClockTime = new TimeSpan(0, 0, int.Parse(xmlnodeGame.GetAttribute("ClockSeconds")));
			}
			if (xmlnodeGame.GetAttribute("MaximumSearchDepth")!="")
			{
				Game.MaximumSearchDepth = int.Parse(xmlnodeGame.GetAttribute("MaximumSearchDepth"));
			}
			if (xmlnodeGame.GetAttribute("Pondering")!="")
			{
				Game.EnablePondering = (xmlnodeGame.GetAttribute("Pondering")=="1");
			}
			if (xmlnodeGame.GetAttribute("UseRandomOpeningMoves")!="")
			{
				Game.UseRandomOpeningMoves = (xmlnodeGame.GetAttribute("UseRandomOpeningMoves")=="1");
			}

			XmlNodeList xmlnodelist;
			xmlnodelist = xmldoc.SelectNodes("/Game/Move");

			Square from;
			Square to;

			TimeSpan tsnTimeStamp;
			foreach (XmlElement xmlnode in xmlnodelist)
			{
				if (xmlnode.GetAttribute("FromFile")!="")
				{
					from = Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank")));
					to = Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank")));
				}
				else
				{
					from = Board.GetSquare(xmlnode.GetAttribute("From"));
					to = Board.GetSquare(xmlnode.GetAttribute("To"));
				}

				MakeAMove_Internal( Move.MoveNameFromString(xmlnode.GetAttribute("Name")), from.Piece, to );
				if (xmlnode.GetAttribute("SecondsElapsed")=="")
				{
					if (m_movesHistory.Count<=2)
					{
						tsnTimeStamp = (new TimeSpan(0));
					}
					else
					{
						tsnTimeStamp = ( m_movesHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0,0,30)) );
					}
				}
				else
				{
					tsnTimeStamp = new TimeSpan(0,0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
				}
				m_movesHistory.Last.TimeStamp = tsnTimeStamp;
				m_movesHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
			}

			int intTurnNo = xmlnodeGame.GetAttribute("TurnNo")!="" ? int.Parse(xmlnodeGame.GetAttribute("TurnNo")) : xmlnodelist.Count;

			for (int intIndex=xmlnodelist.Count; intIndex>intTurnNo; intIndex--)
			{
				Game.UndoMove_Internal();
			}
			return true;
		}

		public static void Save(string FileName)
		{
			SuspendPondering();

			SaveBackup();
			SaveGame(FileName);
			m_strFileName = FileName;

			GameSaved();

			ResumePondering();
		}

		private static void SaveBackup()
		{
			SaveGame(m_strBackupGamePath);
		}

		private static void SaveGame(string FileName)
		{
			XmlDocument xmldoc = new XmlDocument();
			XmlElement xmlnodeGame = xmldoc.CreateElement("Game");

			xmldoc.AppendChild(xmlnodeGame);

			xmlnodeGame.SetAttribute("FEN", Game.FENStartPosition==FEN.GameStartPosition ? "" : Game.FENStartPosition);
			xmlnodeGame.SetAttribute("TurnNo", Game.TurnNo.ToString());
			xmlnodeGame.SetAttribute("WhitePlayer", Game.PlayerWhite.Intellegence==Player.enmIntellegence.Human ? "Human" : "Computer");
			xmlnodeGame.SetAttribute("BlackPlayer", Game.PlayerBlack.Intellegence==Player.enmIntellegence.Human ? "Human" : "Computer");
			xmlnodeGame.SetAttribute("BoardOrientation", Board.Orientation==Board.enmOrientation.White ? "White" : "Black");
			xmlnodeGame.SetAttribute("Version", Application.ProductVersion.ToString());
			xmlnodeGame.SetAttribute("DifficultyLevel", Game.DifficultyLevel.ToString());
			xmlnodeGame.SetAttribute("ClockMoves", Game.ClockMoves.ToString());
			xmlnodeGame.SetAttribute("ClockSeconds", Game.ClockTime.TotalSeconds.ToString());
			xmlnodeGame.SetAttribute("MaximumSearchDepth", Game.MaximumSearchDepth.ToString());
			xmlnodeGame.SetAttribute("Pondering", Game.EnablePondering ? "1" : "0");
			xmlnodeGame.SetAttribute("UseRandomOpeningMoves", Game.UseRandomOpeningMoves ? "1" : "0");
			
			foreach(Move move in m_movesHistory)
			{
				AddSaveGameNode(xmldoc, xmlnodeGame, move);
			}
			// Redo moves
			for (int intIndex=m_movesRedoList.Count-1; intIndex>=0; intIndex--)
			{
				AddSaveGameNode(xmldoc, xmlnodeGame, m_movesRedoList[intIndex]);
			}

			xmldoc.Save(FileName);
		}

		private static void AddSaveGameNode(XmlDocument xmldoc, XmlElement xmlnodeGame, Move move)
		{
			XmlElement xmlnodeMove = xmldoc.CreateElement("Move");
			xmlnodeGame.AppendChild( xmlnodeMove );
			xmlnodeMove.SetAttribute("MoveNo", move.MoveNo.ToString());
			xmlnodeMove.SetAttribute("Name", move.Name.ToString());
			xmlnodeMove.SetAttribute("From", move.From.Name);
			xmlnodeMove.SetAttribute("To", move.To.Name);
			xmlnodeMove.SetAttribute("SecondsElapsed", (Convert.ToInt32(move.TimeStamp.TotalSeconds)).ToString() );
		}

		public static void MakeAMove(Move.enmName MoveName, Piece piece, Square square)
		{
			SuspendPondering();
			MakeAMove_Internal(MoveName, piece, square);
			Game.SaveBackup();
			SendBoardPositionChangeEvent();
			CheckIfAutoNextMove();
		}

		private static void MakeAMove_Internal(Move.enmName MoveName, Piece piece, Square square)
		{
			m_movesRedoList.Clear();
			Move move = piece.Move(MoveName, square);
			move.EnemyStatus = move.Piece.Player.OtherPlayer.Status;
			m_playerToPlay.Clock.Stop();
			m_movesHistory.Last.TimeStamp = m_playerToPlay.Clock.TimeElapsed;
			if (m_playerToPlay.Intellegence == Player.enmIntellegence.Computer)
			{
				WinBoard.SendMove(move);
				if (!m_playerToPlay.OtherPlayer.CanMove)
				{
					if (m_playerToPlay.OtherPlayer.IsInCheckMate)
					{
						WinBoard.SendCheckMate();
					}
					else if (!m_playerToPlay.OtherPlayer.IsInCheck)
					{
						WinBoard.SendCheckStaleMate();
					}
				}
				else if (m_playerToPlay.OtherPlayer.CanClaimThreeMoveRepetitionDraw)
				{
					WinBoard.SendDrawByRepetition();
				}
				else if (m_playerToPlay.OtherPlayer.CanClaimFiftyMoveDraw)
				{
					WinBoard.SendDrawByFiftyMoveRule();
				}
				else if (m_playerToPlay.OtherPlayer.CanClaimInsufficientMaterialDraw)
				{
					WinBoard.SendDrawByInsufficientMaterial();
				}
			}
			m_playerToPlay = m_playerToPlay.OtherPlayer;
			m_playerToPlay.Clock.Start();
		}

		public static bool IsInAnalyseMode
		{
			get { return m_blnIsInAnalyseMode; }
			set { m_blnIsInAnalyseMode = value; }
		}

		public static int ThreadCounter
		{
			get { return m_intThreadCounter; }
			set { m_intThreadCounter = value; }
		}

		public static int FiftyMoveDrawBase
		{
			get { return m_intFiftyMoveDrawBase; }
			set { m_intFiftyMoveDrawBase = value; }
		}

		public static string FENStartPosition
		{
			get { return m_strFENStartPosition; }
			set { m_strFENStartPosition = value; }
		}

		static void SendBoardPositionChangeEvent()
		{
			BoardPositionChanged();
		}

		public static void CaptureAllPieces()
		{
			Game.PlayerWhite.CaptureAllPieces();
			Game.PlayerBlack.CaptureAllPieces();
		}

		public static void DemoteAllPieces()
		{
			Game.PlayerWhite.DemoteAllPieces();
			Game.PlayerBlack.DemoteAllPieces();
		}

		public static void SettingsUpdate()
		{
			SuspendPondering();
			if (!WinBoard.Active)
			{
				Game.SaveBackup();
			}
			SettingsUpdated();
			ResumePondering();
		}

		public static void Think()
		{
			SuspendPondering();
			MakeNextComputerMove();
		}

		public static void UndoAllMoves()
		{
			SuspendPondering();
			UndoAllMoves_Internal();
			SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		public static void UndoAllMoves_Internal()
		{
			while (m_movesHistory.Count>0)
			{
				UndoMove_Internal();
			}
		}

		public static void RedoAllMoves()
		{
			SuspendPondering();
			while (m_movesRedoList.Count>0)
			{
				RedoMove_Internal();
			}
			SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		public static void UndoMove()
		{
			SuspendPondering();
			UndoMove_Internal();
			SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		public static void RedoMove()
		{
			SuspendPondering();
			RedoMove_Internal();
			SaveBackup();
			SendBoardPositionChangeEvent();
			ResumePondering();
		}

		private static void UndoMove_Internal()
		{
			if (m_movesHistory.Count>0)
			{
				Move moveUndo = m_movesHistory.Last;
				m_playerToPlay.Clock.Revert();
				m_movesRedoList.Add(moveUndo);
				Move.Undo( moveUndo );
				m_playerToPlay = m_playerToPlay.OtherPlayer;
				if (m_movesHistory.Count>1)
				{
					Move movePenultimate = m_movesHistory[m_movesHistory.Count-2];
					m_playerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
				}
				else
				{
					m_playerToPlay.Clock.TimeElapsed = new TimeSpan(0);
				}
				m_playerToPlay.Clock.Start();
			}
		}

		private static void RedoMove_Internal()
		{
			if (m_movesRedoList.Count>0)
			{
				Move moveRedo = m_movesRedoList[m_movesRedoList.Count-1];
				m_playerToPlay.Clock.Revert();
				moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
				m_playerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
				m_movesHistory.Last.TimeStamp = moveRedo.TimeStamp;
				m_movesHistory.Last.EnemyStatus = moveRedo.Piece.Player.OtherPlayer.Status; // 14Mar05 Nimzo
				m_playerToPlay = m_playerToPlay.OtherPlayer;
				m_movesRedoList.RemoveLast();
				m_playerToPlay.Clock.Start();
			}
		}

		public static void ResumePlay()
		{
			m_playerToPlay.Clock.Start();
			GameResumed();
			if (Game.PlayerToPlay.Intellegence==Player.enmIntellegence.Computer)
			{
				MakeNextComputerMove();
			}
			else
			{
				ResumePondering();
			}
		}

		public static void PausePlay()
		{
			m_playerToPlay.Clock.Stop();
			Game.PlayerToPlay.ForceImmediateMove();
			GamePaused();
		}

		private static void CheckIfAutoNextMove()
		{
			if (Game.PlayerWhite.Intellegence==Player.enmIntellegence.Computer && Game.PlayerBlack.Intellegence==Player.enmIntellegence.Computer)
			{
				// Dont want an infinate loop of Computer moves
				return;
			}
			if (Game.PlayerToPlay.Intellegence==Player.enmIntellegence.Computer)
			{
				if (Game.PlayerToPlay.CanMove)
				{
					MakeNextComputerMove();
				}
			}
		}

		private static void MakeNextComputerMove()
		{
			if (Game.PlayerToPlay.CanMove)
			{
				Game.PlayerToPlay.StartThinking();
			}
		}

		public static void SuspendPondering()
		{
			if (Game.PlayerToPlay.IsPondering)
			{
				Game.PlayerToPlay.ForceImmediateMove();
			}
			else if (Game.PlayerToPlay.IsThinking)
			{
				Game.PlayerToPlay.ForceImmediateMove();
				Game.UndoMove();
			}
		}

		public static void ResumePondering()
		{
			if (!Game.EnablePondering)
			{
				return;
			}
			if (!Game.PlayerToPlay.CanMove)
			{
				return;
			}
			if (Game.PlayerWhite.Intellegence==Player.enmIntellegence.Computer && Game.PlayerBlack.Intellegence==Player.enmIntellegence.Computer)
			{
				return;
			}
			if (Game.PlayerToPlay.OtherPlayer.Intellegence==Player.enmIntellegence.Computer)
			{
				if (!Game.PlayerToPlay.IsPondering)
				{
					Game.PlayerToPlay.StartPondering();
				}
			}
		}

        public static void ToggleEditMode()
        {
            m_blnEditModeActive = !EditModeActive;
        }

	}
}
