﻿/*
==========================================================================
This file is part of One Bit of Engine, an OpenGL/OpenTK 1-bit graphic
engine by @akaAgar (https://github.com/akaAgar/one-bit-of-engine)
One Bit of Engine is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
One Bit of Engine is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with One Bit of Engine. If not, see https://www.gnu.org/licenses/
==========================================================================
*/

using System;
using OneBitOfEngine.Core;
using OneBitOfEngine.Input;
using OneBitOfEngine.OpenGL;

namespace OneBitOfEngine.UI
{
    /// <summary>
    /// UIEnvironment. 
    /// </summary>
    public sealed class UIEnvironment
    {
        /// <summary>
        /// Is this UI currently active (aka "Is a menu page currently displayed?")
        /// </summary>
        public bool Active { get { return Page != null; } }

        /// <summary>
        /// The OneBitOfGame this UIEnvironment belongs to.
        /// </summary>
        public OneBitOfGame Game { get; private set; }

        /// <summary>
        /// The cursor tile (not the mouse cursor)
        /// </summary>
        public UICursor Cursor { get; private set; }

        /// <summary>
        /// (Private) The current displayed UIPage.
        /// </summary>
        private UIPage Page = null;

        /// <summary>
        /// (Private) The VBO used to draw this UI.
        /// </summary>
        private VBO UIVBO;

        /// <summary>
        /// Does the UI VBO requires an update?
        /// </summary>
        private bool Invalidated;

        /// <summary>
        /// (Internal) Constructor.
        /// </summary>
        /// <param name="game">The OneBitOfGame this UIEnvironment belongs to</param>
        internal UIEnvironment(OneBitOfGame game)
        {
            Game = game;
            Cursor = new UICursor();
        }

        /// <summary>
        /// (Internal) Called before entering the main loop. Creates the UI and the Cursor VBOs.
        /// </summary>
        internal void OnLoad()
        {
            UIVBO = new VBO(Game.Renderer, Game.Renderer.TileCount.Width, Game.Renderer.TileCount.Height);
            Cursor.OnLoad(Game.Renderer);
        }

        /// <summary>
        /// Displays a UIPage. If a page is already displayed, it is closed.
        /// </summary>
        /// <typeparam name="T">Type of page to display.</typeparam>
        /// <param name="parameters">Parameters to pass the page will be handled by the <see cref="UIPage.OnInitialize(object[])"/> method.</param>
        public void ShowPage<T>(params object[] parameters) where T : UIPage, new()
        {
            ClosePage();
            Page = new T();
            Page.Initialize(this, parameters);
            Invalidate();
        }

        /// <summary>
        /// Close the current UIPage.
        /// </summary>
        public void ClosePage()
        {
            if (Page == null) return;
            Page.Destroy();
            Page = null;
            Invalidate();
        }

        /// <summary>
        /// (Internal) Send the char matching a key press event to the current page, if any.
        /// </summary>
        /// <param name="keyChar">Char for the key input</param>
        internal void OnKeyPressEvent(char keyChar)
        {
            if (Page != null)
                Page.OnKeyPressEventInternal(keyChar);
        }

        /// <summary>
        /// Draws the current UIPage, if any.
        /// </summary>
        internal void OnRenderFrame()
        {
            if (Invalidated)
            {
                ClearTiles();
                if (Active) Page.DrawTiles(UIVBO);
                Invalidated = false;
            }

            if (!Active) return;
            UIVBO.Render();
        }

        /// <summary>
        /// (Internal) Notifies the UI that its VBO needs to be redrawn. Should be called each time a page or control is changed.
        /// </summary>
        internal void Invalidate()
        {
            Invalidated = true;
        }

        /// <summary>
        /// (Private) Clears all tiles in the VBO.
        /// </summary>
        private void ClearTiles()
        {
            int x, y;

            for (x = 0; x < Game.Renderer.TileCount.Width; x++)
                for (y = 0; y < Game.Renderer.TileCount.Height; y++)
                    UIVBO.UpdateTileData(x, y, 0, RGBColor.Black);
        }

        /// <summary>
        /// (Internal) Closes the UI, destroys the VBO and the cursor.
        /// </summary>
        internal void Destroy()
        {
            ClosePage();
            UIVBO.Dispose();
            Cursor.Destroy();
        }

        /// <summary>
        /// (Internal) Called whenever a key is pressed down. Passes the key input to the current displayed page, if any.
        /// </summary>
        /// <param name="key">The key or gamepad button that raised the event</param>
        /// <param name="modifiers">Which modifier keys are down?</param>
        /// <param name="gamepadIndex">Index of the gamepad that raised the event, if the key is a gamepad button, or -1 if it was a keyboard key</param>
        /// <param name="isRepeat">Is this a "repeated key press" event, automatically generated while the used holds the key down?</param>
        internal void OnInputEvent(KeyCode key, ModifierKeys modifiers, int gamepadIndex, bool isRepeat)
        {
            Cursor.OnInputEventInternal(key, modifiers, gamepadIndex, isRepeat);
            if (!Active) return;

            Page.OnInputEventInternal(key, modifiers, gamepadIndex, isRepeat);
        }
    }
}
