using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Threading;

namespace SharpChess
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>

	public class frmMain : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		const int SQUARE_SIZE = 42;
		const int INTELLEGENCE_HUMAN = 0;
		const int INTELLEGENCE_COMPUTER = 1;
        const int SQUARE_BRIGHTNESS = 48;

		public delegate void delegatetypePlayer_MoveConsideredHandler();
		public delegate void delegatetypePlayer_ThinkingBeginningHandler();
		public delegate void delegatetypeWinBoardMessageHandler(string strMessage);
		public delegate void delegatetypeWinBoardStandardHandler();
		public delegate void delegatetypeGame_BoardPositionChangedHandler();

		private System.Drawing.Color BOARD_SQUARE_COLOUR_WHITE = System.Drawing.Color.FromArgb(229,197,105);
		private System.Drawing.Color BOARD_SQUARE_COLOUR_BLACK = System.Drawing.Color.FromArgb(189,117,53);

        private System.Drawing.Color BOARD_SQUARE_COLOUR_WHITE_BRIGHT = System.Drawing.Color.FromArgb(Math.Min(229 + SQUARE_BRIGHTNESS,255), Math.Min(197 + SQUARE_BRIGHTNESS,255), Math.Min(105 + SQUARE_BRIGHTNESS,255));
        private System.Drawing.Color BOARD_SQUARE_COLOUR_BLACK_BRIGHT = System.Drawing.Color.FromArgb(Math.Min(189 + SQUARE_BRIGHTNESS,255), Math.Min(117 + SQUARE_BRIGHTNESS,255), Math.Min(53 + SQUARE_BRIGHTNESS,255));

        bool m_blnInMouseDown = false;
        bool m_blnIsLeftMouseButtonDown = false;
        Square m_squareFrom = null;
        Square m_squareTo = null;
        Moves m_movesPossible = new Moves();
		frmWinBoard m_formWinBoard = new frmWinBoard();
		frmMoveAnalysis m_formMoveAnalysis = new frmMoveAnalysis();
        Cursor[] m_acurPieceCursors = new Cursor[12];
        Cursor m_curPieceCursor;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.StatusBar sbr;
		PictureBox[,] m_picSquares;
		PictureBox[] m_picWhitesCaptures;
		private System.Windows.Forms.ImageList imgPieces;
		private System.Windows.Forms.MainMenu mnu;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuNew;
		private System.Windows.Forms.MenuItem mnuSave;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem mnuSep1;
		private System.Windows.Forms.MenuItem mnuUndoMove;
		private System.Windows.Forms.MenuItem mnuSep2;
		private System.Windows.Forms.ToolBar tbr;
		private System.Windows.Forms.ToolBarButton tbrNew;
		private System.Windows.Forms.ImageList imgToolMenus;
		private System.Windows.Forms.ToolBarButton tbrOpen;
		private System.Windows.Forms.ToolBarButton tbrSave;
		private System.Windows.Forms.ToolBarButton tbrSep1;
		private System.Windows.Forms.ToolBarButton tbrUndoMove;
		private System.Windows.Forms.ToolBarButton tbrSep2;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Label lblStage;
		private System.Windows.Forms.ListView lvwMoveHistory;
		private System.Windows.Forms.ColumnHeader lvcMoveNo;
		private System.Windows.Forms.ProgressBar pbr;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel pnlEdging;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ImageList imgTiles;
		private System.Windows.Forms.MenuItem mnuRedoMove;
		private System.Windows.Forms.MenuItem mnuUndoAllMoves;
		private System.Windows.Forms.ToolBarButton tbrRedoMove;
		private System.Windows.Forms.ToolBarButton tbrUndoAllMoves;
		private System.Windows.Forms.ToolBarButton tbrRedoAllMoves;
		private System.Windows.Forms.MenuItem mnuRedoAllMoves;
		private System.Windows.Forms.MenuItem mnuShowThinking;
		private System.Windows.Forms.MenuItem mnuDisplayMoveAnalysisTree;
		private System.Windows.Forms.Label lblBlackClock;
		private System.Windows.Forms.Label lblBlackPosition;
		private System.Windows.Forms.Label lblBlackScore;
		private System.Windows.Forms.ComboBox cboIntellegenceBlack;
		private System.Windows.Forms.Label lblBlackPoints;
		private System.Windows.Forms.Label lblWhiteClock;
		private System.Windows.Forms.Label lblWhitePosition;
		private System.Windows.Forms.Label lblWhiteScore;
		private System.Windows.Forms.ComboBox cboIntellegenceWhite;
		private System.Windows.Forms.Label lblWhitePoints;
		private System.Windows.Forms.Label lblPlayerClocks;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblPlayer;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.ToolBarButton tbrResumePlay;
		private System.Windows.Forms.ToolBarButton tbrPausePlay;
		private System.Windows.Forms.MenuItem mnuResumePlay;
		private System.Windows.Forms.MenuItem mnuPausePlay;
		private System.Windows.Forms.ColumnHeader lvcTime;
		private System.Windows.Forms.Label lblWhitesCaptures;
		private System.Windows.Forms.Label lblBlacksCaptures;
		private System.Windows.Forms.ColumnHeader lvcMove;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		PictureBox[] m_picBlacksCaptures;
		private System.Windows.Forms.ToolBarButton tbrFlipBoard;
		private System.Windows.Forms.ToolBarButton tbrSep3;
		private System.Windows.Forms.Button btnPGNtoXML;
		private System.Windows.Forms.Button btnXMLtoOB;
		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.Button btnPrune;
		private System.Windows.Forms.Label lblGamePaused;
		private System.Windows.Forms.MenuItem mnuDifficulty;
		private System.Windows.Forms.ToolBarButton tbrMoveNow;
		private System.Windows.Forms.MenuItem mnuMoveNow;
		private System.Windows.Forms.ToolBarButton tbrSep4;
		private System.Windows.Forms.ToolBarButton tbrThink;
		private System.Windows.Forms.MenuItem mnuThink;
		private System.Windows.Forms.MenuItem mnuDisplayWinBoardMessageLog;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuFlipBoard;
		private System.Windows.Forms.MenuItem mnuPasteFEN;
		private System.Windows.Forms.MenuItem mnuSep3;
		private System.Windows.Forms.MenuItem mnuGame;
		private System.Windows.Forms.MenuItem mnuComputer;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.Button btnPerft;
        private System.Windows.Forms.NumericUpDown numPerftDepth;
        private ImageList imageList1;
        private MenuItem menuItem2;
        private MenuItem mnuEditBoardPosition;
		private System.Windows.Forms.MenuItem mnuCopyFEN;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.sbr = new System.Windows.Forms.StatusBar();
            this.imgPieces = new System.Windows.Forms.ImageList(this.components);
            this.mnu = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuNew = new System.Windows.Forms.MenuItem();
            this.mnuOpen = new System.Windows.Forms.MenuItem();
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.mnuSep1 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuUndoMove = new System.Windows.Forms.MenuItem();
            this.mnuRedoMove = new System.Windows.Forms.MenuItem();
            this.mnuUndoAllMoves = new System.Windows.Forms.MenuItem();
            this.mnuRedoAllMoves = new System.Windows.Forms.MenuItem();
            this.mnuSep2 = new System.Windows.Forms.MenuItem();
            this.mnuCopyFEN = new System.Windows.Forms.MenuItem();
            this.mnuPasteFEN = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuEditBoardPosition = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuFlipBoard = new System.Windows.Forms.MenuItem();
            this.mnuSep3 = new System.Windows.Forms.MenuItem();
            this.mnuShowThinking = new System.Windows.Forms.MenuItem();
            this.mnuDisplayMoveAnalysisTree = new System.Windows.Forms.MenuItem();
            this.mnuDisplayWinBoardMessageLog = new System.Windows.Forms.MenuItem();
            this.mnuGame = new System.Windows.Forms.MenuItem();
            this.mnuPausePlay = new System.Windows.Forms.MenuItem();
            this.mnuResumePlay = new System.Windows.Forms.MenuItem();
            this.mnuComputer = new System.Windows.Forms.MenuItem();
            this.mnuDifficulty = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuThink = new System.Windows.Forms.MenuItem();
            this.mnuMoveNow = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.tbr = new System.Windows.Forms.ToolBar();
            this.tbrNew = new System.Windows.Forms.ToolBarButton();
            this.tbrOpen = new System.Windows.Forms.ToolBarButton();
            this.tbrSave = new System.Windows.Forms.ToolBarButton();
            this.tbrSep1 = new System.Windows.Forms.ToolBarButton();
            this.tbrUndoAllMoves = new System.Windows.Forms.ToolBarButton();
            this.tbrUndoMove = new System.Windows.Forms.ToolBarButton();
            this.tbrResumePlay = new System.Windows.Forms.ToolBarButton();
            this.tbrPausePlay = new System.Windows.Forms.ToolBarButton();
            this.tbrRedoMove = new System.Windows.Forms.ToolBarButton();
            this.tbrRedoAllMoves = new System.Windows.Forms.ToolBarButton();
            this.tbrSep2 = new System.Windows.Forms.ToolBarButton();
            this.tbrFlipBoard = new System.Windows.Forms.ToolBarButton();
            this.tbrSep3 = new System.Windows.Forms.ToolBarButton();
            this.tbrThink = new System.Windows.Forms.ToolBarButton();
            this.tbrSep4 = new System.Windows.Forms.ToolBarButton();
            this.tbrMoveNow = new System.Windows.Forms.ToolBarButton();
            this.imgToolMenus = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.numPerftDepth = new System.Windows.Forms.NumericUpDown();
            this.btnPerft = new System.Windows.Forms.Button();
            this.lblGamePaused = new System.Windows.Forms.Label();
            this.btnPrune = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lvwMoveHistory = new System.Windows.Forms.ListView();
            this.lvcMoveNo = new System.Windows.Forms.ColumnHeader();
            this.lvcTime = new System.Windows.Forms.ColumnHeader();
            this.lvcMove = new System.Windows.Forms.ColumnHeader();
            this.btnXMLtoOB = new System.Windows.Forms.Button();
            this.btnPGNtoXML = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBlacksCaptures = new System.Windows.Forms.Label();
            this.lblWhitesCaptures = new System.Windows.Forms.Label();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.lblBlackClock = new System.Windows.Forms.Label();
            this.lblBlackPosition = new System.Windows.Forms.Label();
            this.lblBlackScore = new System.Windows.Forms.Label();
            this.cboIntellegenceBlack = new System.Windows.Forms.ComboBox();
            this.lblBlackPoints = new System.Windows.Forms.Label();
            this.lblWhiteClock = new System.Windows.Forms.Label();
            this.lblWhitePosition = new System.Windows.Forms.Label();
            this.lblWhiteScore = new System.Windows.Forms.Label();
            this.cboIntellegenceWhite = new System.Windows.Forms.ComboBox();
            this.lblWhitePoints = new System.Windows.Forms.Label();
            this.lblPlayerClocks = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbr = new System.Windows.Forms.ProgressBar();
            this.lblStage = new System.Windows.Forms.Label();
            this.pnlEdging = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imgTiles = new System.Windows.Forms.ImageList(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPerftDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // sbr
            // 
            this.sbr.Location = new System.Drawing.Point(0, 523);
            this.sbr.Name = "sbr";
            this.sbr.Size = new System.Drawing.Size(690, 16);
            this.sbr.SizingGrip = false;
            this.sbr.TabIndex = 7;
            // 
            // imgPieces
            // 
            this.imgPieces.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgPieces.ImageStream")));
            this.imgPieces.TransparentColor = System.Drawing.Color.Transparent;
            this.imgPieces.Images.SetKeyName(0, "");
            this.imgPieces.Images.SetKeyName(1, "");
            this.imgPieces.Images.SetKeyName(2, "");
            this.imgPieces.Images.SetKeyName(3, "");
            this.imgPieces.Images.SetKeyName(4, "");
            this.imgPieces.Images.SetKeyName(5, "");
            this.imgPieces.Images.SetKeyName(6, "");
            this.imgPieces.Images.SetKeyName(7, "");
            this.imgPieces.Images.SetKeyName(8, "");
            this.imgPieces.Images.SetKeyName(9, "");
            this.imgPieces.Images.SetKeyName(10, "");
            this.imgPieces.Images.SetKeyName(11, "");
            // 
            // mnu
            // 
            this.mnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView,
            this.mnuGame,
            this.mnuComputer,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave,
            this.mnuSep1,
            this.mnuExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Index = 0;
            this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Index = 1;
            this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuOpen.Text = "&Open...";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Index = 2;
            this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSave.Text = "Save &As...";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Index = 3;
            this.mnuSep1.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 4;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuUndoMove,
            this.mnuRedoMove,
            this.mnuUndoAllMoves,
            this.mnuRedoAllMoves,
            this.mnuSep2,
            this.mnuCopyFEN,
            this.mnuPasteFEN,
            this.menuItem2,
            this.mnuEditBoardPosition});
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuUndoMove
            // 
            this.mnuUndoMove.Index = 0;
            this.mnuUndoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndoMove.Text = "&Undo Move";
            this.mnuUndoMove.Click += new System.EventHandler(this.mnuUndoMove_Click);
            // 
            // mnuRedoMove
            // 
            this.mnuRedoMove.Index = 1;
            this.mnuRedoMove.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedoMove.Text = "&Redo Move";
            this.mnuRedoMove.Click += new System.EventHandler(this.mnuRedoMove_Click);
            // 
            // mnuUndoAllMoves
            // 
            this.mnuUndoAllMoves.Index = 2;
            this.mnuUndoAllMoves.Text = "U&ndo All Moves";
            this.mnuUndoAllMoves.Click += new System.EventHandler(this.mnuUndoAllMoves_Click);
            // 
            // mnuRedoAllMoves
            // 
            this.mnuRedoAllMoves.Index = 3;
            this.mnuRedoAllMoves.Text = "Re&do All Moves";
            this.mnuRedoAllMoves.Click += new System.EventHandler(this.mnuRedoAllMoves_Click);
            // 
            // mnuSep2
            // 
            this.mnuSep2.Index = 4;
            this.mnuSep2.Text = "-";
            // 
            // mnuCopyFEN
            // 
            this.mnuCopyFEN.Index = 5;
            this.mnuCopyFEN.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopyFEN.Text = "&Copy FEN Position";
            this.mnuCopyFEN.Click += new System.EventHandler(this.mnuCopyFEN_Click);
            // 
            // mnuPasteFEN
            // 
            this.mnuPasteFEN.Index = 6;
            this.mnuPasteFEN.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPasteFEN.Text = "&Paste FEN Position";
            this.mnuPasteFEN.Click += new System.EventHandler(this.mnuPasteFEN_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 7;
            this.menuItem2.Text = "-";
            // 
            // mnuEditBoardPosition
            // 
            this.mnuEditBoardPosition.Index = 8;
            this.mnuEditBoardPosition.Text = "&Edit Board Position";
            this.mnuEditBoardPosition.Click += new System.EventHandler(this.mnuEditPosition_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 2;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFlipBoard,
            this.mnuSep3,
            this.mnuShowThinking,
            this.mnuDisplayMoveAnalysisTree,
            this.mnuDisplayWinBoardMessageLog});
            this.mnuView.Text = "&View";
            // 
            // mnuFlipBoard
            // 
            this.mnuFlipBoard.Index = 0;
            this.mnuFlipBoard.Text = "&Flip Board";
            this.mnuFlipBoard.Click += new System.EventHandler(this.mnuFlipBoard_Click);
            // 
            // mnuSep3
            // 
            this.mnuSep3.Index = 1;
            this.mnuSep3.Text = "-";
            // 
            // mnuShowThinking
            // 
            this.mnuShowThinking.Index = 2;
            this.mnuShowThinking.Text = "&Show Thinking";
            this.mnuShowThinking.Click += new System.EventHandler(this.mnuShowThinking_Click);
            // 
            // mnuDisplayMoveAnalysisTree
            // 
            this.mnuDisplayMoveAnalysisTree.Index = 3;
            this.mnuDisplayMoveAnalysisTree.Text = "S&how Move Analysis Tree";
            this.mnuDisplayMoveAnalysisTree.Click += new System.EventHandler(this.mnuDisplayMoveAnalysisTree_Click);
            // 
            // mnuDisplayWinBoardMessageLog
            // 
            this.mnuDisplayWinBoardMessageLog.Index = 4;
            this.mnuDisplayWinBoardMessageLog.Text = "Sh&ow WinBoard Message Log";
            this.mnuDisplayWinBoardMessageLog.Click += new System.EventHandler(this.mnuDisplayWinBoardMessageLog_Click);
            // 
            // mnuGame
            // 
            this.mnuGame.Index = 3;
            this.mnuGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPausePlay,
            this.mnuResumePlay});
            this.mnuGame.Text = "&Game";
            // 
            // mnuPausePlay
            // 
            this.mnuPausePlay.Enabled = false;
            this.mnuPausePlay.Index = 0;
            this.mnuPausePlay.Text = "&Pause Game";
            this.mnuPausePlay.Click += new System.EventHandler(this.mnuPausePlay_Click);
            // 
            // mnuResumePlay
            // 
            this.mnuResumePlay.Index = 1;
            this.mnuResumePlay.Text = "&Resume Game";
            this.mnuResumePlay.Click += new System.EventHandler(this.mnuResumePlay_Click);
            // 
            // mnuComputer
            // 
            this.mnuComputer.Index = 4;
            this.mnuComputer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDifficulty,
            this.menuItem1,
            this.mnuThink,
            this.mnuMoveNow});
            this.mnuComputer.Text = "&Computer";
            // 
            // mnuDifficulty
            // 
            this.mnuDifficulty.Index = 0;
            this.mnuDifficulty.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.mnuDifficulty.Text = "&Difficulty...";
            this.mnuDifficulty.Click += new System.EventHandler(this.mnuDifficulty_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // mnuThink
            // 
            this.mnuThink.Index = 2;
            this.mnuThink.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuThink.Text = "&Think";
            this.mnuThink.Click += new System.EventHandler(this.mnuThink_Click);
            // 
            // mnuMoveNow
            // 
            this.mnuMoveNow.Index = 3;
            this.mnuMoveNow.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuMoveNow.Text = "&Move Now";
            this.mnuMoveNow.Click += new System.EventHandler(this.mnuMoveNow_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 5;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "&About SharpChess";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tbr
            // 
            this.tbr.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbr.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbrNew,
            this.tbrOpen,
            this.tbrSave,
            this.tbrSep1,
            this.tbrUndoAllMoves,
            this.tbrUndoMove,
            this.tbrResumePlay,
            this.tbrPausePlay,
            this.tbrRedoMove,
            this.tbrRedoAllMoves,
            this.tbrSep2,
            this.tbrFlipBoard,
            this.tbrSep3,
            this.tbrThink,
            this.tbrSep4,
            this.tbrMoveNow});
            this.tbr.DropDownArrows = true;
            this.tbr.ImageList = this.imgToolMenus;
            this.tbr.Location = new System.Drawing.Point(0, 0);
            this.tbr.Name = "tbr";
            this.tbr.ShowToolTips = true;
            this.tbr.Size = new System.Drawing.Size(690, 28);
            this.tbr.TabIndex = 32;
            this.tbr.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbr.Wrappable = false;
            this.tbr.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbr_ButtonClick);
            // 
            // tbrNew
            // 
            this.tbrNew.ImageIndex = 0;
            this.tbrNew.Name = "tbrNew";
            this.tbrNew.Tag = "New";
            this.tbrNew.ToolTipText = "Start a new chess game";
            // 
            // tbrOpen
            // 
            this.tbrOpen.ImageIndex = 1;
            this.tbrOpen.Name = "tbrOpen";
            this.tbrOpen.Tag = "Open";
            this.tbrOpen.ToolTipText = "Open a saved chess game";
            // 
            // tbrSave
            // 
            this.tbrSave.ImageIndex = 2;
            this.tbrSave.Name = "tbrSave";
            this.tbrSave.Tag = "Save";
            this.tbrSave.ToolTipText = "Save the current chess game";
            // 
            // tbrSep1
            // 
            this.tbrSep1.Name = "tbrSep1";
            this.tbrSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrUndoAllMoves
            // 
            this.tbrUndoAllMoves.ImageIndex = 6;
            this.tbrUndoAllMoves.Name = "tbrUndoAllMoves";
            this.tbrUndoAllMoves.Tag = "UndoAllMoves";
            this.tbrUndoAllMoves.ToolTipText = "Undo all moves played so far";
            // 
            // tbrUndoMove
            // 
            this.tbrUndoMove.ImageIndex = 4;
            this.tbrUndoMove.Name = "tbrUndoMove";
            this.tbrUndoMove.Tag = "UndoMove";
            this.tbrUndoMove.ToolTipText = "Undo the last move";
            // 
            // tbrResumePlay
            // 
            this.tbrResumePlay.ImageIndex = 8;
            this.tbrResumePlay.Name = "tbrResumePlay";
            this.tbrResumePlay.Tag = "ResumePlay";
            this.tbrResumePlay.ToolTipText = "Resume play";
            // 
            // tbrPausePlay
            // 
            this.tbrPausePlay.Enabled = false;
            this.tbrPausePlay.ImageIndex = 9;
            this.tbrPausePlay.Name = "tbrPausePlay";
            this.tbrPausePlay.Tag = "PausePlay";
            this.tbrPausePlay.ToolTipText = "Pause play";
            // 
            // tbrRedoMove
            // 
            this.tbrRedoMove.ImageIndex = 5;
            this.tbrRedoMove.Name = "tbrRedoMove";
            this.tbrRedoMove.Tag = "RedoMove";
            this.tbrRedoMove.ToolTipText = "Redo move";
            // 
            // tbrRedoAllMoves
            // 
            this.tbrRedoAllMoves.ImageIndex = 7;
            this.tbrRedoAllMoves.Name = "tbrRedoAllMoves";
            this.tbrRedoAllMoves.Tag = "RedoAllMoves";
            this.tbrRedoAllMoves.ToolTipText = "Redo all moves";
            // 
            // tbrSep2
            // 
            this.tbrSep2.Name = "tbrSep2";
            this.tbrSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrFlipBoard
            // 
            this.tbrFlipBoard.ImageIndex = 10;
            this.tbrFlipBoard.Name = "tbrFlipBoard";
            this.tbrFlipBoard.Tag = "FlipBoard";
            this.tbrFlipBoard.Text = "Flip Board";
            // 
            // tbrSep3
            // 
            this.tbrSep3.Name = "tbrSep3";
            this.tbrSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrThink
            // 
            this.tbrThink.ImageIndex = 3;
            this.tbrThink.Name = "tbrThink";
            this.tbrThink.Tag = "Think";
            this.tbrThink.Text = "Think";
            this.tbrThink.ToolTipText = "Make the computer play the next move";
            // 
            // tbrSep4
            // 
            this.tbrSep4.Name = "tbrSep4";
            this.tbrSep4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbrMoveNow
            // 
            this.tbrMoveNow.ImageIndex = 11;
            this.tbrMoveNow.Name = "tbrMoveNow";
            this.tbrMoveNow.Tag = "MoveNow";
            this.tbrMoveNow.Text = "Move Now";
            this.tbrMoveNow.ToolTipText = "Make the computer immediately play the best move it has found so far";
            // 
            // imgToolMenus
            // 
            this.imgToolMenus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolMenus.ImageStream")));
            this.imgToolMenus.TransparentColor = System.Drawing.Color.Transparent;
            this.imgToolMenus.Images.SetKeyName(0, "");
            this.imgToolMenus.Images.SetKeyName(1, "");
            this.imgToolMenus.Images.SetKeyName(2, "");
            this.imgToolMenus.Images.SetKeyName(3, "");
            this.imgToolMenus.Images.SetKeyName(4, "");
            this.imgToolMenus.Images.SetKeyName(5, "");
            this.imgToolMenus.Images.SetKeyName(6, "");
            this.imgToolMenus.Images.SetKeyName(7, "");
            this.imgToolMenus.Images.SetKeyName(8, "");
            this.imgToolMenus.Images.SetKeyName(9, "");
            this.imgToolMenus.Images.SetKeyName(10, "");
            this.imgToolMenus.Images.SetKeyName(11, "");
            // 
            // pnlMain
            // 
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
            this.pnlMain.Location = new System.Drawing.Point(0, 26);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(690, 496);
            this.pnlMain.TabIndex = 33;
            // 
            // numPerftDepth
            // 
            this.numPerftDepth.Location = new System.Drawing.Point(64, 416);
            this.numPerftDepth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPerftDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPerftDepth.Name = "numPerftDepth";
            this.numPerftDepth.Size = new System.Drawing.Size(40, 20);
            this.numPerftDepth.TabIndex = 143;
            this.numPerftDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPerftDepth.Visible = false;
            // 
            // btnPerft
            // 
            this.btnPerft.Location = new System.Drawing.Point(104, 416);
            this.btnPerft.Name = "btnPerft";
            this.btnPerft.Size = new System.Drawing.Size(75, 23);
            this.btnPerft.TabIndex = 142;
            this.btnPerft.Text = "Perft";
            this.btnPerft.Visible = false;
            this.btnPerft.Click += new System.EventHandler(this.btnPerft_Click);
            // 
            // lblGamePaused
            // 
            this.lblGamePaused.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGamePaused.Location = new System.Drawing.Point(40, 168);
            this.lblGamePaused.Name = "lblGamePaused";
            this.lblGamePaused.Size = new System.Drawing.Size(320, 24);
            this.lblGamePaused.TabIndex = 141;
            this.lblGamePaused.Text = "Game Paused";
            this.lblGamePaused.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGamePaused.Visible = false;
            // 
            // btnPrune
            // 
            this.btnPrune.Location = new System.Drawing.Point(200, 416);
            this.btnPrune.Name = "btnPrune";
            this.btnPrune.Size = new System.Drawing.Size(75, 23);
            this.btnPrune.TabIndex = 140;
            this.btnPrune.Text = "Prune";
            this.btnPrune.Visible = false;
            this.btnPrune.Click += new System.EventHandler(this.btnPrune_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(280, 416);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(120, 40);
            this.txtOutput.TabIndex = 139;
            this.txtOutput.Visible = false;
            // 
            // lvwMoveHistory
            // 
            this.lvwMoveHistory.BackColor = System.Drawing.SystemColors.Control;
            this.lvwMoveHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcMoveNo,
            this.lvcTime,
            this.lvcMove});
            this.lvwMoveHistory.Location = new System.Drawing.Point(400, 176);
            this.lvwMoveHistory.Name = "lvwMoveHistory";
            this.lvwMoveHistory.Size = new System.Drawing.Size(248, 128);
            this.lvwMoveHistory.TabIndex = 39;
            this.lvwMoveHistory.UseCompatibleStateImageBehavior = false;
            this.lvwMoveHistory.View = System.Windows.Forms.View.Details;
            // 
            // lvcMoveNo
            // 
            this.lvcMoveNo.Text = "#";
            this.lvcMoveNo.Width = 19;
            // 
            // lvcTime
            // 
            this.lvcTime.Text = "Time";
            this.lvcTime.Width = 56;
            // 
            // lvcMove
            // 
            this.lvcMove.Text = "Move";
            this.lvcMove.Width = 152;
            // 
            // btnXMLtoOB
            // 
            this.btnXMLtoOB.Location = new System.Drawing.Point(496, 416);
            this.btnXMLtoOB.Name = "btnXMLtoOB";
            this.btnXMLtoOB.Size = new System.Drawing.Size(75, 23);
            this.btnXMLtoOB.TabIndex = 138;
            this.btnXMLtoOB.Text = "XML to OB";
            this.btnXMLtoOB.Visible = false;
            this.btnXMLtoOB.Click += new System.EventHandler(this.btnXMLtoOB_Click);
            // 
            // btnPGNtoXML
            // 
            this.btnPGNtoXML.Location = new System.Drawing.Point(408, 416);
            this.btnPGNtoXML.Name = "btnPGNtoXML";
            this.btnPGNtoXML.Size = new System.Drawing.Size(75, 23);
            this.btnPGNtoXML.TabIndex = 137;
            this.btnPGNtoXML.Text = "PGN to XML";
            this.btnPGNtoXML.Visible = false;
            this.btnPGNtoXML.Click += new System.EventHandler(this.btnPGNtoXML_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(552, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 23);
            this.label5.TabIndex = 136;
            this.label5.Text = "Black";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(448, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 135;
            this.label3.Text = "White";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlacksCaptures
            // 
            this.lblBlacksCaptures.BackColor = System.Drawing.Color.Transparent;
            this.lblBlacksCaptures.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlacksCaptures.CausesValidation = false;
            this.lblBlacksCaptures.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlacksCaptures.Location = new System.Drawing.Point(644, 428);
            this.lblBlacksCaptures.Name = "lblBlacksCaptures";
            this.lblBlacksCaptures.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlacksCaptures.Size = new System.Drawing.Size(42, 42);
            this.lblBlacksCaptures.TabIndex = 134;
            this.lblBlacksCaptures.Text = "0";
            this.lblBlacksCaptures.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhitesCaptures
            // 
            this.lblWhitesCaptures.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitesCaptures.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitesCaptures.CausesValidation = false;
            this.lblWhitesCaptures.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhitesCaptures.Location = new System.Drawing.Point(644, 384);
            this.lblWhitesCaptures.Name = "lblWhitesCaptures";
            this.lblWhitesCaptures.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitesCaptures.Size = new System.Drawing.Size(42, 42);
            this.lblWhitesCaptures.TabIndex = 133;
            this.lblWhitesCaptures.Text = "0";
            this.lblWhitesCaptures.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayer
            // 
            this.lblPlayer.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayer.Location = new System.Drawing.Point(392, 32);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(48, 24);
            this.lblPlayer.TabIndex = 131;
            this.lblPlayer.Text = "Player";
            this.lblPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBlackClock
            // 
            this.lblBlackClock.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackClock.CausesValidation = false;
            this.lblBlackClock.ForeColor = System.Drawing.Color.Black;
            this.lblBlackClock.Location = new System.Drawing.Point(552, 64);
            this.lblBlackClock.Name = "lblBlackClock";
            this.lblBlackClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackClock.Size = new System.Drawing.Size(96, 23);
            this.lblBlackClock.TabIndex = 130;
            this.lblBlackClock.Text = ":";
            this.lblBlackClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlackPosition
            // 
            this.lblBlackPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPosition.CausesValidation = false;
            this.lblBlackPosition.Location = new System.Drawing.Point(552, 144);
            this.lblBlackPosition.Name = "lblBlackPosition";
            this.lblBlackPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPosition.Size = new System.Drawing.Size(96, 23);
            this.lblBlackPosition.TabIndex = 128;
            this.lblBlackPosition.Text = "0";
            this.lblBlackPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBlackScore
            // 
            this.lblBlackScore.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackScore.CausesValidation = false;
            this.lblBlackScore.Location = new System.Drawing.Point(552, 96);
            this.lblBlackScore.Name = "lblBlackScore";
            this.lblBlackScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackScore.Size = new System.Drawing.Size(96, 23);
            this.lblBlackScore.TabIndex = 127;
            this.lblBlackScore.Text = "0";
            this.lblBlackScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboIntellegenceBlack
            // 
            this.cboIntellegenceBlack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceBlack.Items.AddRange(new object[] {
            "Human",
            "Computer"});
            this.cboIntellegenceBlack.Location = new System.Drawing.Point(552, 32);
            this.cboIntellegenceBlack.Name = "cboIntellegenceBlack";
            this.cboIntellegenceBlack.Size = new System.Drawing.Size(96, 21);
            this.cboIntellegenceBlack.TabIndex = 126;
            this.cboIntellegenceBlack.SelectedIndexChanged += new System.EventHandler(this.cboIntellegenceBlack_SelectedIndexChanged);
            // 
            // lblBlackPoints
            // 
            this.lblBlackPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblBlackPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBlackPoints.CausesValidation = false;
            this.lblBlackPoints.Location = new System.Drawing.Point(552, 120);
            this.lblBlackPoints.Name = "lblBlackPoints";
            this.lblBlackPoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBlackPoints.Size = new System.Drawing.Size(96, 23);
            this.lblBlackPoints.TabIndex = 125;
            this.lblBlackPoints.Text = "0";
            this.lblBlackPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhiteClock
            // 
            this.lblWhiteClock.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteClock.CausesValidation = false;
            this.lblWhiteClock.ForeColor = System.Drawing.Color.Black;
            this.lblWhiteClock.Location = new System.Drawing.Point(448, 64);
            this.lblWhiteClock.Name = "lblWhiteClock";
            this.lblWhiteClock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteClock.Size = new System.Drawing.Size(96, 23);
            this.lblWhiteClock.TabIndex = 124;
            this.lblWhiteClock.Text = ":";
            this.lblWhiteClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhitePosition
            // 
            this.lblWhitePosition.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePosition.CausesValidation = false;
            this.lblWhitePosition.Location = new System.Drawing.Point(448, 144);
            this.lblWhitePosition.Name = "lblWhitePosition";
            this.lblWhitePosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePosition.Size = new System.Drawing.Size(96, 23);
            this.lblWhitePosition.TabIndex = 122;
            this.lblWhitePosition.Text = "0";
            this.lblWhitePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWhiteScore
            // 
            this.lblWhiteScore.BackColor = System.Drawing.Color.Transparent;
            this.lblWhiteScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhiteScore.CausesValidation = false;
            this.lblWhiteScore.Location = new System.Drawing.Point(448, 96);
            this.lblWhiteScore.Name = "lblWhiteScore";
            this.lblWhiteScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhiteScore.Size = new System.Drawing.Size(96, 23);
            this.lblWhiteScore.TabIndex = 121;
            this.lblWhiteScore.Text = "0";
            this.lblWhiteScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboIntellegenceWhite
            // 
            this.cboIntellegenceWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntellegenceWhite.Items.AddRange(new object[] {
            "Human",
            "Computer"});
            this.cboIntellegenceWhite.Location = new System.Drawing.Point(448, 32);
            this.cboIntellegenceWhite.Name = "cboIntellegenceWhite";
            this.cboIntellegenceWhite.Size = new System.Drawing.Size(96, 21);
            this.cboIntellegenceWhite.TabIndex = 120;
            this.cboIntellegenceWhite.SelectedIndexChanged += new System.EventHandler(this.cboIntellegenceWhite_SelectedIndexChanged);
            // 
            // lblWhitePoints
            // 
            this.lblWhitePoints.BackColor = System.Drawing.Color.Transparent;
            this.lblWhitePoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWhitePoints.CausesValidation = false;
            this.lblWhitePoints.Location = new System.Drawing.Point(448, 120);
            this.lblWhitePoints.Name = "lblWhitePoints";
            this.lblWhitePoints.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWhitePoints.Size = new System.Drawing.Size(96, 23);
            this.lblWhitePoints.TabIndex = 119;
            this.lblWhitePoints.Text = "0";
            this.lblWhitePoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayerClocks
            // 
            this.lblPlayerClocks.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerClocks.Location = new System.Drawing.Point(392, 64);
            this.lblPlayerClocks.Name = "lblPlayerClocks";
            this.lblPlayerClocks.Size = new System.Drawing.Size(48, 24);
            this.lblPlayerClocks.TabIndex = 118;
            this.lblPlayerClocks.Text = "Clock";
            this.lblPlayerClocks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(392, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 24);
            this.label2.TabIndex = 116;
            this.label2.Text = "Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(400, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 24);
            this.label4.TabIndex = 115;
            this.label4.Text = "Score";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(400, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 24);
            this.label1.TabIndex = 114;
            this.label1.Text = "Points";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(32, 350);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 8);
            this.panel1.TabIndex = 55;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(366, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 352);
            this.panel3.TabIndex = 57;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(24, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 8);
            this.panel4.TabIndex = 58;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(24, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(8, 350);
            this.panel2.TabIndex = 56;
            // 
            // pbr
            // 
            this.pbr.Location = new System.Drawing.Point(0, 472);
            this.pbr.Name = "pbr";
            this.pbr.Size = new System.Drawing.Size(688, 23);
            this.pbr.TabIndex = 54;
            // 
            // lblStage
            // 
            this.lblStage.BackColor = System.Drawing.Color.Transparent;
            this.lblStage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStage.CausesValidation = false;
            this.lblStage.Location = new System.Drawing.Point(376, 358);
            this.lblStage.Name = "lblStage";
            this.lblStage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStage.Size = new System.Drawing.Size(312, 23);
            this.lblStage.TabIndex = 50;
            this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlEdging
            // 
            this.pnlEdging.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEdging.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEdging.Location = new System.Drawing.Point(28, 12);
            this.pnlEdging.Name = "pnlEdging";
            this.pnlEdging.Size = new System.Drawing.Size(340, 340);
            this.pnlEdging.TabIndex = 35;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Location = new System.Drawing.Point(0, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 8);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            // 
            // imgTiles
            // 
            this.imgTiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTiles.ImageStream")));
            this.imgTiles.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTiles.Images.SetKeyName(0, "");
            this.imgTiles.Images.SetKeyName(1, "");
            // 
            // timer
            // 
            this.timer.Interval = 333;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 539);
            this.Controls.Add(this.tbr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sbr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mnu;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpChess";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPerftDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		Square m_squareLastFrom = null;
		Square m_squareLastTo = null;

		private void Player_MoveConsidered()
		{
			delegatetypePlayer_MoveConsideredHandler MoveConsideredPointer = new delegatetypePlayer_MoveConsideredHandler(Player_MoveConsideredHandler);
			this.BeginInvoke(MoveConsideredPointer, null);
		}	

		private void Player_MoveConsideredHandler()
		{
			RenderStatus();

			if (m_squareLastFrom != null)
			{
				m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor = (m_squareLastFrom.Colour==Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE : BOARD_SQUARE_COLOUR_BLACK);
			}
			if (Game.ShowThinking && Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
			{
				if (Game.PlayerToPlay.CurrentMove != null)
				{
					m_squareLastFrom = Game.PlayerToPlay.CurrentMove.From;
					//m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor = System.Drawing.Color.Yellow;
                    m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].BackColor = (Board.GetSquare((int)m_picSquares[m_squareLastFrom.File, m_squareLastFrom.Rank].Tag).Colour == Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE_BRIGHT : BOARD_SQUARE_COLOUR_BLACK_BRIGHT);
                }
			}

			if (m_squareLastTo != null)
			{
				m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor = (m_squareLastTo.Colour==Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE : BOARD_SQUARE_COLOUR_BLACK);
			}
			if (Game.ShowThinking && Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
			{
				if (Game.PlayerToPlay.CurrentMove != null)
				{
					m_squareLastTo = Game.PlayerToPlay.CurrentMove.To;
					//m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor=System.Drawing.Color.Yellow;
                    m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].BackColor = (Board.GetSquare((int)m_picSquares[m_squareLastTo.File, m_squareLastTo.Rank].Tag).Colour == Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE_BRIGHT : BOARD_SQUARE_COLOUR_BLACK_BRIGHT);
                }
			}

		}

		private void Player_ThinkingBeginning()
		{
			delegatetypePlayer_ThinkingBeginningHandler ThinkingBeginningPointer = new delegatetypePlayer_ThinkingBeginningHandler(Player_ThinkingBeginningHandler);
			this.Invoke(ThinkingBeginningPointer, null);
		}

		private void Player_ThinkingBeginningHandler()
		{
			SetFormState();
		}

		private void Game_BoardPositionChanged()
		{
			delegatetypeGame_BoardPositionChangedHandler BoardPositionChangedPointer = new delegatetypeGame_BoardPositionChangedHandler(Game_BoardPositionChangedHandler);
			this.Invoke(BoardPositionChangedPointer, null);
		}

		private void Game_BoardPositionChangedHandler()
		{
//			if (Game.IsPaused) return;

			if (Game.MoveHistory.Count>0)
			{
				Move move = Game.MoveHistory.Last;
				sbr.Text += "  Moved: " + move.Piece.Name.ToString() + " " + move.From.Name+"-"+move.To.Name + " " + move.Description;
			}
			pbr.Value = 0;

			m_squareFrom = null;
			m_movesPossible = new Moves();

			RenderMoveAnalysis();
			RenderBoard();
		}

		private void WinBoard_InputReceived(string strMessage)
		{
			delegatetypeWinBoardMessageHandler WinBoardMessageHandlerPointer = new delegatetypeWinBoardMessageHandler(HandleWinBoardInputReceivedMessage);

			object[] oParams = {strMessage};
			this.Invoke(WinBoardMessageHandlerPointer, oParams);
		}	

		private void WinBoard_OutputSent(string strMessage)
		{
			delegatetypeWinBoardMessageHandler WinBoardMessageHandlerPointer = new delegatetypeWinBoardMessageHandler(HandleWinBoardOutputSentMessage);

			object[] oParams = {strMessage};
			this.Invoke(WinBoardMessageHandlerPointer, oParams);
		}

		private void WinBoard_Quit()
		{
			delegatetypeWinBoardStandardHandler WinBoardQuitHandlerPointer = new delegatetypeWinBoardStandardHandler(HandleWinBoardQuit);
			this.BeginInvoke(WinBoardQuitHandlerPointer, null);
		}

		private void WinBoard_TimeUpdated()
		{
			delegatetypeWinBoardStandardHandler WinBoardTimeUpdatedHandlerPointer = new delegatetypeWinBoardStandardHandler(HandleWinBoardTimeUpdated);
			this.Invoke(WinBoardTimeUpdatedHandlerPointer, null);
		}

		private void HandleWinBoardInputReceivedMessage(string strMessage)
		{
			LogWinBoardMessage("In", strMessage);
			WinBoard.ProcessInputEvent(strMessage);
		}

		private void HandleWinBoardOutputSentMessage(string strMessage)
		{
			LogWinBoardMessage("Out", strMessage);
		}

		private void HandleWinBoardTimeout()
		{
			LogWinBoardMessage("dbg", "Timeout received");
			StartNormalGame();
		}

		private void HandleWinBoardQuit()
		{
			this.Close();
		}

		private void HandleWinBoardTimeUpdated()
		{
			RenderClocks();
		}

		private void LogWinBoardMessage(string strDirection, string strMessage)
		{
			m_formWinBoard.LogWinBoardMessage(strDirection, strMessage);
		}

		public void MoveAnalysisClosed()
		{
			AssignMenuChecks();
		}

		public void WinBoardClosed()
		{
			AssignMenuChecks();
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			Game.PlayerWhite.MoveConsidered += new Player.delegatetypePlayerEvent(Player_MoveConsidered);
			Game.PlayerBlack.MoveConsidered += new Player.delegatetypePlayerEvent(Player_MoveConsidered);
			Game.PlayerWhite.ThinkingBeginning += new Player.delegatetypePlayerEvent(Player_ThinkingBeginning);
			Game.PlayerBlack.ThinkingBeginning += new Player.delegatetypePlayerEvent(Player_ThinkingBeginning);

			Game.BoardPositionChanged += new Game.delegatetypeGameEvent(Game_BoardPositionChanged);
			Game.GamePaused += new Game.delegatetypeGameEvent(RenderBoard);
			Game.GameResumed += new Game.delegatetypeGameEvent(RenderBoard);
			Game.GameSaved += new Game.delegatetypeGameEvent(RenderBoard);
			Game.SettingsUpdated += new Game.delegatetypeGameEvent(RenderBoard);

			WinBoard.WinBoardInputEvent += new WinBoard.delegatetypeCommunicationEvent(WinBoard_InputReceived);
			WinBoard.WinBoardOutputEvent += new WinBoard.delegatetypeCommunicationEvent(WinBoard_OutputSent);
			WinBoard.WinBoardQuitEvent += new WinBoard.delegatetypeStandardEvent(WinBoard_Quit);
			WinBoard.WinBoardTimeUpdatedEvent += new WinBoard.delegatetypeStandardEvent(WinBoard_TimeUpdated);
			
			m_formMoveAnalysis.MoveAnalysisClosedEvent += new frmMoveAnalysis.delegatetypeMoveAnalysisClosed(MoveAnalysisClosed);
			m_formWinBoard.WinBoardClosedEvent += new frmWinBoard.delegatetypeWinBoardClosed(WinBoardClosed);

			pnlEdging.Visible = false;

            LoadCursors();

            CreateBoard();

			Game.BackupGamePath = Application.StartupPath + @"\BackupGame.sharpchess";			

			this.Text = Application.ProductName + " - " + Game.FileName;
			AssignMenuChecks();
			SizeHistoryPane();

			OrientBoard();
			RenderBoard();
			RenderClocks();

			WinBoard.DetermineStatus();
			if (WinBoard.Active)
			{
                if (!WinBoard.ShowGUI)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
				this.Show();
				AssignMenuChecks();
				WinBoard.StartListener();
			}
			else
			{
				this.Show();
				StartNormalGame();
			}

			pnlEdging.Visible = true;
		}

		private void StartNormalGame()
		{
			if (!Game.LoadBackup())
			{
				frmDifficulty formDifficulty = new frmDifficulty();
				formDifficulty.ShowDialog(this);
			}

			Game.StartNormalGame();

			OrientBoard();
			RenderBoard();
			RenderClocks();
			SetFormState();
			timer.Start();
		}

		private void RenderClocks()
		{
			if (WinBoard.Active)
			{
				lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeRemaining.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeRemaining.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeRemaining.Seconds.ToString().PadLeft(2,'0');
				lblBlackClock.Text = Game.PlayerBlack.Clock.TimeRemaining.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeRemaining.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeRemaining.Seconds.ToString().PadLeft(2,'0');
			}
			else
			{
				lblWhiteClock.Text = Game.PlayerWhite.Clock.TimeElapsedDisplay.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeElapsedDisplay.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerWhite.Clock.TimeElapsedDisplay.Seconds.ToString().PadLeft(2,'0');
				lblBlackClock.Text = Game.PlayerBlack.Clock.TimeElapsedDisplay.Hours.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeElapsedDisplay.Minutes.ToString().PadLeft(2,'0') + ":" + Game.PlayerBlack.Clock.TimeElapsedDisplay.Seconds.ToString().PadLeft(2,'0');
			}
			lblWhiteClock.Refresh();
			lblBlackClock.Refresh();
		}

		private void RenderBoard()
		{
			Square square;

            RenderBoardColours();

			for (int intOrdinal=0; intOrdinal<Board.SQUARE_COUNT; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);
				
				if (square!=null)
				{
                    if (square.Piece == null || square == m_squareFrom) //  || Game.IsPaused
					{
						m_picSquares[square.File, square.Rank].Image = null;
					}
					else
					{
						m_picSquares[square.File, square.Rank].Image = GetPieceImage(square.Piece);
					}

					m_picSquares[square.File, square.Rank].BorderStyle = System.Windows.Forms.BorderStyle.None;
				}
			}

			// Render Last Move highlights
            if (!Game.EditModeActive && Game.MoveHistory.Count > 0)
			{
				m_picSquares[Game.MoveHistory[Game.MoveHistory.Count-1].From.File, Game.MoveHistory[Game.MoveHistory.Count-1].From.Rank].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				m_picSquares[Game.MoveHistory[Game.MoveHistory.Count-1].To.File  , Game.MoveHistory[Game.MoveHistory.Count-1].To.Rank  ].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			}

			// Render pieces taken
			for (int intIndex=0; intIndex<15; intIndex++)
			{
				m_picWhitesCaptures[intIndex].Image = null;
				m_picBlacksCaptures[intIndex].Image = null;
			}
			for (int intIndex=0; intIndex<Game.PlayerWhite.CapturedEnemyPieces.Count; intIndex++)
			{
				m_picWhitesCaptures[intIndex].Image = GetPieceImage(Game.PlayerWhite.CapturedEnemyPieces.Item(intIndex));
			}
			for (int intIndex=0; intIndex<Game.PlayerBlack.CapturedEnemyPieces.Count; intIndex++)
			{
				m_picBlacksCaptures[intIndex].Image = GetPieceImage(Game.PlayerBlack.CapturedEnemyPieces.Item(intIndex));
			}

			// Render player status
			if (Game.PlayerToPlay == Game.PlayerWhite)
			{
				lblWhiteClock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				lblBlackClock.BorderStyle = System.Windows.Forms.BorderStyle.None;
				lblWhiteClock.BackColor = Game.PlayerWhite.Status==Player.enmStatus.InCheckMate ? Color.Red : (Game.PlayerWhite.IsInCheck ? Color.Orange: Color.LightGray);
				lblBlackClock.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
			}
			else
			{
				lblBlackClock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				lblWhiteClock.BorderStyle = System.Windows.Forms.BorderStyle.None;
				lblWhiteClock.BackColor = Color.FromName(System.Drawing.KnownColor.Control.ToString());
				lblBlackClock.BackColor = Game.PlayerBlack.Status==Player.enmStatus.InCheckMate ? Color.Red : (Game.PlayerBlack.IsInCheck ? Color.Orange : Color.LightGray );
			}

			lblBlackClock.ForeColor = Color.Black;
			lblWhiteClock.ForeColor = Color.Black;
			lblGamePaused.Visible = false;

			// Set form state
            lblWhitesCaptures.Text = (Game.PlayerWhite.CapturedEnemyPiecesTotalBasicValue).ToString();
            lblBlacksCaptures.Text = (Game.PlayerBlack.CapturedEnemyPiecesTotalBasicValue).ToString();

			lblWhitePosition.Text = Game.PlayerWhite.PositionPoints.ToString();
			lblBlackPosition.Text = Game.PlayerBlack.PositionPoints.ToString();

			lblWhitePoints.Text = Game.PlayerWhite.Points.ToString();
			lblBlackPoints.Text = Game.PlayerBlack.Points.ToString();

			lblWhiteScore.Text = Game.PlayerWhite.Score.ToString();
			lblBlackScore.Text = Game.PlayerBlack.Score.ToString();

			lblStage.Text  = Game.Stage.ToString() + " Game - ";
			switch (Game.PlayerToPlay.Status)
			{
				case Player.enmStatus.Normal:
					lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " to play";
					//	lblStage.Text = "A: " + Board.HashCodeA.ToString() + "     B: " + Board.HashCodeB.ToString();
					break;

				case Player.enmStatus.InCheck:
					lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in check!";
					break;

				case Player.enmStatus.InCheckMate:
					lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in checkmate!";
					break;

				case Player.enmStatus.InStaleMate:
					lblStage.Text += Game.PlayerToPlay.Colour.ToString() + " in stalemate!";
					break;
			}

			// Update move history
			while (lvwMoveHistory.Items.Count < Game.MoveHistory.Count)
			{
				AddMoveToHistory(Game.MoveHistory[lvwMoveHistory.Items.Count]);
			}
			while (lvwMoveHistory.Items.Count > Game.MoveHistory.Count)
			{
				RemoveLastHistoryItem();
			}

			SetFormState();

			RenderStatus();

			this.Text = Application.ProductName + " - " + Game.FileName;

			this.Refresh();
		}

        void RenderBoardColours()
        {
            Square square;

            for (int intOrdinal = 0; intOrdinal < Board.SQUARE_COUNT; intOrdinal++)
            {
                square = Board.GetSquare(intOrdinal);

                if (square != null)
                {
                    if (square.Colour == Square.enmColour.White)
                    {
                        m_picSquares[square.File, square.Rank].BackColor = BOARD_SQUARE_COLOUR_WHITE;
                    }
                    else
                    {
                        m_picSquares[square.File, square.Rank].BackColor = BOARD_SQUARE_COLOUR_BLACK;
                    }
                }
            }

            
            // Render selection highlights
            if (m_squareFrom != null)
            {
                foreach (Move move in m_movesPossible)
                {
                    m_picSquares[move.To.File, move.To.Rank].BackColor = (Board.GetSquare((int)m_picSquares[move.To.File, move.To.Rank].Tag).Colour == Square.enmColour.White ? BOARD_SQUARE_COLOUR_WHITE_BRIGHT : BOARD_SQUARE_COLOUR_BLACK_BRIGHT);
                }
            }

        }

        private Image GetPieceImage(Piece piece)
        {
            return imgPieces.Images[piece.ImageIndex];
        }

        private void RenderStatus()
		{
			Player playerToPlay = Game.PlayerToPlay;

			string strMsg = "";

			if (playerToPlay.IsThinking )
			{
				strMsg += Game.PlayerToPlay.IsPondering ? "Pondering..." : "Thinking...";

				if (Game.ShowThinking)
				{
					strMsg += "Ply: " + playerToPlay.SearchDepth.ToString() + "/" + playerToPlay.MaxSearchDepth.ToString();
					strMsg += ". Move: " + playerToPlay.SearchPositionNo.ToString() + "/" + playerToPlay.TotalPositionsToSearch.ToString();
				}

				if (!Game.PlayerToPlay.IsPondering)
				{
					strMsg += ". Secs: " + ((int)(playerToPlay.ThinkingTimeRemaining.TotalSeconds)).ToString() + "/" + ((int)playerToPlay.ThinkingTimeAllotted.TotalSeconds).ToString();
				}
				if (Game.ShowThinking)
				{
					strMsg += " Pos: " + playerToPlay.PositionsSearched + " Q: " + playerToPlay.MaxQuiesDepth  + " E: " + playerToPlay.MaxExtensions;
					strMsg += " P/S: " + playerToPlay.PositionsPerSecond.ToString();
					if (!Game.PlayerToPlay.IsPondering)
					{
						if (playerToPlay.PrincipalVariation!=null && playerToPlay.PrincipalVariation.Count>0)
						{
							strMsg += " Scr:" + playerToPlay.PrincipalVariation[0].Score;
						}
						strMsg += " " + playerToPlay.PrincipalVariationText;
					}
				}
			}
			else
			{
				if (Game.MoveHistory.Count>0)
				{
//					strMsg += "Last move: " + Game.MoveHistory.Last.Piece.Player.Colour.ToString() + ": " + Game.MoveHistory.Last.Description;
				}
			}

			if (!Game.ShowThinking || !Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering)
			{
				pbr.Maximum = 0;
				pbr.Value = 0;
			}
			else
			{
				pbr.Maximum = Math.Max(playerToPlay.TotalPositionsToSearch, playerToPlay.SearchPositionNo);
				pbr.Value = playerToPlay.SearchPositionNo;
			}

			if (strMsg!="" && sbr.Text!=strMsg)
			{	
				sbr.Text = strMsg;
			}
		}

		private void SetFormState()
		{
			mnuNew.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
			mnuOpen.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
			mnuSave.Enabled = !Game.EditModeActive && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            mnuUndoMove.Enabled = !Game.EditModeActive && !WinBoard.Active && (((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering) && Game.MoveHistory.Count > 0));
            mnuRedoMove.Enabled = !Game.EditModeActive && !WinBoard.Active && (((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering) && Game.MoveRedoList.Count > 0));
            mnuUndoAllMoves.Enabled = !Game.EditModeActive && !WinBoard.Active && (mnuUndoMove.Enabled);
            mnuRedoAllMoves.Enabled = !Game.EditModeActive && !WinBoard.Active && (mnuRedoMove.Enabled);
            mnuEditBoardPosition.Enabled = !WinBoard.Active;
			mnuPasteFEN.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            mnuThink.Enabled = !Game.EditModeActive && !WinBoard.Active && (((!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering) && Game.PlayerToPlay.CanMove));
            mnuMoveNow.Enabled = !Game.EditModeActive && !WinBoard.Active && (Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering);
            mnuResumePlay.Enabled = !Game.EditModeActive && !WinBoard.Active && Game.IsPaused && Game.PlayerToPlay.CanMove;
            mnuPausePlay.Enabled = !Game.EditModeActive && !WinBoard.Active && (!Game.IsPaused);
            mnuDifficulty.Enabled = !Game.EditModeActive && !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
            mnuGame.Enabled = !Game.EditModeActive;
            mnuComputer.Enabled = !Game.EditModeActive;
            mnuShowThinking.Enabled = !Game.EditModeActive;

			tbrNew.Enabled = mnuNew.Enabled;
			tbrOpen.Enabled = mnuOpen.Enabled;
			tbrSave.Enabled = mnuSave.Enabled;
			tbrUndoMove.Enabled = mnuUndoMove.Enabled;
			tbrRedoMove.Enabled = mnuRedoMove.Enabled;
			tbrUndoAllMoves.Enabled = mnuUndoAllMoves.Enabled;
			tbrRedoAllMoves.Enabled = mnuRedoAllMoves.Enabled;
			tbrThink.Enabled = mnuThink.Enabled;
			tbrMoveNow.Enabled = mnuMoveNow.Enabled;
			tbrResumePlay.Enabled = mnuResumePlay.Enabled;
			tbrPausePlay.Enabled = mnuPausePlay.Enabled;
			
			cboIntellegenceWhite.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);
			cboIntellegenceBlack.Enabled = !WinBoard.Active && (!Game.PlayerToPlay.IsThinking || Game.PlayerToPlay.IsPondering);

			foreach (PictureBox pic in m_picSquares)
			{
				pic.Enabled = !WinBoard.Active && Game.PlayerToPlay.CanMove; // && (!Game.IsPaused)
			}
			cboIntellegenceWhite.SelectedIndex = Game.PlayerWhite.Intellegence==Player.enmIntellegence.Human ? INTELLEGENCE_HUMAN : INTELLEGENCE_COMPUTER;
			cboIntellegenceBlack.SelectedIndex = Game.PlayerBlack.Intellegence==Player.enmIntellegence.Human ? INTELLEGENCE_HUMAN : INTELLEGENCE_COMPUTER;
		}

		private void CreateBoard()
		{
			PictureBox picSquare;
			Square square;
			Label lblRank;
			Label lblFile;

			for (int intRank=0; intRank<Board.RANK_COUNT; intRank++)
			{
				lblRank = new System.Windows.Forms.Label();
				lblRank.BackColor = System.Drawing.Color.Transparent;
				lblRank.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				lblRank.Name = "lblRank" + intRank.ToString();
				lblRank.Size = new System.Drawing.Size(SQUARE_SIZE/2, SQUARE_SIZE);
				lblRank.TabIndex = 12;
				lblRank.Text = Board.GetSquare(0, intRank).RankName;
				lblRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				lblRank.Left = 0;
				lblRank.Top = (Board.RANK_COUNT-1)*SQUARE_SIZE - intRank*SQUARE_SIZE + 16;
				pnlMain.Controls.Add( lblRank );
			}

			m_picSquares = new PictureBox[Board.FILE_COUNT, Board.RANK_COUNT];

			for (int intFile=0; intFile<Board.FILE_COUNT; intFile++)
			{

				lblFile = new System.Windows.Forms.Label();
				lblFile.BackColor = System.Drawing.Color.Transparent;
				lblFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				lblFile.Name = "lblFile" + intFile.ToString();
				lblFile.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE/2);
				lblFile.TabIndex = 12;
				lblFile.Text = Board.GetSquare(intFile, 0).FileName;
				lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				lblFile.Left = intFile*SQUARE_SIZE + 30;
				lblFile.Top = (Board.RANK_COUNT)*SQUARE_SIZE + 24;
				pnlMain.Controls.Add( lblFile );
				
			}

			for (int intOrdinal=0; intOrdinal<Board.SQUARE_COUNT; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);

				if (square!=null)
				{
					picSquare = new System.Windows.Forms.PictureBox();

					if (square.Colour == Square.enmColour.White)
					{
						picSquare.BackColor = BOARD_SQUARE_COLOUR_WHITE;
					}
					else
					{
						picSquare.BackColor = BOARD_SQUARE_COLOUR_BLACK;
					}
					picSquare.Name = "picSquare" + square.File.ToString() + square.Rank.ToString();
					picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
					picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
					picSquare.TabIndex = 0;
					picSquare.TabStop = false;
					picSquare.Tag = square.Ordinal;
                    picSquare.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSquare_MouseDown);
                    picSquare.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picSquare_MouseUp);
                    picSquare.DragDrop += new System.Windows.Forms.DragEventHandler(this.picSquare_DragDrop);
                    picSquare.DragOver += new System.Windows.Forms.DragEventHandler(this.picSquare_DragOver);
					picSquare.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.picSquare_GiveFeedback);
					picSquare.AllowDrop = true;
					pnlEdging.Controls.Add( picSquare );
					m_picSquares[square.File, square.Rank] = picSquare;
				}
			}

			m_picWhitesCaptures = new PictureBox[15];
			m_picBlacksCaptures = new PictureBox[15];

			for (int intIndex=0; intIndex<15; intIndex++)
			{
				picSquare = new System.Windows.Forms.PictureBox();
				picSquare.Left = intIndex*(SQUARE_SIZE+1)+1;
				picSquare.Top = 384;
				picSquare.BackColor = System.Drawing.SystemColors.ControlDark;
				picSquare.Name = "picSquareWhite" + intIndex.ToString();
				picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
				picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				m_picWhitesCaptures[intIndex] = picSquare;

				picSquare = new System.Windows.Forms.PictureBox();
				picSquare.Left = intIndex*(SQUARE_SIZE+1)+1;
				picSquare.Top = 384 + SQUARE_SIZE+1;
				picSquare.BackColor = System.Drawing.SystemColors.ControlDark;
				picSquare.Name = "picSquareBlack" + intIndex.ToString();
				picSquare.Size = new System.Drawing.Size(SQUARE_SIZE, SQUARE_SIZE);
				picSquare.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
				picSquare.TabIndex = 0;
				picSquare.TabStop = false;
				picSquare.Tag = intIndex;
				pnlMain.Controls.Add( picSquare );
				m_picBlacksCaptures[intIndex] = picSquare;
			}
		}

		private void OrientBoard()
		{
			PictureBox picSquare;
			Square square;

			for (int intOrdinal=0; intOrdinal<Board.SQUARE_COUNT; intOrdinal++)
			{
				square = Board.GetSquare(intOrdinal);

				if (square!=null)
				{
					picSquare = m_picSquares[square.File, square.Rank];
					switch (Board.Orientation)
					{
						case Board.enmOrientation.White:
							picSquare.Left = square.File*SQUARE_SIZE + 1;
							picSquare.Top  = (Board.RANK_COUNT-1)*SQUARE_SIZE - square.Rank*SQUARE_SIZE + 1;
							break;

						case Board.enmOrientation.Black:
							picSquare.Left = (Board.FILE_COUNT-1)*SQUARE_SIZE - square.File*SQUARE_SIZE + 1;
							picSquare.Top  = square.Rank*SQUARE_SIZE + 1;
							break;
					}
				}
			}

			// 29Mar05 Nimzo - Made the rank and file labels flip also.
			for (int indCtrl = 0; indCtrl < pnlMain.Controls.Count; indCtrl++) // new
			{   // Position each label of coordinates according to the orientation
				string strName = pnlMain.Controls[indCtrl].Name;
				if (strName.StartsWith("lblFile"))
				{    // For the hard-coded constants "lblFile" and "+ 30" see CreateBoard()
					int iFileSize = System.Convert.ToInt32(strName.Substring(7, strName.Length - 7)) * SQUARE_SIZE;
					pnlMain.Controls[indCtrl].Left = ((Board.Orientation == Board.enmOrientation.White) ?
					iFileSize : (Board.FILE_COUNT - 1) * SQUARE_SIZE - iFileSize) + 30;
				}
				else if (strName.StartsWith("lblRank"))
				{   // For the hard-coded constants "lblRank" and "+ 16" see CreateBoard()
					int iRankSize = System.Convert.ToInt32(strName.Substring(7, strName.Length - 7)) * SQUARE_SIZE;
					pnlMain.Controls[indCtrl].Top = ((Board.Orientation == Board.enmOrientation.White) ?
						(Board.RANK_COUNT - 1) * SQUARE_SIZE - iRankSize : iRankSize) + 16;
				}
			} 		
		}

		private void AddMoveToHistory(Move move)
		{
			string[] lvi = {	move.MoveNo.ToString(), move.TimeStamp.Hours.ToString().PadLeft(2,'0') + ":" + move.TimeStamp.Minutes.ToString().PadLeft(2,'0') + ":" + move.TimeStamp.Seconds.ToString().PadLeft(2,'0') 
							   , move.Description + (move.pieceCaptured!=null ? (" (" + move.pieceCaptured.Name.ToString() + ")") : "")
						   };

			lvwMoveHistory.Items.Add( new ListViewItem( lvi ) );
			switch (move.Piece.Player.Colour)
			{
				case Player.enmColour.White:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Blue;
					break;

				case Player.enmColour.Black:
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].BackColor = Color.White;
					lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].ForeColor = Color.Black;
					break;
			}
			lvwMoveHistory.Items[lvwMoveHistory.Items.Count-1].EnsureVisible();
		}

		private void RemoveLastHistoryItem()
		{
			lvwMoveHistory.Items.RemoveAt(lvwMoveHistory.Items.Count-1);
			m_squareFrom = null;
			m_movesPossible = new Moves();
		}

		private void RenderMoveAnalysis()
		{
			m_formMoveAnalysis.RenderMoveAnalysis();
		}

		private void NewGame()
		{
			frmDifficulty formDifficulty = new frmDifficulty();
			formDifficulty.ShowDialog(this);
			if (formDifficulty.Confirmed)
			{
				Game.New();
			}
		}

		private void OpenGame()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			openFileDialog.Title = "Load a saved chess game" ;
			openFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
			openFileDialog.FilterIndex = 2 ;

			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if( openFileDialog.FileName!="" )
				{
					lvwMoveHistory.Items.Clear();
					Game.Load(openFileDialog.FileName);
				}
			}

			OrientBoard();
			SetFormState();
		}

		private void SaveGame()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
 
			saveFileDialog.Filter = "SharpChess files (*.SharpChess)|*.SharpChess";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.FileName = Game.FileName;
 
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if( saveFileDialog.FileName!="" )
				{
					Game.Save(saveFileDialog.FileName);
				}
			}
			this.Text = Application.ProductName + " - " + Game.FileName;
		}

		private void UndoMove()
		{
			Game.UndoMove();
		}

		private void RedoMove()
		{
			Game.RedoMove();
		}

		private void UndoAllMoves()
		{
			Game.UndoAllMoves();
		}

		private void RedoAllMoves()
		{
			Game.RedoAllMoves();
		}

        private void EditBoardPosition()
        {
            Game.ToggleEditMode();
            AssignMenuChecks();
            SetFormState();
            RenderBoard();
        }

		private void Think()
		{
			Game.Think();
		}

		private void MoveNow()
		{
			if ( Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering)
			{
				Game.PlayerToPlay.ForceImmediateMove();
			}
		}

		public void FlipBoard()
		{
			Board.Flip();
			OrientBoard();
		}

		private void cboIntellegenceWhite_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Game.PlayerWhite.Intellegence = cboIntellegenceWhite.SelectedIndex==INTELLEGENCE_HUMAN ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
		}

		private void cboIntellegenceBlack_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Game.PlayerBlack.Intellegence = cboIntellegenceBlack.SelectedIndex==INTELLEGENCE_HUMAN ? Player.enmIntellegence.Human : Player.enmIntellegence.Computer;
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout formAbout = new frmAbout();
			formAbout.ShowDialog(this);
		}

		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			NewGame();
		}

		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			OpenGame();
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			SaveGame();
		}

		private void mnuUndoMove_Click(object sender, System.EventArgs e)
		{
			UndoMove();
		}

        private void mnuEditPosition_Click(object sender, EventArgs e)
        {
            EditBoardPosition();
        }

        private void mnuRedoMove_Click(object sender, System.EventArgs e)
		{
			RedoMove();
		}


		private void mnuUndoAllMoves_Click(object sender, System.EventArgs e)
		{
			UndoAllMoves();		
		}

		private void mnuRedoAllMoves_Click(object sender, System.EventArgs e)
		{
			RedoAllMoves();		
		}
		private void mnuThink_Click(object sender, System.EventArgs e)
		{
			Think();
		}

		private void mnuMoveNow_Click(object sender, System.EventArgs e)
		{
			MoveNow();
		}

		private void mnuFlipBoard_Click(object sender, System.EventArgs e)
		{
			FlipBoard();
		}

		private void mnuPausePlay_Click(object sender, System.EventArgs e)
		{
			PausePlay();
		}

		private void mnuResumePlay_Click(object sender, System.EventArgs e)
		{
			ResumePlay();
		}

		private void tbr_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(e.Button.Tag.ToString())
			{
				case "New":
					NewGame();
					break; 

				case "Open":
					OpenGame();
					break; 

				case "Save":
					SaveGame();
					break; 

				case "UndoMove":
					UndoMove();
					break;

				case "RedoMove":
					RedoMove();
					break;

				case "UndoAllMoves":
					UndoAllMoves();
					break;

				case "RedoAllMoves":
					RedoAllMoves();
					break;

				case "FlipBoard":
					FlipBoard();
					break;

				case "Think":
					Think();
					break;

				case "MoveNow":
					MoveNow();
					break;

				case "ResumePlay":
					ResumePlay();
					break;

				case "PausePlay":
					PausePlay();
					break;
			}
		}

		private void mnuDifficulty_Click(object sender, System.EventArgs e)
		{
			frmDifficulty formDifficulty = new frmDifficulty();
			formDifficulty.ShowDialog(this);
			Game.SettingsUpdate();
		}

		private void mnuShowThinking_Click(object sender, System.EventArgs e)
		{
			Game.ShowThinking = !Game.ShowThinking;
			AssignMenuChecks();
			SizeHistoryPane();
		}

		private void mnuDisplayMoveAnalysisTree_Click(object sender, System.EventArgs e)
		{
			if (m_formMoveAnalysis.Visible)
			{
				m_formMoveAnalysis.Hide();
			}
			else
			{
				m_formMoveAnalysis.Show();
			}
			AssignMenuChecks();
		}

		private void mnuDisplayWinBoardMessageLog_Click(object sender, System.EventArgs e)
		{
			if (m_formWinBoard.Visible)
			{
				m_formWinBoard.Hide();
			}
			else
			{
				m_formWinBoard.Show();
			}
			AssignMenuChecks();
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			this.Close(); // 01Apr05 Nimzo Close down threads properly before exit.
			Application.Exit();
		}

		private void AssignMenuChecks()
		{
			mnuShowThinking.Checked = Game.ShowThinking;
			Game.CaptureMoveAnalysisData = mnuDisplayMoveAnalysisTree.Checked = m_formMoveAnalysis.Visible;
            mnuDisplayWinBoardMessageLog.Checked = m_formWinBoard.Visible;
            mnuEditBoardPosition.Checked = Game.EditModeActive;
        }

		/// <summary>Change Top and Height to hide or not the evaluation of the position</summary>
		/// <remarks>If <see cref="Game.ShowThinking"/>, show the evaluation of the position</remarks>
		private void SizeHistoryPane()
		{
			// The hard-coded constants allow to see the entire last visible row
			lvwMoveHistory.Top = (Game.ShowThinking) ?
				lblWhitePosition.Top + lblWhitePosition.Height + 13 :
				lblWhiteClock.Top + lblWhiteClock.Height + 9;
			lvwMoveHistory.Height = lblStage.Top - lvwMoveHistory.Top;

		} // end SizeHistoryPane

		private void timer_Tick(object sender, System.EventArgs e)
		{
			RenderClocks();
		}

		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Game.TerminateGame();
		}

		private void PausePlay()
		{
			Game.PausePlay();
		}

		private void ResumePlay()
		{
			Game.ResumePlay();
		}

		private void btnPGNtoXML_Click(object sender, System.EventArgs e)
		{
//			Game.PGNtoXML();
		}

		private void btnXMLtoOB_Click(object sender, System.EventArgs e)
		{
//			Game.XMLtoOB();
		}

		private void btnPrune_Click(object sender, System.EventArgs e)
		{
			txtOutput.Text = OpeningBookSimple.Import();
		}


		private void mnuCopyFEN_Click(object sender, System.EventArgs e)
		{
			// Put FEN position string into the clipboard
			Clipboard.SetDataObject(FEN.GetBoardPosition()); 		
		}

		private void mnuPasteFEN_Click(object sender, System.EventArgs e)
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
				catch (FEN.ValidationException x)
				{
					MessageBox.Show(x.FENMessage); 
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

		private void btnPerft_Click(object sender, System.EventArgs e)
		{
			Game.PlayerToPlay.Perft((int)numPerftDepth.Value);
			MessageBox.Show( Game.PlayerToPlay.PositionsSearched.ToString() );
		}

        /// <summary>
        /// Get this cursor for the specfied piece
        /// </summary>
        private Cursor GetPieceCursor(Piece piece)
        {
            return m_acurPieceCursors[piece.ImageIndex];
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

            m_acurPieceCursors[0] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackBishop.cur"));
            m_acurPieceCursors[1] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteBishop.cur"));
            m_acurPieceCursors[2] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackRook.cur"));
            m_acurPieceCursors[3] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteRook.cur"));
            m_acurPieceCursors[4] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackKing.cur"));
            m_acurPieceCursors[5] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteKing.cur"));
            m_acurPieceCursors[6] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackKnight.cur"));
            m_acurPieceCursors[7] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteKnight.cur"));
            m_acurPieceCursors[8] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackPawn.cur"));
            m_acurPieceCursors[9] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhitePawn.cur"));
            m_acurPieceCursors[10] = new Cursor(asmMain.GetManifestResourceStream(strPath + "BlackQueen.cur"));
            m_acurPieceCursors[11] = new Cursor(asmMain.GetManifestResourceStream(strPath + "WhiteQueen.cur"));
        }

        private void picSquare_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_blnInMouseDown || e.Button != MouseButtons.Left)
            {
                return;
            }
            m_blnIsLeftMouseButtonDown = true;
            m_blnInMouseDown = true;

            if (Game.PlayerToPlay.IsThinking && !Game.PlayerToPlay.IsPondering) return;

            Game.SuspendPondering();

            PictureBox picFrom = (PictureBox)sender;

            int intOrdinalFrom = Convert.ToInt32(picFrom.Tag);

            Square squareFrom = Board.GetSquare(intOrdinalFrom);

            Piece pieceFrom = squareFrom.Piece;
            if (pieceFrom != null && pieceFrom.Player.Colour == Game.PlayerToPlay.Colour)
            {
                picFrom.Image = null;
                picFrom.Refresh();

                m_curPieceCursor = GetPieceCursor(pieceFrom);
                pnlEdging.Cursor = m_curPieceCursor;

                // Mark possible moves
                m_squareFrom = squareFrom;
                m_squareTo = null;
                m_movesPossible = new Moves();
                pieceFrom.GenerateLegalMoves(m_movesPossible);
                RenderBoardColours();
                this.pnlEdging.Refresh();

                Game.ResumePondering();

                if ( m_blnIsLeftMouseButtonDown && (((PictureBox)sender).DoDragDrop(pieceFrom, DragDropEffects.Move)) == DragDropEffects.Move )
                {
                    Game.SuspendPondering();
                    
                    bool blnMoveMade = false;
                    Piece pieceTo = m_squareTo.Piece;

                    // Is it an empty space or enemy piece
                    if (pieceTo == null || pieceTo != null && pieceTo.Player.Colour != Game.PlayerToPlay.Colour)
                    {
                        // Check to see it the move is valid, by comparing against all possible valid moves

                        bool blnIsPromotion = false;
                        SharpChess.Move.enmName movenamePromotion = SharpChess.Move.enmName.NullMove;
                        foreach (Move move in m_movesPossible)
                        {
                            if (move.To == m_squareTo)
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
                                    m_squareFrom = null;
                                    m_movesPossible = new Moves();

                                    Game.MakeAMove(move.Name, move.Piece, move.To);
                                    blnMoveMade = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!blnMoveMade)
                    {
                        m_picSquares[m_squareFrom.File, m_squareFrom.Rank].Image = this.imgPieces.Images[m_squareFrom.Piece.ImageIndex];
                        m_squareFrom = null;
                        m_movesPossible = null;
                        RenderBoardColours();
                    }

                }
                else
                {
                    Game.SuspendPondering();

                    m_picSquares[m_squareFrom.File, m_squareFrom.Rank].Image = this.imgPieces.Images[m_squareFrom.Piece.ImageIndex];
                    m_squareFrom = null;
                    m_movesPossible = null;
                    RenderBoardColours();

                    Game.ResumePondering();

                }

                pnlEdging.Cursor = Cursors.Default;
            }
            else
            {
                Game.ResumePondering();
            }

            m_blnInMouseDown = false;
        }

        private void picSquare_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_blnIsLeftMouseButtonDown = false;
        }

        private void picSquare_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void picSquare_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            int intOrdinal = Convert.ToInt32(((PictureBox)sender).Tag);
            m_squareTo = Board.GetSquare(intOrdinal);
        }

        private void picSquare_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e.UseDefaultCursors != false)
            {
                e.UseDefaultCursors = false;
            }
            if (pnlEdging.Cursor != this.Cursor )
            {
                pnlEdging.Cursor = m_curPieceCursor;
            }
        }

	}
}
