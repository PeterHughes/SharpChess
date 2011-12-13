// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The game.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2011 Peter Hughes
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace SharpChess
{
    #region Using

    using System;
    using System.Windows.Forms;
    using System.Xml;

    using Microsoft.Win32;

    #endregion

    /// <summary>
    /// The game.
    /// </summary>
    public class Game
    {
        #region Constants and Fields

        /// <summary>
        /// The random.
        /// </summary>
        public static Random random = new Random();

        /// <summary>
        /// The m_str fen start position.
        /// </summary>
        protected static string m_strFENStartPosition = string.Empty;

        /// <summary>
        /// The m_moves history.
        /// </summary>
        private static readonly Moves m_movesHistory = new Moves();

        /// <summary>
        /// The m_moves redo list.
        /// </summary>
        private static readonly Moves m_movesRedoList = new Moves();

        /// <summary>
        /// The m_player black.
        /// </summary>
        private static readonly Player m_playerBlack;

        /// <summary>
        /// The m_player white.
        /// </summary>
        private static readonly Player m_playerWhite;

        /// <summary>
        /// The m_bln edit mode active.
        /// </summary>
        private static bool m_blnEditModeActive;

        /// <summary>
        /// The m_bln show thinking.
        /// </summary>
        private static bool m_blnShowThinking;

        /// <summary>
        /// The m_bln use random opening moves.
        /// </summary>
        private static bool m_blnUseRandomOpeningMoves = true;

        /// <summary>
        /// The m_int clock moves.
        /// </summary>
        private static int m_intClockMoves = 40;

        /// <summary>
        /// The m_int difficulty level.
        /// </summary>
        private static int m_intDifficultyLevel = 1;

        /// <summary>
        /// The m_int maximum search depth.
        /// </summary>
        private static int m_intMaximumSearchDepth = 1;

        /// <summary>
        /// The m_int turn no.
        /// </summary>
        private static int m_intTurnNo;

        /// <summary>
        /// The m_moves analysis.
        /// </summary>
        private static Moves m_movesAnalysis = new Moves();

        /// <summary>
        /// The m_player to play.
        /// </summary>
        private static Player m_playerToPlay;

        /// <summary>
        /// The m_str backup game path.
        /// </summary>
        private static string m_strBackupGamePath;

        /// <summary>
        /// The m_str file name.
        /// </summary>
        private static string m_strFileName = string.Empty;

        /// <summary>
        /// The m_tsn clock fixed time per move.
        /// </summary>
        private static TimeSpan m_tsnClockFixedTimePerMove = new TimeSpan(0, 0, 0);

        /// <summary>
        /// The m_tsn clock increment per move.
        /// </summary>
        private static TimeSpan m_tsnClockIncrementPerMove = new TimeSpan(0, 0, 0);

        /// <summary>
        /// The m_tsn clock time.
        /// </summary>
        private static TimeSpan m_tsnClockTime = new TimeSpan(0, 5, 0);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Game"/> class.
        /// </summary>
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

            PlayerWhite.ReadyToMakeMove += Player_ReadyToMakeMove;
            PlayerBlack.ReadyToMakeMove += Player_ReadyToMakeMove;

            RegistryKey registryKeySoftware = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

            if (registryKeySharpChess.GetValue("FileName") == null)
            {
                m_strFileName = string.Empty;
            }
            else
            {
                m_strFileName = registryKeySharpChess.GetValue("FileName").ToString();
            }

            if (registryKeySharpChess.GetValue("ShowThinking") == null)
            {
                m_blnShowThinking = true;
            }
            else
            {
                m_blnShowThinking = registryKeySharpChess.GetValue("ShowThinking").ToString() == "1";
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

            // OpeningBook.BookConvert(Game.PlayerWhite);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype game event.
        /// </summary>
        public delegate void delegatetypeGameEvent();

        #endregion

        #region Public Events

        /// <summary>
        /// The board position changed.
        /// </summary>
        public static event delegatetypeGameEvent BoardPositionChanged;

        /// <summary>
        /// The game paused.
        /// </summary>
        public static event delegatetypeGameEvent GamePaused;

        /// <summary>
        /// The game resumed.
        /// </summary>
        public static event delegatetypeGameEvent GameResumed;

        /// <summary>
        /// The game saved.
        /// </summary>
        public static event delegatetypeGameEvent GameSaved;

        /// <summary>
        /// The settings updated.
        /// </summary>
        public static event delegatetypeGameEvent SettingsUpdated;

        #endregion

        #region Enums

        /// <summary>
        /// The enm stage.
        /// </summary>
        public enum enmStage
        {
            /// <summary>
            /// The opening.
            /// </summary>
            Opening, 

            /// <summary>
            /// The middle.
            /// </summary>
            Middle, 

            /// <summary>
            /// The end.
            /// </summary>
            End
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets AvailableMegaBytes.
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
        /// Gets or sets BackupGamePath.
        /// </summary>
        public static string BackupGamePath
        {
            get
            {
                return m_strBackupGamePath;
            }

            set
            {
                m_strBackupGamePath = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether CaptureMoveAnalysisData.
        /// </summary>
        public static bool CaptureMoveAnalysisData { get; set; }

        /// <summary>
        /// Gets or sets ClockFixedTimePerMove.
        /// </summary>
        public static TimeSpan ClockFixedTimePerMove
        {
            get
            {
                return m_tsnClockFixedTimePerMove;
            }

            set
            {
                m_tsnClockFixedTimePerMove = value;
            }
        }

        /// <summary>
        /// Gets or sets ClockIncrementPerMove.
        /// </summary>
        public static TimeSpan ClockIncrementPerMove
        {
            get
            {
                return m_tsnClockIncrementPerMove;
            }

            set
            {
                m_tsnClockIncrementPerMove = value;
            }
        }

        /// <summary>
        /// Gets or sets ClockMoves.
        /// </summary>
        public static int ClockMoves
        {
            get
            {
                return m_intClockMoves;
            }

            set
            {
                m_intClockMoves = value;
            }
        }

        /// <summary>
        /// Gets or sets ClockTime.
        /// </summary>
        public static TimeSpan ClockTime
        {
            get
            {
                return m_tsnClockTime;
            }

            set
            {
                m_tsnClockTime = value;
            }
        }

        /// <summary>
        /// Gets or sets DifficultyLevel.
        /// </summary>
        public static int DifficultyLevel
        {
            get
            {
                return m_intDifficultyLevel;
            }

            set
            {
                m_intDifficultyLevel = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether EditModeActive.
        /// </summary>
        public static bool EditModeActive
        {
            get
            {
                return m_blnEditModeActive;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether EnablePondering.
        /// </summary>
        public static bool EnablePondering { get; set; }

        /// <summary>
        /// Gets or sets FENStartPosition.
        /// </summary>
        public static string FENStartPosition
        {
            get
            {
                return m_strFENStartPosition;
            }

            set
            {
                m_strFENStartPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets FiftyMoveDrawBase.
        /// </summary>
        public static int FiftyMoveDrawBase { get; set; }

        /// <summary>
        /// Gets FileName.
        /// </summary>
        public static string FileName
        {
            get
            {
                return m_strFileName == string.Empty ? "New Game" : m_strFileName;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsInAnalyseMode.
        /// </summary>
        public static bool IsInAnalyseMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsPaused.
        /// </summary>
        public static bool IsPaused
        {
            get
            {
                return !m_playerToPlay.Clock.IsTicking;
            }
        }

        /// <summary>
        /// Gets LowestMaterialCount.
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
        /// Gets MaxMaterialCount.
        /// </summary>
        public static int MaxMaterialCount
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        /// Gets or sets MaximumSearchDepth.
        /// </summary>
        public static int MaximumSearchDepth
        {
            get
            {
                return m_intMaximumSearchDepth;
            }

            set
            {
                m_intMaximumSearchDepth = value;
            }
        }

        /// <summary>
        /// Gets or sets MoveAnalysis.
        /// </summary>
        public static Moves MoveAnalysis
        {
            get
            {
                return m_movesAnalysis;
            }

            set
            {
                m_movesAnalysis = value;
            }
        }

        /// <summary>
        /// Gets MoveHistory.
        /// </summary>
        public static Moves MoveHistory
        {
            get
            {
                return m_movesHistory;
            }
        }

        /// <summary>
        /// Gets MoveNo.
        /// </summary>
        public static int MoveNo
        {
            get
            {
                return m_intTurnNo >> 1;
            }
        }

        /// <summary>
        /// Gets MoveRedoList.
        /// </summary>
        public static Moves MoveRedoList
        {
            get
            {
                return m_movesRedoList;
            }
        }

        /// <summary>
        /// Gets PlayerBlack.
        /// </summary>
        public static Player PlayerBlack
        {
            get
            {
                return m_playerBlack;
            }
        }

        /// <summary>
        /// Gets or sets PlayerToPlay.
        /// </summary>
        public static Player PlayerToPlay
        {
            get
            {
                return m_playerToPlay;
            }

            set
            {
                m_playerToPlay = value;
            }
        }

        /// <summary>
        /// Gets PlayerWhite.
        /// </summary>
        public static Player PlayerWhite
        {
            get
            {
                return m_playerWhite;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowThinking.
        /// </summary>
        public static bool ShowThinking
        {
            get
            {
                return m_blnShowThinking;
            }

            set
            {
                m_blnShowThinking = value;
            }
        }

        /// <summary>
        /// Gets Stage.
        /// </summary>
        public static enmStage Stage
        {
            get
            {
                if (LowestMaterialCount >= MaxMaterialCount)
                {
                    return enmStage.Opening;
                }
                else if (LowestMaterialCount <= 3)
                {
                    return enmStage.End;
                }

                return enmStage.Middle;
            }
        }

        /// <summary>
        /// Gets or sets ThreadCounter.
        /// </summary>
        public static int ThreadCounter { get; set; }

        /// <summary>
        /// Gets or sets TurnNo.
        /// </summary>
        public static int TurnNo
        {
            get
            {
                return m_intTurnNo;
            }

            set
            {
                m_intTurnNo = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseRandomOpeningMoves.
        /// </summary>
        public static bool UseRandomOpeningMoves
        {
            get
            {
                return m_blnUseRandomOpeningMoves;
            }

            set
            {
                m_blnUseRandomOpeningMoves = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The capture all pieces.
        /// </summary>
        public static void CaptureAllPieces()
        {
            PlayerWhite.CaptureAllPieces();
            PlayerBlack.CaptureAllPieces();
        }

        /// <summary>
        /// The demote all pieces.
        /// </summary>
        public static void DemoteAllPieces()
        {
            PlayerWhite.DemoteAllPieces();
            PlayerBlack.DemoteAllPieces();
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="FileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The load.
        /// </returns>
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
                if (IsPaused)
                {
                    ResumePlay();
                }
            }

            ResumePondering();

            return blnSuccess;
        }

        /// <summary>
        /// The load backup.
        /// </summary>
        /// <returns>
        /// The load backup.
        /// </returns>
        public static bool LoadBackup()
        {
            return LoadGame(m_strBackupGamePath);
        }

        /// <summary>
        /// The make a move.
        /// </summary>
        /// <param name="MoveName">
        /// The move name.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <param name="square">
        /// The square.
        /// </param>
        public static void MakeAMove(Move.enmName MoveName, Piece piece, Square square)
        {
            SuspendPondering();
            MakeAMove_Internal(MoveName, piece, square);
            SaveBackup();
            SendBoardPositionChangeEvent();
            CheckIfAutoNextMove();
        }

        /// <summary>
        /// The new.
        /// </summary>
        public static void New()
        {
            New(string.Empty);
        }

        /// <summary>
        /// The new.
        /// </summary>
        /// <param name="strFEN">
        /// The str fen.
        /// </param>
        public static void New(string strFEN)
        {
            SuspendPondering();

            New_Internal(strFEN);
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        /// The pause play.
        /// </summary>
        public static void PausePlay()
        {
            m_playerToPlay.Clock.Stop();
            PlayerToPlay.ForceImmediateMove();
            GamePaused();
        }

        /// <summary>
        /// The redo all moves.
        /// </summary>
        public static void RedoAllMoves()
        {
            SuspendPondering();
            while (m_movesRedoList.Count > 0)
            {
                RedoMove_Internal();
            }

            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        /// The redo move.
        /// </summary>
        public static void RedoMove()
        {
            SuspendPondering();
            RedoMove_Internal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        /// The resume play.
        /// </summary>
        public static void ResumePlay()
        {
            m_playerToPlay.Clock.Start();
            GameResumed();
            if (PlayerToPlay.Intellegence == Player.enmIntellegence.Computer)
            {
                MakeNextComputerMove();
            }
            else
            {
                ResumePondering();
            }
        }

        /// <summary>
        /// The resume pondering.
        /// </summary>
        public static void ResumePondering()
        {
            if (!EnablePondering)
            {
                return;
            }

            if (!PlayerToPlay.CanMove)
            {
                return;
            }

            if (PlayerWhite.Intellegence == Player.enmIntellegence.Computer && PlayerBlack.Intellegence == Player.enmIntellegence.Computer)
            {
                return;
            }

            if (PlayerToPlay.OtherPlayer.Intellegence == Player.enmIntellegence.Computer)
            {
                if (!PlayerToPlay.IsPondering)
                {
                    PlayerToPlay.StartPondering();
                }
            }
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="FileName">
        /// The file name.
        /// </param>
        public static void Save(string FileName)
        {
            SuspendPondering();

            SaveBackup();
            SaveGame(FileName);
            m_strFileName = FileName;

            GameSaved();

            ResumePondering();
        }

        /// <summary>
        /// The settings update.
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

        /// <summary>
        /// The start normal game.
        /// </summary>
        public static void StartNormalGame()
        {
            PlayerToPlay.Clock.Start();
            ResumePondering();
        }

        /// <summary>
        /// The suspend pondering.
        /// </summary>
        public static void SuspendPondering()
        {
            if (PlayerToPlay.IsPondering)
            {
                PlayerToPlay.ForceImmediateMove();
            }
            else if (PlayerToPlay.IsThinking)
            {
                PlayerToPlay.ForceImmediateMove();
                UndoMove();
            }
        }

        /// <summary>
        /// The terminate game.
        /// </summary>
        public static void TerminateGame()
        {
            WinBoard.StopListener();

            SuspendPondering();
            PlayerWhite.AbortThinking();
            PlayerBlack.AbortThinking();

            RegistryKey registryKeySoftware = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey registryKeySharpChess = registryKeySoftware.CreateSubKey(@"PeterHughes.org\SharpChess");

            if (registryKeySharpChess != null)
            {
                registryKeySharpChess.SetValue("FileName", m_strFileName);
                registryKeySharpChess.SetValue("ShowThinking", m_blnShowThinking ? "1" : "0");
            }
        }

        /// <summary>
        /// The think.
        /// </summary>
        public static void Think()
        {
            SuspendPondering();
            MakeNextComputerMove();
        }

        /// <summary>
        /// The toggle edit mode.
        /// </summary>
        public static void ToggleEditMode()
        {
            m_blnEditModeActive = !EditModeActive;
        }

        /// <summary>
        /// The undo all moves.
        /// </summary>
        public static void UndoAllMoves()
        {
            SuspendPondering();
            UndoAllMoves_Internal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        /// The undo all moves_ internal.
        /// </summary>
        public static void UndoAllMoves_Internal()
        {
            while (m_movesHistory.Count > 0)
            {
                UndoMove_Internal();
            }
        }

        /// <summary>
        /// The undo move.
        /// </summary>
        public static void UndoMove()
        {
            SuspendPondering();
            UndoMove_Internal();
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add save game node.
        /// </summary>
        /// <param name="xmldoc">
        /// The xmldoc.
        /// </param>
        /// <param name="xmlnodeGame">
        /// The xmlnode game.
        /// </param>
        /// <param name="move">
        /// The move.
        /// </param>
        private static void AddSaveGameNode(XmlDocument xmldoc, XmlElement xmlnodeGame, Move move)
        {
            XmlElement xmlnodeMove = xmldoc.CreateElement("Move");
            xmlnodeGame.AppendChild(xmlnodeMove);
            xmlnodeMove.SetAttribute("MoveNo", move.MoveNo.ToString());
            xmlnodeMove.SetAttribute("Name", move.Name.ToString());
            xmlnodeMove.SetAttribute("From", move.From.Name);
            xmlnodeMove.SetAttribute("To", move.To.Name);
            xmlnodeMove.SetAttribute("SecondsElapsed", Convert.ToInt32(move.TimeStamp.TotalSeconds).ToString());
        }

        /// <summary>
        /// The check if auto next move.
        /// </summary>
        private static void CheckIfAutoNextMove()
        {
            if (PlayerWhite.Intellegence == Player.enmIntellegence.Computer && PlayerBlack.Intellegence == Player.enmIntellegence.Computer)
            {
                // Dont want an infinate loop of Computer moves
                return;
            }

            if (PlayerToPlay.Intellegence == Player.enmIntellegence.Computer)
            {
                if (PlayerToPlay.CanMove)
                {
                    MakeNextComputerMove();
                }
            }
        }

        /// <summary>
        /// The load game.
        /// </summary>
        /// <param name="strFileName">
        /// The str file name.
        /// </param>
        /// <returns>
        /// The load game.
        /// </returns>
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

            XmlElement xmlnodeGame = (XmlElement)xmldoc.SelectSingleNode("/Game");

            if (xmlnodeGame.GetAttribute("FEN") != string.Empty)
            {
                New_Internal(xmlnodeGame.GetAttribute("FEN"));
            }

            if (xmlnodeGame.GetAttribute("WhitePlayer") != string.Empty)
            {
                PlayerWhite.Intellegence = xmlnodeGame.GetAttribute("WhitePlayer") == "Human" ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
            }

            if (xmlnodeGame.GetAttribute("BlackPlayer") != string.Empty)
            {
                PlayerBlack.Intellegence = xmlnodeGame.GetAttribute("BlackPlayer") == "Human" ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
            }

            if (xmlnodeGame.GetAttribute("BoardOrientation") != string.Empty)
            {
                Board.Orientation = xmlnodeGame.GetAttribute("BoardOrientation") == "White" ? Board.enmOrientation.White : Board.enmOrientation.Black;
            }

            if (xmlnodeGame.GetAttribute("DifficultyLevel") != string.Empty)
            {
                DifficultyLevel = int.Parse(xmlnodeGame.GetAttribute("DifficultyLevel"));
            }

            if (xmlnodeGame.GetAttribute("ClockMoves") != string.Empty)
            {
                ClockMoves = int.Parse(xmlnodeGame.GetAttribute("ClockMoves"));
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

            Square from;
            Square to;

            TimeSpan tsnTimeStamp;
            foreach (XmlElement xmlnode in xmlnodelist)
            {
                if (xmlnode.GetAttribute("FromFile") != string.Empty)
                {
                    from = Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("FromFile")), Convert.ToInt32(xmlnode.GetAttribute("FromRank")));
                    to = Board.GetSquare(Convert.ToInt32(xmlnode.GetAttribute("ToFile")), Convert.ToInt32(xmlnode.GetAttribute("ToRank")));
                }
                else
                {
                    from = Board.GetSquare(xmlnode.GetAttribute("From"));
                    to = Board.GetSquare(xmlnode.GetAttribute("To"));
                }

                MakeAMove_Internal(Move.MoveNameFromString(xmlnode.GetAttribute("Name")), from.Piece, to);
                if (xmlnode.GetAttribute("SecondsElapsed") == string.Empty)
                {
                    if (m_movesHistory.Count <= 2)
                    {
                        tsnTimeStamp = new TimeSpan(0);
                    }
                    else
                    {
                        tsnTimeStamp = m_movesHistory.PenultimateForSameSide.TimeStamp + (new TimeSpan(0, 0, 30));
                    }
                }
                else
                {
                    tsnTimeStamp = new TimeSpan(0, 0, int.Parse(xmlnode.GetAttribute("SecondsElapsed")));
                }

                m_movesHistory.Last.TimeStamp = tsnTimeStamp;
                m_movesHistory.Last.Piece.Player.Clock.TimeElapsed = tsnTimeStamp;
            }

            int intTurnNo = xmlnodeGame.GetAttribute("TurnNo") != string.Empty ? int.Parse(xmlnodeGame.GetAttribute("TurnNo")) : xmlnodelist.Count;

            for (int intIndex = xmlnodelist.Count; intIndex > intTurnNo; intIndex--)
            {
                UndoMove_Internal();
            }

            return true;
        }

        /// <summary>
        /// The make a move_ internal.
        /// </summary>
        /// <param name="MoveName">
        /// The move name.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <param name="square">
        /// The square.
        /// </param>
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

        /// <summary>
        /// The make next computer move.
        /// </summary>
        private static void MakeNextComputerMove()
        {
            if (PlayerToPlay.CanMove)
            {
                PlayerToPlay.StartThinking();
            }
        }

        /// <summary>
        /// The new_ internal.
        /// </summary>
        private static void New_Internal()
        {
            New_Internal(string.Empty);
        }

        /// <summary>
        /// The new_ internal.
        /// </summary>
        /// <param name="strFEN">
        /// The str fen.
        /// </param>
        private static void New_Internal(string strFEN)
        {
            if (strFEN == string.Empty)
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
            m_strFileName = string.Empty;
            FEN.SetBoardPosition(strFEN);
            m_playerWhite.Clock.Reset();
            m_playerBlack.Clock.Reset();
        }

        /// <summary>
        /// The player_ ready to make move.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Raised when prinipal variation is empty.
        /// </exception>
        private static void Player_ReadyToMakeMove()
        {
            Move move = null;
            if (PlayerToPlay.PrincipalVariation.Count > 0)
            {
                move = PlayerToPlay.PrincipalVariation[0];
            }
            else
            {
                throw new ApplicationException("Player_ReadToMakeMove: Principal Variation is empty.");
            }

            MakeAMove_Internal(move.Name, move.Piece, move.To);
            SaveBackup();
            SendBoardPositionChangeEvent();
            ResumePondering();
        }

        /// <summary>
        /// The redo move_ internal.
        /// </summary>
        private static void RedoMove_Internal()
        {
            if (m_movesRedoList.Count > 0)
            {
                Move moveRedo = m_movesRedoList[m_movesRedoList.Count - 1];
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

        /// <summary>
        /// The save backup.
        /// </summary>
        private static void SaveBackup()
        {
            SaveGame(m_strBackupGamePath);
        }

        /// <summary>
        /// The save game.
        /// </summary>
        /// <param name="FileName">
        /// The file name.
        /// </param>
        private static void SaveGame(string FileName)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlnodeGame = xmldoc.CreateElement("Game");

            xmldoc.AppendChild(xmlnodeGame);

            xmlnodeGame.SetAttribute("FEN", FENStartPosition == FEN.GameStartPosition ? string.Empty : FENStartPosition);
            xmlnodeGame.SetAttribute("TurnNo", TurnNo.ToString());
            xmlnodeGame.SetAttribute("WhitePlayer", PlayerWhite.Intellegence == Player.enmIntellegence.Human ? "Human" : "Computer");
            xmlnodeGame.SetAttribute("BlackPlayer", PlayerBlack.Intellegence == Player.enmIntellegence.Human ? "Human" : "Computer");
            xmlnodeGame.SetAttribute("BoardOrientation", Board.Orientation == Board.enmOrientation.White ? "White" : "Black");
            xmlnodeGame.SetAttribute("Version", Application.ProductVersion);
            xmlnodeGame.SetAttribute("DifficultyLevel", DifficultyLevel.ToString());
            xmlnodeGame.SetAttribute("ClockMoves", ClockMoves.ToString());
            xmlnodeGame.SetAttribute("ClockSeconds", ClockTime.TotalSeconds.ToString());
            xmlnodeGame.SetAttribute("MaximumSearchDepth", MaximumSearchDepth.ToString());
            xmlnodeGame.SetAttribute("Pondering", EnablePondering ? "1" : "0");
            xmlnodeGame.SetAttribute("UseRandomOpeningMoves", UseRandomOpeningMoves ? "1" : "0");

            foreach (Move move in m_movesHistory)
            {
                AddSaveGameNode(xmldoc, xmlnodeGame, move);
            }

            // Redo moves
            for (int intIndex = m_movesRedoList.Count - 1; intIndex >= 0; intIndex--)
            {
                AddSaveGameNode(xmldoc, xmlnodeGame, m_movesRedoList[intIndex]);
            }

            xmldoc.Save(FileName);
        }

        /// <summary>
        /// The send board position change event.
        /// </summary>
        private static void SendBoardPositionChangeEvent()
        {
            BoardPositionChanged();
        }

        /// <summary>
        /// The undo move_ internal.
        /// </summary>
        private static void UndoMove_Internal()
        {
            if (m_movesHistory.Count > 0)
            {
                Move moveUndo = m_movesHistory.Last;
                m_playerToPlay.Clock.Revert();
                m_movesRedoList.Add(moveUndo);
                Move.Undo(moveUndo);
                m_playerToPlay = m_playerToPlay.OtherPlayer;
                if (m_movesHistory.Count > 1)
                {
                    Move movePenultimate = m_movesHistory[m_movesHistory.Count - 2];
                    m_playerToPlay.Clock.TimeElapsed = movePenultimate.TimeStamp;
                }
                else
                {
                    m_playerToPlay.Clock.TimeElapsed = new TimeSpan(0);
                }

                m_playerToPlay.Clock.Start();
            }
        }

        #endregion
    }
}