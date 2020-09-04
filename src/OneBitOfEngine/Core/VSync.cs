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

namespace OneBitOfEngine.Core
{
    /// <summary>
    /// Enumerates available VSync modes.
    /// </summary>
    public enum VSync
    {
        /// <summary>
        /// Vsync disabled.
        /// </summary>
        Off = 0,
        /// <summary>
        /// VSync enabled.
        /// </summary>
        On = 1,
        /// <summary>
        /// VSync enabled, unless framerate falls below one half of target framerate. If no target framerate is specified, this behaves exactly like OpenTK.VSyncMode.On.
        /// </summary>
        Adaptive = 2
    }
}