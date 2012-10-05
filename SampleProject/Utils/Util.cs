using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleProject.Utils
{
    class Util
    {
        /// <summary>
        /// バイト配列を16進数文字列に変換します
        /// <param name="byteArray">バイト配列</param>
        /// </summary>
        public static string ToHexString(byte[] byteArray)
        {
            return Util.ToHexString(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// バイト配列を16進数文字列に変換します
        /// <param name="byteArray">バイト配列</param>
        /// <param name="offset">対象の開始位置</param>
        /// <param name="length">対象の数</param>
        /// </summary>
        public static string ToHexString(byte[] byteArray, int offset, int length)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                buffer.Append(byteArray[offset + i].ToString("X2")).Append(" ");
            }
            return buffer.ToString().Trim();
        }
    }
}
