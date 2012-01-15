// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Represents the game of chess over its lfetime. Holds the board, players, turn number and everything related to the chess game in progress.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace SharpChess.Model
{
    #region Using

    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Xml;

    using Microsoft.Win32;

    using SharpChess.Model.AI;

    #endregion

    /// <summary>
    ///   Represents the game of chess over its lfetime. Holds the board, players, turn number and everything related to the chess game in progress.
    /// </summary>
    public static class Game
    {
        #region Constants and Fields

        /// <summary>
        ///   The file name.
        /// </summary>
        private static string saveGameFileName = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref="Game" /> class.
        /// </summary>
        static Game()
        {
            EnableFeatures();
            ClockIncrementPerMove = new TimeSpan(0, 0, 0);
            ClockFixedTimePerMove = new TimeSpan(0, 0, 0);
            DifficultyLevel = 1;
            ClockTime = new TimeSpan(0, 5, 0);
            ClockMaxMoves = 40;
            UseRandomOpeningMoves = true;
            MoveRedoList = new Moves();
            MaximumSearchDepth = 1;
            MoveAnalysis = new Moves();
            MoveHistory = new Moves();
            FenStartPosition = string.Empty;
            HashTable.Initialise();
            HashTablePawn.Initialise();
            HashTableCheck.Initialise();

            PlayerWhite = new PlayerWhite();
            PlayerBlack = new PlayerBlack();
            PlayerToPlay = PlayerWhite;
            Board.EstablishHashKey();
            OpeningBookSimple.Initialise();

            PlayerWhite.Brain.ReadyToMakeMoveEvent += PlayerReadyToMakeMove;
            PlayerBlack.Brain.ReadyToMakeMoveEvent += PlayerReadyToMakeMove;

            RegistryKey registryKeySoftware = Registry.CurrentUser.OpenSubKey("Software", true);
            if (registryKeySoftware != null)
            {
                RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

                if (registryKeySharpChess != null)
                {
                    if (registryKeySharpChess.GetValue("FileName") == null)
                    {
                        saveGameFileName = string.Empty;
                    }
                    else
                    {
                        saveGameFileName = registryKeySharpChess.GetValue("FileName").ToString();
                    }

                    if (registryKeySharpChess.GetValue("ShowThinking") == null)
                    {
                        ShowThinking = true;
                    }
                    else
                    {
                        ShowThinking = registryKeySharpChess.GetValue("ShowThinking").ToString() == "1";
                    }

                    // Delete deprecated values
                    if (registryKeySharpChess.GetValue("EnablePondering") != null)
                    {
                        registryKeySharpChess.DeleteValue("EnablePondering");
                    }

                    if (registryKeySharpChess.GetValue("DisplayMoveAnalysisTree") != null)
                    {
                        registryKeySharpChess.DeleteValue("DisplayMoveAnalysisTree");
                    }

                    if (registryKeySharpChess.GetValue("ClockMoves") != null)
                    {
                        registryKeySharpChess.DeleteValue("ClockMoves");
                    }

                    if (registryKeySharpChess.GetValue("ClockMinutes") != null)
                    {
                        registryKeySharpChess.DeleteValue("ClockMinutes");
                    }
                }
            }

            // OpeningBook.BookConvert(Game.PlayerWhite);
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   The game event type, raised to the UI when significant game events occur.
        /// </summary>
        public delegate void GameEvent();

        #endregion

        #region Public Events

        /// <summary>
        ///   Raised when the board position changes.
        /// </summary>
        public static event GameEvent BoardPositionChanged;

        /// <summary>
        ///   Raised when the game is paused.
        /// </summary>
        public static event GameEvent GamePaused;

        /// <summary>
        ///   Raised when the game is resumed.
        /// </summary>
        public static event GameEvent GameResumed;

        /// <summary>
        ///   Riased when the game is saved.
        /// </summary>
        public static event GameEvent GameSaved;

        /// <summary>
        ///   Raised when settings are updated.
        /// </summary>
        public static event GameEvent SettingsUpdated;

        #endregion

        #region Enums

        /// <summary>
        ///   Game stages.
        /// </summary>
        public enum GameStageNames
        {
            /// <summary>
            ///   The opening.
            /// </summary>
            Opening,

            /// <summary>
            ///   The middle.
            /// </summary>
            Middle,

            /// <summary>
            ///   The end.
            /// </summary>
            End
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the available MegaBytes of free computer memory.
        /// </summary>
        public static uint AvailableMegaBytes
        {
            get
            {
                try
                {
                    // PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"); 
                    // return ((uint)Math.Max(Convert.ToInt32(ramCounter.NextValue()) - 25, 16));
                    return 16;
                }
                catch
                {
                    return 16;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the Backup Game Path.
        /// </summary>
        public static string BackupGamePath { private get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether CaptureMoveAnalysisData.
        /// </summary>
        public static bool CaptureMoveAnalysisData { get; set; }

        /// <summary>
        ///   Gets or sets the Clock Fixed Time Per Move.
        /// </summary>
        public static TimeSpan ClockFixedTimePerMove { get; set; }

        /// <summary>
        ///   Gets or sets the Clock Increment Per Move.
        /// </summary>
        public static TimeSpan ClockIncrementPerMove { get; set; }

        /// <summary>
        ///   Gets or sets the max number of moves on the clock. e.g. 60 moves in 30 minutes
        /// </summary>
        public static int ClockMaxMoves { get; set; }

        /// <summary>
        ///   Gets or sets the Clock Time.
        /// </summary>
        public static TimeSpan ClockTime { get; set; }

        /// <summary>
        ///   Gets or sets game Difficulty Level.
        /// </summary>
        public static int DifficultyLevel { get; set; }

        /// <summary>
        ///   Gets a value indicating whether Edit Mode is Active.
        /// </summary>
        public static bool EditModeActive { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Aspiration Search.
        /// </summary>
        public static bool EnableAspiration { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Search Extensions.
        /// </summary>
        public static bool EnableExtensions { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use the history heuristic ( <see cref="HistoryHeuristic" /> class).
        /// </summary>
        public static bool EnableHistoryHeuristic { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use the killer move heuristic ( <see cref="KillerMoves" /> class).
        /// </summary>
        public static bool EnableKillerMoves { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Null Move Forward Pruning.
        /// </summary>
        public static bool EnableNullMovePruning { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Pondering has been enabled.
        /// </summary>
        public static bool EnablePondering { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use PVS Search.
        /// </summary>
        public static bool EnablePvsSearch { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Quiescense.
        /// </summary>
        public static bool EnableQuiescense { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Search Reductions.
        /// </summary>
        public static bool EnableReductions { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Late Move Reductions.
        /// </summary>
        public static bool EnableReductionLateMove { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Margin Futilty Reductions.
        /// </summary>
        public static bool EnableReductionFutilityMargin { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use Fixed Depth Futilty Reductions.
        /// </summary>
        public static bool EnableReductionFutilityFixedDepth { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use the transposition table ( <see cref="HashTable" /> class).
        /// </summary>
        public static bool EnableTranspositionTable { get; set; }

        /// <summary>
        ///   Gets or sets the FEN string for the chess Start Position.
        /// </summary>
        public static string FenStartPosition { private get; set; }

        /// <summary>
        ///   Gets or sets FiftyMoveDrawBase. Appears to be a value set when using a FEN string. Doesn't seem quite right! TODO Invesigate FiftyMoveDrawBase.
        /// </summary>
        public static int FiftyMoveDrawBase { get; set; }

        /// <summary>
        ///   Gets the current game save file name.
        /// </summary>
        public static string FileName
        {
            get
            {
                return saveGameFileName == string.Empty ? "New Game" : saveGameFileName;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether Analyse Mode is active.
        /// </summary>
        public static bool IsInAnalyseMode { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the game is paused.
        /// </summary>
        public static bool IsPaused
        {
            get
            {
                return !PlayerToPlay.Clock.IsTicking;
            }
        }

        /// <summary>
        ///   Gets the lowest material count for black or white.
        /// </summary>
        public static int LowestMaterialCount
        {
            get
            {
                int intWhiteMaterialCount = PlayerWhite.MaterialCount;
                int intBlackMaterialCount = PlayerBlack.MaterialCount;
                return intWhiteMaterialCount < intBlackMaterialCount ? intWhiteMaterialCount : intBlackMaterialCount;
            }
        }

        /// <summary>
        ///   Gets the largest valid Material Count.
        /// </summary>
        public static int MaxMaterialCount
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum search depth.
        /// </summary>
        public static int MaximumSearchDepth { get; set; }

        /// <summary>
        ///   Gets or sets the list of move-analysis moves.
        /// </summary>
        public static Moves MoveAnalysis { get; set; }

        /// <summary>
        ///   Gets the currebt move history.
        /// </summary>
        public static Moves MoveHistory { get; private set; }

        /// <summary>
        ///   Gets the current move number.
        /// </summary>
        public static int MoveNo
        {
            get
            {
                return TurnNo >> 1;
            }
        }

        /// <summary>
        ///   Gets the move redo list.
        /// </summary>
        public static Moves MoveRedoList { get; private set; }

        /// <summary>
        ///   Gets black player.
        /// </summary>
        public static Player PlayerBlack { get; private set; }

        /// <summary>
        ///   Gets or sets the player to play.
        /// </summary>
        public static Player PlayerToPlay { get; set; }

        /// <summary>
        ///   Gets white player.
        /// </summary>
        public static Player PlayerWhite { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to show thinking.
        /// </summary>
        public static bool ShowThinking { get; set; }

        /// <summary>
        ///   Gets current game stage.
        /// </summary>
        public static GameStageNames Stage
        {
            get
            {
                if (LowestMaterialCount >= MaxMaterialCount)
                {
                    return GameStageNames.Opening;
                }

                return LowestMaterialCount <= 3 ? GameStageNames.End : GameStageNames.Middle;
            }
        }

        /// <summary>
        ///   Gets ThreadCounter.
        /// </summary>
        public static int ThreadCounter { get; internal set; }

        /// <summary>
        ///   Gets the current turn number.
        /// </summary>
        public static int TurnNo { get; internal set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use random opening moves.
        /// </summary>
        public static bool UseRandomOpeningMoves { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Captures all pieces.
        /// </summary>
        public static void CaptureAllPieces()
        {
            PlayerWhite.CaptureAllPieces();
            PlayerBlack.CaptureAllPieces();
        }

        /// <summary>
        ///   Demotes all pieces.
        /// </summary>
        public static void DemoteAllPieces()
        {
            PlayerWhite.DemoteAllPieces();
            PlayerBlack.DemoteAllPieces();
        }

        /// <summary>
        ///   Load a saved game.
        /// </summary>
        /// <param name="fileName"> File name. </param>
        /// <returns> Returns True is game loaded successfully. </returns>
        public static bool Load(string fileName)
        {
            SuspendPondering();

            NewInternal();
            saveGameFileName = fileName;
            bool blnSuccess = LoadGame(fileName);
            if (blnSuccess)
            {
                SaveBackup();
                SendBoardPositionChangeEvent();
            }

            PausePlay();

            return blnSuccess;
        }

        /// <summary>
        ///   Load backup game.
        /// </summary>
        /// <returns> Returns True is game loaded successfully. </returns>
        public static bool LoadBackup()
        {
            return LoadGame(BackupGamePath);
        }

        /// <summary>
        ///   Make a move.
        /// </summary>
        /// <param name="moveName"> The move name. </param>
        /// <param name="piece"> The piece to move. </param>
        /// <param name="square"> The square to move to. </param>
        public static void MakeAMove(Move.MoveNames moveName, Piece piece, Square square)
        {
            SuspendPondering();
            MakeAMoveInternal(moveName, piece, square);
            SaveBackup();
            SendBoardPositionChangeEvent();
            CheckIfAutoNextMove();
        }

        /// <summary>
        ///   Start a new game.
        /// </summary>
        public static void New()
        {
            New(string.Empty);
        }

        /// <summary>
        ///   Start a new game using a FEN string.
        /// </summary>
        /// <param name="fenString"> The FEN string. </param>
        public static void New(string fenString)
        {
            SuspendPondering();

            NewInternal(fenString);
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        ///   Pause the game.
        /// </summary>
        public static void PausePlay()
        {
            PlayerToPlay.Clock.Stop();
            PlayerToPlay.Brain.ForceImmediateMove();
            GamePaused();
        }

        /// <summary>
        ///   Redo all moves.
        /// </summary>
        public static void RedoAllMoves()
        {
            SuspendPondering();
            while (MoveRedoList.Count > 0)
            {
                RedoMoveInternal();
            }

            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        ///   Redo a move.
        /// </summary>
        public static void RedoMove()
        {
            SuspendPondering();
            RedoMoveInternal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        ///   Resume then game.
        /// </summary>
        public static void ResumePlay()
        {
            PlayerToPlay.Clock.Start();
            GameResumed();
            if (PlayerToPlay.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                MakeNextComputerMove();
            }
            else
            {
                ResumePondering();
            }
        }

        /// <summary>
        ///   Resume pondering.
        /// </summary>
        public static void ResumePondering()
        {
            if (IsPaused)
            {
                return;
            }

            if (!EnablePondering)
            {
                return;
            }

            if (!PlayerToPlay.CanMove)
            {
                return;
            }

            if (PlayerWhite.Intellegence == Player.PlayerIntellegenceNames.Computer
                && PlayerBlack.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                return;
            }

            if (PlayerToPlay.OpposingPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                if (!PlayerToPlay.Brain.IsPondering)
                {
                    PlayerToPlay.Brain.StartPondering();
                }
            }
        }

        /// <summary>
        ///   Save the game as a file name.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        public static void Save(string fileName)
        {
            SuspendPondering();

            SaveBackup();
            SaveGame(fileName);
            saveGameFileName = fileName;

            GameSaved();

            ResumePondering();
        }

        /// <summary>
        ///   Call when settings have been changed in the UI.
        /// </summary>
        public static void SettingsUpdate()
        {
            SuspendPondering();
            if (!WinBoard.Active)
            {
                SaveBackup();
            }

            SettingsUpdated();
            ResumePondering();
        }

/*
        /// <summary>
        ///   Start normal game.
        /// </summary>
        public static void StartNormalGame()
        {
            PlayerToPlay.Clock.Start();
            ResumePondering();
        }
*/

        /// <summary>
        ///   Suspend pondering.
        /// </summary>
        public static void SuspendPondering()
        {
            if (PlayerToPlay.Brain.IsPondering)
            {
                PlayerToPlay.Brain.ForceImmediateMove();
            }
            else if (PlayerToPlay.Brain.IsThinking)
            {
                PlayerToPlay.Brain.ForceImmediateMove();
                UndoMove();
            }
        }

        /// <summary>
        ///   Terminate the game.
        /// </summary>
        public static void TerminateGame()
        {
            WinBoard.StopListener();

            SuspendPondering();
            PlayerWhite.Brain.AbortThinking();
            PlayerBlack.Brain.AbortThinking();

            RegistryKey registryKeySoftware = Registry.CurrentUser.OpenSubKey("Software", true);
            if (registryKeySoftware != null)
            {
                RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

                if (registryKeySharpChess != null)
                {
                    registryKeySharpChess.SetValue("FileName", saveGameFileName);
                    registryKeySharpChess.SetValue("ShowThinking", ShowThinking ? "1" : "0");
                }
            }
        }

        /// <summary>
        ///   Instruct the computer to begin thinking, and take its turn.
        /// </summary>
        public static void Think()
        {
            SuspendPondering();
            MakeNextComputerMove();
        }

        /// <summary>
        ///   Toggle edit mode.
        /// </summary>
        public static void ToggleEditMode()
        {
            EditModeActive = !EditModeActive;
        }

        /// <summary>
        ///   Undo all moves.
        /// </summary>
        public static void UndoAllMoves()
        {
            SuspendPondering();
            UndoAllMovesInternal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        ///   Undo the last move.
        /// </summary>
        public static void UndoMove()
        {
            SuspendPondering();
            UndoMoveInternal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Add a move node to the save game XML document.
        /// </summary>
        /// <param name="xmldoc"> Xml document representing the save game file. </param>
        /// <param name="xmlnodeGame"> Parent game xmlnode. </param>
        /// <param name="move"> Move to append to the save game Xml document. </param>
        private static void AddSaveGameNode(XmlDocument xmldoc, XmlElement xmlnodeGame, Move move)
        {
            XmlElement xmlnodeMove = xmldoc.CreateElement("Move");
            xmlnodeGame.AppendChild(xmlnodeMove);
            xmlnodeMove.SetAttribute("MoveNo", move.MoveNo.ToString(CultureInfo.InvariantCulture));
            xmlnodeMove.SetAttribute("Name", move.Name.ToString());
            xmlnodeMove.SetAttribute("From", move.From.Name);
            xmlnodeMove.SetAttribute("To", move.To.Name);
            xmlnodeMove.SetAttribute("SecondsElapsed", Convert.ToInt32(move.TimeStamp.TotalSeconds).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Start then next move automatically, if its the computers turn.
        /// </summary>
        private static void CheckIfAutoNextMove()
        {
            if (PlayerWhite.Intellegence == Player.PlayerIntellegenceNames.Computer
                && PlayerBlack.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                // Dont want an infinate loop of Computer moves
                return;
            }

            if (PlayerToPlay.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                if (PlayerToPlay.CanMove)
                {
                    MakeNextComputerMove();
                }
            }
        }

        /// <summary>
        /// Enable or disable SharpChess's features
        /// </summary>
        private static void EnableFeatures()
        {
            EnableAspiration = false;
            EnableExtensions = true;
            EnableHistoryHeuristic = true;
            EnableKillerMoves = true;
            EnableNullMovePruning = true;
            EnablePvsSearch = true;
            EnableQuiescense = true;
            EnableReductions = true;
            EnableReductionFutilityMargin = false;
            EnableReductionFutilityFixedDepth = true;
            EnableReductionLateMove = true;
            EnableTranspositionTable = true;
        }

        /// <summary>
        ///   Load game from the specified file name.
        /// </summary>
        /// <param name="strFileName"> The file name. </param>
        /// <returns> True if load was successful. </returns>
        private static bool LoadGame(string strFileName)
        {
            MoveRedoList.Clear();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(strFileName);
            }
            catch
            {
                return false;
            }

            XmlElement xmlnodeGame = (XmlElement)xmldoc.SelectSingleNode("/Game");

            if (xmlnodeGame == null)
            {
                return false;
            }

            if (xmlnodeGame.GetAttribute("FEN") != string.Empty)
            {
                NewInternal(xmlnodeGame.GetAttribute("FEN"));
            }

            if (xmlnodeGame.GetAttribute("WhitePlayer") != string.Empty)
            {
                PlayerWhite.Intellegence = xmlnodeGame.GetAttribute("WhitePlayer") == "Human"
                                               ? Player.PlayerIntellegenceNames.Human
                                               : Player.PlayerIntellegenceNames.Computer;
            }

            if (xmlnodeGame.GetAttribute("BlackPlayer") != string.Empty)
            {
                PlayerBlack.Intellegence = xmlnodeGame.GetAttribute("BlackPlayer") == "Human"
                                               ? Player.PlayerIntellegenceNames.Human
                                               : Player.PlayerIntellegenceNames.Computer;
            }

            if (xmlnodeGame.GetAttribute("BoardOrientation") != string.Empty)
            {
                Board.Orientation = xmlnodeGame.GetAttribute("BoardOrientation") == "White"
                                        ? Board.OrientationNames.White
                                        : Board.OrientationNames.Black;
            }

            if (xmlnodeGame.GetAttribute("DifficultyLevel") != string.Empty)
            {
                DifficultyLevel = int.Parse(xmlnodeGame.GetAttribute("DifficultyLevel"));
            }

            if (xmlnodeGame.GetAttribute("ClockMoves") != string.Empty)
            {
                ClockMaxMoves = int.Parse(xmlnodeGame.GetAttribute("ClockMoves"));
            }

            if (xmlnodeGame.GetAttribute("ClockMinutes") != string.Empty)
            {
                ClockTime = new TimeSpan(0, int.Parse(xmlnodeGame.GetAttribute("ClockMinutes")), 0);
            }

            if (xmlnodeGame.GetAttribute("ClockSeconds") != string.Empty)
            {
                ClockTime = new TimeSpan(0, 0, int.Parse(xmlnodeGame.GetAttribute("ClockSeconds")));
            }

            if (xmlnodeGame.GetAttribute("MaximumSearchDepth") != string.Empty)
            {
                MaximumSearchDepth = int.Parse(xmlnodeGame.GetAttribute("MaximumSearchDepth"));
            }

            if (xmlnodeGame.GetAttribute("Pondering") != string.Empty)
            {
                EnablePondering = xmlnodeGame.GetAttribute("Pondering") == "1";
            }

            if (xmlnodeGame.GetAttribute("UseRandomOpeningMoves") != string.Empty)
            {
                UseRandomOpeningMoves = xmlnodeGame.GetAttribute("UseRandomOpeningMoves") == "1";
            }

            XmlNodeList xmlnodelist = xmldoc.SelectNodes("/Game/Move");

            if (xmlnodelist != null)
            {
                foreach (XmlElement xmlnode in xmlnodelist)
                {
                    Square from;
                    Square to;
                    if (xmlnode.GetAttribute("FromFile") != string.Empty)
                    {
                        from = Board.GetSquare(
                            Convert.ToInt32(xmlnode.GetAttribute("FromFile")),
                            Convert.ToInt32(xmlnode.GetAttribute("FromRank")));
                        to = Board.GetSquare(
                            Convert.ToInt32(xmlnode.GetAttribute("ToFile")),
                            Convert.ToInt32(xmlnode.GetAttribute("ToRank")));
                    }
                    else
                    {
                        from = Board.GetSquare(xmlnode.GetAttribute("From"));
                        to = Board.GetSquare(xmlnode.GetAttribute("To"));
                    }

                    MakeAMoveInternal(Move.MoveNameFromString(xmlnode.GetAttribute("Name")), from.Piece, to);
                    TimeSpan tsnTimeStamp;
                    if (xmlnode.GetAttribute("SecondsElapsed") == string.Empty)
                    {
                        if (MoveHistory.Count <= 2)
                        {
                            tsnTimeStamp = new TimeSpan(0);
                        }
                        else
                        {
                            tsnTimeStamp = MoveHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0, 0, 30));
                        }
                    }
                    else
                    {
                        tsnTimeStamp = new TimeSpan(0, 0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
                    }

                    MoveHistory.Last.TimeStamp = tsnTimeStamp;
                    MoveHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
                }

                int intTurnNo = xmlnodeGame.GetAttribute("TurnNo") != string.Empty
                                    ? int.Parse(xmlnodeGame.GetAttribute("TurnNo"))
                                    : xmlnodelist.Count;

                for (int intIndex = xmlnodelist.Count; intIndex > intTurnNo; intIndex--)
                {
                    UndoMoveInternal();
                }
            }

            return true;
        }

        /// <summary>
        ///   Make the specified move. For internal use only.
        /// </summary>
        /// <param name="moveName"> The move name. </param>
        /// <param name="piece"> The piece to move. </param>
        /// <param name="square"> The square to move to. </param>
        private static void MakeAMoveInternal(Move.MoveNames moveName, Piece piece, Square square)
        {
            MoveRedoList.Clear();
            Move move = piece.Move(moveName, square);
            move.EnemyStatus = move.Piece.Player.OpposingPlayer.Status;
            PlayerToPlay.Clock.Stop();
            MoveHistory.Last.TimeStamp = PlayerToPlay.Clock.TimeElapsed;
            if (PlayerToPlay.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                WinBoard.SendMove(move);
                if (!PlayerToPlay.OpposingPlayer.CanMove)
                {
                    if (PlayerToPlay.OpposingPlayer.IsInCheckMate)
                    {
                        WinBoard.SendCheckMate();
                    }
                    else if (!PlayerToPlay.OpposingPlayer.IsInCheck)
                    {
                        WinBoard.SendCheckStaleMate();
                    }
                }
                else if (PlayerToPlay.OpposingPlayer.CanClaimThreeMoveRepetitionDraw)
                {
                    WinBoard.SendDrawByRepetition();
                }
                else if (PlayerToPlay.OpposingPlayer.CanClaimFiftyMoveDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                }
                else if (PlayerToPlay.OpposingPlayer.CanClaimInsufficientMaterialDraw)
                {
                    WinBoard.SendDrawByInsufficientMaterial();
                }
            }

            PlayerToPlay = PlayerToPlay.OpposingPlayer;
            PlayerToPlay.Clock.Start();
        }

        /// <summary>
        ///   Instruct the computer to make its next move.
        /// </summary>
        private static void MakeNextComputerMove()
        {
            if (PlayerToPlay.CanMove)
            {
                PlayerToPlay.Brain.StartThinking();
            }
        }

        /// <summary>
        ///   Start a new game. For internal use only.
        /// </summary>
        private static void NewInternal()
        {
            NewInternal(string.Empty);
        }

        /// <summary>
        ///   Start a new game from the specified FEN string position. For internal use only.
        /// </summary>
        /// <param name="fenString"> The str fen. </param>
        private static void NewInternal(string fenString)
        {
            if (fenString == string.Empty)
            {
                fenString = Fen.GameStartPosition;
            }

            Fen.Validate(fenString);

            HashTable.Clear();
            HashTablePawn.Clear();
            HashTableCheck.Clear();
            KillerMoves.Clear();
            HistoryHeuristic.Clear();

            UndoAllMovesInternal();
            MoveRedoList.Clear();
            saveGameFileName = string.Empty;
            Fen.SetBoardPosition(fenString);
            PlayerWhite.Clock.Reset();
            PlayerBlack.Clock.Reset();
        }

        /// <summary>
        ///   Called when the computer has finished thinking, and is ready to make its move.
        /// </summary>
        /// <exception cref="ApplicationException">Raised when principal variation is empty.</exception>
        private static void PlayerReadyToMakeMove()
        {
            Move move;
            if (PlayerToPlay.Brain.PrincipalVariation.Count > 0)
            {
                move = PlayerToPlay.Brain.PrincipalVariation[0];
            }
            else
            {
                throw new ApplicationException("Player_ReadToMakeMove: Principal Variation is empty.");
            }

            MakeAMoveInternal(move.Name, move.Piece, move.To);
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        ///   Redo move. For internal use only.
        /// </summary>
        private static void RedoMoveInternal()
        {
            if (MoveRedoList.Count > 0)
            {
                Move moveRedo = MoveRedoList[MoveRedoList.Count - 1];
                PlayerToPlay.Clock.Revert();
                moveRedo.Piece.Move(moveRedo.Name, moveRedo.To);
                PlayerToPlay.Clock.TimeElapsed = moveRedo.TimeStamp;
                MoveHistory.Last.TimeStamp = moveRedo.TimeStamp;
                MoveHistory.Last.EnemyStatus = moveRedo.Piece.Player.OpposingPlayer.Status; // 14Mar05 Nimzo
                PlayerToPlay = PlayerToPlay.OpposingPlayer;
                MoveRedoList.RemoveLast();
                if (!IsPaused)
                {
                    PlayerToPlay.Clock.Start();
                }
            }
        }

        /// <summary>
        ///   Save a backup of the current game.
        /// </summary>
        private static void SaveBackup()
        {
            if (!WinBoard.Active)
            {
                // Only save backups if not using WinBoard.
                SaveGame(BackupGamePath);
            }
        }

        /// <summary>
        ///   Save game using the specified file name.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        private static void SaveGame(string fileName)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlnodeGame = xmldoc.CreateElement("Game");

            xmldoc.AppendChild(xmlnodeGame);

            xmlnodeGame.SetAttribute("FEN", FenStartPosition == Fen.GameStartPosition ? string.Empty : FenStartPosition);
            xmlnodeGame.SetAttribute("TurnNo", TurnNo.ToString(CultureInfo.InvariantCulture));
            xmlnodeGame.SetAttribute(
                "WhitePlayer", PlayerWhite.Intellegence == Player.PlayerIntellegenceNames.Human ? "Human" : "Computer");
            xmlnodeGame.SetAttribute(
                "BlackPlayer", PlayerBlack.Intellegence == Player.PlayerIntellegenceNames.Human ? "Human" : "Computer");
            xmlnodeGame.SetAttribute(
                "BoardOrientation", Board.Orientation == Board.OrientationNames.White ? "White" : "Black");
            xmlnodeGame.SetAttribute("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            xmlnodeGame.SetAttribute("DifficultyLevel", DifficultyLevel.ToString(CultureInfo.InvariantCulture));
            xmlnodeGame.SetAttribute("ClockMoves", ClockMaxMoves.ToString(CultureInfo.InvariantCulture));
            xmlnodeGame.SetAttribute("ClockSeconds", ClockTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            xmlnodeGame.SetAttribute("MaximumSearchDepth", MaximumSearchDepth.ToString(CultureInfo.InvariantCulture));
            xmlnodeGame.SetAttribute("Pondering", EnablePondering ? "1" : "0");
            xmlnodeGame.SetAttribute("UseRandomOpeningMoves", UseRandomOpeningMoves ? "1" : "0");

            foreach (Move move in MoveHistory)
            {
                AddSaveGameNode(xmldoc, xmlnodeGame, move);
            }

            // Redo moves
            for (int intIndex = MoveRedoList.Count - 1; intIndex >= 0; intIndex--)
            {
                AddSaveGameNode(xmldoc, xmlnodeGame, MoveRedoList[intIndex]);
            }

            xmldoc.Save(fileName);
        }

        /// <summary>
        ///   The send board position change event.
        /// </summary>
        private static void SendBoardPositionChangeEvent()
        {
            BoardPositionChanged();
        }

        /// <summary>
        ///   Undo all moves. For internal use pnly.
        /// </summary>
        private static void UndoAllMovesInternal()
        {
            while (MoveHistory.Count > 0)
            {
                UndoMoveInternal();
            }
        }

        /// <summary>
        ///   Undo move. For internal use only.
        /// </summary>
        private static void UndoMoveInternal()
        {
            if (MoveHistory.Count > 0)
            {
                Move moveUndo = MoveHistory.Last;
                PlayerToPlay.Clock.Revert();
                MoveRedoList.Add(moveUndo);
                Move.Undo(moveUndo);
                PlayerToPlay = PlayerToPlay.OpposingPlayer;
                if (MoveHistory.Count > 1)
                {
                    Move movePenultimate = MoveHistory[MoveHistory.Count - 2];
                    PlayerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
                }
                else
                {
                    PlayerToPlay.Clock.TimeElapsed = new TimeSpan(0);
                }

                if (!IsPaused)
                {
                    PlayerToPlay.Clock.Start();
                }
            }
        }

        #endregion
    }
}