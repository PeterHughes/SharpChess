// --------------------------------------------------------------------------------------------------------------------
// <copyright file="frmMain.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   Summary description for frmMain.
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Summary description for frmMain.
    /// </summary>
    public class frmMain : Form
    {
        #region Constants and Fields

        /// <summary>
        /// The intellegenc e_ computer.
        /// </summary>
        private const int INTELLEGENCE_COMPUTER = 1;

        /// <summary>
        /// The intellegenc e_ human.
        /// </summary>
        private const int INTELLEGENCE_HUMAN = 0;

        /// <summary>
        /// The squar e_ brightness.
        /// </summary>
        private const int SQUARE_BRIGHTNESS = 48;

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private const int SQUARE_SIZE = 42;

        /// <summary>
        /// The boar d_ squar e_ colou r_ black.
        /// </summary>
        private readonly Color BOARD_SQUARE_COLOUR_BLACK = Color.FromArgb(189, 117, 53);

        /// <summary>
        /// The boar d_ squar e_ colou r_ blac k_ bright.
        /// </summary>
        private readonly Color BOARD_SQUARE_COLOUR_BLACK_BRIGHT = Color.FromArgb(
            Math.Min(189 + SQUARE_BRIGHTNESS, 255), 
            Math.Min(117 + SQUARE_BRIGHTNESS, 255), 
            Math.Min(53 + SQUARE_BRIGHTNESS, 255));

        /// <summary>
        /// The boar d_ squar e_ colou r_ white.
        /// </summary>
        private readonly Color BOARD_SQUARE_COLOUR_WHITE = Color.FromArgb(229, 197, 105);

        /// <summary>
        /// The boar d_ squar e_ colou r_ whit e_ bright.
        /// </summary>
        private readonly Color BOARD_SQUARE_COLOUR_WHITE_BRIGHT = Color.FromArgb(
            Math.Min(229 + SQUARE_BRIGHTNESS, 255), 
            Math.Min(197 + SQUARE_BRIGHTNESS, 255), 
            Math.Min(105 + SQUARE_BRIGHTNESS, 255));

        /// <summary>
        /// The m_acur piece cursors.
        /// </summary>
        private readonly Cursor[] m_acurPieceCursors = new Cursor[12];

        /// <summary>
        /// The m_form move analysis.
        /// </summary>
        private readonly frmMoveAnalysis m_formMoveAnalysis = new frmMoveAnalysis();

        /// <summary>
        /// The m_form win board.
        /// </summary>
        private readonly frmWinBoard m_formWinBoard = new frmWinBoard();

        /// <summary>
        /// The btn pg nto xml.
        /// </summary>
        private Button btnPGNtoXML;

        /// <summary>
        /// The btn perft.
        /// </summary>
        private Button btnPerft;

        /// <summary>
        /// The btn prune.
        /// </summary>
        private Button btnPrune;

        /// <summary>
        /// The btn xm lto ob.
        /// </summary>
        private Button btnXMLtoOB;

        /// <summary>
        /// The cbo intellegence black.
        /// </summary>
        private ComboBox cboIntellegenceBlack;

        /// <summary>
        /// The cbo intellegence white.
        /// </summary>
        private ComboBox cboIntellegenceWhite;

        /// <summary>
        /// The components.
        /// </summary>
        private IContainer components;

        /// <summary>
        /// The group box 1.
        /// </summary>
        private GroupBox groupBox1;

        /// <summary>
        /// The image list 1.
        /// </summary>
        private ImageList imageList1;

        /// <summary>
        /// The img pieces.
        /// </summary>
        private ImageList imgPieces;

        /// <summary>
        /// The img tiles.
        /// </summary>
        private ImageList imgTiles;

        /// <summary>
        /// The img tool menus.
        /// </summary>
        private ImageList imgToolMenus;

        /// <summary>
        /// The label 1.
        /// </summary>
        private Label label1;

        /// <summary>
        /// The label 2.
        /// </summary>
        private Label label2;

        /// <summary>
        /// The label 3.
        /// </summary>
        private Label label3;

        /// <summary>
        /// The label 4.
        /// </summary>
        private Label label4;

        /// <summary>
        /// The label 5.
        /// </summary>
        private Label label5;

        /// <summary>
        /// The lbl black clock.
        /// </summary>
        private Label lblBlackClock;

        /// <summary>
        /// The lbl black points.
        /// </summary>
        private Label lblBlackPoints;

        /// <summary>
        /// The lbl black position.
        /// </summary>
        private Label lblBlackPosition;

        /// <summary>
        /// The lbl black score.
        /// </summary>
        private Label lblBlackScore;

        /// <summary>
        /// The lbl blacks captures.
        /// </summary>
        private Label lblBlacksCaptures;

        /// <summary>
        /// The lbl game paused.
        /// </summary>
        private Label lblGamePaused;

        /// <summary>
        /// The lbl player.
        /// </summary>
        private Label lblPlayer;

        /// <summary>
        /// The lbl player clocks.
        /// </summary>
        private Label lblPlayerClocks;

        /// <summary>
        /// The lbl stage.
        /// </summary>
        private Label lblStage;

        /// <summary>
        /// The lbl white clock.
        /// </summary>
        private Label lblWhiteClock;

        /// <summary>
        /// The lbl white points.
        /// </summary>
        private Label lblWhitePoints;

        /// <summary>
        /// The lbl white position.
        /// </summary>
        private Label lblWhitePosition;

        /// <summary>
        /// The lbl white score.
        /// </summary>
        private Label lblWhiteScore;

        /// <summary>
        /// The lbl whites captures.
        /// </summary>
        private Label lblWhitesCaptures;

        /// <summary>
        /// The lvc move.
        /// </summary>
        private ColumnHeader lvcMove;

        /// <summary>
        /// The lvc move no.
        /// </summary>
        private ColumnHeader lvcMoveNo;

        /// <summary>
        /// The lvc time.
        /// </summary>
        private ColumnHeader lvcTime;

        /// <summary>
        /// The lvw move history.
        /// </summary>
        private ListView lvwMoveHistory;

        /// <summary>
        /// The m_bln in mouse down.
        /// </summary>
        private bool m_blnInMouseDown;

        /// <summary>
        /// The m_bln is left mouse button down.
        /// </summary>
        private bool m_blnIsLeftMouseButtonDown;

        /// <summary>
        /// The m_cur piece cursor.
        /// </summary>
        private Cursor m_curPieceCursor;

        /// <summary>
        /// The m_moves possible.
        /// </summary>
        private Moves m_movesPossible = new Moves();

        /// <summary>
        /// The m_pic blacks captures.
        /// </summary>
        private PictureBox[] m_picBlacksCaptures;

        /// <summary>
        /// The m_pic squares.
        /// </summary>
        private PictureBox[,] m_picSquares;

        /// <summary>
        /// The m_pic whites captures.
        /// </summary>
        private PictureBox[] m_picWhitesCaptures;

        /// <summary>
        /// The m_square from.
        /// </summary>
        private Square m_squareFrom;

        /// <summary>
        /// The m_square last from.
        /// </summary>
        private Square m_squareLastFrom;

        /// <summary>
        /// The m_square last to.
        /// </summary>
        private Square m_squareLastTo;

        /// <summary>
        /// The m_square to.
        /// </summary>
        private Square m_squareTo;

        /// <summary>
        /// The menu item 1.
        /// </summary>
        private MenuItem menuItem1;

        /// <summary>
        /// The menu item 2.
        /// </summary>
        private MenuItem menuItem2;

        /// <summary>
        /// The mnu.
        /// </summary>
        private MainMenu mnu;

        /// <summary>
        /// The mnu about.
        /// </summary>
        private MenuItem mnuAbout;

        /// <summary>
        /// The mnu computer.
        /// </summary>
        private MenuItem mnuComputer;

        /// <summary>
        /// The mnu copy fen.
        /// </summary>
        private MenuItem mnuCopyFEN;

        /// <summary>
        /// The mnu difficulty.
        /// </summary>
        private MenuItem mnuDifficulty;

        /// <summary>
        /// The mnu display move analysis tree.
        /// </summary>
        private MenuItem mnuDisplayMoveAnalysisTree;

        /// <summary>
        /// The mnu display win board message log.
        /// </summary>
        private MenuItem mnuDisplayWinBoardMessageLog;

        /// <summary>
        /// The mnu edit.
        /// </summary>
        private MenuItem mnuEdit;

        /// <summary>
        /// The mnu edit board position.
        /// </summary>
        private MenuItem mnuEditBoardPosition;

        /// <summary>
        /// The mnu exit.
        /// </summary>
        private MenuItem mnuExit;

        /// <summary>
        /// The mnu file.
        /// </summary>
        private MenuItem mnuFile;

        /// <summary>
        /// The mnu flip board.
        /// </summary>
        private MenuItem mnuFlipBoard;

        /// <summary>
        /// The mnu game.
        /// </summary>
        private MenuItem mnuGame;

        /// <summary>
        /// The mnu help.
        /// </summary>
        private MenuItem mnuHelp;

        /// <summary>
        /// The mnu move now.
        /// </summary>
        private MenuItem mnuMoveNow;

        /// <summary>
        /// The mnu new.
        /// </summary>
        private MenuItem mnuNew;

        /// <summary>
        /// The mnu open.
        /// </summary>
        private MenuItem mnuOpen;

        /// <summary>
        /// The mnu paste fen.
        /// </summary>
        private MenuItem mnuPasteFEN;

        /// <summary>
        /// The mnu pause play.
        /// </summary>
        private MenuItem mnuPausePlay;

        /// <summary>
        /// The mnu redo all moves.
        /// </summary>
        private MenuItem mnuRedoAllMoves;

        /// <summary>
        /// The mnu redo move.
        /// </summary>
        private MenuItem mnuRedoMove;

        /// <summary>
        /// The mnu resume play.
        /// </summary>
        private MenuItem mnuResumePlay;

        /// <summary>
        /// The mnu save.
        /// </summary>
        private MenuItem mnuSave;

        /// <summary>
        /// The mnu sep 1.
        /// </summary>
        private MenuItem mnuSep1;

        /// <summary>
        /// The mnu sep 2.
        /// </summary>
        private MenuItem mnuSep2;

        /// <summary>
        /// The mnu sep 3.
        /// </summary>
        private MenuItem mnuSep3;

        /// <summary>
        /// The mnu show thinking.
        /// </summary>
        private MenuItem mnuShowThinking;

        /// <summary>
        /// The mnu think.
        /// </summary>
        private MenuItem mnuThink;

        /// <summary>
        /// The mnu undo all moves.
        /// </summary>
        private MenuItem mnuUndoAllMoves;

        /// <summary>
        /// The mnu undo move.
        /// </summary>
        private MenuItem mnuUndoMove;

        /// <summary>
        /// The mnu view.
        /// </summary>
        private MenuItem mnuView;

        /// <summary>
        /// The num perft depth.
        /// </summary>
        private NumericUpDown numPerftDepth;

        /// <summary>
        /// The panel 1.
        /// </summary>
        private Panel panel1;

        /// <summary>
        /// The panel 2.
        /// </summary>
        private Panel panel2;

        /// <summary>
        /// The panel 3.
        /// </summary>
        private Panel panel3;

        /// <summary>
        /// The panel 4.
        /// </summary>
        private Panel panel4;

        /// <summary>
        /// The pbr.
        /// </summary>
        private ProgressBar pbr;

        /// <summary>
        /// The pnl edging.
        /// </summary>
        private Panel pnlEdging;

        /// <summary>
        /// The pnl main.
        /// </summary>
        private Panel pnlMain;

        /// <summary>
        /// The sbr.
        /// </summary>
        private StatusBar sbr;

        /// <summary>
        /// The tbr.
        /// </summary>
        private ToolBar tbr;

        /// <summary>
        /// The tbr flip board.
        /// </summary>
        private ToolBarButton tbrFlipBoard;

        /// <summary>
        /// The tbr move now.
        /// </summary>
        private ToolBarButton tbrMoveNow;

        /// <summary>
        /// The tbr new.
        /// </summary>
        private ToolBarButton tbrNew;

        /// <summary>
        /// The tbr open.
        /// </summary>
        private ToolBarButton tbrOpen;

        /// <summary>
        /// The tbr pause play.
        /// </summary>
        private ToolBarButton tbrPausePlay;

        /// <summary>
        /// The tbr redo all moves.
        /// </summary>
        private ToolBarButton tbrRedoAllMoves;

        /// <summary>
        /// The tbr redo move.
        /// </summary>
        private ToolBarButton tbrRedoMove;

        /// <summary>
        /// The tbr resume play.
        /// </summary>
        private ToolBarButton tbrResumePlay;

        /// <summary>
        /// The tbr save.
        /// </summary>
        private ToolBarButton tbrSave;

        /// <summary>
        /// The tbr sep 1.
        /// </summary>
        private ToolBarButton tbrSep1;

        /// <summary>
        /// The tbr sep 2.
        /// </summary>
        private ToolBarButton tbrSep2;

        /// <summary>
        /// The tbr sep 3.
        /// </summary>
        private ToolBarButton tbrSep3;

        /// <summary>
        /// The tbr sep 4.
        /// </summary>
        private ToolBarButton tbrSep4;

        /// <summary>
        /// The tbr think.
        /// </summary>
        private ToolBarButton tbrThink;

        /// <summary>
        /// The tbr undo all moves.
        /// </summary>
        private ToolBarButton tbrUndoAllMoves;

        /// <summary>
        /// The tbr undo move.
        /// </summary>
        private ToolBarButton tbrUndoMove;

        /// <summary>
        /// The timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The txt output.
        /// </summary>
        private TextBox txtOutput;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype game_ board position changed handler.
        /// </summary>
        public delegate void delegatetypeGame_BoardPositionChangedHandler();

        /// <summary>
        /// The delegatetype player_ move considered handler.
        /// </summary>
        public delegate void delegatetypePlayer_MoveConsideredHandler();

        /// <summary>
        /// The delegatetype player_ thinking beginning handler.
        /// </summary>
        public delegate void delegatetypePlayer_ThinkingBeginningHandler();

        /// <summary>
        /// The delegatetype win board message handler.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        public delegate void delegatetypeWinBoardMessageHandler(string strMessage);

        /// <summary>
        /// The delegatetype win board standard handler.
        /// </summary>
        public delegate void delegatetypeWinBoardStandardHandler();

        #endregion

        #region Public Methods

        /// <summary>
        /// The flip board.
        /// </summary>
        public void FlipBoard()
        {
            Board.Flip();
            this.OrientBoard();
        }

        /// <summary>
        /// The move analysis closed.
        /// </summary>
        public void MoveAnalysisClosed()
        {
            this.AssignMenuChecks();
        }

        /// <summary>
        /// The win board closed.
        /// </summary>
        public void WinBoardClosed()
        {
            this.AssignMenuChecks();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.Run(new frmMain());
        }

        /// <summary>
        /// The add move to history.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        private void AddMoveToHistory(Move move)
        {
            string[] lvi = 
                {
                    move.MoveNo.ToString(), 
                    move.TimeStamp.Hours.ToString().PadLeft(2, '0') + ":"
                    + move.TimeStamp.Minutes.ToString().PadLeft(2, '0') + ":"
                    + move.TimeStamp.Seconds.ToString().PadLeft(2, '0'), 
                    move.Description
                    + (move.pieceCaptured != null ? (" (" + move.pieceCaptured.Name.ToString() + ")") : string.Empty)
                };

            this.lvwMoveHistory.Items.Add(new ListViewItem(lvi));
            switch (move.Piece.Player.Colour)
            {
                case Player.enmColour.White:
                    this.lvwMoveHistory.Items[this.lvwMoveHistory.Items.Count - 1].BackColor = Color.White;
                    this.lvwMoveHistory.Items[this.lvwMoveHistory.Items.Count - 1].ForeColor = Color.Blue;
                    break;

                case Player.enmColour.Black:
                    this.lvwMoveHistory.Items[this.lvwMoveHistory.Items.Count - 1].BackColor = Color.White;
                    this.lvwMoveHistory.Items[this.lvwMoveHistory.Items.Count - 1].ForeColor = Color.Black;
                    break;
            }

            this.lvwMoveHistory.Items[this.lvwMoveHistory.Items.Count - 1].EnsureVisible();
        }

        /// <summary>
        /// The assign menu checks.
        /// </summary>
        private void AssignMenuChecks()
        {
            this.mnuShowThinking.Checked = Game.ShowThinking;
            Game.CaptureMoveAnalysisData = this.mnuDisplayMoveAnalysisTree.Checked = this.m_formMoveAnalysis.Visible;
            this.mnuDisplayWinBoardMessageLog.Checked = this.m_formWinBoard.Visible;
            this.mnuEditBoardPosition.Checked = Game.EditModeActive;
        }

        /// <summary>
        /// The create board.
        /// </summary>
        private void CreateBoard()
        {
            PictureBox picSquare;
            Square square;
            Label lblRank;
            Label lblFile;

            for (int intRank = 0; intRank < Board.RankCount; intRank++)
            {
                lblRank = new Label();
                lblRank.BackColor = Color.Transparent;
                lblRank.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
                lblRank.Name = "lblRank" + intRank.ToString();
                lblRank.Size = new Size(SQUARE_SIZE / 2, SQUARE_SIZE);
                lblRank.TabIndex = 12;
                lblRank.Text = Board.GetSquare(0, intRank).RankName;
                lblRank.TextAlign = ContentAlignment.MiddleCenter;
                lblRank.Left = 0;
                lblRank.Top = (Board.RankCount - 1) * SQUARE_SIZE - intRank * SQUARE_SIZE + 16;
                this.pnlMain.Controls.Add(lblRank);
            }

            this.m_picSquares = new PictureBox[Board.FileCount, Board.RankCount];

            for (int intFile = 0; intFile < Board.FileCount; intFile++)
            {
                lblFile = new Label();
                lblFile.BackColor = Color.Transparent;
                lblFile.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
                lblFile.Name = "lblFile" + intFile.ToString();
                lblFile.Size = new Size(SQUARE_SIZE, SQUARE_SIZE / 2);
                lblFile.TabIndex = 12;
                lblFile.Text = Board.GetSquare(intFile, 0).FileName;
                lblFile.TextAlign = ContentAlignment.MiddleCenter;
                lblFile.Left = intFile * SQUARE_SIZE + 30;
                lblFile.Top = Board.RankCount * SQUARE_SIZE + 24;
                this.pnlMain.Controls.Add(lblFile);
            }

            for (int intOrdinal = 0; intOrdinal < Board.SquareCount; intOrdinal++)
            {
                square = Board.GetSquare(intOrdinal);

                if (square != null)
                {
                    picSquare = new PictureBox();

                    if (square.Colour == Square.enmColour.White)
                    {
                        picSquare.BackColor = this.BOARD_SQUARE_COLOUR_WHITE;
                    }
                    else
                    {
                        picSquare.BackColor = this.BOARD_SQUARE_COLOUR_BLACK;
                    }

                    picSquare.Name = "picSquare" + square.File.ToString() + square.Rank.ToString();
                    picSquare.Size = new Size(SQUARE_SIZE, SQUARE_SIZE);
                    picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
                    picSquare.TabIndex = 0;
                    picSquare.TabStop = false;
                    picSquare.Tag = square.Ordinal;
                    picSquare.MouseDown += this.picSquare_MouseDown;
                    picSquare.MouseUp += this.picSquare_MouseUp;
                    picSquare.DragDrop += this.picSquare_DragDrop;
                    picSquare.DragOver += this.picSquare_DragOver;
                    picSquare.GiveFeedback += this.picSquare_GiveFeedback;
                    picSquare.AllowDrop = true;
                    this.pnlEdging.Controls.Add(picSquare);
                    this.m_picSquares[square.File, square.Rank] = picSquare;
                }
            }

            this.m_picWhitesCaptures = new PictureBox[15];
            this.m_picBlacksCaptures = new PictureBox[15];

            for (int intIndex = 0; intIndex < 15; intIndex++)
            {
                picSquare = new PictureBox();
                picSquare.Left = intIndex * (SQUARE_SIZE + 1) + 1;
                picSquare.Top = 384;
                picSquare.BackColor = SystemColors.ControlDark;
                picSquare.Name = "picSquareWhite" + intIndex.ToString();
                picSquare.Size = new Size(SQUARE_SIZE, SQUARE_SIZE);
                picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
                picSquare.TabIndex = 0;
                picSquare.TabStop = false;
                picSquare.Tag = intIndex;
                this.pnlMain.Controls.Add(picSquare);
                this.m_picWhitesCaptures[intIndex] = picSquare;

                picSquare = new PictureBox();
                picSquare.Left = intIndex * (SQUARE_SIZE + 1) + 1;
                picSquare.Top = 384 + SQUARE_SIZE + 1;
                picSquare.BackColor = SystemColors.ControlDark;
                picSquare.Name = "picSquareBlack" + intIndex.ToString();
                picSquare.Size = new Size(SQUARE_SIZE, SQUARE_SIZE);
                picSquare.SizeMode = PictureBoxSizeMode.CenterImage;
                picSquare.TabIndex = 0;
                picSquare.TabStop = false;
                picSquare.Tag = intIndex;
                this.pnlMain.Controls.Add(picSquare);
                this.m_picBlacksCaptures[intIndex] = picSquare;
            }
        }

        /// <summary>
        /// The edit board position.
        /// </summary>
        private void EditBoardPosition()
        {
            Game.ToggleEditMode();
            this.AssignMenuChecks();
            this.SetFormState();
            this.RenderBoard();
        }

        /// <summary>
        /// The game_ board position changed.
        /// </summary>
        private void Game_BoardPositionChanged()
        {
            delegatetypeGame_BoardPositionChangedHandler BoardPositionChangedPointer =
                this.Game_BoardPositionChangedHandler;
            this.Invoke(BoardPositionChangedPointer, null);
        }

        /// <summary>
        /// The game_ board position changed handler.
        /// </summary>
        private void Game_BoardPositionChangedHandler()
        {
            // 			if (Game.IsPaused) return;
            if (Game.MoveHistory.Count > 0)
            {
                Move move = Game.MoveHistory.Last;
                this.sbr.Text += "  Moved: " + move.Piece.Name.ToString() + " " + move.From.Name + "-" + move.To.Name
                                 + " " + move.Description;
            }

            this.pbr.Value = 0;

            this.m_squareFrom = null;
            this.m_movesPossible = new Moves();

            this.RenderMoveAnalysis();
            this.RenderBoard();
        }

        /// <summary>
        /// Get this cursor for the specfied piece
        /// </summary>
        /// <param name="piece">
        /// The piece.
        /// </param>
        private Cursor GetPieceCursor(Piece piece)
        {
            return this.m_acurPieceCursors[piece.ImageIndex];
        }

        /// <summary>
        /// The get piece image.
        /// </summary>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <returns>
        /// </returns>
        private Image GetPieceImage(Piece piece)
        {
            return this.imgPieces.Images[piece.ImageIndex];
        }

        /// <summary>
        /// The handle win board input received message.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private void HandleWinBoardInputReceivedMessage(string strMessage)
        {
            this.LogWinBoardMessage("In", strMessage);
            WinBoard.ProcessInputEvent(strMessage);
        }

        /// <summary>
        /// The handle win board output sent message.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private void HandleWinBoardOutputSentMessage(string strMessage)
        {
            this.LogWinBoardMessage("Out", strMessage);
        }

        /// <summary>
        /// The handle win board quit.
        /// </summary>
        private void HandleWinBoardQuit()
        {
            this.Close();
        }

        /// <summary>
        /// The handle win board time updated.
        /// </summary>
        private void HandleWinBoardTimeUpdated()
        {
            this.RenderClocks();
        }

        /// <summary>
        /// The handle win board timeout.
        /// </summary>
        private void HandleWinBoardTimeout()
        {
            this.LogWinBoardMessage("dbg", "Timeout received");
            this.StartNormalGame();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));
            this.sbr = new StatusBar();
            this.imgPieces = new ImageList(this.components);
            this.mnu = new MainMenu(this.components);
            this.mnuFile = new MenuItem();
            this.mnuNew = new MenuItem();
            this.mnuOpen = new MenuItem();
            this.mnuSave = new MenuItem();
            this.mnuSep1 = new MenuItem();
            this.mnuExit = new MenuItem();
            this.mnuEdit = new MenuItem();
            this.mnuUndoMove = new MenuItem();
            this.mnuRedoMove = new MenuItem();
            this.mnuUndoAllMoves = new MenuItem();
            this.mnuRedoAllMoves = new MenuItem();
            this.mnuSep2 = new MenuItem();
            this.mnuCopyFEN = new MenuItem();
            this.mnuPasteFEN = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.mnuEditBoardPosition = new MenuItem();
            this.mnuView = new MenuItem();
            this.mnuFlipBoard = new MenuItem();
            this.mnuSep3 = new MenuItem();
            this.mnuShowThinking = new MenuItem();
            this.mnuDisplayMoveAnalysisTree = new MenuItem();
            this.mnuDisplayWinBoardMessageLog = new MenuItem();
            this.mnuGame = new MenuItem();
            this.mnuPausePlay = new MenuItem();
            this.mnuResumePlay = new MenuItem();
            this.mnuComputer = new MenuItem();
            this.mnuDifficulty = new MenuItem();
            this.menuItem1 = new MenuItem();
            this.mnuThink = new MenuItem();
            this.mnuMoveNow = new MenuItem();
            this.mnuHelp = new MenuItem();
            this.mnuAbout = new MenuItem();
            this.tbr = new ToolBar();
            this.tbrNew = new ToolBarButton();
            this.tbrOpen = new ToolBarButton();
            this.tbrSave = new ToolBarButton();
            this.tbrSep1 = new ToolBarButton();
            this.tbrUndoAllMoves = new ToolBarButton();
            this.tbrUndoMove = new ToolBarButton();
            this.tbrResumePlay = new ToolBarButton();
            this.tbrPausePlay = new ToolBarButton();
            this.tbrRedoMove = new ToolBarButton();
            this.tbrRedoAllMoves = new ToolBarButton();
            this.tbrSep2 = new ToolBarButton();
            this.tbrFlipBoard = new ToolBarButton();
            this.tbrSep3 = new ToolBarButton();
            this.tbrThink = new ToolBarButton();
            this.tbrSep4 = new ToolBarButton();
            this.tbrMoveNow = new ToolBarButton();
            this.imgToolMenus = new ImageList(this.components);
            this.pnlMain = new Panel();
            this.numPerftDepth = new NumericUpDown();
            this.btnPerft = new Button();
            this.lblGamePaused = new Label();
            this.btnPrune = new Button();
            this.txtOutput = new TextBox();
            this.lvwMoveHistory = new ListView();
            this.lvcMoveNo = new ColumnHeader();
            this.lvcTime = new ColumnHeader();
            this.lvcMove = new ColumnHeader();
            this.btnXMLtoOB = new Button();
            this.btnPGNtoXML = new Button();
            this.label5 = new Label();
            this.label3 = new Label();
            this.lblBlacksCaptures = new Label();
            this.lblWhitesCaptures = new Label();
            this.lblPlayer = new Label();
            this.lblBlackClock = new Label();
            this.lblBlackPosition = new Label();
            this.lblBlackScore = new Label();
            this.cboIntellegenceBlack = new ComboBox();
            this.lblBlackPoints = new Label();
            this.lblWhiteClock = new Label();
            this.lblWhitePosition = new Label();
            this.lblWhiteScore = new Label();
            this.cboIntellegenceWhite = new ComboBox();
            this.lblWhitePoints = new Label();
            this.lblPlayerClocks = new Label();
            this.label2 = new Label();
            this.label4 = new Label();
            this.label1 = new Label();
            this.panel1 = new Panel();
            this.panel3 = new Panel();
            this.panel4 = new Panel();
            this.panel2 = new Panel();
            this.pbr = new ProgressBar();
            this.lblStage = new Label();
            this.pnlEdging = new Panel();
            this.groupBox1 = new GroupBox();
            this.imgTiles = new ImageList(this.components);
            this.timer = new Timer(this.components);
            this.imageList1 = new ImageList(this.components);
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numPerftDepth).BeginInit();
            this.SuspendLayout();

            // sbr
            this.sbr.Location = new Point(0, 523);
            this.sbr.Name = "sbr";
            this.sbr.Size = new Size(690, 16);
            this.sbr.SizingGrip = false;
            this.sbr.TabIndex = 7;

            // imgPieces
            this.imgPieces.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgPieces.ImageStream");
            this.imgPieces.TransparentColor = System.Drawing.Color.Transparent;
            this.imgPieces.Images.SetKeyName(0, string.Empty);
            this.imgPieces.Images.SetKeyName(1, string.Empty);
            this.imgPieces.Images.SetKeyName(2, string.Empty);
            this.imgPieces.Images.SetKeyName(3, string.Empty);
            this.imgPieces.Images.SetKeyName(4, string.Empty);
            this.imgPieces.Images.SetKeyName(5, string.Empty);
            this.imgPieces.Images.SetKeyName(6, string.Empty);
            this.imgPieces.Images.SetKeyName(7, string.Empty);
            this.imgPieces.Images.SetKeyName(8, string.Empty);
            this.imgPieces.Images.SetKeyName(9, string.Empty);
            this.imgPieces.Images.SetKeyName(10, string.Empty);
            this.imgPieces.Images.SetKeyName(11, string.Empty);

            // mnu
            this.mnu.MenuItems.AddRange(
                new[] { this.mnuFile, this.mnuEdit, this.mnuView, this.mnuGame, this.mnuComputer, this.mnuHelp });

            // mnuFile
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(
                new[] { this.mnuNew, this.mnuOpen, this.mnuSave, this.mnuSep1, this.mnuExit });
            this.mnuFile.Text = "&File";

            // mnuNew
            this.mnuNew.Index = 0;
            this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new EventHandler(this.mnuNew_Click);

            // mnuOpen
            this.mnuOpen.Index = 1;
            this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuOpen.Text = "&Open...";
            this.mnuOpen.Click += new EventHandler(this.mnuOpen_Click);

            // mnuSave
            this.mnuSave.Index = 2;
            this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSave.Text = "Save &As...";
            this.mnuSave.Click += new EventHandler(this.mnuSave_Click);

            // mnuSep1
            this.mnuSep1.Index = 3;
            this.mnuSep1.Text = "-";

            // mnuExit
            this.mnuExit.Index = 4;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new EventHandler(this.mnuExit_Click);

            // mnuEdit
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(
                new[]
                    {
                        this.mnuUndoMove, this.mnuRedoMove, this.mnuUndoAllMoves, this.mnuRedoAllMoves, this.mnuSep2, 
                        this.mnuCopyFEN, this.mnuPasteFEN, this.menuItem2, this.mnuEditBoardPosition
                    });
            this.mnuEdit.Text = "&Edit";

            // mnuUndoMove
            this.mnuUndoMove.Index = 0;
            this.mnuUndoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndoMove.Text = "&Undo Move";
            this.mnuUndoMove.Click += new EventHandler(this.mnuUndoMove_Click);

            // mnuRedoMove
            this.mnuRedoMove.Index = 1;
            this.mnuRedoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedoMove.Text = "&Redo Move";
            this.mnuRedoMove.Click += new EventHandler(this.mnuRedoMove_Click);

            // mnuUndoAllMoves
            this.mnuUndoAllMoves.Index = 2;
            this.mnuUndoAllMoves.Text = "U&ndo All Moves";
            this.mnuUndoAllMoves.Click += new EventHandler(this.mnuUndoAllMoves_Click);

            // mnuRedoAllMoves
            this.mnuRedoAllMoves.Index = 3;
            this.mnuRedoAllMoves.Text = "Re&do All Moves";
            this.mnuRedoAllMoves.Click += new EventHandler(this.mnuRedoAllMoves_Click);

            // mnuSep2
            this.mnuSep2.Index = 4;
            this.mnuSep2.Text = "-";

            // mnuCopyFEN
            this.mnuCopyFEN.Index = 5;
            this.mnuCopyFEN.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopyFEN.Text = "&Copy FEN Position";
            this.mnuCopyFEN.Click += new EventHandler(this.mnuCopyFEN_Click);

            // mnuPasteFEN
            this.mnuPasteFEN.Index = 6;
            this.mnuPasteFEN.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPasteFEN.Text = "&Paste FEN Position";
            this.mnuPasteFEN.Click += new EventHandler(this.mnuPasteFEN_Click);

            // menuItem2
            this.menuItem2.Index = 7;
            this.menuItem2.Text = "-";

            // mnuEditBoardPosition
            this.mnuEditBoardPosition.Index = 8;
            this.mnuEditBoardPosition.Text = "&Edit Board Position";
            this.mnuEditBoardPosition.Click += new EventHandler(this.mnuEditPosition_Click);

            // mnuView
            this.mnuView.Index = 2;
            this.mnuView.MenuItems.AddRange(
                new[]
                    {
                        this.mnuFlipBoard, this.mnuSep3, this.mnuShowThinking, this.mnuDisplayMoveAnalysisTree, 
                        this.mnuDisplayWinBoardMessageLog
                    });
            this.mnuView.Text = "&View";

            // mnuFlipBoard
            this.mnuFlipBoard.Index = 0;
            this.mnuFlipBoard.Text = "&Flip Board";
            this.mnuFlipBoard.Click += new EventHandler(this.mnuFlipBoard_Click);

            // mnuSep3
            this.mnuSep3.Index = 1;
            this.mnuSep3.Text = "-";

            // mnuShowThinking
            this.mnuShowThinking.Index = 2;
            this.mnuShowThinking.Text = "&Show Thinking";
            this.mnuShowThinking.Click += new EventHandler(this.mnuShowThinking_Click);

            // mnuDisplayMoveAnalysisTree
            this.mnuDisplayMoveAnalysisTree.Index = 3;
            this.mnuDisplayMoveAnalysisTree.Text = "S&how Move Analysis Tree";
            this.mnuDisplayMoveAnalysisTree.Click += new EventHandler(this.mnuDisplayMoveAnalysisTree_Click);

            // mnuDisplayWinBoardMessageLog
            this.mnuDisplayWinBoardMessageLog.Index = 4;
            this.mnuDisplayWinBoardMessageLog.Text = "Sh&ow WinBoard Message Log";
            this.mnuDisplayWinBoardMessageLog.Click += new EventHandler(this.mnuDisplayWinBoardMessageLog_Click);

            // mnuGame
            this.mnuGame.Index = 3;
            this.mnuGame.MenuItems.AddRange(new[] { this.mnuPausePlay, this.mnuResumePlay });
            this.mnuGame.Text = "&Game";

            // mnuPausePlay
            this.mnuPausePlay.Enabled = false;
            this.mnuPausePlay.Index = 0;
            this.mnuPausePlay.Text = "&Pause Game";
            this.mnuPausePlay.Click += new EventHandler(this.mnuPausePlay_Click);

            // mnuResumePlay
            this.mnuResumePlay.Index = 1;
            this.mnuResumePlay.Text = "&Resume Game";
            this.mnuResumePlay.Click += new EventHandler(this.mnuResumePlay_Click);

            // mnuComputer
            this.mnuComputer.Index = 4;
            this.mnuComputer.MenuItems.AddRange(
                new[] { this.mnuDifficulty, this.menuItem1, this.mnuThink, this.mnuMoveNow });
            this.mnuComputer.Text = "&Computer";

            // mnuDifficulty
            this.mnuDifficulty.Index = 0;
            this.mnuDifficulty.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.mnuDifficulty.Text = "&Difficulty...";
            this.mnuDifficulty.Click += new EventHandler(this.mnuDifficulty_Click);

            // menuItem1
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";

            // mnuThink
            this.mnuThink.Index = 2;
            this.mnuThink.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuThink.Text = "&Think";
            this.mnuThink.Click += new EventHandler(this.mnuThink_Click);

            // mnuMoveNow
            this.mnuMoveNow.Index = 3;
            this.mnuMoveNow.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuMoveNow.Text = "&Move Now";
            this.mnuMoveNow.Click += new EventHandler(this.mnuMoveNow_Click);

            // mnuHelp
            this.mnuHelp.Index = 5;
            this.mnuHelp.MenuItems.AddRange(new[] { this.mnuAbout });
            this.mnuHelp.Text = "&Help";

            // mnuAbout
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "&About SharpChess";
            this.mnuAbout.Click += new EventHandler(this.mnuAbout_Click);

            // tbr
            this.tbr.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbr.Buttons.AddRange(
                new[]
                    {
                        this.tbrNew, this.tbrOpen, this.tbrSave, this.tbrSep1, this.tbrUndoAllMoves, this.tbrUndoMove, 
                        this.tbrResumePlay, this.tbrPausePlay, this.tbrRedoMove, this.tbrRedoAllMoves, this.tbrSep2, 
                        this.tbrFlipBoard, this.tbrSep3, this.tbrThink, this.tbrSep4, this.tbrMoveNow
                    });
            this.tbr.DropDownArrows = true;
            this.tbr.ImageList = this.imgToolMenus;
            this.tbr.Location = new Point(0, 0);
            this.tbr.Name = "tbr";
            this.tbr.ShowToolTips = true;
            this.tbr.Size = new Size(690, 28);
            this.tbr.TabIndex = 32;
            this.tbr.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbr.Wrappable = false;
            this.tbr.ButtonClick += new ToolBarButtonClickEventHandler(this.tbr_ButtonClick);

            // tbrNew
            this.tbrNew.ImageIndex = 0;
            this.tbrNew.Name = "tbrNew";
            this.tbrNew.Tag = "New";
            this.tbrNew.ToolTipText = "Start a new chess game";

            // tbrOpen
            this.tbrOpen.ImageIndex = 1;
            this.tbrOpen.Name = "tbrOpen";
            this.tbrOpen.Tag = "Open";
            this.tbrOpen.ToolTipText = "Open a saved chess game";

            // tbrSave
            this.tbrSave.ImageIndex = 2;
            this.tbrSave.Name = "tbrSave";
            this.tbrSave.Tag = "Save";
            this.tbrSave.ToolTipText = "Save the current chess game";

            // tbrSep1
            this.tbrSep1.Name = "tbrSep1";
            this.tbrSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;

            // tbrUndoAllMoves
            this.tbrUndoAllMoves.ImageIndex = 6;
            this.tbrUndoAllMoves.Name = "tbrUndoAllMoves";
            this.tbrUndoAllMoves.Tag = "UndoAllMoves";
            this.tbrUndoAllMoves.ToolTipText = "Undo all moves played so far";

            // tbrUndoMove
            this.tbrUndoMove.ImageIndex = 4;
            this.tbrUndoMove.Name = "tbrUndoMove";
            this.tbrUndoMove.Tag = "UndoMove";
            this.tbrUndoMove.ToolTipText = "Undo the last move";

            // tbrResumePlay
            this.tbrResumePlay.ImageIndex = 8;
            this.tbrResumePlay.Name = "tbrResumePlay";
            this.tbrResumePlay.Tag = "ResumePlay";
            this.tbrResumePlay.ToolTipText = "Resume play";

            // tbrPausePlay
            this.tbrPausePlay.Enabled = false;
            this.tbrPausePlay.ImageIndex = 9;
            this.tbrPausePlay.Name = "tbrPausePlay";
            this.tbrPausePlay.Tag = "PausePlay";
            this.tbrPausePlay.ToolTipText = "Pause play";

            // tbrRedoMove
            this.tbrRedoMove.ImageIndex = 5;
            this.tbrRedoMove.Name = "tbrRedoMove";
            this.tbrRedoMove.Tag = "RedoMove";
            this.tbrRedoMove.ToolTipText = "Redo move";

            // tbrRedoAllMoves
            this.tbrRedoAllMoves.ImageIndex = 7;
            this.tbrRedoAllMoves.Name = "tbrRedoAllMoves";
            this.tbrRedoAllMoves.Tag = "RedoAllMoves";
            this.tbrRedoAllMoves.ToolTipText = "Redo all moves";

            // tbrSep2
            this.tbrSep2.Name = "tbrSep2";
            this.tbrSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;

            // tbrFlipBoard
            this.tbrFlipBoard.ImageIndex = 10;
            this.tbrFlipBoard.Name = "tbrFlipBoard";
            this.tbrFlipBoard.Tag = "FlipBoard";
            this.tbrFlipBoard.Text = "Flip Board";

            // tbrSep3
            this.tbrSep3.Name = "tbrSep3";
            this.tbrSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;

            // tbrThink
            this.tbrThink.ImageIndex = 3;
            this.tbrThink.Name = "tbrThink";
            this.tbrThink.Tag = "Think";
            this.tbrThink.Text = "Think";
            this.tbrThink.ToolTipText = "Make the computer play the next move";

            // tbrSep4
            this.tbrSep4.Name = "tbrSep4";
            this.tbrSep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;

            // tbrMoveNow
            this.tbrMoveNow.ImageIndex = 11;
            this.tbrMoveNow.Name = "tbrMoveNow";
            this.tbrMoveNow.Tag = "MoveNow";
            this.tbrMoveNow.Text = "Move Now";
            this.tbrMoveNow.ToolTipText = "Make the computer immediately play the best move it has found so far";

            // imgToolMenus
            this.imgToolMenus.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgToolMenus.ImageStream");
            this.imgToolMenus.TransparentColor = System.Drawing.Color.Transparent;
            this.imgToolMenus.Images.SetKeyName(0, string.Empty);
            this.imgToolMenus.Images.SetKeyName(1, string.Empty);
            this.imgToolMenus.Images.SetKeyName(2, string.Empty);
            this.imgToolMenus.Images.SetKeyName(3, string.Empty);
            this.imgToolMenus.Images.SetKeyName(4, string.Empty);
            this.imgToolMenus.Images.SetKeyName(5, string.Empty);
            this.imgToolMenus.Images.SetKeyName(6, string.Empty);
            this.imgToolMenus.Images.SetKeyName(7, string.Empty);
            this.imgToolMenus.Images.SetKeyName(8, string.Empty);
            this.imgToolMenus.Images.SetKeyName(9, string.Empty);
            this.imgToolMenus.Images.SetKeyName(10, string.Empty);
            this.imgToolMenus.Images.SetKeyName(11, string.Empty);

            // pnlMain
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.numPerftDepth);
            this.pnlMain.Controls.Add(this.btnPerft);
            this.pnlMain.Controls.Add(this.lblGamePaused);
            this.pnlMain.Controls.Add(this.btnPrune);
            this.pnlMain.Controls.Add(this.txtOutput);
            this.pnlMain.Controls.Add(this.lvwMoveHistory);
            this.pnlMain.Controls.Add(this.btnXMLtoOB);
            this.pnlMain.Controls.Add(this.btnPGNtoXML);
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.lblBlacksCaptures);
            this.pnlMain.Controls.Add(this.lblWhitesCaptures);
            this.pnlMain.Controls.Add(this.lblPlayer);
            this.pnlMain.Controls.Add(this.lblBlackClock);
            this.pnlMain.Controls.Add(this.lblBlackPosition);
            this.pnlMain.Controls.Add(this.lblBlackScore);
            this.pnlMain.Controls.Add(this.cboIntellegenceBlack);
            this.pnlMain.Controls.Add(this.lblBlackPoints);
            this.pnlMain.Controls.Add(this.lblWhiteClock);
            this.pnlMain.Controls.Add(this.lblWhitePosition);
            this.pnlMain.Controls.Add(this.lblWhiteScore);
            this.pnlMain.Controls.Add(this.cboIntellegenceWhite);
            this.pnlMain.Controls.Add(this.lblWhitePoints);
            this.pnlMain.Controls.Add(this.lblPlayerClocks);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.pbr);
            this.pnlMain.Controls.Add(this.lblStage);
            this.pnlMain.Controls.Add(this.pnlEdging);
            this.pnlMain.Location = new Point(0, 26);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(690, 496);
            this.pnlMain.TabIndex = 33;

            // numPerftDepth
            this.numPerftDepth.Location = new Point(64, 416);
            this.numPerftDepth.Maximum = new decimal(new[] { 10, 0, 0, 0 });
            this.numPerftDepth.Minimum = new decimal(new[] { 1, 0, 0, 0 });
            this.numPerftDepth.Name = "numPerftDepth";
            this.numPerftDepth.Size = new Size(40, 20);
            this.numPerftDepth.TabIndex = 143;
            this.numPerftDepth.Value = new decimal(new[] { 1, 0, 0, 0 });
            this.numPerftDepth.Visible = false;

            // btnPerft
            this.btnPerft.Location = new Point(104, 416);
            this.btnPerft.Name = "btnPerft";
            this.btnPerft.Size = new Size(75, 23);
            this.btnPerft.TabIndex = 142;
            this.btnPerft.Text = "Perft";
            this.btnPerft.Visible = false;
            this.btnPerft.Click += new EventHandler(this.btnPerft_Click);

            // lblGamePaused
            this.lblGamePaused.Font = new Font(
                "Microsoft Sans Serif", 
                12F, 
                (System.Drawing.FontStyle)(System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic), 
                System.Drawing.GraphicsUnit.Point, 
                (byte)0);
            this.lblGamePaused.Location = new Point(40, 168);
            this.lblGamePaused.Name = "lblGamePaused";
            this.lblGamePaused.Size = new Size(320, 24);
            this.lblGamePaused.TabIndex = 141;
            this.lblGamePaused.Text = "Game Paused";
            this.lblGamePaused.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGamePaused.Visible = false;

            // btnPrune
            this.btnPrune.Location = new Point(200, 416);
            this.btnPrune.Name = "btnPrune";
            this.btnPrune.Size = new Size(75, 23);
            this.btnPrune.TabIndex = 140;
            this.btnPrune.Text = "Prune";
            this.btnPrune.Visible = false;
            this.btnPrune.Click += new EventHandler(this.btnPrune_Click);

            // txtOutput
            this.txtOutput.Location = new Point(280, 416);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new Size(120, 40);
            this.txtOutput.TabIndex = 139;
            this.txtOutput.Visible = false;

            // lvwMoveHistory
            this.lvwMoveHistory.BackColor = System.Drawing.SystemColors.Control;
            this.lvwMoveHistory.Columns.AddRange(new[] { this.lvcMoveNo, this.lvcTime, this.lvcMove });
            this.lvwMoveHistory.Location = new Point(400, 176);
            this.lvwMoveHistory.Name = "lvwMoveHistory";
            this.lvwMoveHistory.Size = new Size(248, 128);
            this.lvwMoveHistory.TabIndex = 39;
            this.lvwMoveHistory.UseCompatibleStateImageBehavior = false;
            this.lvwMoveHistory.View = System.Windows.Forms.View.Details;

            // lvcMoveNo
            this.lvcMoveNo.Text = "#";
            this.lvcMoveNo.Width = 19;

            // lvcTime
            this.lvcTime.Text = "Time";
            this.lvcTime.Width = 56;

            // lvcMove
            this.lvcMove.Text = "Move";
            this.lvcMove.Width = 152;

            // btnXMLtoOB
            this.btnXMLtoOB.Location = new Point(496, 416);
            this.btnXMLtoOB.Name = "btnXMLtoOB";
            this.btnXMLtoOB.Size = new Size(75, 23);
            this.btnXMLtoOB.TabIndex = 138;
            this.btnXMLtoOB.Text = "XML to OB";
            this.btnXMLtoOB.Visible = false;
            this.btnXMLtoOB.Click += new EventHandler(this.btnXMLtoOB_Click);

            // btnPGNtoXML
            this.btnPGNtoXML.Location = new Point(408, 416);
            this.btnPGNtoXML.Name = "btnPGNtoXML";
            this.btnPGNtoXML.Size = new Size(75, 23);
            this.btnPGNtoXML.TabIndex = 137;
            this.btnPGNtoXML.Text = "PGN to XML";
            this.btnPGNtoXML.Visible = false;
            this.btnPGNtoXML.Click += new EventHandler(this.btnPGNtoXML_Click);

            // label5
            this.label5.Font = new Font(
                "Microsoft Sans Serif", 
                8.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)0);
            this.label5.Location = new Point(552, 8);
            this.label5.Name = "label5";
            this.label5.Size = new Size(96, 23);
            this.label5.TabIndex = 136;
            this.label5.Text = "Black";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // label3
            this.label3.Font = new Font(
                "Microsoft Sans Serif", 
                8.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)0);
            this.label3.Location = new Point(448, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(96, 23);
            this.label3.TabIndex = 135;
            this.label3.Text = "White";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblBlacksCaptures
            this.lblBlacksCaptures.BackColor = System.Drawing.Color.Transparent;
            this.lblBlacksCaptures.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlacksCaptures.CausesValidation = false;
            this.lblBlacksCaptures.Font = new Font(
                "Microsoft Sans Serif", 
                12F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)0);
            this.lblBlacksCaptures.Location = new Point(644, 428);
            this.lblBlacksCaptures.Name = "lblBlacksCaptures";
            this.lblBlacksCaptures.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlacksCaptures.Size = new Size(42, 42);
            this.lblBlacksCaptures.TabIndex = 134;
            this.lblBlacksCaptures.Text = "0";
            this.lblBlacksCaptures.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblWhitesCaptures
            this.lblWhitesCaptures.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitesCaptures.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitesCaptures.CausesValidation = false;
            this.lblWhitesCaptures.Font = new Font(
                "Microsoft Sans Serif", 
                12F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)0);
            this.lblWhitesCaptures.Location = new Point(644, 384);
            this.lblWhitesCaptures.Name = "lblWhitesCaptures";
            this.lblWhitesCaptures.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitesCaptures.Size = new Size(42, 42);
            this.lblWhitesCaptures.TabIndex = 133;
            this.lblWhitesCaptures.Text = "0";
            this.lblWhitesCaptures.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblPlayer
            this.lblPlayer.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer.Location = new Point(392, 32);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new Size(48, 24);
            this.lblPlayer.TabIndex = 131;
            this.lblPlayer.Text = "Player";
            this.lblPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // lblBlackClock
            this.lblBlackClock.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackClock.CausesValidation = false;
            this.lblBlackClock.ForeColor = System.Drawing.Color.Black;
            this.lblBlackClock.Location = new Point(552, 64);
            this.lblBlackClock.Name = "lblBlackClock";
            this.lblBlackClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackClock.Size = new Size(96, 23);
            this.lblBlackClock.TabIndex = 130;
            this.lblBlackClock.Text = ":";
            this.lblBlackClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblBlackPosition
            this.lblBlackPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPosition.CausesValidation = false;
            this.lblBlackPosition.Location = new Point(552, 144);
            this.lblBlackPosition.Name = "lblBlackPosition";
            this.lblBlackPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPosition.Size = new Size(96, 23);
            this.lblBlackPosition.TabIndex = 128;
            this.lblBlackPosition.Text = "0";
            this.lblBlackPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblBlackScore
            this.lblBlackScore.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackScore.CausesValidation = false;
            this.lblBlackScore.Location = new Point(552, 96);
            this.lblBlackScore.Name = "lblBlackScore";
            this.lblBlackScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackScore.Size = new Size(96, 23);
            this.lblBlackScore.TabIndex = 127;
            this.lblBlackScore.Text = "0";
            this.lblBlackScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cboIntellegenceBlack
            this.cboIntellegenceBlack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceBlack.Items.AddRange(new object[] { "Human", "Computer" });
            this.cboIntellegenceBlack.Location = new Point(552, 32);
            this.cboIntellegenceBlack.Name = "cboIntellegenceBlack";
            this.cboIntellegenceBlack.Size = new Size(96, 21);
            this.cboIntellegenceBlack.TabIndex = 126;
            this.cboIntellegenceBlack.SelectedIndexChanged +=
                new EventHandler(this.cboIntellegenceBlack_SelectedIndexChanged);

            // lblBlackPoints
            this.lblBlackPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPoints.CausesValidation = false;
            this.lblBlackPoints.Location = new Point(552, 120);
            this.lblBlackPoints.Name = "lblBlackPoints";
            this.lblBlackPoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPoints.Size = new Size(96, 23);
            this.lblBlackPoints.TabIndex = 125;
            this.lblBlackPoints.Text = "0";
            this.lblBlackPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblWhiteClock
            this.lblWhiteClock.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteClock.CausesValidation = false;
            this.lblWhiteClock.ForeColor = System.Drawing.Color.Black;
            this.lblWhiteClock.Location = new Point(448, 64);
            this.lblWhiteClock.Name = "lblWhiteClock";
            this.lblWhiteClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteClock.Size = new Size(96, 23);
            this.lblWhiteClock.TabIndex = 124;
            this.lblWhiteClock.Text = ":";
            this.lblWhiteClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblWhitePosition
            this.lblWhitePosition.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePosition.CausesValidation = false;
            this.lblWhitePosition.Location = new Point(448, 144);
            this.lblWhitePosition.Name = "lblWhitePosition";
            this.lblWhitePosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePosition.Size = new Size(96, 23);
            this.lblWhitePosition.TabIndex = 122;
            this.lblWhitePosition.Text = "0";
            this.lblWhitePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblWhiteScore
            this.lblWhiteScore.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhiteScore.CausesValidation = false;
            this.lblWhiteScore.Location = new Point(448, 96);
            this.lblWhiteScore.Name = "lblWhiteScore";
            this.lblWhiteScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteScore.Size = new Size(96, 23);
            this.lblWhiteScore.TabIndex = 121;
            this.lblWhiteScore.Text = "0";
            this.lblWhiteScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cboIntellegenceWhite
            this.cboIntellegenceWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceWhite.Items.AddRange(new object[] { "Human", "Computer" });
            this.cboIntellegenceWhite.Location = new Point(448, 32);
            this.cboIntellegenceWhite.Name = "cboIntellegenceWhite";
            this.cboIntellegenceWhite.Size = new Size(96, 21);
            this.cboIntellegenceWhite.TabIndex = 120;
            this.cboIntellegenceWhite.SelectedIndexChanged +=
                new EventHandler(this.cboIntellegenceWhite_SelectedIndexChanged);

            // lblWhitePoints
            this.lblWhitePoints.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePoints.CausesValidation = false;
            this.lblWhitePoints.Location = new Point(448, 120);
            this.lblWhitePoints.Name = "lblWhitePoints";
            this.lblWhitePoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePoints.Size = new Size(96, 23);
            this.lblWhitePoints.TabIndex = 119;
            this.lblWhitePoints.Text = "0";
            this.lblWhitePoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblPlayerClocks
            this.lblPlayerClocks.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerClocks.Location = new Point(392, 64);
            this.lblPlayerClocks.Name = "lblPlayerClocks";
            this.lblPlayerClocks.Size = new Size(48, 24);
            this.lblPlayerClocks.TabIndex = 118;
            this.lblPlayerClocks.Text = "Clock";
            this.lblPlayerClocks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // label2
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new Point(392, 144);
            this.label2.Name = "label2";
            this.label2.Size = new Size(48, 24);
            this.label2.TabIndex = 116;
            this.label2.Text = "Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // label4
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new Point(400, 96);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 24);
            this.label4.TabIndex = 115;
            this.label4.Text = "Score";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // label1
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new Point(400, 120);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 24);
            this.label1.TabIndex = 114;
            this.label1.Text = "Points";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // panel1
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panel1.BackgroundImage");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new Point(32, 350);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(342, 8);
            this.panel1.TabIndex = 55;

            // panel3
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.BackgroundImage = (System.Drawing.Image)resources.GetObject("panel3.BackgroundImage");
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new Point(366, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(8, 352);
            this.panel3.TabIndex = 57;

            // panel4
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.BackgroundImage = (System.Drawing.Image)resources.GetObject("panel4.BackgroundImage");
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new Point(24, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(342, 8);
            this.panel4.TabIndex = 58;

            // panel2
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = (System.Drawing.Image)resources.GetObject("panel2.BackgroundImage");
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new Point(24, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(8, 350);
            this.panel2.TabIndex = 56;

            // pbr
            this.pbr.Location = new Point(0, 472);
            this.pbr.Name = "pbr";
            this.pbr.Size = new Size(688, 23);
            this.pbr.TabIndex = 54;

            // lblStage
            this.lblStage.BackColor = System.Drawing.Color.Transparent;
            this.lblStage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStage.CausesValidation = false;
            this.lblStage.Location = new Point(376, 358);
            this.lblStage.Name = "lblStage";
            this.lblStage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStage.Size = new Size(312, 23);
            this.lblStage.TabIndex = 50;
            this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // pnlEdging
            this.pnlEdging.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEdging.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEdging.Location = new Point(28, 12);
            this.pnlEdging.Name = "pnlEdging";
            this.pnlEdging.Size = new Size(340, 340);
            this.pnlEdging.TabIndex = 35;

            // groupBox1
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Location = new Point(0, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(800, 8);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;

            // imgTiles
            this.imgTiles.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgTiles.ImageStream");
            this.imgTiles.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTiles.Images.SetKeyName(0, string.Empty);
            this.imgTiles.Images.SetKeyName(1, string.Empty);

            // timer
            this.timer.Interval = 333;
            this.timer.Tick += new EventHandler(this.timer_Tick);

            // imageList1
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;

            // frmMain
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(690, 539);
            this.Controls.Add(this.tbr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sbr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Menu = this.mnu;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpChess";
            this.Closing += new CancelEventHandler(this.frmMain_Closing);
            this.Load += new EventHandler(this.frmMain_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numPerftDepth).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Load the m_acurPieceCursors array with the cursors that are embedded in this assembly
        /// </summary>
        private void LoadCursors()
        {
            Assembly asmMain = Assembly.GetExecutingAssembly();
            string strAsmName = asmMain.GetName().Name;
            if (strAsmName == "SharpChess2")
            {
                strAsmName = "SharpChess"; // Fix the assembly name with regard to the executable name
            }

            // The relative pathname of an embedded resource begins with a period
            string strPath = strAsmName + ".Cursors.";

            this.m_acurPieceCursors[0] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackBishop.cur"));
            this.m_acurPieceCursors[1] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteBishop.cur"));
            this.m_acurPieceCursors[2] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackRook.cur"));
            this.m_acurPieceCursors[3] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteRook.cur"));
            this.m_acurPieceCursors[4] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackKing.cur"));
            this.m_acurPieceCursors[5] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteKing.cur"));
            this.m_acurPieceCursors[6] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackKnight.cur"));
            this.m_acurPieceCursors[7] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteKnight.cur"));
            this.m_acurPieceCursors[8] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackPawn.cur"));
            this.m_acurPieceCursors[9] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhitePawn.cur"));
            this.m_acurPieceCursors[10] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackQueen.cur"));
            this.m_acurPieceCursors[11] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteQueen.cur"));
        }

        /// <summary>
        /// The log win board message.
        /// </summary>
        /// <param name="strDirection">
        /// The str direction.
        /// </param>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private void LogWinBoardMessage(string strDirection, string strMessage)
        {
            this.m_formWinBoard.LogWinBoardMessage(strDirection, strMessage);
        }

        /// <summary>
        /// The move now.
        /// </summary>
        private void MoveNow()
        {
            if (Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
            {
                Game.PlayerToPlay.ForceImmediateMove();
            }
        }

        /// <summary>
        /// The new game.
        /// </summary>
        private void NewGame()
        {
            frmDifficulty formDifficulty = new frmDifficulty();
            formDifficulty.ShowDialog(this);
            if (formDifficulty.Confirmed)
            {
                Game.New();
            }
        }

        /// <summary>
        /// The open game.
        /// </summary>
        private void OpenGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Load a saved chess game";
            openFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
            openFileDialog.FilterIndex = 2;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName != string.Empty)
                {
                    this.lvwMoveHistory.Items.Clear();
                    Game.Load(openFileDialog.FileName);
                }
            }

            this.OrientBoard();
            this.SetFormState();
        }

        /// <summary>
        /// The orient board.
        /// </summary>
        private void OrientBoard()
        {
            PictureBox picSquare;
            Square square;

            for (int intOrdinal = 0; intOrdinal < Board.SquareCount; intOrdinal++)
            {
                square = Board.GetSquare(intOrdinal);

                if (square != null)
                {
                    picSquare = this.m_picSquares[square.File, square.Rank];
                    switch (Board.Orientation)
                    {
                        case Board.OrientationNames.White:
                            picSquare.Left = square.File * SQUARE_SIZE + 1;
                            picSquare.Top = (Board.RankCount - 1) * SQUARE_SIZE - square.Rank * SQUARE_SIZE + 1;
                            break;

                        case Board.OrientationNames.Black:
                            picSquare.Left = (Board.FileCount - 1) * SQUARE_SIZE - square.File * SQUARE_SIZE + 1;
                            picSquare.Top = square.Rank * SQUARE_SIZE + 1;
                            break;
                    }
                }
            }

            // 29Mar05 Nimzo - Made the rank and file labels flip also.
            for (int indCtrl = 0; indCtrl < this.pnlMain.Controls.Count; indCtrl++)
            {
                // new
                // Position each label of coordinates according to the orientation
                string strName = this.pnlMain.Controls[indCtrl].Name;
                if (strName.StartsWith("lblFile"))
                {
                    // For the hard-coded constants "lblFile" and "+ 30" see CreateBoard()
                    int iFileSize = Convert.ToInt32(strName.Substring(7, strName.Length - 7)) * SQUARE_SIZE;
                    this.pnlMain.Controls[indCtrl].Left = ((Board.Orientation == Board.OrientationNames.White)
                                                               ? iFileSize
                                                               : (Board.FileCount - 1) * SQUARE_SIZE - iFileSize) + 30;
                }
                else if (strName.StartsWith("lblRank"))
                {
                    // For the hard-coded constants "lblRank" and "+ 16" see CreateBoard()
                    int iRankSize = Convert.ToInt32(strName.Substring(7, strName.Length - 7)) * SQUARE_SIZE;
                    this.pnlMain.Controls[indCtrl].Top = ((Board.Orientation == Board.OrientationNames.White)
                                                              ? (Board.RankCount - 1) * SQUARE_SIZE - iRankSize
                                                              : iRankSize) + 16;
                }
            }
        }

        /// <summary>
        /// The pause play.
        /// </summary>
        private void PausePlay()
        {
            Game.PausePlay();
        }

        /// <summary>
        /// The player_ move considered.
        /// </summary>
        private void Player_MoveConsidered()
        {
            delegatetypePlayer_MoveConsideredHandler MoveConsideredPointer = this.Player_MoveConsideredHandler;
            this.BeginInvoke(MoveConsideredPointer, null);
        }

        /// <summary>
        /// The player_ move considered handler.
        /// </summary>
        private void Player_MoveConsideredHandler()
        {
            this.RenderStatus();

            if (this.m_squareLastFrom != null)
            {
                this.m_picSquares[this.m_squareLastFrom.File, this.m_squareLastFrom.Rank].BackColor =
                    this.m_squareLastFrom.Colour == Square.enmColour.White
                         ? this.BOARD_SQUARE_COLOUR_WHITE
                         : this.BOARD_SQUARE_COLOUR_BLACK;
            }

            if (Game.ShowThinking && Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
            {
                if (Game.PlayerToPlay.CurrentMove != null)
                {
                    this.m_squareLastFrom = Game.PlayerToPlay.CurrentMove.From;

                    // m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor = System.Drawing.Color.Yellow;
                    this.m_picSquares[this.m_squareLastFrom.File, this.m_squareLastFrom.Rank].BackColor =
                        Board.GetSquare(
                            (int)this.m_picSquares[this.m_squareLastFrom.File, this.m_squareLastFrom.Rank].Tag).Colour
                        == Square.enmColour.White
                            ? this.BOARD_SQUARE_COLOUR_WHITE_BRIGHT
                            : this.BOARD_SQUARE_COLOUR_BLACK_BRIGHT;
                }
            }

            if (this.m_squareLastTo != null)
            {
                this.m_picSquares[this.m_squareLastTo.File, this.m_squareLastTo.Rank].BackColor =
                    this.m_squareLastTo.Colour == Square.enmColour.White
                         ? this.BOARD_SQUARE_COLOUR_WHITE
                         : this.BOARD_SQUARE_COLOUR_BLACK;
            }

            if (Game.ShowThinking && Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
            {
                if (Game.PlayerToPlay.CurrentMove != null)
                {
                    this.m_squareLastTo = Game.PlayerToPlay.CurrentMove.To;

                    // m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor=System.Drawing.Color.Yellow;
                    this.m_picSquares[this.m_squareLastTo.File, this.m_squareLastTo.Rank].BackColor =
                        Board.GetSquare((int)this.m_picSquares[this.m_squareLastTo.File, this.m_squareLastTo.Rank].Tag).
                            Colour == Square.enmColour.White
                            ? this.BOARD_SQUARE_COLOUR_WHITE_BRIGHT
                            : this.BOARD_SQUARE_COLOUR_BLACK_BRIGHT;
                }
            }
        }

        /// <summary>
        /// The player_ thinking beginning.
        /// </summary>
        private void Player_ThinkingBeginning()
        {
            delegatetypePlayer_ThinkingBeginningHandler ThinkingBeginningPointer = this.Player_ThinkingBeginningHandler;
            this.Invoke(ThinkingBeginningPointer, null);
        }

        /// <summary>
        /// The player_ thinking beginning handler.
        /// </summary>
        private void Player_ThinkingBeginningHandler()
        {
            this.SetFormState();
        }

        /// <summary>
        /// The redo all moves.
        /// </summary>
        private void RedoAllMoves()
        {
            Game.RedoAllMoves();
        }

        /// <summary>
        /// The redo move.
        /// </summary>
        private void RedoMove()
        {
            Game.RedoMove();
        }

        /// <summary>
        /// The remove last history item.
        /// </summary>
        private void RemoveLastHistoryItem()
        {
            this.lvwMoveHistory.Items.RemoveAt(this.lvwMoveHistory.Items.Count - 1);
            this.m_squareFrom = null;
            this.m_movesPossible = new Moves();
        }

        /// <summary>
        /// The render board.
        /// </summary>
        private void RenderBoard()
        {
            Square square;

            this.RenderBoardColours();

            for (int intOrdinal = 0; intOrdinal < Board.SquareCount; intOrdinal++)
            {
                square = Board.GetSquare(intOrdinal);

                if (square != null)
                {
                    if (square.Piece == null || square == this.m_squareFrom)
                    {
                        // || Game.IsPaused
                        this.m_picSquares[square.File, square.Rank].Image = null;
                    }
                    else
                    {
                        this.m_picSquares[square.File, square.Rank].Image = this.GetPieceImage(square.Piece);
                    }

                    this.m_picSquares[square.File, square.Rank].BorderStyle = BorderStyle.None;
                }
            }

            // Render Last Move highlights
            if (!Game.EditModeActive && Game.MoveHistory.Count > 0)
            {
                this.m_picSquares[
                    Game.MoveHistory[Game.MoveHistory.Count - 1].From.File, 
                    Game.MoveHistory[Game.MoveHistory.Count - 1].From.Rank].BorderStyle = BorderStyle.Fixed3D;
                this.m_picSquares[
                    Game.MoveHistory[Game.MoveHistory.Count - 1].To.File, 
                    Game.MoveHistory[Game.MoveHistory.Count - 1].To.Rank].BorderStyle = BorderStyle.Fixed3D;
            }

            // Render pieces taken
            for (int intIndex = 0; intIndex < 15; intIndex++)
            {
                this.m_picWhitesCaptures[intIndex].Image = null;
                this.m_picBlacksCaptures[intIndex].Image = null;
            }

            for (int intIndex = 0; intIndex < Game.PlayerWhite.CapturedEnemyPieces.Count; intIndex++)
            {
                this.m_picWhitesCaptures[intIndex].Image =
                    this.GetPieceImage(Game.PlayerWhite.CapturedEnemyPieces.Item(intIndex));
            }

            for (int intIndex = 0; intIndex < Game.PlayerBlack.CapturedEnemyPieces.Count; intIndex++)
            {
                this.m_picBlacksCaptures[intIndex].Image =
                    this.GetPieceImage(Game.PlayerBlack.CapturedEnemyPieces.Item(intIndex));
            }

            // Render player status
            if (Game.PlayerToPlay == Game.PlayerWhite)
            {
                this.lblWhiteClock.BorderStyle = BorderStyle.FixedSingle;
                this.lblBlackClock.BorderStyle = BorderStyle.None;
                this.lblWhiteClock.BackColor = Game.PlayerWhite.Status == Player.enmStatus.InCheckMate
                                                   ? Color.Red
                                                   : (Game.PlayerWhite.IsInCheck ? Color.Orange : Color.LightGray);
                this.lblBlackClock.BackColor = Color.FromName(KnownColor.Control.ToString());
            }
            else
            {
                this.lblBlackClock.BorderStyle = BorderStyle.FixedSingle;
                this.lblWhiteClock.BorderStyle = BorderStyle.None;
                this.lblWhiteClock.BackColor = Color.FromName(KnownColor.Control.ToString());
                this.lblBlackClock.BackColor = Game.PlayerBlack.Status == Player.enmStatus.InCheckMate
                                                   ? Color.Red
                                                   : (Game.PlayerBlack.IsInCheck ? Color.Orange : Color.LightGray);
            }

            this.lblBlackClock.ForeColor = Color.Black;
            this.lblWhiteClock.ForeColor = Color.Black;
            this.lblGamePaused.Visible = false;

            // Set form state
            this.lblWhitesCaptures.Text = Game.PlayerWhite.CapturedEnemyPiecesTotalBasicValue.ToString();
            this.lblBlacksCaptures.Text = Game.PlayerBlack.CapturedEnemyPiecesTotalBasicValue.ToString();

            this.lblWhitePosition.Text = Game.PlayerWhite.PositionPoints.ToString();
            this.lblBlackPosition.Text = Game.PlayerBlack.PositionPoints.ToString();

            this.lblWhitePoints.Text = Game.PlayerWhite.Points.ToString();
            this.lblBlackPoints.Text = Game.PlayerBlack.Points.ToString();

            this.lblWhiteScore.Text = Game.PlayerWhite.Score.ToString();
            this.lblBlackScore.Text = Game.PlayerBlack.Score.ToString();

            this.lblStage.Text = Game.Stage.ToString() + " Game - ";
            switch (Game.PlayerToPlay.Status)
            {
                case Player.enmStatus.Normal:
                    this.lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " to play";

                    // 	lblStage.Text = "A: " + Board.HashCodeA.ToString() + "     B: " + Board.HashCodeB.ToString();
                    break;

                case Player.enmStatus.InCheck:
                    this.lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in check!";
                    break;

                case Player.enmStatus.InCheckMate:
                    this.lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in checkmate!";
                    break;

                case Player.enmStatus.InStaleMate:
                    this.lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in stalemate!";
                    break;
            }

            // Update move history
            while (this.lvwMoveHistory.Items.Count < Game.MoveHistory.Count)
            {
                this.AddMoveToHistory(Game.MoveHistory[this.lvwMoveHistory.Items.Count]);
            }

            while (this.lvwMoveHistory.Items.Count > Game.MoveHistory.Count)
            {
                this.RemoveLastHistoryItem();
            }

            this.SetFormState();

            this.RenderStatus();

            this.Text = Application.ProductName + " - " + Game.FileName;

            this.Refresh();
        }

        /// <summary>
        /// The render board colours.
        /// </summary>
        private void RenderBoardColours()
        {
            Square square;

            for (int intOrdinal = 0; intOrdinal < Board.SquareCount; intOrdinal++)
            {
                square = Board.GetSquare(intOrdinal);

                if (square != null)
                {
                    if (square.Colour == Square.enmColour.White)
                    {
                        this.m_picSquares[square.File, square.Rank].BackColor = this.BOARD_SQUARE_COLOUR_WHITE;
                    }
                    else
                    {
                        this.m_picSquares[square.File, square.Rank].BackColor = this.BOARD_SQUARE_COLOUR_BLACK;
                    }
                }
            }

            // Render selection highlights
            if (this.m_squareFrom != null)
            {
                foreach (Move move in this.m_movesPossible)
                {
                    this.m_picSquares[move.To.File, move.To.Rank].BackColor =
                        Board.GetSquare((int)this.m_picSquares[move.To.File, move.To.Rank].Tag).Colour
                         == Square.enmColour.White
                             ? this.BOARD_SQUARE_COLOUR_WHITE_BRIGHT
                             : this.BOARD_SQUARE_COLOUR_BLACK_BRIGHT;
                }
            }
        }

        /// <summary>
        /// The render clocks.
        /// </summary>
        private void RenderClocks()
        {
            if (WinBoard.Active)
            {
                this.lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeRemaining.Hours.ToString().PadLeft(2, '0') + ":"
                                          + Game.PlayerWhite.Clock.TimeRemaining.Minutes.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerWhite.Clock.TimeRemaining.Seconds.ToString().PadLeft(2, '0');
                this.lblBlackClock.Text = Game.PlayerBlack.Clock.TimeRemaining.Hours.ToString().PadLeft(2, '0') + ":"
                                          + Game.PlayerBlack.Clock.TimeRemaining.Minutes.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerBlack.Clock.TimeRemaining.Seconds.ToString().PadLeft(2, '0');
            }
            else
            {
                this.lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeElapsedDisplay.Hours.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerWhite.Clock.TimeElapsedDisplay.Minutes.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerWhite.Clock.TimeElapsedDisplay.Seconds.ToString().PadLeft(2, '0');
                this.lblBlackClock.Text = Game.PlayerBlack.Clock.TimeElapsedDisplay.Hours.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerBlack.Clock.TimeElapsedDisplay.Minutes.ToString().PadLeft(2, '0')
                                          + ":"
                                          + Game.PlayerBlack.Clock.TimeElapsedDisplay.Seconds.ToString().PadLeft(2, '0');
            }

            this.lblWhiteClock.Refresh();
            this.lblBlackClock.Refresh();
        }

        /// <summary>
        /// The render move analysis.
        /// </summary>
        private void RenderMoveAnalysis()
        {
            this.m_formMoveAnalysis.RenderMoveAnalysis();
        }

        /// <summary>
        /// The render status.
        /// </summary>
        private void RenderStatus()
        {
            Player playerToPlay = Game.PlayerToPlay;

            string strMsg = string.Empty;

            if (playerToPlay.IsThinking)
            {
                strMsg += Game.PlayerToPlay.IsPondering ? "Pondering..." : "Thinking...";

                if (Game.ShowThinking)
                {
                    strMsg += "Ply: " + playerToPlay.SearchDepth.ToString() + "/"
                              + playerToPlay.MaxSearchDepth.ToString();
                    strMsg += ". Move: " + playerToPlay.SearchPositionNo.ToString() + "/"
                              + playerToPlay.TotalPositionsToSearch.ToString();
                }

                if (!Game.PlayerToPlay.IsPondering)
                {
                    strMsg += ". Secs: " + ((int)playerToPlay.ThinkingTimeRemaining.TotalSeconds).ToString() + "/"
                              + ((int)playerToPlay.ThinkingTimeAllotted.TotalSeconds).ToString();
                }

                if (Game.ShowThinking)
                {
                    strMsg += " Pos: " + playerToPlay.PositionsSearched + " Q: " + playerToPlay.MaxQuiesDepth + " E: "
                              + playerToPlay.MaxExtensions;
                    strMsg += " P/S: " + playerToPlay.PositionsPerSecond.ToString();
                    if (!Game.PlayerToPlay.IsPondering)
                    {
                        if (playerToPlay.PrincipalVariation != null && playerToPlay.PrincipalVariation.Count > 0)
                        {
                            strMsg += " Scr:" + playerToPlay.PrincipalVariation[0].Score;
                        }

                        strMsg += " " + playerToPlay.PrincipalVariationText;
                    }
                }
            }
            else
            {
                if (Game.MoveHistory.Count > 0)
                {
                    // 					strMsg += "Last move: " + Game.MoveHistory.Last.Piece.Player.Colour.ToString() + ": " + Game.MoveHistory.Last.Description;
                }
            }

            if (!Game.ShowThinking || !Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering)
            {
                this.pbr.Maximum = 0;
                this.pbr.Value = 0;
            }
            else
            {
                this.pbr.Maximum = Math.Max(playerToPlay.TotalPositionsToSearch, playerToPlay.SearchPositionNo);
                this.pbr.Value = playerToPlay.SearchPositionNo;
            }

            if (strMsg != string.Empty && this.sbr.Text != strMsg)
            {
                this.sbr.Text = strMsg;
            }
        }

        /// <summary>
        /// The resume play.
        /// </summary>
        private void ResumePlay()
        {
            Game.ResumePlay();
        }

        /// <summary>
        /// The save game.
        /// </summary>
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.FileName = Game.FileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName != string.Empty)
                {
                    Game.Save(saveFileDialog.FileName);
                }
            }

            this.Text = Application.ProductName + " - " + Game.FileName;
        }

        /// <summary>
        /// The set form state.
        /// </summary>
        private void SetFormState()
        {
            this.mnuNew.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.mnuOpen.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.mnuSave.Enabled = !Game.EditModeActive
                                   && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.mnuUndoMove.Enabled = !Game.EditModeActive && !WinBoard.Active
                                       &&
                                       ((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering)
                                         && Game.MoveHistory.Count > 0);
            this.mnuRedoMove.Enabled = !Game.EditModeActive && !WinBoard.Active
                                       &&
                                       ((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering)
                                         && Game.MoveRedoList.Count > 0);
            this.mnuUndoAllMoves.Enabled = !Game.EditModeActive && !WinBoard.Active && this.mnuUndoMove.Enabled;
            this.mnuRedoAllMoves.Enabled = !Game.EditModeActive && !WinBoard.Active && this.mnuRedoMove.Enabled;
            this.mnuEditBoardPosition.Enabled = !WinBoard.Active;
            this.mnuPasteFEN.Enabled = !WinBoard.Active
                                       && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.mnuThink.Enabled = !Game.EditModeActive && !WinBoard.Active
                                    &&
                                    ((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering)
                                      && Game.PlayerToPlay.CanMove);
            this.mnuMoveNow.Enabled = !Game.EditModeActive && !WinBoard.Active
                                      && (Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering);
            this.mnuResumePlay.Enabled = !Game.EditModeActive && !WinBoard.Active && Game.IsPaused
                                         && Game.PlayerToPlay.CanMove;
            this.mnuPausePlay.Enabled = !Game.EditModeActive && !WinBoard.Active && (!Game.IsPaused);
            this.mnuDifficulty.Enabled = !Game.EditModeActive && !WinBoard.Active
                                         && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.mnuGame.Enabled = !Game.EditModeActive;
            this.mnuComputer.Enabled = !Game.EditModeActive;
            this.mnuShowThinking.Enabled = !Game.EditModeActive;

            this.tbrNew.Enabled = this.mnuNew.Enabled;
            this.tbrOpen.Enabled = this.mnuOpen.Enabled;
            this.tbrSave.Enabled = this.mnuSave.Enabled;
            this.tbrUndoMove.Enabled = this.mnuUndoMove.Enabled;
            this.tbrRedoMove.Enabled = this.mnuRedoMove.Enabled;
            this.tbrUndoAllMoves.Enabled = this.mnuUndoAllMoves.Enabled;
            this.tbrRedoAllMoves.Enabled = this.mnuRedoAllMoves.Enabled;
            this.tbrThink.Enabled = this.mnuThink.Enabled;
            this.tbrMoveNow.Enabled = this.mnuMoveNow.Enabled;
            this.tbrResumePlay.Enabled = this.mnuResumePlay.Enabled;
            this.tbrPausePlay.Enabled = this.mnuPausePlay.Enabled;

            this.cboIntellegenceWhite.Enabled = !WinBoard.Active
                                                && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            this.cboIntellegenceBlack.Enabled = !WinBoard.Active
                                                && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);

            foreach (PictureBox pic in this.m_picSquares)
            {
                pic.Enabled = !WinBoard.Active && Game.PlayerToPlay.CanMove; // && (!Game.IsPaused)
            }

            this.cboIntellegenceWhite.SelectedIndex = Game.PlayerWhite.Intellegence == Player.enmIntellegence.Human
                                                          ? INTELLEGENCE_HUMAN
                                                          : INTELLEGENCE_COMPUTER;
            this.cboIntellegenceBlack.SelectedIndex = Game.PlayerBlack.Intellegence == Player.enmIntellegence.Human
                                                          ? INTELLEGENCE_HUMAN
                                                          : INTELLEGENCE_COMPUTER;
        }

        /// <summary>
        /// Change Top and Height to hide or not the evaluation of the position
        /// </summary>
        /// <remarks>
        /// If <see cref="Game.ShowThinking"/>, show the evaluation of the position
        /// </remarks>
        private void SizeHistoryPane()
        {
            // The hard-coded constants allow to see the entire last visible row
            this.lvwMoveHistory.Top = Game.ShowThinking
                                          ? this.lblWhitePosition.Top + this.lblWhitePosition.Height + 13
                                          : this.lblWhiteClock.Top + this.lblWhiteClock.Height + 9;
            this.lvwMoveHistory.Height = this.lblStage.Top - this.lvwMoveHistory.Top;
        }

        /// <summary>
        /// The start normal game.
        /// </summary>
        private void StartNormalGame()
        {
            if (!Game.LoadBackup())
            {
                frmDifficulty formDifficulty = new frmDifficulty();
                formDifficulty.ShowDialog(this);
            }

            Game.StartNormalGame();

            this.OrientBoard();
            this.RenderBoard();
            this.RenderClocks();
            this.SetFormState();
            this.timer.Start();
        }

        /// <summary>
        /// The think.
        /// </summary>
        private void Think()
        {
            Game.Think();
        }

        /// <summary>
        /// The undo all moves.
        /// </summary>
        private void UndoAllMoves()
        {
            Game.UndoAllMoves();
        }

        /// <summary>
        /// The undo move.
        /// </summary>
        private void UndoMove()
        {
            Game.UndoMove();
        }

        /// <summary>
        /// The win board_ input received.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private void WinBoard_InputReceived(string strMessage)
        {
            delegatetypeWinBoardMessageHandler WinBoardMessageHandlerPointer = this.HandleWinBoardInputReceivedMessage;

            object[] oParams = { strMessage };
            this.Invoke(WinBoardMessageHandlerPointer, oParams);
        }

        /// <summary>
        /// The win board_ output sent.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private void WinBoard_OutputSent(string strMessage)
        {
            delegatetypeWinBoardMessageHandler WinBoardMessageHandlerPointer = this.HandleWinBoardOutputSentMessage;

            object[] oParams = { strMessage };
            this.Invoke(WinBoardMessageHandlerPointer, oParams);
        }

        /// <summary>
        /// The win board_ quit.
        /// </summary>
        private void WinBoard_Quit()
        {
            delegatetypeWinBoardStandardHandler WinBoardQuitHandlerPointer = this.HandleWinBoardQuit;
            this.BeginInvoke(WinBoardQuitHandlerPointer, null);
        }

        /// <summary>
        /// The win board_ time updated.
        /// </summary>
        private void WinBoard_TimeUpdated()
        {
            delegatetypeWinBoardStandardHandler WinBoardTimeUpdatedHandlerPointer = this.HandleWinBoardTimeUpdated;
            this.Invoke(WinBoardTimeUpdatedHandlerPointer, null);
        }

        /// <summary>
        /// The btn pg nto xm l_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnPGNtoXML_Click(object sender, EventArgs e)
        {
            // 			Game.PGNtoXML();
        }

        /// <summary>
        /// The btn perft_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnPerft_Click(object sender, EventArgs e)
        {
            Game.PlayerToPlay.Perft((int)this.numPerftDepth.Value);
            MessageBox.Show(Game.PlayerToPlay.PositionsSearched.ToString());
        }

        /// <summary>
        /// The btn prune_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnPrune_Click(object sender, EventArgs e)
        {
            this.txtOutput.Text = OpeningBookSimple.Import();
        }

        /// <summary>
        /// The btn xm lto o b_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnXMLtoOB_Click(object sender, EventArgs e)
        {
            // 			Game.XMLtoOB();
        }

        /// <summary>
        /// The cbo intellegence black_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void cboIntellegenceBlack_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.PlayerBlack.Intellegence = this.cboIntellegenceBlack.SelectedIndex == INTELLEGENCE_HUMAN
                                                ? Player.enmIntellegence.Human
                                                : Player.enmIntellegence.Computer;
        }

        /// <summary>
        /// The cbo intellegence white_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void cboIntellegenceWhite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.PlayerWhite.Intellegence = this.cboIntellegenceWhite.SelectedIndex == INTELLEGENCE_HUMAN
                                                ? Player.enmIntellegence.Human
                                                : Player.enmIntellegence.Computer;
        }

        /// <summary>
        /// The frm main_ closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void frmMain_Closing(object sender, CancelEventArgs e)
        {
            Game.TerminateGame();
        }

        /// <summary>
        /// The frm main_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            Game.PlayerWhite.MoveConsidered += this.Player_MoveConsidered;
            Game.PlayerBlack.MoveConsidered += this.Player_MoveConsidered;
            Game.PlayerWhite.ThinkingBeginning += this.Player_ThinkingBeginning;
            Game.PlayerBlack.ThinkingBeginning += this.Player_ThinkingBeginning;

            Game.BoardPositionChanged += this.Game_BoardPositionChanged;
            Game.GamePaused += this.RenderBoard;
            Game.GameResumed += this.RenderBoard;
            Game.GameSaved += this.RenderBoard;
            Game.SettingsUpdated += this.RenderBoard;

            WinBoard.WinBoardInputEvent += this.WinBoard_InputReceived;
            WinBoard.WinBoardOutputEvent += this.WinBoard_OutputSent;
            WinBoard.WinBoardQuitEvent += this.WinBoard_Quit;
            WinBoard.WinBoardTimeUpdatedEvent += this.WinBoard_TimeUpdated;

            this.m_formMoveAnalysis.MoveAnalysisClosedEvent += this.MoveAnalysisClosed;
            this.m_formWinBoard.WinBoardClosedEvent += this.WinBoardClosed;

            this.pnlEdging.Visible = false;

            this.LoadCursors();

            this.CreateBoard();

            Game.BackupGamePath = Application.StartupPath + @"\BackupGame.sharpchess";

            this.Text = Application.ProductName + " - " + Game.FileName;
            this.AssignMenuChecks();
            this.SizeHistoryPane();

            this.OrientBoard();
            this.RenderBoard();
            this.RenderClocks();

            WinBoard.DetermineStatus();
            if (WinBoard.Active)
            {
                if (!WinBoard.ShowGUI)
                {
                    this.WindowState = FormWindowState.Minimized;
                }

                this.Show();
                this.AssignMenuChecks();
                WinBoard.StartListener();
            }
            else
            {
                this.Show();
                this.StartNormalGame();
            }

            this.pnlEdging.Visible = true;
        }

        /// <summary>
        /// The mnu about_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            frmAbout formAbout = new frmAbout();
            formAbout.ShowDialog(this);
        }

        /// <summary>
        /// The mnu copy fe n_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuCopyFEN_Click(object sender, EventArgs e)
        {
            // Put FEN position string into the clipboard
            Clipboard.SetDataObject(Fen.GetBoardPosition());
        }

        /// <summary>
        /// The mnu difficulty_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuDifficulty_Click(object sender, EventArgs e)
        {
            frmDifficulty formDifficulty = new frmDifficulty();
            formDifficulty.ShowDialog(this);
            Game.SettingsUpdate();
        }

        /// <summary>
        /// The mnu display move analysis tree_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuDisplayMoveAnalysisTree_Click(object sender, EventArgs e)
        {
            if (this.m_formMoveAnalysis.Visible)
            {
                this.m_formMoveAnalysis.Hide();
            }
            else
            {
                this.m_formMoveAnalysis.Show();
            }

            this.AssignMenuChecks();
        }

        /// <summary>
        /// The mnu display win board message log_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuDisplayWinBoardMessageLog_Click(object sender, EventArgs e)
        {
            if (this.m_formWinBoard.Visible)
            {
                this.m_formWinBoard.Hide();
            }
            else
            {
                this.m_formWinBoard.Show();
            }

            this.AssignMenuChecks();
        }

        /// <summary>
        /// The mnu edit position_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuEditPosition_Click(object sender, EventArgs e)
        {
            this.EditBoardPosition();
        }

        /// <summary>
        /// The mnu exit_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close(); // 01Apr05 Nimzo Close down threads properly before exit.
            Application.Exit();
        }

        /// <summary>
        /// The mnu flip board_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuFlipBoard_Click(object sender, EventArgs e)
        {
            this.FlipBoard();
        }

        /// <summary>
        /// The mnu move now_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuMoveNow_Click(object sender, EventArgs e)
        {
            this.MoveNow();
        }

        /// <summary>
        /// The mnu new_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            this.NewGame();
        }

        /// <summary>
        /// The mnu open_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            this.OpenGame();
        }

        /// <summary>
        /// The mnu paste fe n_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuPasteFEN_Click(object sender, EventArgs e)
        {
            // Retrieves the data from the clipboard.
            IDataObject itfDataObj = Clipboard.GetDataObject();
            if (itfDataObj.GetDataPresent(DataFormats.Text))
            {
                try
                {
                    string strFen = ((String)itfDataObj.GetData(DataFormats.Text)).Trim();
                    Game.New(strFen);
                }
                catch (Fen.ValidationException x)
                {
                    MessageBox.Show(x.FenMessage);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
            else
            {
                MessageBox.Show("Not a FEN position");
            }
        }

        /// <summary>
        /// The mnu pause play_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuPausePlay_Click(object sender, EventArgs e)
        {
            this.PausePlay();
        }

        /// <summary>
        /// The mnu redo all moves_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuRedoAllMoves_Click(object sender, EventArgs e)
        {
            this.RedoAllMoves();
        }

        /// <summary>
        /// The mnu redo move_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuRedoMove_Click(object sender, EventArgs e)
        {
            this.RedoMove();
        }

        /// <summary>
        /// The mnu resume play_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuResumePlay_Click(object sender, EventArgs e)
        {
            this.ResumePlay();
        }

        /// <summary>
        /// The mnu save_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            this.SaveGame();
        }

        /// <summary>
        /// The mnu show thinking_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuShowThinking_Click(object sender, EventArgs e)
        {
            Game.ShowThinking = !Game.ShowThinking;
            this.AssignMenuChecks();
            this.SizeHistoryPane();
        }

        /// <summary>
        /// The mnu think_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuThink_Click(object sender, EventArgs e)
        {
            this.Think();
        }

        /// <summary>
        /// The mnu undo all moves_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuUndoAllMoves_Click(object sender, EventArgs e)
        {
            this.UndoAllMoves();
        }

        /// <summary>
        /// The mnu undo move_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void mnuUndoMove_Click(object sender, EventArgs e)
        {
            this.UndoMove();
        }

        /// <summary>
        /// The pic square_ drag drop.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picSquare_DragDrop(object sender, DragEventArgs e)
        {
            int intOrdinal = Convert.ToInt32(((PictureBox)sender).Tag);
            this.m_squareTo = Board.GetSquare(intOrdinal);
        }

        /// <summary>
        /// The pic square_ drag over.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picSquare_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// The pic square_ give feedback.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picSquare_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.UseDefaultCursors)
            {
                e.UseDefaultCursors = false;
            }

            if (this.pnlEdging.Cursor != this.Cursor)
            {
                this.pnlEdging.Cursor = this.m_curPieceCursor;
            }
        }

        /// <summary>
        /// The pic square_ mouse down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picSquare_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_blnInMouseDown || e.Button != MouseButtons.Left)
            {
                return;
            }

            this.m_blnIsLeftMouseButtonDown = true;
            this.m_blnInMouseDown = true;

            if (Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
            {
                return;
            }

            Game.SuspendPondering();

            PictureBox picFrom = (PictureBox)sender;

            int intOrdinalFrom = Convert.ToInt32(picFrom.Tag);

            Square squareFrom = Board.GetSquare(intOrdinalFrom);

            Piece pieceFrom = squareFrom.Piece;
            if (pieceFrom != null && pieceFrom.Player.Colour == Game.PlayerToPlay.Colour)
            {
                picFrom.Image = null;
                picFrom.Refresh();

                this.m_curPieceCursor = this.GetPieceCursor(pieceFrom);
                this.pnlEdging.Cursor = this.m_curPieceCursor;

                // Mark possible moves
                this.m_squareFrom = squareFrom;
                this.m_squareTo = null;
                this.m_movesPossible = new Moves();
                pieceFrom.GenerateLegalMoves(this.m_movesPossible);
                this.RenderBoardColours();
                this.pnlEdging.Refresh();

                Game.ResumePondering();

                if (this.m_blnIsLeftMouseButtonDown
                    && ((PictureBox)sender).DoDragDrop(pieceFrom, DragDropEffects.Move) == DragDropEffects.Move)
                {
                    Game.SuspendPondering();

                    bool blnMoveMade = false;
                    Piece pieceTo = this.m_squareTo.Piece;

                    // Is it an empty space or enemy piece
                    if (pieceTo == null || pieceTo != null && pieceTo.Player.Colour != Game.PlayerToPlay.Colour)
                    {
                        // Check to see it the move is valid, by comparing against all possible valid moves
                        bool blnIsPromotion = false;
                        Move.enmName movenamePromotion = SharpChess.Move.enmName.NullMove;
                        foreach (Move move in this.m_movesPossible)
                        {
                            if (move.To == this.m_squareTo)
                            {
                                if (!blnIsPromotion)
                                {
                                    switch (move.Name)
                                    {
                                        case SharpChess.Move.enmName.PawnPromotionQueen:
                                        case SharpChess.Move.enmName.PawnPromotionRook:
                                        case SharpChess.Move.enmName.PawnPromotionBishop:
                                        case SharpChess.Move.enmName.PawnPromotionKnight:
                                            blnIsPromotion = true;
                                            frmPieceSelector formPieceSelector = new frmPieceSelector();
                                            formPieceSelector.Colour = move.Piece.Player.Colour;
                                            formPieceSelector.ShowDialog(this);
                                            movenamePromotion = formPieceSelector.MoveNameSelected;
                                            break;
                                    }
                                }

                                if (!blnIsPromotion || move.Name == movenamePromotion)
                                {
                                    this.m_squareFrom = null;
                                    this.m_movesPossible = new Moves();

                                    Game.MakeAMove(move.Name, move.Piece, move.To);
                                    blnMoveMade = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!blnMoveMade)
                    {
                        this.m_picSquares[this.m_squareFrom.File, this.m_squareFrom.Rank].Image =
                            this.imgPieces.Images[this.m_squareFrom.Piece.ImageIndex];
                        this.m_squareFrom = null;
                        this.m_movesPossible = null;
                        this.RenderBoardColours();
                    }
                }
                else
                {
                    Game.SuspendPondering();

                    this.m_picSquares[this.m_squareFrom.File, this.m_squareFrom.Rank].Image =
                        this.imgPieces.Images[this.m_squareFrom.Piece.ImageIndex];
                    this.m_squareFrom = null;
                    this.m_movesPossible = null;
                    this.RenderBoardColours();

                    Game.ResumePondering();
                }

                this.pnlEdging.Cursor = Cursors.Default;
            }
            else
            {
                Game.ResumePondering();
            }

            this.m_blnInMouseDown = false;
        }

        /// <summary>
        /// The pic square_ mouse up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picSquare_MouseUp(object sender, MouseEventArgs e)
        {
            this.m_blnIsLeftMouseButtonDown = false;
        }

        /// <summary>
        /// The tbr_ button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void tbr_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag.ToString())
            {
                case "New":
                    this.NewGame();
                    break;

                case "Open":
                    this.OpenGame();
                    break;

                case "Save":
                    this.SaveGame();
                    break;

                case "UndoMove":
                    this.UndoMove();
                    break;

                case "RedoMove":
                    this.RedoMove();
                    break;

                case "UndoAllMoves":
                    this.UndoAllMoves();
                    break;

                case "RedoAllMoves":
                    this.RedoAllMoves();
                    break;

                case "FlipBoard":
                    this.FlipBoard();
                    break;

                case "Think":
                    this.Think();
                    break;

                case "MoveNow":
                    this.MoveNow();
                    break;

                case "ResumePlay":
                    this.ResumePlay();
                    break;

                case "PausePlay":
                    this.PausePlay();
                    break;
            }
        }

        /// <summary>
        /// The timer_ tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.RenderClocks();
        }

        #endregion
    }
}