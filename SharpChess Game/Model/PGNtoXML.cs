// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PGNtoXML.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Converts PGN opening book files, into SharpChess XML files, which may then be used with <see cref = "OpeningBook" /> and <see cref = "OpeningBookSimple" /> classes.
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
    using SharpChess.Model.AI;

    /// <summary>
    /// Converts PGN opening book files, into SharpChess XML files, which may then be used with <see cref = "OpeningBook" /> and <see cref = "OpeningBookSimple" /> classes.
    /// </summary>
    public class PGNtoXML
    {
        #region Enums

        /// <summary>
        /// The enm state.
        /// </summary>
        private enum StateNames
        {
            /// <summary>
            ///   The move no.
            /// </summary>
            MoveNo, 

            /// <summary>
            ///   The move notation.
            /// </summary>
            MoveNotation
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The go.
        /// </summary>
        public static void Go()
        {
            /*
            const string strSourceFileName = @"d:\eco.pgn";
            const string strRootOutputFileName = @"c:\OpeningBook_";
            const int NO_OF_PLYS = 8;

            New();

            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlnodeRoot = xmldoc.CreateElement("OpeningBook");
            XmlElement xmlnodeParent = null;
            XmlElement xmlnodeMove = null;
            FileStream filestream = new FileStream(strSourceFileName, System.IO.FileMode.Open);
            int intByte = 0;
            char charNext;
            StateNames state = StateNames.MoveNo;
            Move move = null;
            int intWhiteScore = 0;
            int intBlackScore = 0;
            int intGameNo = 0;
            int intNoOfErrors = 0;
            

            xmldoc.AppendChild(xmlnodeRoot);

            while (intByte>-1)
            {
                try
                {
                    New();

                    intGameNo++;

                    // Read game info
                    string strInfo = "";
                    while ((intByte=filestream.ReadByte())!=-1)
                    {
                        charNext = Convert.ToChar((intByte & 0xFF));
                        if (intByte==13 || intByte==10)
                        {
                            if (intByte==13) filestream.ReadByte(); // skip char 10
                            if (strInfo.Length==0) break;
                            if (strInfo.StartsWith("[Result \"") && strInfo.Length==14)
                            {
                                intWhiteScore=Convert.ToInt32(strInfo.Substring(9,1),10);
                                intBlackScore=Convert.ToInt32(strInfo.Substring(11,1),10);
                            }
                            strInfo="";
                        }
                        else
                        {
                            strInfo += charNext;
                        }
                    }

                    xmlnodeParent = xmlnodeRoot;
                    state=StateNames.MoveNo;
                    string strNotation = "";
                    int intPly = 1;
                    while ((intByte=filestream.ReadByte())!=-1)
                    {
                        charNext = Convert.ToChar((intByte & 0xFF));
                        if (charNext=='[')
                        {
                            filestream.Seek(-1, SeekOrigin.Current); break;
                        }
                        if (intPly<=NO_OF_PLYS ) // && charNext!=10
                        {
                            switch (state)
                            {
                                case StateNames.MoveNo:
                                    if (char.IsLetter(charNext))
                                    {
                                        state=StateNames.MoveNotation; filestream.Seek(-1, SeekOrigin.Current);
                                    }
                                    break;

                                case StateNames.MoveNotation:
                                    if (char.IsWhiteSpace(charNext)) 
                                    {
                                        move = PlayerToPlay.MoveFromNotation( strNotation );
                                        MakeAMove(move.Name, move.Piece, move.To);

                                        xmlnodeMove = null;
                                        foreach (XmlElement xmlnodeSearch in xmlnodeParent.ChildNodes)
                                        {
                                            if (xmlnodeSearch.GetAttribute("f")==move.From.Name && xmlnodeSearch.GetAttribute("t")==move.To.Name )
                                            {
                                                xmlnodeMove = xmlnodeSearch;
                                                break;
                                            }
                                        }
                                        if (xmlnodeMove==null)
                                        {
                                            xmlnodeMove = xmldoc.CreateElement("m");
                                            xmlnodeMove.SetAttribute("w", "0" );
                                            xmlnodeMove.SetAttribute("b", "0" ); 
                                        }

                                        xmlnodeMove.SetAttribute("f", move.From.Name);
                                        xmlnodeMove.SetAttribute("t", move.To.Name);
                                        if (move.Name!=Move.enmName.Standard)
                                        { 
                                            xmlnodeMove.SetAttribute("n", move.Name.ToString());
                                        }
                                        xmlnodeMove.SetAttribute("w", (Convert.ToInt32(xmlnodeMove.GetAttribute("w"))+ intWhiteScore).ToString() );
                                        xmlnodeMove.SetAttribute("b", (Convert.ToInt32(xmlnodeMove.GetAttribute("b"))+ intBlackScore).ToString() ); 

                                        xmlnodeParent.AppendChild(xmlnodeMove);
                                        xmlnodeParent = xmlnodeMove;

                                        state=StateNames.MoveNo; 
                                        strNotation=""; 
                                        intPly++;
                                    }
                                    if (char.IsLetterOrDigit(charNext) )
                                    {
                                        strNotation += charNext.ToString();
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                    intNoOfErrors++;
                    xmlnodeRoot.SetAttribute("NoOfErrors", intNoOfErrors.ToString());
                }


                xmlnodeRoot.SetAttribute("NoOfGames", intGameNo.ToString());

                if (intGameNo%10000==0)
                {
                    xmldoc.Save(strRootOutputFileName + NO_OF_PLYS.ToString() + "plys_" + intGameNo.ToString() + ".xml");
                }
            }

            filestream.Close();
            xmldoc.Save(strRootOutputFileName + NO_OF_PLYS.ToString() + "plys_" + intGameNo.ToString() + ".xml");
*/
        }

        #endregion

        /*
                private struct BookEntry
                {
                    public ulong HashCodeA;
                    public ulong HashCodeB;
                    public byte From;
                    public byte To;
                    public Move.enmName MoveName;
                }

                public static void XMLtoOB()
                {
                    const string strXMLFileName = @"d:\6Test.xml";
                    //const string strXMLFileName = @"d:\OpeningBook.xml";
                    //const string strXMLFileName = @"d:\OpeningBook_16plys_146027.xml";
            
                    const string strOBFileName = @"d:\6Test.sharpbook";

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(strXMLFileName);
                    XmlNodeList xmlnodelist = xmldoc.SelectNodes("//*");
            
                    int intNodeCount = xmlnodelist.Count;
                }
        */
    }
}