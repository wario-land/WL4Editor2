﻿using WL4EditorCore.Exception;

namespace WL4EditorCore.Util
{
    /// <summary>
    /// Static functions which translate data formats of the GBA into formats that are more easily accessible
    /// </summary>
    public class GBAUtils
    {
        /// <summary>
        /// Get a little-endian int value (4 bytes) from data
        /// </summary>
        /// <param name="offset">The offset in data where to start reading the int</param>
        /// <returns>The int value</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the offset or part of the int being read is out of bounds of the data</exception>
        public static uint GetIntValue(uint offset) => GetValueHelper(offset, 4);

        /// <summary>
        /// Get a 4-byte pointer value from data. It is converted to a 0-based offset.
        /// </summary>
        /// <param name="offset">The offset in data where to start reading the pointer</param>
        /// <returns>The pointer value</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the offset or part of the pointer being read is out of bounds of the data</exception>
        public static uint GetPointer(uint offset) => GetIntValue(offset) & 0x7FFFFFFu;

        /// <summary>
        /// Get a little-endian short value (2 bytes) from data
        /// </summary>
        /// <param name="offset">The offset in data where to start reading the short</param>
        /// <returns>The short value</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the offset or part of the short being read is out of bounds of the data</exception>
        public static ushort GetShortValue(uint offset) => (ushort) GetValueHelper(offset, 2);

        // Get a little-endian value of specified size from data
        private static uint GetValueHelper(uint offset, int size)
        {
            if(Singleton.Instance == null)
            {
                throw new InternalException("Singleton not initialized (WL4EditorCore.Util.GetValueHelper)");
            }
            var data = Singleton.Instance.RomDataProvider.Data();

            // Verify that parameters are valid
            if (size != 2 && size != 4)
            {
                throw new InternalException($"Invalid size: {size} (must be 2 or 4)");
            }
            if (offset + size > data.Length)
            {
                throw new IndexOutOfRangeException($"offset {offset} plus type length ({size}) out of range for ROM data (length {data.Length})");
            }

            // Get the value
            uint ret = 0;
            for (int i = 0; i < size; ++i)
            {
                ret |= (uint) data[(int)(offset + size - i - 1)] << (i * 8);
            }
            return ret;
        }
    }
}
