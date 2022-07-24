using WL4EditorCore.Util;
using WL4EditorCore.Exception;
using System.Collections.Immutable;

namespace WL4EditorTests.Util
{
    [TestClass]
    public class CompressionTests : TestBase
    {
        private static string CompressedDataDirectory = $"{TestDataDirectory}\\Compression\\Compressed";
        private static string DecompressedDataDirectory = $"{TestDataDirectory}\\Compression\\Decompressed";

        [TestInitialize]
        [Description("Tests must synchronize since Singleton is modified")]
        public void TestInit()
        {
            TestClassSynchronizationLock.WaitOne();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mocks.MockRomDataProvider.Invocations.Clear();
            TestClassSynchronizationLock.ReleaseMutex();
        }

        [TestMethod]
        [Description("Test minimal valid cases for RLE decompression")]
        [DataRow("01 03 AACCEE 00 01 03 BBDDFF 00", 0, "AABB CCDD EEFF")] // data run (RLE8)
        [DataRow("01 84 AA 00 01 84 BB 00", 0, "AABB AABB AABB AABB")] // data repeat (RLE8)
        [DataRow("00 0004 AACCEE88 0000 00 0004 BBDDFF99 0000", 0, "AABB CCDD EEFF 8899")] // data run (RLE16)
        [DataRow("00 8003 AA 0000 00 8003 BB 0000", 0, "AABB AABB AABB")] // data repeat (RLE16)
        [DataRow("00000000000000 01 03 AACCEE 00 01 03 BBDDFF 00", 7, "AABB CCDD EEFF")] // starting from an offset
        public void Test_RLEDecompress_Valid(string inputStr, int offset, string expectedStr)
        {
            var input = StringToByteArray(inputStr);
            var expected = StringToByteArray(expectedStr);
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(input));
            var actual = Compression.RLEDecompress(offset);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        //[Timeout(30000)]
        [Description("Test decompression on hundreds of compressed data files. Depends on correctness of Test_RLEDecompress_Valid")]
        public void Test_RLEDecompress_Valid_LargeTest()
        {
            var filenames = Directory.EnumerateFiles(CompressedDataDirectory).Select((fullPath) => Path.GetFileName(fullPath));
            foreach(var fileName in filenames)
            {
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
                Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(inputData));
                var actualData = Compression.RLEDecompress(0);
                var expectedData = File.ReadAllBytes(expectedDataFile);
                CollectionAssert.AreEqual(expectedData, actualData);
            }
        }

        [TestMethod]
        [Timeout(30000)]
        [Description("Test compression and decompression on hundreds of compressed data files. Depends on correctness of Test_RLEDecompress_Valid_LargeTest")]
        public void Test_RLECompress_Valid_LargeTest()
        {
            var filenames = Directory.EnumerateFiles(CompressedDataDirectory).Select((fullPath) => Path.GetFileName(fullPath));
            foreach(var fileName in filenames)
            {
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
                Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(cUpper.ToArray()));
                var actualData = Compression.RLEDecompress(0);
                CollectionAssert.AreEqual(inputData, actualData);
            }
        }

        [TestMethod]
        [Description("Test cases in which RLE decompression throws IndexOutOfRangeException")]
        [DataRow("")] // immediately hit the end of data
        [DataRow("01 05 AABBCC")] // index OOB during a data run
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_RLEDecompress_Invalid_EndOfData(string inputStr)
        {
            var input = StringToByteArray(inputStr);
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(input));
            Compression.RLEDecompress(0);
        }

        [TestMethod]
        [Description("Test cases in which RLE decompression throws DataException")]
        [DataRow("01 04 AABBCCDD 00 01 03 AABBCC 00")] // 4 byte upper, 3 byte lower
        [ExpectedException(typeof(DataException))]
        public void Test_RLEDecompress_Invalid_SizeMismatch(string inputStr)
        {
            var input = StringToByteArray(inputStr);
            Mocks.MockRomDataProvider.Setup(a => a.Data()).Returns(ImmutableArray.Create(input));
            Compression.RLEDecompress(0);
        }
    }
}