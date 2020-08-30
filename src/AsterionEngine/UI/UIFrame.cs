﻿using Asterion.Core;
using Asterion.OpenGL;

namespace Asterion.UI
{
    /// <summary>
    /// A rectangular frame, s
    /// </summary>
    public class UIFrame : UIControl
    {
        /// <summary>
        /// The tile to use for the frame.
        /// Frame tile must follow one another on the tilemap (but can be on multiple rows) in this order:
        /// upper-left corner, upper-right corner, lower-left corner, lower-right corner, top border, left border, bottom border, right border.
        /// </summary>
        public int FrameTile { get { return FrameTile_; } set { FrameTile_ = value; Page.UI.Invalidate(); } }
        private int FrameTile_ = 0;

        /// <summary>
        /// Size of the control.
        /// </summary>
        public Dimension Size { get { return Size_; } set { Size_ = value; Page.UI.Invalidate(); } }
        private Dimension Size_ = Dimension.One;

        /// <summary>
        /// Width of the control.
        /// </summary>
        public int Width { get { return Size_.Width; } set { Size_ = new Dimension(value, Size_.Height); Page.UI.Invalidate(); } }

        /// <summary>
        /// Height of the control.
        /// </summary>
        public int Height { get { return Size_.Height; } set { Size_ = new Dimension(Size_.Width, value); Page.UI.Invalidate(); } }

        /// <summary>
        /// The tile to use to fill the frame background, or null if none.
        /// If null, frame background will be transparent.
        /// </summary>
        public int? FillTile { get { return FillTile_; } set { FillTile_ = value; Page.UI.Invalidate(); } }
        private int? FillTile_ = null;

        /// <summary>
        /// (Internal) Draws the control on the provided VBO.
        /// </summary>
        /// <param name="vbo">UI VBO on which to draw the control.</param>
        internal override void UpdateVBOTiles(VBO vbo)
        {
            int x, y;
            int frameTileIndex;
            Tile frameTile;

            Area rect = new Area(Position, Size);

            for (x = rect.Left; x < rect.Right; x++)
                for (y = rect.Top; y < rect.Bottom; y++)
                {
                    frameTileIndex = FrameTile;

                    if (x == rect.Left)
                    {
                        if (y == rect.Top) frameTileIndex += 0;
                        else if (y == rect.Bottom - 1) frameTileIndex += 2;
                        else frameTileIndex += 5;
                    }
                    else if (x == rect.Right - 1)
                    {
                        if (y == rect.Top) frameTileIndex += 1;
                        else if (y == rect.Bottom - 1) frameTileIndex += 3;
                        else frameTileIndex += 7;
                    }
                    else if (y == rect.Top) frameTileIndex += 4;
                    else if (y == rect.Bottom - 1) frameTileIndex += 6;
                    else if (FillTile.HasValue)
                        frameTileIndex = FillTile.Value;
                    else
                        continue;

                    frameTile = new Tile(frameTileIndex, Color, Tilemap);

                    vbo.UpdateTileData(x, y, frameTile);
                }
        }
    }
}
