using System.Collections.Immutable;

namespace WL4EditorCore.Interfaces.Util
{
    public interface IRomDataProvider
    {
        /// <summary>
        /// Load data into ROM data provider by reading the contents of a file as a byte array.
        /// </summary>
        /// <param name="filePath">The file to read.</param>
        /// <exception cref="FileNotFoundException">If the file does not exist.</exception>
        void LoadDatafromFile(string filePath);

        /// <summary>
        /// Get the ROM data as an immutable array.
        /// </summary>
        /// <returns>The ROM data.</returns>
        ImmutableArray<byte> Data();
    }
}
