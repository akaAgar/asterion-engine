﻿/*
==========================================================================
This file is part of Asterion Engine, an OpenGL/OpenTK 1-bit graphic
engine by @akaAgar (https://github.com/akaAgar/one-bit-of-engine)
Asterion Engine is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Asterion Engine is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with Asterion Engine. If not, see https://www.gnu.org/licenses/
==========================================================================
*/

using Asterion.Core;
using Asterion.Input;
using Asterion.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace Asterion.UI
{
    /// <summary>
    /// Describes an UI Page. Basically the Asterion.UI equivalent to a Windows Form.
    /// </summary>
    public class UIPage
    {
        /// <summary>
        /// (Private) A list of controls on this form.
        /// </summary>
        private List<UIControl> Controls = new List<UIControl>();

        /// <summary>
        /// The background tile to draw on this page where no controls appears.
        /// </summary>
        public Tile BackgroundTile { get { return BackgroundTileP; } set { BackgroundTileP = value; UI.Invalidate(); } }

        /// <summary>
        /// (Private) The background tile to draw on this page where no controls appears.
        /// </summary>
        private Tile BackgroundTileP { get; set; } = new Tile(0, RGBColor.Black);

        /// <summary>
        /// The <see cref="UIEnvironment"/> this UIPage belongs to.
        /// </summary>
        public UIEnvironment UI { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UIPage() { }

        /// <summary>
        /// Adds a new <see cref="UIControl"/> to the page's controls.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="UIControl"/> to create</typeparam>
        /// <returns>The new <see cref="UIControl"/></returns>
        protected T AddControl<T>() where T : UIControl, new()
        {
            T newControl = new T();
            newControl.Initialize(this);
            Controls.Add(newControl);
            UI.Invalidate();
            return newControl;
        }

        /// <summary>
        /// Removes a control from the page. The control will be destroyed and should not be used anymore after it has been removed.
        /// </summary>
        /// <param name="control">The control to remove.</param>
        protected void RemoveControl(UIControl control)
        {
            if (!Controls.Contains(control)) return;
            Controls.Remove(control);
            control.Destroy();
            UI.Invalidate();
        }

        /// <summary>
        /// (Internal) Called just after the page the page is created. Calls the overridable method <see cref="OnInitialize(object[])"/>.
        /// </summary>
        /// <param name="ui">The <see cref="UIEnvironment"/> this page belongs to</param>
        /// <param name="parameters">A series of parameters to pass to <see cref="OnInitialize(object[])"/></param>
        internal void Initialize(UIEnvironment ui, object[] parameters)
        {
            UI = ui;
            OnInitialize(parameters);
            UI.Invalidate();
        }

        /// <summary>
        /// (Protected) Called just after the page is created. Put your control creation logic here.
        /// UIPage.OnInitialize does nothing, so there's not need to call base.OnInitialize.
        /// </summary>
        /// <param name="parameters"></param>
        protected virtual void OnInitialize(object[] parameters) {  }

        /// <summary>
        /// (Protected) Called whenever a key is pressed when this page is displayed.
        /// UIPage.OnKeyDown does nothing, so there's not need to call base.OnKeyDown.
        /// </summary>
        /// <param name="key">The key that raised the event</param>
        /// <param name="shift">Was the shift modifier key down?</param>
        /// <param name="control">Was the control modifier key down?</param>
        /// <param name="alt">Was the alt modifier key down?</param>
        /// <param name="isRepeat">Is this a "repeated key press" event, automatically generated while the used holds the key down?</param>
        protected virtual void OnKeyDown(KeyCode key, bool shift, bool control, bool alt, bool isRepeat) { }

        /// <summary>
        /// (Internal) Called whenever a key is pressed when this page is displayed.
        /// </summary>
        /// <param name="key">The key that raised the event</param>
        /// <param name="shift">Was the shift modifier key down?</param>
        /// <param name="control">Was the control modifier key down?</param>
        /// <param name="alt">Was the alt modifier key down?</param>
        /// <param name="isRepeat">Is this a "repeated key press" event, automatically generated while the used holds the key down?</param>
        internal virtual void KeyDown(KeyCode key, bool shift, bool control, bool alt, bool isRepeat)
        {
            OnKeyDown(key, shift, control, alt, isRepeat);
        }

        /// <summary>
        /// (Internal) Draws the tiles of all controls on this page.
        /// </summary>
        /// <param name="vbo">The UI VBO on which the tiles must be drawn</param>
        internal void DrawTiles(VBO vbo)
        {
            int x, y;
            for (x = 0; x < vbo.Columns; x++)
                for (y = 0; y < vbo.Rows; y++)
                    vbo.UpdateTileData(x, y, BackgroundTile);
            
            Controls = Controls.OrderBy(c => c.ZOrder).ToList(); // Order controls by Z-Order

            foreach (UIControl control in Controls)
                control.UpdateVBOTiles(vbo);
        }

        /// <summary>
        /// (Protected) Called just before the page is closed.
        /// UIPage.OnClose does nothing, so there's not need to call base.OnClose.
        /// </summary>
        protected virtual void OnClose() { }

        /// <summary>
        /// (Internal) Called just before the page is closed.
        /// </summary>
        internal void Destroy()
        {
            OnClose();
        }
    }
}
