using WL4EditorCore.Exception;

namespace WL4EditorCore.Util
{
    /// <summary>
    /// Functions pertaining to decompression and recompression of data in the format used by WL4
    /// </summary>
    public class Compression
    {
        #region Decompression
        /// <summary>
        /// Decompress RLE data in WL4's format
        /// Data is compressed in 2 sequential regions in either RL8 or RLe16 format, and interleaved into the high and low bytes of shorts in the output.
        /// </summary>
        /// <param name="data">The data from which decompression will be performed.</param>
        /// <param name="offset">The offset into data which marks the start of decompression.</param>
        /// <returns>The decompressed data as a byte array.</returns>
        /// <exception cref="DataException">If the end of data is reached while performing an RLE pass, or if the upper and lower portions do not match in size.</exception>
        public static byte[] RLEDecompress(byte[] data, uint offset)
        {
            List<byte> upper, lower;

            // Get both RLE passes
            try
            {
                var address = offset;
                upper = RLEDecompressSinglePass(data, ref address);
                lower = RLEDecompressSinglePass(data, ref address);
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException($"Reached the end of data while attempting to decompress address 0x{offset:X}");
            }
            if (upper.Count != lower.Count)
            {
                throw new DataException($"Upper and lower half size mismatch while decompressing address 0x{offset:X}");
            }

            // Interleave and return the result
            var result = new byte[upper.Count * 2];
            for (int i = 0; i < upper.Count; ++i)
            {
                result[i * 2] = upper[i];
                result[i * 2 + 1] = lower[i];
            }
            return result;
        }

        // Decompress a single pass of data. 2 passes must be interleaved to construct the output.
        private static List<byte> RLEDecompressSinglePass(byte[] data, ref uint offset)
        {
            List<byte> outputData = new List<byte>();
            uint runData;
            var rle8 = data[offset++] == 1;
            while (true)
            {
                uint ctrl = rle8 ? data[offset] : GBAUtils.GetShortValue(data, offset);
                offset += rle8 ? 1u : 2;
                uint ctrlMask = rle8 ? 0x80u : 0x8000;
                if (ctrl == 0)
                {
                    // End of data
                    break;
                }
                else if ((ctrl & ctrlMask) != 0)
                {
                    // Repeating value
                    runData = ctrl & (ctrlMask - 1);
                    for (int j = 0; j < runData; j++)
                    {
                        outputData.Add(data[offset]);
                    }
                    offset++;
                }
                else
                {
                    // Data run
                    runData = ctrl;
                    for (int j = 0; j < runData; j++)
                    {
                        outputData.Add(data[offset + j]);
                    }
                    offset += runData;
                }
            }
            return outputData;
        }
        #endregion

        #region Compression
        /// <summary>
        /// Compress a single pass of data with RLE.
        /// rle8 and rle16 are tried. The one which compresses better is returned.
        /// </summary>
        /// <param name="data">The data to compress.</param>
        /// <returns>The compressed data</returns>
        public static byte[] RLECompress(byte[] data)
        {
            var rle8data = RLECompressHelper(data, true);
            var rle16data = RLECompressHelper(data, false);
            return rle16data.Length < rle8data.Length ? rle16data : rle8data;
        }

        private static byte[] RLECompressHelper(byte[] data, bool rle8)
        {
            // Generate R and C jumptables
            var jumpTable = GenerateJumpTable(data, rle8);
            var R = jumpTable[0];
            var C = jumpTable[1];

            return CompressUsingJumptable(data, R, C, rle8);
        }

        private static ushort[][] GenerateJumpTable(byte[] data, bool rle8)
        {
            var R = new ushort[data.Length];
            var C = new ushort[data.Length];
            var jumpLimit = rle8 ? 0x7F : 0x7FFF;
            ushort cons = (ushort)0;
            int minRun = rle8 ? 3 : 5;

            // Seed the dynamic programming jump table
            R[data.Length - 1] = 1;

            // Populate R backwards in the jump table
            for (int i = data.Length - 1; i >= 1; --i)
            {
                R[i - 1] = (ushort)((R[i] == jumpLimit || data[i] != data[i - 1]) ? 1 : R[i] + 1);
            }

            // Populate C forwards in the jump table
            for (int i = 0; i < data.Length; ++i)
            {
                if (R[i] < minRun)
                {
                    ++cons;
                }
                if (R[i] >= minRun)
                {
                    C[i - cons] = cons;
                    cons = 0;
                    i += R[i] - 1;
                }
                else if (cons == jumpLimit)
                {
                    C[i - cons + 1] = cons;
                    cons = 0;
                }
            }
            if (cons != 0)
            {
                C[data.Length - cons] = cons;
            }

            return new ushort[][] { R, C };
        }

        private static byte[] CompressUsingJumptable(byte[] data, ushort[] R, ushort[] C, bool rle8)
        {
            var i = 0;
            var minRun = rle8 ? 3 : 5;
            var typeIdentifier = (byte)(rle8 ? 1 : 2);

            // Add an opcode to compressed output which may be 8 or 16 bits and requires the high bit set for run mode
            Action<List<byte>, ushort, bool> AddOpcode = (cData, opcode, runMode) =>
            {
                if (runMode)
                {
                    opcode |= (ushort)(rle8 ? 0x80 : 0x8000);
                }
                if (!rle8)
                {
                    cData.Add((byte)(opcode >> 8));
                }
                cData.Add((byte)opcode);
            };

            // Populate the compressed data
            var compressedData = new List<byte>
            {
                typeIdentifier
            };
            while (i < data.Length)
            {
                bool runmode = R[i] >= minRun;
                ushort len = runmode ? R[i] : C[i];
                if(len == 0)
                {
                    var Rvalues = "[" + string.Join(", ", R) + "]";
                    var Cvalues = "[" + string.Join(", ", C) + "]";
                    throw new InternalException($"CompressUsingJumptable has entered an infinite loop due to an invalid jumptable value. R: {Rvalues} C: {Cvalues}");
                }
                AddOpcode(compressedData, len, runmode);
                for (int j = 0; j < (runmode ? 1 : len); ++j)
                {
                    compressedData.Add(data[i + j]);
                }
                i += len;
            }
            AddOpcode(compressedData, 0, false);

            return compressedData.ToArray();
        }
        #endregion
    }
}