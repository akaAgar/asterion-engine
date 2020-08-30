﻿using Asterion.Core;
using Asterion.OpenGL;

namespace Asterion.UI
{
    public sealed class UIEnvironment
    {
        private UIPage Page = null;

        private VBO TilesVBO;

        public bool InMenu { get { return Page != null; } }

        public AsterionGame Game { get; private set; }

        public UICursor Cursor { get; private set; }

        internal UIEnvironment(AsterionGame game)
        {
            Game = game;
            Cursor = new UICursor();
        }

        internal void OnLoad()
        {
            TilesVBO = new VBO(Game.Renderer, Game.Renderer.TileCount.Width, Game.Renderer.TileCount.Height);
            Cursor.OnLoad(Game);
        }

        public void ShowPage<T>(params object[] parameters) where T : UIPage, new()
        {
            ClosePage();
            Page = new T();
            Page.Initialize(this, parameters);
        }

        public void ClosePage()
        {
            if (Page == null) return;
            Page.Dispose();
            Page = null;
        }

        internal void Dispose()
        {
            ClosePage();
        }

        internal void Render()
        {
            if (!InMenu) return;

            TilesVBO.Render();
        }

        internal void UpdateTiles()
        {
            ClearTiles();

            if (InMenu)
                Page.SetTiles(TilesVBO);
        }

        private void ClearTiles()
        {
            int x, y;

            for (x = 0; x < Game.Renderer.TileCount.Width; x++)
                for (y = 0; y < Game.Renderer.TileCount.Height; y++)
                    TilesVBO.UpdateTileData(x, y, new Tile(0, RGBColor.Black));
        }
    }
}
