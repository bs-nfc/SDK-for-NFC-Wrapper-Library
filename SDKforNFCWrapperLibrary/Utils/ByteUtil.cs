using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDKforNFCWrapperLibrary.Utils
{
    public class ByteUtil
    {
        public static string ByteArrayToString(byte[] byteArray)
        {
            return ByteArrayToString(byteArray, 0, byteArray.Length);
        }

        public static string ByteArrayToString(byte[] byteArray, int offset, int length)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                byte b = byteArray[offset + i];
                string byteString = ByteToString(b);
                buffer.Append(byteString).Append(" ");
            }
            return buffer.ToString().Trim();
        }

        public static string ByteToString(byte b)
        {
            return b.ToString("X2");
        }
    }
}
