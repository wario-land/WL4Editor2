using WL4EditorCore.Exception;

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
        /// <param name="data">The data from which to get an int</param>
        /// <param name="offset">The offset in data where to start reading the int</param>
        /// <returns>The int value</returns>
        /// <exception cref="ArgumentNullException">If data is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the offset or part of the int being read is out of bounds of the data</exception>
        public static uint GetIntValue(byte[] data, uint offset) => GetValueHelper(data, offset, 4);

        /// <summary>
        /// Get a little-endian short value (2 bytes) from data
        /// </summary>
        /// <param name="data">The data from which to get a short</param>
        /// <param name="offset">The offset in data where to start reading the short</param>
        /// <returns>The short value</returns>
        /// <exception cref="ArgumentNullException">If data is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the offset or part of the short being read is out of bounds of the data</exception>
        public static ushort GetShortValue(byte[] data, uint offset) => (ushort) GetValueHelper(data, offset, 2);

        // Get a little-endian value of specified size from data
        private static uint GetValueHelper(byte[] data, uint offset, int size)
        {
            // Verify that parameters are valid
            if (size != 2 && size != 4)
            {
                throw new InternalException($"Invalid size: {size} (must be 2 or 4)");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (offset + size > data.Length)
            {
                throw new IndexOutOfRangeException($"offset {offset} plus type length ({size}) out of range for data (length {data.Length})");
            }

            // Get the value
            uint ret = 0;
            for (int i = 0; i < size; ++i)
            {
                ret |= (uint) data[offset + size - i - 1] << (i * 8);
            }
            return ret;
        }
    }
}