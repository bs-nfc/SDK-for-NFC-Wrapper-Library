using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDKforNFCWrapperLibrary.Utils;
using System.Diagnostics;

namespace SDKforNFCWrapperLibrary.FeliCa.Response
{
    public class CommandResponse
    {
        private byte[] _rawResponseBytes;

        public CommandResponse(byte[] rawResponseBytes)
        {
            _rawResponseBytes = rawResponseBytes;
        }

        public ushort GetLength()
        {
            return _rawResponseBytes[0];
        }

        public byte[] GetActualResponseBytes()
        {
            int length = GetLength();
            byte[] actualResponseBytes = new byte[length - 1];
            Buffer.BlockCopy(_rawResponseBytes, 1, actualResponseBytes, 0, length - 1);
            return actualResponseBytes;
        }

        public void Dump()
        {
            Debug.WriteLine("CommandResponse");
            Debug.WriteLine(ByteUtil.ByteArrayToString(GetActualResponseBytes()));
        }
    }
}
