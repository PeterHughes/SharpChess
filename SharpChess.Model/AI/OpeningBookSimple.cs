// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpeningBookSimple.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   A small, in-memory, book of opening moves used simply to vary SharpChess's games against human. Not competition standard by any means!
//   XML opening book files are created from PGN files using the <see cref = "PGNtoXML" /> class.
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

namespace SharpChess.Model.AI
{
    #region Using

    using System;
    using System.Collections;
    using System.Xml;

    #endregion

    /// <summary>
    /// A small, in-memory, book of opening moves, used simply to vary SharpChess's games against human players. Not competition standard by any means!
    ///   XML opening book files are created from PGN files using the <see cref = "PGNtoXML" /> class.
    /// </summary>
    public static class OpeningBookSimple
    {
        #region Constants and Fields

        /// <summary>
        /// The m_ hashtable black.
        /// </summary>
        private static readonly Hashtable HashtableBlack = new Hashtable();

        /// <summary>
        /// The m_ hashtable white.
        /// </summary>
        private static readonly Hashtable HashtableWhite = new Hashtable();

        #endregion

        #region Public Methods

        /// <summary>
        /// Import opening book XML for file.
        /// </summary>
        /// <returns>
        /// Imported XML.
        /// </returns>
        public static string Import()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(@"d:\OpeningBook_8plys_2014.xml");
            Prune((XmlElement)xmldoc.FirstChild, 1);
            return xmldoc.OuterXml;
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            string strBook = string.Empty;

            strBook += "<root>";
            strBook += "<m f='b2' t='b4'><m f='g8' t='h6' /><m f='c7' t='c6' /></m><m f='g2' t='g3'><m f='h7' t='h5' /><m f='e7' t='e5'><m f='g1' t='f3' /></m></m><m f='g2' t='g4'><m f='d7' t='d5'><m f='f1' t='g2'><m f='c7' t='c6'><m f='g4' t='g5' /></m><m f='c8' t='g4'><m f='c2' t='c4'><m f='d5' t='d4' /></m></m></m></m></m><m f='h2' t='h3'><m f='e7' t='e5'><m f='a2' t='a3' /></m></m><m f='g1' t='h3'><m f='d7' t='d5'><m f='g2' t='g3'><m f='e7' t='e5'><m f='f2' t='f4'><m f='c8' t='h3'><m f='f1' t='h3'><m f='e5' t='f4' /></m></m></m></m></m></m></m><m f='b1' t='c3'><m f='e7' t='e5'><m f='a2' t='a3' /></m><m f='c7' t='c5'><m f='d2' t='d4'><m f='c5' t='d4'><m f='d1' t='d4'><m f='b8' t='c6'><m f='d4' t='h4' /></m></m></m></m></m></m><m f='a2' t='a4'><m f='e7' t='e5'><m f='h2' t='h4' /></m></m><m f='d2' t='d3'><m f='e7' t='e5'><m f='b1' t='d2' /></m><m f='c7' t='c5'><m f='b1' t='c3'>";
            strBook += "<m f='b8' t='c6'><m f='g2' t='g3' /></m></m></m></m><m f='e2' t='e3'><m f='e7' t='e5'><m f='c2' t='c4'><m f='d7' t='d6'><m f='b1' t='c3'><m f='b8' t='c6'><m f='b2' t='b3'><m f='g8' t='f6' /></m></m></m></m></m></m></m><m f='f2' t='f3'><m f='e7' t='e5'><m f='e1' t='f2' /></m></m><m f='b2' t='b3'><m f='e7' t='e5' /><m f='g8' t='f6' /><m f='d7' t='d5' /><m f='c7' t='c5' /><m f='f7' t='f5' /></m><m f='f2' t='f4'><m f='e7' t='e5'><m f='f4' t='e5'><m f='d7' t='d6'><m f='e5' t='d6'><m f='f8' t='d6'><m f='g1' t='f3'><m f='g7' t='g5' /><m f='g8' t='h6' /></m></m></m></m></m></m><m f='f7' t='f5'><m f='e2' t='e4'><m f='f5' t='e4'><m f='b1' t='c3'><m f='g8' t='f6'><m f='g2' t='g4' /></m></m></m></m></m><m f='g7' t='g5' /><m f='d7' t='d5'><m f='c2' t='c4' /><m f='e2' t='e4' /><m f='g1' t='f3'><m f='g8' t='f6'><m f='e2' t='e3'><m f='c7' t='c5' /></m></m></m></m></m><m f='g1' t='f3'>";
            strBook += "<m f='f7' t='f5'><m f='e2' t='e4' /><m f='d2' t='d3'><m f='g8' t='f6'><m f='e2' t='e4' /></m></m></m><m f='d7' t='d6'><m f='e2' t='e4'><m f='c8' t='g4' /></m></m><m f='g7' t='g5' /><m f='g8' t='f6'><m f='g2' t='g3'><m f='b7' t='b5' /><m f='g7' t='g6'><m f='b2' t='b4' /></m></m></m><m f='d7' t='d5'><m f='d2' t='d3' /><m f='b2' t='b4' /><m f='e2' t='e4' /><m f='g2' t='g3'><m f='g8' t='f6'><m f='f1' t='g2'><m f='c7' t='c6'><m f='e1' t='g1' n='CastleKingSide'><m f='c8' t='g4' /></m></m></m></m><m f='c8' t='g4'><m f='f1' t='g2'><m f='b8' t='d7' /></m></m><m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7'><m f='e1' t='g1' n='CastleKingSide'><m f='e7' t='e5' /></m></m></m></m><m f='c7' t='c5'><m f='f1' t='g2'><m f='b8' t='c6'><m f='e1' t='g1' n='CastleKingSide'><m f='e7' t='e6' /></m></m></m></m></m><m f='c2' t='c4'><m f='d5' t='d4' /><m f='d5' t='c4'><m f='e2' t='e3'><m f='c8' t='e6' /></m></m></m></m></m>";
            strBook += "<m f='c2' t='c4'><m f='g7' t='g6'><m f='e2' t='e4'><m f='e7' t='e5' /></m></m><m f='c7' t='c6'><m f='g1' t='f3'><m f='d7' t='d5'><m f='b2' t='b3'><m f='g8' t='f6'><m f='g2' t='g3'><m f='c8' t='g4' /><m f='c8' t='f5' /></m><m f='c1' t='b2'><m f='g7' t='g6' /><m f='c8' t='f5' /><m f='c8' t='g4' /></m></m><m f='c8' t='g4' /></m></m></m></m><m f='e7' t='e6'><m f='g1' t='f3'><m f='g8' t='f6'><m f='g2' t='g3'><m f='a7' t='a6'><m f='f1' t='g2'><m f='b7' t='b5' /></m></m></m></m><m f='d7' t='d5'><m f='b2' t='b3'><m f='g8' t='f6'><m f='c1' t='b2'><m f='c7' t='c5' /></m></m></m><m f='g2' t='g3'><m f='c7' t='c6' /><m f='g8' t='f6'><m f='f1' t='g2'><m f='d5' t='c4' /><m f='f8' t='e7' /></m></m></m></m></m></m><m f='g8' t='f6'><m f='b2' t='b4' /><m f='g1' t='f3' /><m f='b1' t='c3'><m f='d7' t='d5'><m f='c4' t='d5'><m f='f6' t='d5'><m f='g2' t='g3'><m f='g7' t='g6' /></m>";
            strBook += "<m f='g1' t='f3'><m f='g7' t='g6' /></m></m></m></m><m f='e7' t='e6'><m f='g1' t='f3'><m f='b7' t='b6'><m f='e2' t='e4'><m f='c8' t='b7' /></m></m><m f='f8' t='b4' /></m><m f='e2' t='e4'><m f='d7' t='d5'><m f='e4' t='e5' /></m><m f='b8' t='c6' /><m f='c7' t='c5' /></m></m></m></m><m f='e7' t='e5'><m f='g1' t='f3'><m f='e5' t='e4' /></m><m f='b1' t='c3'><m f='d7' t='d6'><m f='g2' t='g3'><m f='c8' t='e6'><m f='f1' t='g2'><m f='b8' t='c6' /></m></m><m f='c7' t='c6' /></m><m f='g1' t='f3'><m f='c8' t='g4' /></m></m><m f='f8' t='b4' /><m f='g8' t='f6'><m f='g1' t='f3'><m f='e5' t='e4'><m f='f3' t='g5'><m f='b7' t='b5' /></m></m></m><m f='g2' t='g3'><m f='d7' t='d5' /><m f='f8' t='b4' /><m f='c7' t='c6' /><m f='g7' t='g6' /></m></m><m f='b8' t='c6'><m f='g2' t='g3'><m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7' /></m></m></m><m f='g1' t='f3'><m f='g8' t='f6'>";
            strBook += "<m f='d2' t='d4'><m f='e5' t='d4' /><m f='e5' t='e4' /></m><m f='e2' t='e4' /><m f='a2' t='a3' /><m f='d2' t='d3' /><m f='e2' t='e3'><m f='f8' t='b4' /></m><m f='g2' t='g3' /></m></m></m></m></m><m f='c7' t='c5'><m f='g1' t='f3'><m f='g8' t='f6'><m f='g2' t='g3'><m f='b7' t='b6'><m f='f1' t='g2'><m f='c8' t='b7' /></m></m></m><m f='d2' t='d4'><m f='c5' t='d4'><m f='f3' t='d4'><m f='e7' t='e6' /></m></m></m></m></m><m f='b1' t='c3'><m f='g8' t='f6'><m f='g1' t='f3'><m f='d7' t='d5'><m f='c4' t='d5'><m f='f6' t='d5' /></m></m></m><m f='g2' t='g3'><m f='d7' t='d5'><m f='c4' t='d5'><m f='f6' t='d5' /></m></m></m></m><m f='b8' t='c6'><m f='g1' t='f3'><m f='g8' t='f6' /></m><m f='g2' t='g3'><m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7' /></m></m></m></m></m></m></m><m f='e2' t='e4'><m f='g8' t='h6'><m f='d2' t='d4'><m f='g7' t='g6'><m f='c2' t='c4'><m f='f7' t='f6' /></m></m></m></m>";
            strBook += "<m f='f7' t='f6'><m f='d2' t='d4'><m f='e8' t='f7' /></m></m><m f='b7' t='b6'><m f='d2' t='d4'><m f='c8' t='a6' /></m></m><m f='b8' t='c6'><m f='b2' t='b4'><m f='c6' t='b4'><m f='c2' t='c3'><m f='b4' t='c6'><m f='d2' t='d4' /></m></m></m></m><m f='g1' t='f3'><m f='f7' t='f5' /></m><m f='d2' t='d4'><m f='d7' t='d5'><m f='e4' t='d5'><m f='d8' t='d5'><m f='b1' t='c3' /></m></m><m f='b1' t='c3' /></m><m f='f7' t='f6' /></m></m><m f='d7' t='d5'><m f='e4' t='d5'><m f='d8' t='d5'><m f='b1' t='c3'><m f='d5' t='a5'><m f='d2' t='d4'><m f='g8' t='f6' /><m f='e7' t='e5' /></m><m f='b2' t='b4' /></m><m f='d5' t='d6' /></m></m><m f='g8' t='f6'><m f='c2' t='c4'><m f='e7' t='e6' /><m f='c7' t='c6' /></m><m f='d2' t='d4'><m f='f6' t='d5'><m f='c2' t='c4'><m f='d5' t='b4' /></m></m><m f='g7' t='g6' /></m></m></m></m><m f='g8' t='f6'><m f='b1' t='c3'><m f='d7' t='d5'><m f='e4' t='e5'>";
            strBook += "<m f='f6' t='d7'><m f='e5' t='e6' /></m></m></m></m><m f='d2' t='d3' /><m f='f1' t='c4' /><m f='e4' t='e5'><m f='f6' t='e4' /><m f='f6' t='g8' /><m f='f6' t='d5'><m f='f1' t='c4'><m f='d5' t='b6'><m f='c4' t='b3'><m f='c7' t='c5' /></m></m></m><m f='b1' t='c3' /><m f='b2' t='b3' /><m f='c2' t='c4'><m f='d5' t='b6'><m f='b2' t='b3' /><m f='c4' t='c5'><m f='b6' t='d5' /></m></m></m><m f='d2' t='d4'><m f='b7' t='b5' /><m f='d7' t='d6'><m f='f1' t='c4' /><m f='c2' t='c4'><m f='d5' t='b6' /></m><m f='g1' t='f3'><m f='d6' t='e5' /><m f='d5' t='b6' /><m f='g7' t='g6' /><m f='c8' t='g4' /></m></m></m></m></m></m><m f='g7' t='g6'><m f='d2' t='d4'><m f='g8' t='f6'><m f='e4' t='e5'><m f='f6' t='h5'><m f='g2' t='g4'><m f='h5' t='g7' /></m></m></m></m><m f='f8' t='g7'><m f='f2' t='f4' /><m f='b1' t='c3'><m f='c7' t='c6'><m f='f2' t='f4'><m f='d7' t='d5' /></m></m><m f='d7' t='d6'>";
            strBook += "<m f='g1' t='f3'><m f='c7' t='c6' /></m><m f='f2' t='f4' /></m></m><m f='g1' t='f3'><m f='d7' t='d6'><m f='c2' t='c4'><m f='c8' t='g4' /></m><m f='c2' t='c3' /></m></m></m></m></m><m f='d7' t='d6'><m f='d2' t='d4'><m f='g8' t='f6'><m f='b1' t='c3'><m f='c7' t='c6' /><m f='g7' t='g6'><m f='c1' t='e3'><m f='c7' t='c6' /></m><m f='g2' t='g3' /><m f='f1' t='c4' /><m f='c1' t='g5' /><m f='f1' t='e2'><m f='f8' t='g7' /></m><m f='g1' t='f3'><m f='f8' t='g7' /></m><m f='f2' t='f4'><m f='f8' t='g7' /></m></m></m></m></m></m><m f='c7' t='c6'><m f='f1' t='c4' /><m f='c2' t='c4'><m f='d7' t='d5' /></m><m f='d2' t='d3' /><m f='b1' t='c3'><m f='d7' t='d5'><m f='d1' t='f3' /><m f='g1' t='f3'><m f='c8' t='g4' /></m></m></m><m f='d2' t='d4'><m f='b8' t='a6'><m f='b1' t='c3'><m f='a6' t='c7' /></m></m><m f='g8' t='f6' /><m f='d7' t='d5'><m f='f2' t='f3' /><m f='b1' t='d2'><m f='d8' t='b6' /></m>";
            strBook += "<m f='e4' t='e5'><m f='c8' t='f5'><m f='c2' t='c3'><m f='e7' t='e6' /></m></m></m><m f='e4' t='d5'><m f='c6' t='d5'><m f='f1' t='d3'><m f='b8' t='c6' /></m><m f='c2' t='c4'><m f='g8' t='f6' /></m></m></m><m f='b1' t='c3'><m f='b7' t='b5' /><m f='g7' t='g6' /><m f='d5' t='e4'><m f='f2' t='f3' /><m f='c3' t='e4'><m f='g8' t='f6' /><m f='b8' t='d7' /><m f='c8' t='f5' /></m></m></m></m></m></m><m f='c7' t='c5'><m f='c2' t='c4'><m f='d7' t='d6'><m f='b1' t='c3'><m f='b8' t='c6'><m f='g2' t='g3'><m f='h7' t='h5' /></m></m></m></m></m><m f='b2' t='b4'><m f='c5' t='b4'><m f='c2' t='c4' /><m f='a2' t='a3'><m f='d7' t='d5'><m f='e4' t='d5'><m f='d8' t='d5' /></m></m><m f='b4' t='a3' /></m></m></m><m f='d2' t='d4'><m f='c5' t='d4'><m f='g1' t='f3'><m f='e7' t='e5'><m f='c2' t='c3' /></m></m><m f='c2' t='c3'><m f='d4' t='c3'><m f='b1' t='c3'><m f='b8' t='c6' /></m></m></m></m></m><m f='c2' t='c3'>";
            strBook += "<m f='g8' t='f6'><m f='e4' t='e5'><m f='f6' t='d5'><m f='g1' t='f3'><m f='b8' t='c6' /></m></m></m></m></m><m f='b1' t='c3'><m f='e7' t='e6'><m f='g2' t='g3'><m f='d7' t='d5' /></m></m><m f='b8' t='c6'><m f='g1' t='e2' /><m f='f2' t='f4'><m f='g7' t='g6'><m f='g1' t='f3'><m f='f8' t='g7' /></m></m></m><m f='g2' t='g3'><m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7' /></m></m></m></m></m><m f='g1' t='f3'><m f='d8' t='a5' /><m f='d8' t='c7' /><m f='b7' t='b6' /><m f='g7' t='g6'><m f='c2' t='c4'><m f='f8' t='h6' /></m></m><m f='a7' t='a6' /><m f='g8' t='f6'><m f='e4' t='e5'><m f='f6' t='d5'><m f='b1' t='c3'><m f='e7' t='e6' /></m></m></m></m><m f='e7' t='e6'><m f='d2' t='d4'><m f='d7' t='d5' /><m f='c5' t='d4'><m f='f3' t='d4'><m f='g8' t='f6' /><m f='a7' t='a6' /><m f='b8' t='c6' /></m></m></m></m><m f='b8' t='c6'><m f='f1' t='b5'><m f='g7' t='g6'><m f='e1' t='g1' n='CastleKingSide'>";
            strBook += "<m f='f8' t='g7' /></m></m></m><m f='d2' t='d4'><m f='c5' t='d4'><m f='f3' t='d4'><m f='d8' t='c7' /><m f='d7' t='d5' /><m f='e7' t='e5' /><m f='g7' t='g6' /><m f='g8' t='f6' /></m></m></m></m><m f='d7' t='d6'><m f='b2' t='b4' /><m f='f1' t='b5'><m f='c8' t='d7'><m f='b5' t='d7'><m f='d8' t='d7' /></m></m></m><m f='d2' t='d4'><m f='c5' t='d4'><m f='d1' t='d4'><m f='b8' t='c6' /></m><m f='f3' t='d4'><m f='g8' t='f6' /></m></m></m></m></m></m><m f='e7' t='e6'><m f='c2' t='c4' /><m f='g1' t='f3'><m f='d7' t='d5'><m f='e4' t='e5'><m f='c7' t='c5'><m f='b2' t='b4' /></m></m></m></m><m f='b1' t='c3'><m f='d7' t='d5'><m f='f2' t='f4' /><m f='g1' t='f3' /></m></m><m f='d2' t='d3'><m f='d7' t='d5'><m f='b1' t='d2'><m f='g8' t='f6'><m f='g1' t='f3'><m f='b8' t='c6' /></m></m></m></m></m><m f='d2' t='d4'><m f='d7' t='d6' /><m f='a7' t='a6' /><m f='d7' t='d5'><m f='f1' t='d3' /><m f='c1' t='e3' />";
            strBook += "<m f='e4' t='d5'><m f='e6' t='d5'><m f='b1' t='c3'><m f='g8' t='f6' /></m></m></m><m f='e4' t='e5'><m f='c7' t='c5'><m f='d4' t='c5' /><m f='d1' t='g4' /><m f='g1' t='f3' /><m f='c2' t='c3'><m f='d8' t='b6' /><m f='b8' t='c6' /></m></m></m><m f='b1' t='d2'><m f='f7' t='f5' /><m f='b8' t='c6'><m f='g1' t='f3'><m f='g8' t='f6' /></m></m><m f='g8' t='f6'><m f='e4' t='e5'><m f='f6' t='d7' /></m></m><m f='c7' t='c5'><m f='e4' t='d5'><m f='d8' t='d5' /><m f='e6' t='d5' /></m></m></m><m f='b1' t='c3'><m f='c7' t='c5' /><m f='d5' t='e4'><m f='c3' t='e4'><m f='c8' t='d7' /><m f='b8' t='d7' /><m f='d8' t='d5' /></m></m><m f='g8' t='f6'><m f='f1' t='d3' /><m f='c1' t='e3' /><m f='e4' t='e5'><m f='f6' t='d7' /></m><m f='c1' t='g5'><m f='d5' t='e4' /><m f='f8' t='b4' /><m f='f8' t='e7' /></m></m><m f='f8' t='b4'><m f='f1' t='d3'><m f='c7' t='c5' /></m><m f='c1' t='d2' /><m f='g1' t='e2'>";
            strBook += "<m f='d5' t='e4' /></m><m f='e4' t='e5'><m f='d8' t='d7' /><m f='c7' t='c5' /></m></m></m></m></m></m><m f='e7' t='e5'><m f='d2' t='d4'><m f='e5' t='d4'><m f='g1' t='f3'><m f='c7' t='c5'><m f='f1' t='c4'><m f='b7' t='b5' /></m></m></m><m f='f2' t='f4' /><m f='c2' t='c3'><m f='d4' t='c3'><m f='f1' t='c4'><m f='c3' t='b2' /></m></m><m f='d7' t='d5' /></m><m f='d1' t='d4'><m f='b8' t='c6'><m f='d4' t='e3'><m f='f8' t='b4' /><m f='f7' t='f5' /><m f='g8' t='f6' /></m><m f='d4' t='c4' /></m></m></m></m><m f='f1' t='c4'><m f='c7' t='c6'><m f='d2' t='d4'><m f='d7' t='d5'><m f='e4' t='d5'><m f='c6' t='d5' /></m></m></m></m><m f='f7' t='f5'><m f='d2' t='d3' /></m><m f='f8' t='c5'><m f='d1' t='e2'><m f='b8' t='c6'><m f='c2' t='c3'><m f='g8' t='f6' /></m></m></m><m f='c2' t='c3'><m f='g8' t='f6'><m f='d2' t='d4'><m f='e5' t='d4' /></m></m><m f='d7' t='d5' /><m f='d8' t='g5' /></m><m f='d2' t='d4' />";
            strBook += "<m f='b2' t='b4'><m f='c5' t='b4'><m f='f2' t='f4'><m f='e5' t='f4' /></m></m></m></m><m f='g8' t='f6'><m f='f2' t='f4' /><m f='d2' t='d4'><m f='e5' t='d4'><m f='g1' t='f3'><m f='d7' t='d5' /></m></m></m></m></m><m f='b1' t='c3'><m f='f8' t='b4'><m f='d1' t='g4'><m f='g8' t='f6' /></m></m><m f='b8' t='c6'><m f='g2' t='g3' /><m f='d2' t='d4' /><m f='f2' t='f4'><m f='e5' t='f4'><m f='d2' t='d4'><m f='d8' t='h4' /></m><m f='g1' t='f3'><m f='g7' t='g5' /></m></m></m></m><m f='g8' t='f6'><m f='a2' t='a3' /><m f='g2' t='g3' /><m f='f1' t='c4'><m f='f6' t='e4'><m f='d1' t='h5'><m f='e4' t='d6' /></m><m f='g1' t='f3'><m f='d7' t='d5' /></m></m><m f='b8' t='c6' /></m><m f='f2' t='f4'><m f='d7' t='d5'><m f='f4' t='e5'><m f='f6' t='e4' /></m><m f='d2' t='d3' /></m></m></m></m><m f='f2' t='f4'><m f='d8' t='h4'><m f='g2' t='g3'><m f='h4' t='e7' /></m></m><m f='c7' t='c5' /><m f='d8' t='f6'>";
            strBook += "<m f='g1' t='f3'><m f='f6' t='f4'><m f='b1' t='c3'><m f='f8' t='b4' /></m></m></m></m><m f='f8' t='c5'><m f='g1' t='f3'><m f='d7' t='d6'><m f='b1' t='c3'><m f='g8' t='f6' /><m f='b8' t='d7' /></m><m f='c2' t='c3'><m f='c8' t='g4' /><m f='f7' t='f5' /></m><m f='f4' t='e5' /><m f='b2' t='b4' /></m></m></m><m f='g8' t='f6' /><m f='d7' t='d5'><m f='g1' t='f3' /><m f='b1' t='c3' /><m f='e4' t='d5'><m f='c7' t='c6' /><m f='e5' t='e4'><m f='b1' t='c3'><m f='g8' t='f6' /></m><m f='f1' t='b5' /><m f='d2' t='d3'><m f='g8' t='f6' /></m></m></m></m><m f='e5' t='f4'><m f='e1' t='f2' /><m f='b2' t='b3' /><m f='h2' t='h4' /><m f='f1' t='d3' /><m f='d1' t='e2' /><m f='d2' t='d4' /><m f='b1' t='c3' /><m f='d1' t='f3' /><m f='f1' t='e2' /><m f='f1' t='c4'><m f='d8' t='h4'><m f='e1' t='f1'><m f='d7' t='d5' /><m f='f8' t='c5' /><m f='g7' t='g5' /><m f='b8' t='c6' /><m f='b7' t='b5' /></m></m>";
            strBook += "<m f='b7' t='b5' /><m f='g8' t='e7' /><m f='b8' t='c6' /><m f='c7' t='c6' /><m f='f7' t='f5'><m f='d1' t='e2'><m f='d8' t='h4' /></m></m><m f='d7' t='d5'><m f='c4' t='d5'><m f='d8' t='h4' /><m f='c7' t='c6' /><m f='g8' t='f6' /></m></m><m f='g8' t='f6'><m f='b1' t='c3'><m f='f8' t='b4' /><m f='c7' t='c6' /></m></m></m><m f='g1' t='f3'><m f='g8' t='e7' /><m f='f7' t='f5' /><m f='d7' t='d6' /><m f='h7' t='h6' /><m f='g8' t='f6' /><m f='f8' t='e7'><m f='f1' t='c4'><m f='e7' t='h4' /><m f='g8' t='f6' /></m></m><m f='d7' t='d5'><m f='e4' t='d5'><m f='g8' t='f6' /></m></m><m f='g7' t='g5'><m f='b1' t='c3' /><m f='d2' t='d4'><m f='g5' t='g4' /></m><m f='f1' t='c4'><m f='b8' t='c6' /><m f='g5' t='g4' /><m f='f8' t='g7' /></m><m f='h2' t='h4'><m f='g5' t='g4' /></m></m></m></m></m><m f='g1' t='f3'><m f='d8' t='e7' /><m f='d8' t='f6' /><m f='f7' t='f6' /><m f='d7' t='d5'><m f='e4' t='d5'>";
            strBook += "<m f='f8' t='d6' /></m></m><m f='f7' t='f5'><m f='f3' t='e5'><m f='d8' t='f6'><m f='d2' t='d4'><m f='d7' t='d6' /></m></m><m f='b8' t='c6' /></m><m f='f1' t='c4'><m f='f5' t='e4'><m f='f3' t='e5'><m f='d8' t='g5' /><m f='d7' t='d5' /><m f='g8' t='f6' /></m></m></m></m><m f='d7' t='d6'><m f='f1' t='c4'><m f='f8' t='e7'><m f='c2' t='c3' /></m><m f='f7' t='f5'><m f='d2' t='d4'><m f='e5' t='d4' /></m></m></m><m f='d2' t='d4'><m f='f7' t='f5'><m f='d4' t='e5'><m f='f5' t='e4' /></m><m f='b1' t='c3' /></m><m f='e5' t='d4'><m f='d1' t='d4'><m f='c8' t='d7' /></m><m f='f3' t='d4'><m f='d6' t='d5' /><m f='g8' t='f6' /><m f='g7' t='g6' /></m></m><m f='g8' t='f6'><m f='b1' t='c3'><m f='b8' t='d7' /></m><m f='d4' t='e5'><m f='f6' t='e4' /></m><m f='f3' t='g5' /><m f='f1' t='c4' /></m><m f='b8' t='d7'><m f='f1' t='c4'><m f='c7' t='c6' /></m></m></m></m><m f='g8' t='f6'><m f='f3' t='e5'>";
            strBook += "<m f='d7' t='d6'><m f='e5' t='f3'><m f='f6' t='e4' /></m><m f='e5' t='f7' /><m f='e5' t='c4' /></m><m f='f6' t='e4' /></m><m f='b1' t='c3' /><m f='f1' t='c4' /><m f='d2' t='d4'><m f='e5' t='d4'><m f='e4' t='e5'><m f='f6' t='e4' /></m><m f='f1' t='c4' /></m><m f='f6' t='e4'><m f='f1' t='d3'><m f='d7' t='d5' /></m></m></m></m><m f='b8' t='c6'><m f='f3' t='e5'><m f='c6' t='e5'><m f='d2' t='d4' /></m></m><m f='g2' t='g3' /><m f='c2' t='c4' /><m f='f1' t='e2'><m f='g8' t='f6'><m f='d2' t='d3'><m f='d7' t='d5' /></m><m f='d2' t='d4' /></m></m><m f='c2' t='c3'><m f='d7' t='d5'><m f='d1' t='a4'><m f='c8' t='d7' /><m f='g8' t='f6' /><m f='f7' t='f6' /></m></m><m f='g8' t='f6'><m f='d2' t='d4'><m f='f6' t='e4' /></m></m><m f='g8' t='e7' /><m f='f8' t='e7' /><m f='f7' t='f5'><m f='d2' t='d4'><m f='d7' t='d6' /></m></m></m><m f='d2' t='d4'><m f='c6' t='d4'><m f='f3' t='e5'><m f='d4' t='e6' />";
            strBook += "</m></m><m f='e5' t='d4'><m f='f1' t='b5' /><m f='c2' t='c3'><m f='d4' t='c3' /></m><m f='f1' t='c4'><m f='f8' t='c5' /><m f='f8' t='b4' /><m f='f8' t='e7' /><m f='g8' t='f6' /></m><m f='f3' t='d4'><m f='c6' t='d4' /><m f='d8' t='h4' /><m f='g8' t='f6' /><m f='f8' t='c5' /></m></m></m><m f='b1' t='c3'><m f='f8' t='b4'><m f='c3' t='d5'><m f='g8' t='f6' /></m></m><m f='f7' t='f5' /><m f='g7' t='g6'><m f='d2' t='d4'><m f='e5' t='d4' /></m></m><m f='g8' t='f6'><m f='f3' t='e5' /><m f='f1' t='c4' /><m f='a2' t='a3' /><m f='d2' t='d4'><m f='f8' t='b4' /><m f='e5' t='d4' /></m><m f='f1' t='b5'><m f='a7' t='a6' /><m f='f8' t='c5' /><m f='c6' t='d4' /><m f='f8' t='b4' /></m></m></m><m f='f1' t='c4'><m f='c6' t='d4'><m f='f3' t='e5'><m f='d8' t='g5' /></m></m><m f='f7' t='f5' /><m f='f8' t='e7'><m f='d2' t='d4'><m f='e5' t='d4' /></m></m><m f='f8' t='c5'><m f='b1' t='c3'><m f='g8' t='f6' />";
            strBook += "</m><m f='c4' t='f7' /><m f='d2' t='d3'><m f='f7' t='f5' /><m f='g8' t='f6' /></m><m f='b2' t='b4'><m f='c5' t='b6' /><m f='d7' t='d5' /><m f='c5' t='b4' /></m><m f='c2' t='c3'><m f='d7' t='d6' /><m f='d8' t='e7' /><m f='g8' t='f6' /></m></m><m f='g8' t='f6'><m f='e1' t='g1' n='CastleKingSide'><m f='f8' t='c5' /></m><m f='d2' t='d3' /><m f='d2' t='d4'><m f='e5' t='d4' /></m><m f='f3' t='g5'><m f='f8' t='c5' /><m f='d7' t='d5' /></m></m></m><m f='f1' t='b5'><m f='f7' t='f6' /><m f='c6' t='a5' /><m f='f8' t='e7' /><m f='d8' t='e7' /><m f='g7' t='g5' /><m f='g7' t='g6' /><m f='g8' t='e7'><m f='b1' t='c3'><m f='g7' t='g6' /></m></m><m f='c6' t='d4'><m f='f3' t='d4'><m f='e5' t='d4' /></m></m><m f='d7' t='d6'><m f='d2' t='d4'><m f='c8' t='d7' /></m></m><m f='f7' t='f5'><m f='b1' t='c3' /></m><m f='f8' t='c5'><m f='e1' t='g1' n='CastleKingSide'><m f='c6' t='d4' /></m><m f='c2' t='c3'>";
            strBook += "<m f='g8' t='f6' /><m f='c5' t='b6' /><m f='d8' t='e7' /><m f='f7' t='f5' /></m></m><m f='g8' t='f6'><m f='d2' t='d4'><m f='e5' t='d4' /></m><m f='d2' t='d3'><m f='c6' t='e7' /><m f='d7' t='d6' /><m f='f8' t='c5' /></m><m f='e1' t='g1' n='CastleKingSide'><m f='f8' t='c5' /><m f='d7' t='d6' /><m f='f6' t='e4' /></m></m><m f='a7' t='a6'><m f='b5' t='c6'><m f='d7' t='c6' /></m><m f='b5' t='a4'><m f='g7' t='g6' /><m f='g8' t='e7' /><m f='c6' t='d4' /><m f='f8' t='b4' /><m f='f8' t='c5' /><m f='b7' t='b5' /><m f='f7' t='f5' /><m f='d7' t='d6' /><m f='g8' t='f6' /></m></m></m></m></m></m></m><m f='d2' t='d4'><m f='e7' t='e5'><m f='d4' t='e5'><m f='b8' t='c6'><m f='g1' t='f3'><m f='d8' t='e7'><m f='d1' t='d5'><m f='f7' t='f6' /></m></m></m></m></m></m><m f='e7' t='e6'><m f='c2' t='c4'><m f='b7' t='b6' /><m f='f8' t='b4' /></m></m><m f='g7' t='g6'><m f='c2' t='c4'><m f='f8' t='g7'>";
            strBook += "<m f='b1' t='c3'><m f='c7' t='c5'><m f='d4' t='d5'><m f='g7' t='c3' /></m></m></m></m></m></m><m f='d7' t='d6'><m f='g1' t='f3'><m f='c8' t='g4' /></m><m f='c2' t='c4'><m f='g7' t='g6'><m f='b1' t='c3'><m f='f8' t='g7'><m f='e2' t='e4'><m f='c7' t='c5' /><m f='f7' t='f5' /><m f='b8' t='c6' /></m></m></m></m></m></m><m f='c7' t='c5'><m f='d4' t='d5'><m f='e7' t='e6'><m f='e2' t='e4' /></m><m f='f7' t='f5' /><m f='g8' t='f6'><m f='b1' t='c3'><m f='d8' t='a5' /></m><m f='g1' t='f3'><m f='c5' t='c4' /></m></m><m f='d7' t='d6'><m f='b1' t='c3'><m f='g7' t='g6' /></m></m><m f='e7' t='e5'><m f='e2' t='e4'><m f='d7' t='d6' /></m></m></m></m><m f='f7' t='f5'><m f='b1' t='c3'><m f='g8' t='f6'><m f='g2' t='g4' /></m></m><m f='d1' t='d3'><m f='e7' t='e6'><m f='g2' t='g4' /></m></m><m f='g2' t='g3'><m f='g8' t='f6'><m f='f1' t='g2'><m f='e7' t='e6'><m f='g1' t='h3' /></m><m f='g7' t='g6' /></m></m>";
            strBook += "<m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7'><m f='g1' t='f3'><m f='c7' t='c6' /></m><m f='g1' t='h3' /></m></m></m></m><m f='e2' t='e4'><m f='d7' t='d6' /><m f='f5' t='e4'><m f='b1' t='c3'><m f='g8' t='f6'><m f='g2' t='g4' /><m f='c1' t='g5'><m f='g7' t='g6' /><m f='c7' t='c6' /><m f='b7' t='b6' /></m></m></m></m></m><m f='c2' t='c4'><m f='g7' t='g6'><m f='b1' t='c3'><m f='g8' t='h6' /></m></m><m f='e7' t='e6'><m f='b1' t='c3' /><m f='e2' t='e4' /></m><m f='g8' t='f6'><m f='b1' t='c3' /><m f='g2' t='g3'><m f='d7' t='d6'><m f='f1' t='g2'><m f='c7' t='c6' /></m></m><m f='g7' t='g6'><m f='f1' t='g2'><m f='f8' t='g7' /></m></m><m f='e7' t='e6'><m f='f1' t='g2'><m f='f8' t='b4' /><m f='f8' t='e7' /></m></m></m></m></m></m><m f='d7' t='d5'><m f='c1' t='f4'><m f='c7' t='c5' /></m><m f='e2' t='e3'><m f='g8' t='f6'><m f='f1' t='d3' /></m></m><m f='b1' t='c3'><m f='c8' t='g4' />";
            strBook += "<m f='g8' t='f6'><m f='e2' t='e4'><m f='d5' t='e4'><m f='f2' t='f3'><m f='e4' t='f3' /></m></m><m f='e7' t='e5' /></m><m f='c1' t='g5'><m f='c8' t='f5'><m f='g5' t='f6' /><m f='f2' t='f3' /></m></m></m></m><m f='g1' t='f3'><m f='b8' t='c6' /><m f='c7' t='c5' /><m f='g8' t='f6'><m f='c1' t='f4' /><m f='c1' t='g5' /><m f='e2' t='e3'><m f='e7' t='e6'><m f='b1' t='d2'><m f='c7' t='c5' /></m><m f='f1' t='d3'><m f='c7' t='c5' /></m></m></m></m></m><m f='c2' t='c4'><m f='c8' t='f5' /><m f='g8' t='f6' /><m f='c7' t='c5' /><m f='b8' t='c6'><m f='b1' t='c3'><m f='d5' t='c4'><m f='g1' t='f3' /></m></m></m><m f='e7' t='e5'><m f='d4' t='e5'><m f='d5' t='d4'><m f='e2' t='e3'><m f='f8' t='b4' /></m><m f='g1' t='f3'><m f='b8' t='c6' /></m></m></m></m><m f='c7' t='c6'><m f='b1' t='c3'><m f='d5' t='c4'><m f='e2' t='e4' /></m><m f='e7' t='e5' /></m><m f='c4' t='d5' /><m f='g1' t='f3'>";
            strBook += "<m f='g8' t='f6'><m f='b1' t='d2' /><m f='e2' t='e3'><m f='c8' t='f5' /></m><m f='c4' t='d5'><m f='c6' t='d5' /></m><m f='b1' t='c3'><m f='d8' t='b6' /><m f='g7' t='g6' /><m f='d5' t='c4' /></m></m></m></m><m f='d5' t='c4'><m f='e2' t='e4'><m f='c7' t='c5'><m f='d4' t='d5'><m f='g8' t='f6' /></m></m><m f='f7' t='f5' /></m><m f='g1' t='f3'><m f='b7' t='b5' /><m f='a7' t='a6'><m f='e2' t='e4' /><m f='e2' t='e3'><m f='c8' t='g4' /><m f='b7' t='b5' /></m></m><m f='g8' t='f6'><m f='d1' t='a4' /><m f='b1' t='c3'><m f='a7' t='a6' /></m><m f='e2' t='e3'><m f='g7' t='g6' /><m f='c8' t='g4' /><m f='c8' t='e6' /><m f='e7' t='e6' /></m></m></m></m><m f='e7' t='e6'><m f='g1' t='f3'><m f='g8' t='f6'><m f='e2' t='e3'><m f='c7' t='c6' /></m><m f='c1' t='g5'><m f='b8' t='d7' /><m f='f8' t='b4' /><m f='h7' t='h6' /></m></m></m><m f='b1' t='c3'><m f='a7' t='a6' /><m f='b7' t='b6' />";
            strBook += "<m f='f8' t='e7' /><m f='c7' t='c6'><m f='g1' t='f3'><m f='d5' t='c4' /></m><m f='e2' t='e4' /></m><m f='c7' t='c5'><m f='c4' t='d5'><m f='c5' t='d4' /><m f='e6' t='d5' /></m></m><m f='g8' t='f6'><m f='c1' t='f4' /><m f='c4' t='d5'><m f='e6' t='d5' /></m><m f='g1' t='f3'><m f='f8' t='e7' /><m f='f8' t='b4' /><m f='c7' t='c5' /><m f='c7' t='c6' /></m><m f='c1' t='g5'><m f='c7' t='c5' /><m f='b8' t='d7' /><m f='f8' t='e7' /></m></m></m></m></m></m><m f='g8' t='f6'><m f='g2' t='g4' /><m f='f2' t='f4' /><m f='f2' t='f3'><m f='d7' t='d5'><m f='e2' t='e4' /><m f='g2' t='g4' /></m></m><m f='g1' t='f3'><m f='e7' t='e6'><m f='c1' t='g5'><m f='c7' t='c5'><m f='e2' t='e4' /></m></m><m f='e2' t='e3' /></m><m f='f6' t='e4' /><m f='b7' t='b6'><m f='g2' t='g3'><m f='c8' t='b7'><m f='f1' t='g2'><m f='c7' t='c5' /></m></m></m></m><m f='g7' t='g6'><m f='c1' t='g5' /><m f='c1' t='f4' />";
            strBook += "<m f='g2' t='g3' /></m></m><m f='c2' t='c4'><m f='b8' t='c6' /><m f='b7' t='b6' /><m f='e7' t='e5'><m f='d4' t='e5'><m f='f6' t='e4'><m f='d1' t='c2' /></m><m f='f6' t='g4'><m f='g1' t='f3' /><m f='c1' t='f4' /><m f='e2' t='e4'><m f='g4' t='e5' /><m f='d7' t='d6' /></m></m></m></m><m f='d7' t='d6'><m f='b1' t='c3'><m f='c8' t='f5' /><m f='e7' t='e5'><m f='e2' t='e3'><m f='b8' t='d7' /></m><m f='g1' t='f3'><m f='b8' t='d7' /></m></m></m></m><m f='c7' t='c5'><m f='d4' t='d5'><m f='d7' t='d6' /><m f='f6' t='e4' /><m f='e7' t='e5'><m f='b1' t='c3'><m f='d7' t='d6' /></m></m><m f='b7' t='b5'><m f='c4' t='b5'><m f='a7' t='a6' /></m></m><m f='e7' t='e6'><m f='b1' t='c3'><m f='e6' t='d5' /></m></m></m></m><m f='e7' t='e6'><m f='c1' t='g5' /><m f='g2' t='g3'><m f='d7' t='d5'><m f='f1' t='g2'><m f='d5' t='c4' /><m f='f8' t='e7' /></m></m></m><m f='g1' t='f3'><m f='c7' t='c5'>";
            strBook += "<m f='d4' t='d5'><m f='b7' t='b5' /></m></m><m f='a7' t='a6' /><m f='f6' t='e4' /><m f='f8' t='b4'><m f='b1' t='d2' /><m f='c1' t='d2'><m f='d8' t='e7' /><m f='b4' t='d2' /></m></m><m f='b7' t='b6'><m f='c1' t='f4' /><m f='a2' t='a3' /><m f='b1' t='c3'><m f='c8' t='b7' /></m><m f='e2' t='e3'><m f='c8' t='b7' /></m><m f='g2' t='g3'><m f='c8' t='a6' /><m f='c8' t='b7' /></m></m></m><m f='b1' t='c3'><m f='f8' t='b4'><m f='f2' t='f3' /><m f='d1' t='d3' /><m f='g2' t='g3' /><m f='g1' t='f3'><m f='c7' t='c5' /></m><m f='d1' t='b3'><m f='c7' t='c5' /></m><m f='a2' t='a3'><m f='b4' t='c3' /></m><m f='c1' t='g5'><m f='h7' t='h6' /></m><m f='d1' t='c2'><m f='e8' t='g8' n='CastleKingSide' /><m f='b8' t='c6' /><m f='d7' t='d5' /><m f='c7' t='c5' /></m><m f='e2' t='e3'><m f='b8' t='c6' /><m f='c7' t='c5' /><m f='b7' t='b6' /><m f='e8' t='g8' n='CastleKingSide' /></m></m></m></m><m f='g7' t='g6'>";
            strBook += "<m f='f2' t='f3'><m f='d7' t='d5' /></m><m f='g1' t='f3' /><m f='d1' t='c2' /><m f='d4' t='d5'><m f='b7' t='b5' /></m><m f='g2' t='g3'><m f='d7' t='d5'><m f='f1' t='g2'><m f='f8' t='g7' /></m></m><m f='f8' t='g7'><m f='f1' t='g2'><m f='d7' t='d5' /></m></m></m><m f='b1' t='c3'><m f='d7' t='d5'><m f='g2' t='g4' /><m f='c1' t='g5'><m f='f6' t='e4' /></m><m f='d1' t='b3' /><m f='c1' t='f4'><m f='f8' t='g7' /></m><m f='c4' t='d5'><m f='f6' t='d5' /></m><m f='g1' t='f3'><m f='c7' t='c6' /><m f='f8' t='g7' /></m></m><m f='f8' t='g7'><m f='g1' t='f3'><m f='d7' t='d6' /></m><m f='e2' t='e4'><m f='d7' t='d6' /></m></m></m></m></m></m></m>";
            strBook += "</root>";

            XmlDocument xmldocBook = new XmlDocument();
            xmldocBook.LoadXml(strBook);

            BuildHashtable((XmlElement)xmldocBook.FirstChild, Game.PlayerWhite);
        }

        /// <summary>
        /// Suggest random move from the simple opening book.
        /// </summary>
        /// <param name="player">
        /// The player to play.
        /// </param>
        /// <returns>
        /// Suggested move.
        /// </returns>
        public static Move SuggestRandomMove(Player player)
        {
            Hashtable hashtable = player.Colour == Player.PlayerColourNames.White ? HashtableWhite : HashtableBlack;
            if (hashtable.ContainsKey(Board.HashCodeA))
            {
                Moves moves = (Moves)hashtable[Board.HashCodeA];
                if (moves.Count > 0)
                {
                    int intChildCount = CalculateChildNoteTotalScore(moves);
                    int intChildIndexer = 0;
                    Random random = new Random();
                    int intRandomMoveNo = random.Next(intChildCount);

                    foreach (Move move in moves)
                    {
                        intChildIndexer += move.Score;
                        if (intRandomMoveNo < intChildIndexer)
                        {
                            return move;
                        }
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build hashtable.
        /// </summary>
        /// <param name="xmlnodeParent">
        /// The xmlnode parent.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        private static void BuildHashtable(XmlElement xmlnodeParent, Player player)
        {
            Moves moves;
            Hashtable hashtable = player.Colour == Player.PlayerColourNames.White ? HashtableWhite : HashtableBlack;

            if (!hashtable.ContainsKey(Board.HashCodeA))
            {
                moves = new Moves();
                hashtable.Add(Board.HashCodeA, moves);
            }
            else
            {
                moves = (Moves)hashtable[Board.HashCodeA];
            }

            foreach (XmlElement xmlnodeMove in xmlnodeParent.ChildNodes)
            {
                Move moveThis = Board.GetSquare(xmlnodeMove.GetAttribute("f")).Piece.Move(Move.MoveNameFromString(xmlnodeMove.GetAttribute("n")), Board.GetSquare(xmlnodeMove.GetAttribute("t")));
                moveThis.Score = xmlnodeMove.ChildNodes.Count;
                moves.Add(moveThis);
                BuildHashtable(xmlnodeMove, player.OpposingPlayer);
                Move.Undo(moveThis);
            }
        }

        /// <summary>
        /// Calculates the total score for all child nodes.
        /// </summary>
        /// <param name="moves">
        /// List of moves.
        /// </param>
        /// <returns>
        /// The total score.
        /// </returns>
        private static int CalculateChildNoteTotalScore(Moves moves)
        {
            int intTotal = 0;
            foreach (Move move in moves)
            {
                intTotal += move.Score;
            }
            return intTotal;
        }

        /// <summary>
        /// Converts the source opening bok into an XML format that SharpChess can processes.
        /// </summary>
        /// <returns>
        /// Converted XML document.
        /// </returns>
        private static string Convert()
        {
            XmlDocument xmldocSource = new XmlDocument();
            XmlDocument xmldocTarget = new XmlDocument();

            XmlElement xmlnodeBookRoot = (XmlElement)xmldocTarget.AppendChild(xmldocTarget.CreateElement("root"));

            xmldocSource.Load(@"D:\Chess\My Opening Stuff\OpeningBook.xml");

            XmlNodeList xmlnodelistLine = xmldocSource.SelectNodes("//Line");
            if (xmlnodelistLine != null)
            {
                foreach (XmlElement xmlnodeLine in xmlnodelistLine)
                {
                    XmlElement xmlnodeBookPos = xmlnodeBookRoot;
                    foreach (XmlElement xmlnodeMove in xmlnodeLine.ChildNodes)
                    {
                        XmlElement xmlnodeBookChild = (XmlElement)xmlnodeBookPos.SelectSingleNode("m[@f=\"" + xmlnodeMove.GetAttribute("From") + "\" and @t=\"" + xmlnodeMove.GetAttribute("To") + "\"]");
                        if (xmlnodeBookChild == null)
                        {
                            xmlnodeBookChild = (XmlElement)xmlnodeBookPos.AppendChild(xmldocTarget.CreateElement("m"));
                            xmlnodeBookChild.SetAttribute("f", xmlnodeMove.GetAttribute("From"));
                            xmlnodeBookChild.SetAttribute("t", xmlnodeMove.GetAttribute("To"));
                            if (xmlnodeMove.GetAttribute("Name") != "Standard")
                            {
                                xmlnodeBookChild.SetAttribute("n", xmlnodeMove.GetAttribute("Name"));
                            }
                        }

                        xmlnodeBookPos = xmlnodeBookChild;
                    }
                }
            }

            Prune((XmlElement)xmldocTarget.FirstChild, 1);

            return xmldocTarget.OuterXml;
        }

        /// <summary>
        /// Prunes the opening book entries down to to a managable number that can be held im memory, by deleting the leaf nodes that have the fewest children.
        /// </summary>
        /// <param name="xmlnodeParent">
        /// The xmlnode parent.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        private static void Prune(XmlElement xmlnodeParent, int depth)
        {
            for (int intIndex = xmlnodeParent.ChildNodes.Count - 1; intIndex >= 0; intIndex--)
            {
                XmlElement xmlnode = (XmlElement)xmlnodeParent.ChildNodes[intIndex];

                if (xmlnode.ParentNode != null && (xmlnode.ChildNodes.Count == 0 && xmlnode.ParentNode.ChildNodes.Count > 5))
                {
                    xmlnode.ParentNode.RemoveChild(xmlnode);
                }
                else
                {
                    if (depth <= 2)
                    {
                        Prune(xmlnode, depth + 1);
                    }
                }
            }
        }

        #endregion
    }
}