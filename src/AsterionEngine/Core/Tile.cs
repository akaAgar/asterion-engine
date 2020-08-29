﻿namespace Asterion.Core
{
    public struct Tile
    {
        public static Tile Empty { get; } = new Tile(0, RGBColor.Black);

        public int TileIndex { get; }
        public RGBColor Color { get; }
        public int Tilemap { get; }
        public bool Animated { get; }

        public Tile(int tileIndex, RGBColor color, int tilemap = 0, bool animated = false)
        {
            TileIndex = tileIndex;
            Color = color;
            Tilemap = tilemap;
            Animated = animated;
        }

        public Tile(int tileIndex, RGBColor color, bool animated)
        {
            TileIndex = tileIndex;
            Color = color;
            Tilemap = 0;
            Animated = animated;
        }
    }
}
