using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDKforNFCWrapperLibrary.FeliCa.Command;
using System.Diagnostics;
using SDKforNFCWrapperLibrary.Utils;
using SDKforNFCWrapperLibrary.FeliCa.Response;

namespace SDKforNFCWrapperLibrary.FeliCa
{
    public class FeliCaLibWrapper
    {
        private felica_nfc_dll_wrapper FeliCaNfcDllWrapperClass;
        
        public FeliCaLibWrapper()
        {
            FeliCaNfcDllWrapperClass = new felica_nfc_dll_wrapper();
        }

        public CommandResponse PerformCommand(ICommand command)
        {
            command.Dump();

            byte[] rawCommand = command.ToByteArray();
            byte[] rawCommandWithLength = new byte[rawCommand.Length + 1];
            rawCommandWithLength[0] = (byte)(rawCommand.Length + 1);
            Buffer.BlockCopy(rawCommand, 0, rawCommandWithLength, 1, rawCommand.Length);
            byte[] response = PerformRawCommand(rawCommandWithLength);
            CommandResponse commandResponse = new CommandResponse(response);

            commandResponse.Dump();

            return commandResponse;
        }

        public byte[] PerformRawCommand(byte[] command)
        {
            ushort length = (ushort) command.Length;
            byte[] response = new byte[ushort.MaxValue];
            ushort responseLength = 0;
            bool result = FeliCaNfcDllWrapperClass.FeliCaLibNfcThru(command, length, response, ref responseLength);
            if (!result)
                throw new FeliCaException();

            Array.Resize<byte>(ref response, responseLength);

            return response;
        }
    }
}
