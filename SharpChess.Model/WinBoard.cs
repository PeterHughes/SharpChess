// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinBoard.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Responsible for WinBoard Communication.
//   Chess Engine Communication Protocol (WinBoard) comment taken from: http://www.gnu.org/software/xboard/engine-intf.html
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
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

namespace SharpChess.Model
{
    #region Using

    using System;
    using System.Threading;

    #endregion

    /// <summary>
    /// Responsible for WinBoard Communication. 
    /// Chess Engine Communication Protocol (WinBoard) comment taken from: http://www.gnu.org/software/xboard/engine-intf.html
    /// </summary>
    public class WinBoard
    {
        #region Delegates

        /// <summary>
        /// The delegatetype communication event.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        public delegate void CommunicationEvent(string strMessage);

        /// <summary>
        /// The delegatetype standard event.
        /// </summary>
        public delegate void StandardEvent();

        #endregion

        #region Public Events

        /// <summary>
        ///   Winboard input event.
        /// </summary>
        public static event CommunicationEvent WinBoardInputEvent;

        /// <summary>
        ///  Winboard output event.
        /// </summary>
        public static event CommunicationEvent WinBoardOutputEvent;

        /// <summary>
        ///  Winboard quit event.
        /// </summary>
        public static event StandardEvent WinBoardQuitEvent;

        /// <summary>
        ///  Winboard time updated event.
        /// </summary>
        public static event StandardEvent WinBoardTimeUpdatedEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether Winboard is Active.
        /// </summary>
        public static bool Active { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether to show the Winboard message GUI.
        /// </summary>
        public static bool ShowGui { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the thread that this Winboard class run in.
        /// </summary>
        private static Thread ThreadListener { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if Winboard is present and sets the Active property if true.
        /// </summary>
        public static void QueryAndSetWinboardActiveStatus()
        {
            string strMessage = Console.ReadLine();
            Active = strMessage != null && strMessage.StartsWith("xboard");

            if (Active && strMessage != null)
            {
                ShowGui = strMessage.Contains("showgui");
                WinBoardInputEvent(strMessage);
            }

            SendOutputMessage("\n");
        }

        /// <summary>
        /// The process input event.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        /// <exception cref="WinBoardInputException">
        /// Indicates failure to process winboard command.
        /// </exception>
        public static void ProcessInputEvent(string strMessage)
        {
            try
            {
                if (strMessage.StartsWith("xboard"))
                {
                    /*
                     * Chess Engine Communication Protocol 
                        This command will be sent once immediately after your engine process is started. 
                        You can use it to put your engine into "xboard mode" if that is needed. 
                        If your engine prints a prompt to ask for user input, you must turn off the prompt and output a 
                        newline when the "xboard" command comes in.
                        This will be a false 2nd "ghost" message, so ignore it.
                    */
                }
                else if (strMessage.StartsWith("protover "))
                {
                    /*
                     * Chess Engine Communication Protocol
                        ping (boolean, default 0, recommended 1) 
                        If ping=1, xboard may use the protocol's new "ping" command; if ping=0, xboard will not use the command. 
                        setboard (boolean, default 0, recommended 1) 
                        If setboard=1, xboard will use the protocol's new "setboard" command to set up positions; if setboard=0, it will use the older "edit" command. 
                        playother (boolean, default 0, recommended 1) 
                        If playother=1, xboard will use the protocol's new "playother" command when appropriate; if playother=0, it will not use the command. 
                        san (boolean, default 0) 
                        If san=1, xboard will send moves to the engine in standard algebraic notation (SAN); for example, Nf3. If san=0, xboard will send moves in coordinate notation; for example, g1f3. See MOVE in section 8 above for more details of both kinds of notation. 
                        usermove (boolean, default 0) 
                        If usermove=1, xboard will send moves to the engine with the command "usermove MOVE"; if usermove=0, xboard will send just the move, with no command name. 
                        time (boolean, default 1, recommended 1) 
                        If time=1, xboard will send the "time" and "otim" commands to update the engine's clocks; if time=0, it will not. 
                        draw (boolean, default 1, recommended 1) 
                        If draw=1, xboard will send the "draw" command if the engine's opponent offers a draw; if draw=0, xboard will not inform the engine about draw offers. Note that if draw=1, you may receive a draw offer while you are on move; if this will cause you to move immediately, you should set draw=0. 
                        sigint (boolean, default 1) 
                        If sigint=1, xboard may send SIGINT (the interrupt signal) to the engine as section 7 above; if sigint=0, it will not. 
                        sigterm (boolean, default 1) 
                        If sigterm=1, xboard may send SIGTERM (the termination signal) to the engine as section 7 above; if sigterm=0, it will not. 
                        reuse (boolean, default 1, recommended 1) 
                        If reuse=1, xboard may reuse your engine for multiple games. If reuse=0 (or if the user has set the -xreuse option on xboard's command line), xboard will kill the engine process after every game and start a fresh process for the next game. 
                        analyze (boolean, default 1, recommended 1) 
                        If analyze=0, xboard will not try to use the "analyze" command; it will pop up an error message if the user asks for analysis mode. If analyze=1, xboard will try to use the command if the user asks for analysis mode. 
                        myname (string, default determined from engine filename) 
                        This feature lets you set the name that xboard will use for your engine in window banners, in the PGN tags of saved game files, and when sending the "name" command to another engine. 
                        variants (string, see text below) 
                        This feature indicates which chess variants your engine accepts. It should be a comma-separated list of variant names. See the table under the "variant" command in section 8 above. If you do not set this feature, xboard will assume by default that your engine supports all variants. (However, the -zippyVariants command-line option still limits which variants will be accepted in Zippy mode.) It is recommended that you set this feature to the correct value for your engine (just "normal" in most cases) rather than leaving the default in place, so that the user will get an appropriate error message if he tries to play a variant that your engine does not support. 
                        colors (boolean, default 1, recommended 0) 
                        If colors=1, xboard uses the obsolete "white" and "black" commands in a stylized way that works with most older chess engines that require the commands. See the "Idioms" section below for details. If colors=0, xboard does not use the "white" and "black" commands at all. 
                        ics (boolean, default 0) 
                        If ics=1, xboard will use the protocol's new "ics" command to inform the engine of whether or not it is playing on a chess server; if ics=0, it will not. 
                        name (boolean, see text below) 
                        If name=1, xboard will use the protocol's "name" command to inform the engine of the opponent's name; if name=0, it will not. By default, name=1 if the engine is playing on a chess server; name=0 if not. 
                        pause (boolean, default 0) 
                        If pause=1, xboard may use the protocol's new "pause" command; if pause=0, xboard assumes that the engine does not support this command. 
                        done (integer, no default) 
                        If you set done=1 during the initial two-second timeout after xboard sends you the "xboard" command, the timeout will end and xboard will not look for any more feature commands before starting normal operation. If you set done=0, the initial timeout is increased to one hour; in this case, you must set done=1 before xboard will enter normal operation. 
                    */
                    string strFeatures = string.Empty;
                    strFeatures += " ping=1 ";
                    strFeatures += " setboard=1";
                    strFeatures += " playother=1";
                    strFeatures += " san=0";
                    strFeatures += " usermove=1";
                    strFeatures += " time=1";
                    strFeatures += " draw=0";
                    strFeatures += " sigint=0";
                    strFeatures += " sigterm=0";
                    strFeatures += " reuse=1";
                    strFeatures += " analyze=1";
                    strFeatures += " myname=\"SharpChess\"";
                    strFeatures += " variants=\"normal\"";
                    strFeatures += " colors=0";
                    strFeatures += " ics=0";
                    strFeatures += " name=0";
                    strFeatures += " pause=1";
                    strFeatures += " done=1";
                    SendOutputMessage("feature" + strFeatures);
                }
                else if (strMessage.StartsWith("accepted "))
                {
                    // Feature request is accepted.
                }
                else if (strMessage.StartsWith("rejected "))
                {
                    // Feature request is rejected.
                }
                else if (strMessage == "new")
                {
                    Game.SuspendPondering();
                    Game.DifficultyLevel = 0;
                    Game.ClockMaxMoves = 40;
                    Game.ClockTime = new TimeSpan(0, 5, 0);
                    Game.UseRandomOpeningMoves = true;
                    Game.MaximumSearchDepth = 32;
                    Game.EnablePondering = false;
                    Game.ClockIncrementPerMove = new TimeSpan(0, 0, 0);
                    Game.ClockFixedTimePerMove = new TimeSpan(0, 0, 0);
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.New();
                    Game.SuspendPondering();
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.ResumePondering();
                }
                else if (strMessage.StartsWith("load "))
                {
                    // Load saved game
                    string strPath = strMessage.Substring(5);
                    Active = false;
                    if (Game.Load(strPath))
                    {
                        SendOutputMessage("Loaded save game: " + strPath);
                    }
                    else
                    {
                        throw new WinBoardInputException("Unable to load save game: " + strPath);
                    }

                    Active = true;
                }
                else if (strMessage.StartsWith("variant "))
                {
                    // do nothing 
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage == "quit")
                {
                    // Kill sharpchess
                    WinBoardQuitEvent();
                }
                else if (strMessage == "random")
                {
                    // GNU Chess 4 specific - so ignore
                }
                else if (strMessage == "force")
                {
                    // * Chess Engine Communication Protocol
                    // Set the engine to play neither color ("force mode"). Stop clocks. The engine should check that moves 
                    // received in force mode are legal and made in the proper turn, but should not think, ponder, or make 
                    // moves of its own. 
                    Game.SuspendPondering();
                    Game.PlayerToPlay.Clock.Stop();
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Human;
                }
                else if (strMessage == "go")
                {
                    // * Chess Engine Communication Protocol
                    // Leave force mode and set the engine to play the color that is on move. 
                    // Associate the engine's clock with the color that is on move, the opponent's clock with the color that 
                    // is not on move. Start the engine's clock. Start thinking and eventually make a move. 
                    Game.SuspendPondering();
                    Game.PlayerToPlay.OpposingPlayer.Clock.Stop();
                    Game.PlayerToPlay.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.PlayerToPlay.OpposingPlayer.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerToPlay.Clock.Stop();
                    Game.PlayerToPlay.Clock.Start();
                    Game.PlayerToPlay.Brain.StartThinking();
                }
                else if (strMessage == "playother")
                {
                    /*
                       * Chess Engine Communication Protocol
                        (This command is new in protocol version 2. It is not sent unless you enable it with the feature command.) 
                        Leave force mode and set the engine to play the color that is not on move. Associate the opponent's 
                        clock with the color that is on move, the engine's clock with the color that is not on move. Start the 
                        opponent's clock. If pondering is enabled, the engine should begin pondering. If the engine later 
                        receives a move, it should start thinking and eventually reply. 
                    */
                    Game.SuspendPondering();
                    Game.PlayerToPlay = Game.PlayerToPlay.OpposingPlayer;
                    Game.PlayerToPlay.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.PlayerToPlay.OpposingPlayer.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerToPlay.OpposingPlayer.Clock.Stop();
                    Game.PlayerToPlay.Clock.Start();
                    Game.ResumePondering();
                }
                else if (strMessage == "white")
                {
                    // * Chess Engine Communication Protocol
                    // (This command is obsolete as of protocol version 2, but is still sent in some situations to accommodate 
                    // older engines unless you disable it with the feature command.) 
                    // Set White on move. Set the engine to play Black. Stop clocks. 
                    Game.SuspendPondering();
                    if (Game.PlayerToPlay.Brain.IsThinking)
                    {
                        Game.PlayerToPlay.Brain.ForceImmediateMove();
                    }

                    Game.PlayerToPlay.Clock.Stop();
                    Game.PlayerToPlay = Game.PlayerWhite;
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Human;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Computer;
                }
                else if (strMessage == "black")
                {
                    // * Chess Engine Communication Protocol
                    // (This command is obsolete as of protocol version 2, but is still sent in some situations to accommodate 
                    // older engines unless you disable it with the feature command.) 
                    // Set Black on move. Set the engine to play White. Stop clocks. 
                    Game.SuspendPondering();
                    if (Game.PlayerToPlay.Brain.IsThinking)
                    {
                        Game.PlayerToPlay.Brain.ForceImmediateMove();
                    }

                    Game.PlayerToPlay.Clock.Stop();
                    Game.PlayerToPlay = Game.PlayerBlack;
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Human;
                }
                else if (strMessage.StartsWith("level "))
                {
                    SetLevel(strMessage.Substring("level ".Length));
                    WinBoardTimeUpdatedEvent();
                }
                else if (strMessage.StartsWith("st "))
                {
                    // * Chess Engine Communication Protocol
                    // Set time Absolute fixed time-per-move. No time is carried forward from one move to the next. 
                    // The commands "level" and "st" are not used together. 
                    Game.ClockMaxMoves = 1;
                    Game.ClockTime = new TimeSpan(0, 0, 0);
                    Game.ClockIncrementPerMove = new TimeSpan(0, 0, 0);
                    Game.ClockFixedTimePerMove = new TimeSpan(0, 0, int.Parse(strMessage.Substring("st ".Length)));
                    Game.DifficultyLevel = 0;
                    Game.MaximumSearchDepth = 32;
                    Game.EnablePondering = true;
                    Game.UseRandomOpeningMoves = true;

                    WinBoardTimeUpdatedEvent();
                }
                else if (strMessage.StartsWith("depth "))
                {
                    // * Chess Engine Communication Protocol
                    // The engine should limit its thinking to DEPTH ply. 
                    Game.MaximumSearchDepth = int.Parse(strMessage.Substring("depth ".Length));
                }
                else if (strMessage.StartsWith("time "))
                {
                    Game.SuspendPondering();

                    /*
                    // * Chess Engine Communication Protocol
                        Set a clock that always belongs to the engine. N is a number in centiseconds (units of 1/100 second). 
                        Even if the engine changes to playing the opposite color, this clock remains with the engine.
                    */
                    if (Game.ClockFixedTimePerMove.Ticks > 0)
                    {
                        Game.PlayerToPlay.OpposingPlayer.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockFixedTimePerMove.Ticks * Game.MoveNo))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }
                    else if (Game.ClockIncrementPerMove.Ticks > 0)
                    {
                        Game.PlayerToPlay.OpposingPlayer.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockTime.Ticks + (Game.ClockIncrementPerMove.Ticks * Game.MoveNo)))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }
                    else
                    {
                        Game.PlayerToPlay.OpposingPlayer.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockTime.Ticks * Game.PlayerToPlay.OpposingPlayer.Clock.ControlPeriod))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }

                    WinBoardTimeUpdatedEvent();
                }
                else if (strMessage.StartsWith("otim "))
                {
                    Game.SuspendPondering();

                    /*
                       * Chess Engine Communication Protocol
                        Set a clock that always belongs to the opponent. N is a number in centiseconds (units of 1/100 second). Even if the opponent changes to playing the opposite color, this clock remains with the opponent. 
                        If needed for purposes of board display in force mode (where the engine is not participating in the 
                        game) the time clock should be associated with the last color that the engine was set to play, the 
                        otim clock with the opposite color. 

                        Beginning in protocol version 2, if you can't handle the time and otim commands, you can use the 
                        "feature" command to disable them; see below. The following techniques from older protocol versions 
                        also work: You can ignore the time and otim commands (that is, treat them as no-ops), 
                        or send back "Error (unknown command): time" the first time you see "time". 
                    */
                    if (Game.ClockFixedTimePerMove.Ticks > 0)
                    {
                        Game.PlayerToPlay.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockFixedTimePerMove.Ticks * Game.MoveNo))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }
                    else if (Game.ClockIncrementPerMove.Ticks > 0)
                    {
                        Game.PlayerToPlay.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockTime.Ticks + (Game.ClockIncrementPerMove.Ticks * Game.MoveNo)))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }
                    else
                    {
                        Game.PlayerToPlay.Clock.TimeElapsed =
                            (new TimeSpan(Game.ClockTime.Ticks * Game.PlayerToPlay.Clock.ControlPeriod))
                            - (new TimeSpan(long.Parse(strMessage.Substring("time ".Length)) * 100000));
                    }

                    WinBoardTimeUpdatedEvent();
                }
                else if (strMessage.StartsWith("usermove "))
                {
                    /*
                       * Chess Engine Communication Protocol
                        By default, moves are sent to the engine without a command name; the notation is just sent as a line 
                        by itself. Beginning in protocol version 2, you can use the feature command to cause the command name
                        "usermove" to be sent before the move. Example: "usermove e2e4". 
                    */
                    MakeMove(strMessage.Substring("usermove ".Length));
                }
                else if (strMessage == "?")
                {
                    /* 
                        Move now. If your engine is thinking, it should move immediately; otherwise, the command should be 
                        ignored (treated as a no-op). It is permissible for your engine to always ignore the ? command. 
                        The only bad consequence is that xboard's Move Now menu command will do nothing. 
                    */
                    if (Game.PlayerToPlay.Brain.IsThinking && !Game.PlayerToPlay.Brain.IsPondering)
                    {
                        Game.PlayerToPlay.Brain.ForceImmediateMove();
                    }
                }
                else if (strMessage.StartsWith("ping "))
                {
                    /*
                       * Chess Engine Communication Protocol
                        In this command, N is a decimal number. When you receive the command, reply by sending the string 
                        pong N, where N is the same number you received. Important: You must not reply to a "ping" command 
                        until you have finished executing all commands that you received before it. Pondering does not count; 
                        if you receive a ping while pondering, you should reply immediately and continue pondering. Because 
                        of the way xboard uses the ping command, if you implement the other commands in this protocol, you 
                        should never see a "ping" command when it is your move; however, if you do, you must not send the 
                        "pong" reply to xboard until after you send your move. For example, xboard may send "?" immediately 
                        followed by "ping". If you implement the "?" command, you will have moved by the time you see the 
                        subsequent ping command. Similarly, xboard may send a sequence like "force", "new", "ping". You must 
                        not send the pong response until after you have finished executing the "new" command and are ready 
                        for the new game to start. The ping command is new in protocol version 2 and will not be sent unless 
                        you enable it with the "feature" command. Its purpose is to allow several race conditions that could 
                        occur in previous versions of the protocol to be fixed, so it is highly recommended that you implement 
                        it. It is especially important in simple engines that do not ponder and do not poll for input while 
                        thinking, but it is needed in all engines. 
                    */
                    while (Game.PlayerToPlay.Brain.IsThinking && !Game.PlayerToPlay.Brain.IsPondering)
                    {
                        // Wait for thinking to finish
                        Thread.Sleep(250);
                    }

                    SendOutputMessage("pong " + strMessage.Substring(5));
                }
                else if (strMessage == "draw")
                {
                    /*
                        The engine's opponent offers the engine a draw. To accept the draw, send "offer draw". 
                        To decline, ignore the offer (that is, send nothing). If you're playing on ICS, it's possible for the 
                        draw offer to have been withdrawn by the time you accept it, so don't assume the game is over because 
                        you accept a draw offer. Continue playing until xboard tells you the game is over. See also 
                        "offer draw" below. 
                        Ignore all draw offers for now.
                    */
                }
                else if (strMessage.StartsWith("result "))
                {
                    /*
                       * Chess Engine Communication Protocol
                        After the end of each game, xboard will send you a result command. You can use this command to trigger learning. RESULT is either 1-0, 0-1, 1/2-1/2, or *, indicating whether white won, black won, the game was a draw, or the game was unfinished. The COMMENT string is purely a human-readable comment; its content is unspecified and subject to change. In ICS mode, it is passed through from ICS uninterpreted. Example: 
                        result 1-0 {White mates}
                        Here are some notes on interpreting the "result" command. Some apply only to playing on ICS ("Zippy" mode). 

                        If you won but did not just play a mate, your opponent must have resigned or forfeited. If you lost but were not just mated, you probably forfeited on time, or perhaps the operator resigned manually. If there was a draw for some nonobvious reason, perhaps your opponent called your flag when he had insufficient mating material (or vice versa), or perhaps the operator agreed to a draw manually. 

                        You will get a result command even if you already know the game ended -- for example, after you just checkmated your opponent. In fact, if you send the "RESULT {COMMENT}" command (discussed below), you will simply get the same thing fed back to you with "result" tacked in front. You might not always get a "result *" command, however. In particular, you won't get one in local chess engine mode when the user stops playing by selecting Reset, Edit Game, Exit or the like. 
                    */
                    Game.SuspendPondering();
                    Game.PlayerToPlay.Clock.Stop();
                }
                else if (strMessage.StartsWith("setboard "))
                {
                    /*
                       * Chess Engine Communication Protocol
                        The setboard command is the new way to set up positions, beginning in protocol version 2. 
                        It is not used unless it has been selected with the feature command. 
                        Here FEN is a position in Forsythe-Edwards Notation, as defined in the PGN standard. 
                        Illegal positions: N-o-t-e that either setboard or edit can be used to send an illegal position to the engine. 
                        The user can create any position with xboard's Edit Position command (even, say, an empty board, or a 
                        board with 64 white kings and no black ones). If your engine receives a position that it considers 
                        illegal, I suggest that you send the response "tellusererror Illegal position", and then respond to 
                        any attempted move with "Illegal move" until the next new, edit, or setboard command.
                    */
                    try
                    {
                        Game.New(strMessage.Substring(9).Trim());
                    }
                    catch (Fen.ValidationException x)
                    {
                        SendOutputMessage("tellusererror Illegal position: " + x.FenMessage);
                    }
                }
                else if (strMessage == "edit")
                {
                    /*
                       * Chess Engine Communication Protocol
                        The edit command is the old way to set up positions. For compatibility with old engines, it is still used by default, but new engines may prefer to use the feature command (see below) to cause xboard to use setboard instead. The edit command puts the chess engine into a special mode, where it accepts the following subcommands: c change current piece color, initially white  
                        Pa4 (for example) place pawn of current color on a4  
                        xa4 (for example) empty the square a4 (not used by xboard)  
                        # clear board  
                        . leave edit mode  
                        See the Idioms section below for additional subcommands used in ChessBase's implementation of the protocol. 
                        The edit command does not change the side to move. To set up a black-on-move position, xboard uses the following command sequence: 

                            new
                            force
                            a2a3
                            edit
                            <edit commands>
                            .

                        This sequence is used to avoid the "black" command, which is now considered obsolete and which many engines never did implement as specified in this document. 

                        After an edit command is complete, if a king and a rook are on their home squares, castling is assumed to be available to them. En passant capture is assumed to be illegal on the current move regardless of the positions of the pawns. The clock for the 50 move rule starts at zero, and for purposes of the draw by repetition rule, no prior positions are deemed to have occurred. 

                    */
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage == "hint")
                {
                    // * Chess Engine Communication Protocol
                    // If the user asks for a hint, xboard sends your engine the command "hint". Your engine should respond with "Hint: xxx", where xxx is a suggested move. If there is no move to suggest, you can ignore the hint command (that is, treat it as a no-op). 
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage == "bk")
                {
                    // * Chess Engine Communication Protocol
                    // If the user selects "Book" from the xboard menu, xboard will send your engine the command "bk". You can send any text you like as the response, as long as each line begins with a blank space or tab (\t) character, and you send an empty line at the end. The text pops up in a modal information dialog. 
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage == "undo")
                {
                    // * Chess Engine Communication Protocol
                    // If the user asks to back up one move, xboard will send you the "undo" command. xboard will not send this command without putting you in "force" mode first, so you don't have to worry about what should happen if the user asks to undo a move your engine made. (GNU Chess 4 actually switches to playing the opposite color in this case.) 
                    Game.UndoMove();
                }
                else if (strMessage == "remove")
                {
                    // * Chess Engine Communication Protocol
                    // If the user asks to retract a move, xboard will send you the "remove" command. It sends this command only when the user is on move. Your engine should undo the last two moves (one for each player) and continue playing the same color.
                    Game.UndoMove();
                    Game.UndoMove();
                }
                else if (strMessage == "hard")
                {
                    /*
                       * Chess Engine Communication Protocol
                        Turn on pondering (thinking on the opponent's time, also known as "permanent brain"). xboard will not 
                        make any assumption about what your default is for pondering or whether "new" affects this setting. 
                    */
                    Game.EnablePondering = true;
                }
                else if (strMessage == "easy")
                {
                    // * Chess Engine Communication Protocol
                    // Turn off pondering. 
                    Game.EnablePondering = false;
                }
                else if (strMessage == "post")
                {
                    // * Chess Engine Communication Protocol
                    // Turn on thinking/pondering output. See Thinking Output section. 
                    Game.ShowThinking = true;
                }
                else if (strMessage == "nopost")
                {
                    // Turn off thinking/pondering output. 
                    Game.ShowThinking = false;
                }
                else if (strMessage == "analyze")
                {
                    // * Chess Engine Communication Protocol
                    // Enter analyze mode. 
                    Game.SuspendPondering();

                    Game.PlayerToPlay.Clock.Stop();
                    Game.PlayerWhite.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.PlayerBlack.Intellegence = Player.PlayerIntellegenceNames.Computer;
                    Game.IsInAnalyseMode = true;

                    Game.PlayerToPlay.Brain.StartThinking();
                }
                else if (strMessage == ".")
                {
                    // Status update
                    if (Game.PlayerToPlay.Brain.IsThinking)
                    {
                        SendAnalyzeStatus(
                            Game.PlayerToPlay.Brain.ThinkingTimeElpased,
                            Game.PlayerToPlay.Brain.Search.PositionsSearchedThisIteration,
                            Game.PlayerToPlay.Brain.Search.SearchDepth,
                            Game.PlayerToPlay.Brain.Search.TotalPositionsToSearch - Game.PlayerToPlay.Brain.Search.SearchPositionNo,
                            Game.PlayerToPlay.Brain.Search.TotalPositionsToSearch, 
                            Game.PlayerToPlay.Brain.Search.CurrentMoveSearched);
                    }
                }
                else if (strMessage == "exit")
                {
                    // Exit analyze mode.
                    Game.SuspendPondering();

                    if (Game.IsInAnalyseMode)
                    {
                        Game.IsInAnalyseMode = false;
                    }
                }
                else if (strMessage.StartsWith("name "))
                {
                    // * Chess Engine Communication Protocol
                    // This command informs the engine of its opponent's name. When the engine is playing on a chess server, xboard obtains the 
                    // opponent's name from the server. When the engine is playing locally against a human user, xboard obtains the user's login 
                    // name from the local operating system. When the engine is playing locally against another engine, xboard uses either the other 
                    // engine's filename or the name that the other engine supplied in the myname option to the feature command. By default, xboard 
                    // uses the name command only when the engine is playing on a chess server. Beginning in protocol version 2, you can change this
                    // with the name option to the feature command.
                }
                else if (strMessage == "rating")
                {
                    /*
                       * Chess Engine Communication Protocol
                        In ICS mode, xboard obtains the ICS opponent's rating from the "Creating:" message that appears before each game. 
                        (This message may not appear on servers using outdated versions of the FICS code.) In Zippy mode, it sends these ratings on
                        to the chess engine using the "rating" command. The chess engine's own rating comes first, and if either opponent is not
                        rated, his rating is given as 0. In the future this command may also be used in other modes, if ratings are known. 
                        Example: rating 2600 1500
                    */
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage.StartsWith("ics "))
                {
                    // * Chess Engine Communication Protocol
                    // If HOSTNAME is "-", the engine is playing against a local opponent; otherwise, the engine is playing on an Internet Chess Server (ICS)
                    //  with the given hostname. This command is new in protocol version 2 and is not sent unless the engine has enabled it with the "feature"
                    //  command. Example: "ics freechess.org" 
                    throw new WinBoardInputException("Unknown command: " + strMessage);
                }
                else if (strMessage == "computer")
                {
                    // * Chess Engine Communication Protocol
                    // The opponent is also a computer chess engine. Some engines alter their playing style when they receive this command.
                    Game.UseRandomOpeningMoves = false;
                }
                else if (strMessage == "pause")
                {
                    // * Chess Engine Communication Protocol
                    // See resume
                    Game.PausePlay();
                }
                else if (strMessage == "resume")
                {
                    // * Chess Engine Communication Protocol
                    // (These commands are new in protocol version 2 and will not be sent unless feature pause=1 is set. At this writing, xboard actually does not use the commands at all, but it or other interfaces may use them in the future.) The "pause" command puts the engine into a special state where it does not think, ponder, or otherwise consume significant CPU time. The current thinking or pondering (if any) is suspended and both player's clocks are stopped. The only command that the interface may send to the engine while it is in the paused state is "resume". The paused thinking or pondering (if any) resumes from exactly where it left off, and the clock of the player on move resumes running from where it stopped. 
                    Game.ResumePlay();
                }
                else if (strMessage.Length == 4 || strMessage.Length == 5)
                {
                    MakeMove(strMessage);
                }
                else
                {
                    throw new WinBoardInputException("Error (unknown command): " + strMessage);
                }
            }
            catch (WinBoardInputException e)
            {
                SendOutputMessage(e.WinBoardMessage);
            }
        }

        /// <summary>
        /// The send check mate.
        /// </summary>
        public static void SendCheckMate()
        {
            if (Active)
            {
                SendOutputMessage(Game.PlayerBlack.IsInCheckMate ? "1-0 {White mates}" : "0-1 {Black mates}");
            }
        }

        /// <summary>
        /// The send check stale mate.
        /// </summary>
        public static void SendCheckStaleMate()
        {
            if (Active)
            {
                SendOutputMessage("1/2-1/2 {Stalemate}");
            }
        }

        /// <summary>
        /// The send draw by fifty move rule.
        /// </summary>
        public static void SendDrawByFiftyMoveRule()
        {
            if (Active)
            {
                SendOutputMessage("1/2-1/2 {Draw by 50 move rule}");
            }
        }

        /// <summary>
        /// The send draw by insufficient material.
        /// </summary>
        public static void SendDrawByInsufficientMaterial()
        {
            if (Active)
            {
                SendOutputMessage("1/2-1/2 {Draw by insufficient material}");
            }
        }

        /// <summary>
        /// The send draw by repetition.
        /// </summary>
        public static void SendDrawByRepetition()
        {
            if (Active)
            {
                SendOutputMessage("1/2-1/2 {Draw by repetition}");
            }
        }

        /// <summary>
        /// Send move.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        public static void SendMove(Move move)
        {
            if (Active)
            {
                SendOutputMessage("move " + move.From.Name + move.To.Name);
            }
        }

        /// <summary>
        /// Send move time.
        /// </summary>
        /// <param name="timeSpan">
        /// The time span.
        /// </param>
        public static void SendMoveTime(TimeSpan timeSpan)
        {
            if (Active)
            {
                SendOutputMessage("movetime " + timeSpan.ToString());
            }
        }

        /// <summary>
        /// Send thinking.
        /// </summary>
        /// <param name="ply">
        /// The ply.
        /// </param>
        /// <param name="score">
        /// The score.
        /// </param>
        /// <param name="thinkingTime">
        /// Thinking time.
        /// </param>
        /// <param name="nodes">
        /// The number of nodes.
        /// </param>
        /// <param name="prinicalVariation">
        /// The prinical variation.
        /// </param>
        public static void SendThinking(int ply, int score, TimeSpan thinkingTime, int nodes, string prinicalVariation)
        {
            /*
                ply score time nodes pv
                
                Where: ply Integer giving current search depth.  
                score Integer giving current evaluation in centipawns.  
                time Current search time in centiseconds (ex: 1028 = 10.28 seconds).  
                nodes Nodes searched.  
                pv Freeform text giving current "best" line. You can continue the pv onto another line if you start each continuation line with at least four space characters.  

                Example: 

                9 156 1084 48000 Nf3 Nc6 Nc3 Nf6
            */
            if (Active && Game.ShowThinking)
            {
                int intScoreInCentipawns = score / 10;
                int intTimeIncentiseconds = Convert.ToInt32(thinkingTime.TotalMilliseconds / 100);
                SendOutputMessage(
                    ply.ToString() + " " + intScoreInCentipawns.ToString() + " " + intTimeIncentiseconds.ToString()
                    + " " + nodes.ToString() + " " + prinicalVariation);
            }
        }

        /// <summary>
        /// Start WinBoard listener.on new thread.
        /// </summary>
        public static void StartListener()
        {
            if (Active)
            {
                ThreadListener = new Thread(Listen) { Priority = ThreadPriority.Normal };
                ThreadListener.Start();
            }
        }

        /// <summary>
        /// Stop WinBoard listener.
        /// </summary>
        public static void StopListener()
        {
            if (Active)
            {
                ThreadListener.Abort();
                ThreadListener.Join();
                ThreadListener = null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Run an infinte loop that listens for WinBoard messages.
        /// </summary>
        private static void Listen()
        {
            while (true)
            {
                string strMessage = Console.ReadLine();
                if (strMessage == null)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    WinBoardInputEvent(strMessage);
                }
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

        /// <summary>
        /// Process a WinBoard make move message.
        /// </summary>
        /// <param name="strMove">
        /// The move.
        /// </param>
        /// <exception cref="WinBoardInputException">
        /// Winboard exception
        /// </exception>
        private static void MakeMove(string strMove)
        {
            /*
                    See below for the syntax of moves. If the move is illegal, print an error message; see the section "Commands from the engine to xboard". If the move is legal and in turn, make it. If not in force mode, stop the opponent's clock, start the engine's clock, start thinking, and eventually make a move. 
                    When xboard sends your engine a move, it normally sends coordinate algebraic notation. Examples: 

                    Normal moves: e2e4  
                    Pawn promotion: e7e8q  
                    Castling: e1g1, e1c1, e8g8, e8c8  
                    Bughouse/crazyhouse drop: P@h3  
                    ICS Wild 0/1 castling: d1f1, d1b1, d8f8, d8b8  
                    FischerRandom castling: O-O, O-O-O (oh, not zero)  

                    Beginning in protocol version 2, you can use the feature command to select SAN (standard algebraic notation) instead; for example, e4, Nf3, exd5, Bxf7+, Qxf7#, e8=Q, O-O, or P@h3. Note that the last form, P@h3, is a extension to the PGN standard's definition of SAN, which does not support bughouse or crazyhouse. 

                    xboard doesn't reliably detect illegal moves, because it does not keep track of castling unavailability due to king or rook moves, or en passant availability. If xboard sends an illegal move, send back an error message so that xboard can retract it and inform the user; see the section "Commands from the engine to xboard". 
                */
            Game.SuspendPondering();

            Move.MoveNames movename = Move.MoveNames.NullMove;

            if (strMove.Length == 5)
            {
                switch (strMove.Substring(4, 1))
                {
                    case "q":
                        movename = Move.MoveNames.PawnPromotionQueen;
                        break;

                    case "r":
                        movename = Move.MoveNames.PawnPromotionRook;
                        break;

                    case "b":
                        movename = Move.MoveNames.PawnPromotionBishop;
                        break;

                    case "n":
                        movename = Move.MoveNames.PawnPromotionKnight;
                        break;
                }
            }

            Moves moves = new Moves();
            Game.PlayerToPlay.GenerateLegalMoves(moves);

            foreach (Move move in moves)
            {
                if (move.From.Name == strMove.Substring(0, 2) && move.To.Name == strMove.Substring(2, 2)
                    && (movename == Move.MoveNames.NullMove || move.Name == movename))
                {
                    Game.MakeAMove(move.Name, move.Piece, move.To);
                    return;
                }
            }

            throw new WinBoardInputException("Illegal move: " + strMove);
        }

        /// <summary>
        /// Send analyze status.
        /// </summary>
        /// <param name="thinkingTime">
        /// The thinking time.
        /// </param>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        /// <param name="ply">
        /// The ply.
        /// </param>
        /// <param name="movesRemaining">
        /// The moves remaining.
        /// </param>
        /// <param name="totalMoves">
        /// The total moves.
        /// </param>
        /// <param name="moveCurrent">
        /// The move current.
        /// </param>
        private static void SendAnalyzeStatus(
            TimeSpan thinkingTime, int nodes, int ply, int movesRemaining, int totalMoves, Move moveCurrent)
        {
            /*
                stat01: time nodes ply mvleft mvtot mvname

                Where: time Elapsed search time in centiseconds (ie: 567 = 5.67 seconds).  
                nodes Nodes searched so far.  
                ply Search depth so far.  
                mvleft Number of moves left to consider at this depth.  
                mvtot Total number of moves to consider.  
                mvname Move currently being considered (SAN or coordinate notation). Optional; added in protocol version 2.  

                Examples: 

                stat01: 1234 30000 7 5 30
                stat01: 1234 30000 7 5 30 Nf3

                Meaning: 

                After 12.34 seconds, I've searched 7 ply/30000 nodes, there are a total of 30 legal moves, and I have 5 more moves to search before going to depth 8. In the second example, of the 30 legal moves, the one I am currently searching is Nf3.
            */
            if (Active && Game.IsInAnalyseMode)
            {
                int intTimeIncentiseconds = Convert.ToInt32(thinkingTime.TotalMilliseconds / 100);
                SendOutputMessage(
                    "stat01:" + " " + intTimeIncentiseconds.ToString() + " " + nodes.ToString() + " " + ply.ToString()
                    + " " + movesRemaining.ToString() + " " + totalMoves.ToString() + " " + moveCurrent.From.Name
                    + moveCurrent.To.Name);
            }
        }

        /// <summary>
        /// Send output message to WinBoard.
        /// </summary>
        /// <param name="strMessage">
        /// The str message.
        /// </param>
        private static void SendOutputMessage(string strMessage)
        {
            WinBoardOutputEvent(strMessage);
            Console.WriteLine(strMessage);
        }

        /// <summary>
        /// Process WinBoard Set Level message.
        /// </summary>
        /// <param name="strLevel">
        /// Winboard level parameters.
        /// </param>
        private static void SetLevel(string strLevel)
        {
            /*
                // MPS BASE INC
                In conventional clock mode, every time control period is the same. That is, if the time control is 
                40 moves in 5 minutes, then after each side has made 40 moves, they each get an additional 5 minutes, 
                and so on, ad infinitum. At some future time it would be nice to support a series of distinct time 
                controls. This is very low on my personal priority list, but code donations to the xboard project 
                are accepted, so feel free to take a swing at it. I suggest you talk to me first, though. 

                The command to set a conventional time control looks like this: 

                level 40 5 0
                level 40 0:30 0

                The 40 means that there are 40 moves per time control. The 5 means there are 5 minutes in the control. In the second example, the 0:30 means there are 30 seconds. The final 0 means that we are in conventional clock mode. 

            */
            int intPos;
            string strMoves;
            string strTime;
            string strIncrement;
            int intMoves;
            int intMinutes;
            int intSeconds;
            int intIncrement;

            intPos = strLevel.IndexOf(" ");
            strMoves = strLevel.Substring(0, intPos);
            strLevel = strLevel.Substring(intPos + 1);

            intPos = strLevel.IndexOf(" ");
            strTime = strLevel.Substring(0, intPos);
            strLevel = strLevel.Substring(intPos + 1);

            strIncrement = strLevel;
            intIncrement = int.Parse(strIncrement);

            intMoves = int.Parse(strMoves);

            intPos = strTime.IndexOf(':');
            if (intPos >= 0)
            {
                intMinutes = int.Parse(strTime.Substring(0, intPos));
                intSeconds = int.Parse(strTime.Substring(intPos + 1));
            }
            else
            {
                intMinutes = int.Parse(strTime);
                intSeconds = 0;
            }

            Game.ClockMaxMoves = intMoves;
            Game.ClockTime = new TimeSpan(0, intMinutes, intSeconds);
            Game.ClockIncrementPerMove = new TimeSpan(0, 0, intIncrement);
            Game.ClockFixedTimePerMove = new TimeSpan(0, 0, 0);
            Game.DifficultyLevel = 0;
            Game.MaximumSearchDepth = 32;
            Game.EnablePondering = true;
            Game.UseRandomOpeningMoves = true;
        }

        #endregion

        /// <summary>
        /// Winboard input exception.
        /// </summary>
        private class WinBoardInputException : ApplicationException
        {
            #region Constants and Fields

            /// <summary>
            ///   The m_str message.
            /// </summary>
            private readonly string message = string.Empty;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="WinBoardInputException"/> class.
            /// </summary>
            /// <param name="strMessage">
            /// The str message.
            /// </param>
            public WinBoardInputException(string strMessage)
            {
                this.message = strMessage;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///   Gets WinBoardMessage.
            /// </summary>
            public string WinBoardMessage
            {
                get
                {
                    return this.message;
                }
            }

            #endregion
        }
    }
}