using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SDKforNFCWrapperLibrary.Utils;
using System.Diagnostics;

namespace SDKforNFCWrapperLibrary.FeliCa.Command
{
    public class PollingCommand : ICommand
    {
        private const byte COMMAND_CODE_POLLING = 0x00;

        public readonly static byte[] SYSTEM_CODE_WILDCARD = new byte[] { 0xff, 0xff };

        public const byte REQUEST_CODE_NONE = 0x00;

        public const byte REQUEST_CODE_REQUEST_SYSTEM_CODE = 0x01;

        public const byte REQUEST_CODE_REQUEST_COMMUNICATION_PERFORMANCE = 0x02;

        private const byte DEFAULT_TIMESLOT = 0x00;

        private byte[] _systemCode;

        private byte _requestCode;

        public PollingCommand(byte[] systemCode, byte requestCode)
        {
            _systemCode = systemCode;
            _requestCode = requestCode;
        }

        public byte[] ToByteArray()
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                stream.WriteByte(COMMAND_CODE_POLLING);
                stream.Write(_systemCode, 0, _systemCode.Length);
                stream.WriteByte(_requestCode);
                stream.WriteByte(DEFAULT_TIMESLOT);
                return stream.ToArray();
            }
            finally
            {
                stream.Close();
            }
        }

        public void Dump()
        {
            Debug.WriteLine("PollingCommand");
            Debug.WriteLine(ByteUtil.ByteArrayToString(ToByteArray()));
        }
    }
}
