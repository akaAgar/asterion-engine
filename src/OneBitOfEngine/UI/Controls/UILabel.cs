﻿using OneBitOfEngine.Core;
using OneBitOfEngine.OpenGL;
using System;

namespace OneBitOfEngine.UI.Controls
{
    /// <summary>
    /// A label control: a single line string of text
    /// </summary>
    public class UILabel : UIControl
    {
        /// <summary>
        /// The tile to use for this control's font.
        /// Font tiles must follow one another on the tilemap (but can be on multiple rows) and provide all the ASCII characters in the 32 (white space) to 126 (~) range.
        /// </summary>
        public int FontTile { get { return FontTile_; } set { FontTile_ = value; Page.UI.Invalidate(); } }
        private int FontTile_ = 0;

        /// <summary>
        /// The text of this label.
        /// </summary>
        public virtual string Text { get { return Text_; }
            set
            {
                Text_ = TruncateText(value);
                Page.UI.Invalidate();
            }
        }
        private string Text_ = "";

        /// <summary>
        /// Max length of the text. Zero or less means no max length.
        /// </summary>
        public int MaxLength { get { return MaxLength_; } set { MaxLength_ = value; Text_ = TruncateText(Text_); Page.UI.Invalidate(); } }
        private int MaxLength_ = 0;

        /// <summary>
        /// (Internal) Draws the control on the provided VBO.
        /// </summary>
        /// <param name="vbo">UI VBO on which to draw the control.</param>
        internal override void UpdateVBOTiles(VBO vbo)
        {
            if (string.IsNullOrEmpty(Text_)) return;

            DrawTextOnVBO(vbo, Text_, Position.X, Position.Y, FontTile_, Color, TileEffect);
        }

        private string TruncateText(string text)
        {
            if (text == null) return "";
            if (MaxLength_ <= 0) return text; // No length limit

            return text.Substring(0, Math.Min(text.Length, MaxLength_));
        }
    }
}
