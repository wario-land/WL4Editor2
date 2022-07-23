using System.Collections.Immutable;
using WL4EditorCore.Interfaces.Util;

namespace WL4EditorCore
{
    public class RomDataProvider : IRomDataProvider
    {
        private ImmutableArray<byte> _data;

        /// <inheritdoc />
        public void LoadDatafromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
            var bytes = File.ReadAllBytes(filePath);
            _data = ImmutableArray.Create(bytes);
        }

        /// <inheritdoc />
        public ImmutableArray<byte> Data() => _data;
    }
}
