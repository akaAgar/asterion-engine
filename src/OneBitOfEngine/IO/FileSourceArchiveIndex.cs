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

namespace OneBitOfEngine.IO
{
    /// <summary>
    /// (Internal) Data for an index entry in the FileSourceArchive class.
    /// </summary>
    internal struct FileSourceArchiveIndex
    {
        /// <summary>
        /// (Internal) Offset (in bytes) from the beginning of the file at which the entry is stored.
        /// </summary>
        internal int Offset { get; private set; }

        /// <summary>
        /// (Internal) Length of the entry, in bytes.
        /// </summary>
        internal int Length { get; private set; }

        /// <summary>
        /// (Internal) Constructor.
        /// </summary>
        /// <param name="offset">Offset (in bytes) from the beginning of the file at which the entry is stored</param>
        /// <param name="length">Length of the entry, in bytes</param>
        internal FileSourceArchiveIndex(int offset, int length)
        {
            Offset = Math.Max(0, offset);
            Length = Math.Max(0, length);
        }
    }
}
