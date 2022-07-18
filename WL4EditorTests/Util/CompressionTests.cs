using WL4EditorCore.Util;
using System.Text.RegularExpressions;
using WL4EditorCore.Exception;

namespace WL4EditorTests.Util
{
    [TestClass]
    public class CompressionTests : TestBase
    {
        private static string CompressedDataDirectory = $"{TestDataDirectory}\\Compression\\Compressed";
        private static string DecompressedDataDirectory = $"{TestDataDirectory}\\Compression\\Decompressed";

        [TestMethod]
        [Description("Test minimal valid cases for RLE decompression")]
        [DataRow("01 03 AACCEE 00 01 03 BBDDFF 00", 0u, "AABB CCDD EEFF")] // data run (RLE8)
        [DataRow("01 84 AA 00 01 84 BB 00", 0u, "AABB AABB AABB AABB")] // data repeat (RLE8)
        [DataRow("00 0004 AACCEE88 0000 00 0004 BBDDFF99 0000", 0u, "AABB CCDD EEFF 8899")] // data run (RLE16)
        [DataRow("00 8003 AA 0000 00 8003 BB 0000", 0u, "AABB AABB AABB")] // data repeat (RLE16)
        [DataRow("00000000000000 01 03 AACCEE 00 01 03 BBDDFF 00", 7u, "AABB CCDD EEFF")] // starting from an offset
        public void Test_RLEDecompress_Valid(string inputStr, uint offset, string expectedStr)
        {
            var input = StringToByteArray(inputStr);
            var expected = StringToByteArray(expectedStr);
            var actual = Compression.RLEDecompress(input, offset);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Description("Test decompression on hundreds of compressed data files. Depends on correctness of Test_RLEDecompress_Valid")]
        public void Test_RLEDecompress_Valid_LargeTest()
        {
            var filenames = Directory.EnumerateFiles(CompressedDataDirectory).Select((fullPath) => Path.GetFileName(fullPath));
            RunThreadedTests((result) =>
            {
                var fileName = result.Data as string;
                string inputDataFile = $"{CompressedDataDirectory}\\{fileName}";
                string expectedDataFile = $"{DecompressedDataDirectory}\\{fileName}";
                if (!File.Exists(inputDataFile))
                {
                    throw new Exception("Testing exception: Compressed data file not found - " + inputDataFile);
                }
                if (!File.Exists(expectedDataFile))
                {
                    throw new Exception("Testing exception: Decompressed data file not found - " + expectedDataFile);
                }
                var inputData = File.ReadAllBytes(inputDataFile);
                var actualData = Compression.RLEDecompress(inputData, 0);
                var expectedData = File.ReadAllBytes(expectedDataFile);
                CollectionAssert.AreEqual(expectedData, actualData);
                result.Success = true;
            }, filenames);
        }

        [TestMethod]
        [Description("Test compression and decompression on hundreds of compressed data files. Depends on correctness of Test_RLEDecompress_Valid_LargeTest")]
        public void Test_RLECompress_Valid_LargeTest()
        {
            var filenames = Directory.EnumerateFiles(CompressedDataDirectory).Select((fullPath) => Path.GetFileName(fullPath));
            RunThreadedTests((result) =>
            {
                var fileName = result.Data as string;
                string inputDataFile = $"{DecompressedDataDirectory}\\{fileName}";
                if (!File.Exists(inputDataFile))
                {
                    throw new Exception("Testing exception: Decompressed data file not found - " + inputDataFile);
                }
                var inputData = File.ReadAllBytes(inputDataFile);
                if(inputData.Length % 2 != 0)
                {
                    throw new Exception("Testing exception: Input file must have a length which is divisible by 2 - " + inputDataFile);
                }

                // De-interleave the file into upper and lower byte arrays
                var upper = new byte[inputData.Length / 2];
                var lower = new byte[inputData.Length / 2];
                for(int i = 0; i < upper.Length; ++i)
                {
                    upper[i] = inputData[i * 2];
                    lower[i] = inputData[i * 2 + 1];
                }
                var cUpper = new List<byte>(Compression.RLECompress(upper));
                var cLower = new List<byte>(Compression.RLECompress(lower));

                // Coalesce compressed data together, then decompress it to validate
                cUpper.AddRange(cLower);
                var actualData = Compression.RLEDecompress(cUpper.ToArray(), 0);
                CollectionAssert.AreEqual(inputData, actualData);
                result.Success = true;
            }, filenames);
        }

        [TestMethod]
        [Description("Test cases in which RLE decompression throws IndexOutOfRangeException")]
        [DataRow("")] // immediately hit the end of data
        [DataRow("01 05 AABBCC")] // index OOB during a data run
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RLEDecompress_Invalid_EndOfData(string inputStr)
        {
            var input = StringToByteArray(inputStr);
            Compression.RLEDecompress(input, 0);
        }

        [TestMethod]
        [Description("Test cases in which RLE decompression throws DataException")]
        [DataRow("01 04 AABBCCDD 00 01 03 AABBCC 00")] // 4 byte upper, 3 byte lower
        [ExpectedException(typeof(DataException))]
        public void Test_RLEDecompress_Invalid_SizeMismatch(string inputStr)
        {
            var input = StringToByteArray(inputStr);
            Compression.RLEDecompress(input, 0);
        }

        private static byte[] StringToByteArray(string input)
        {
            input = input.Replace(" ", string.Empty);
            var pattern = "^([0-9A-F]{2})*$";
            if (!Regex.IsMatch(input, pattern))
            {
                throw new Exception($"Incorrectly formatted test. Test data does not match pattern {pattern}");
            }
            var result = new byte[input.Length / 2];
            for(int i = 0; i < input.Length / 2; ++i)
            {
                result[i] = byte.Parse(input.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return result;
        }
    }
}