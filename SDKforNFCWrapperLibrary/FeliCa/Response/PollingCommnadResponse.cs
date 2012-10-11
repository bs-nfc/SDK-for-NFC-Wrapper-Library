using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SDKforNFCWrapperLibrary.Utils;

namespace SDKforNFCWrapperLibrary.FeliCa.Response
{
    public class PollingCommnadResponse
    {
        private CommandResponse _commandResponse;

        private byte _responseCode;

        private byte[] _idm;

        private byte[] _pmm;

        private byte[] _requestData;

        public PollingCommnadResponse(CommandResponse commandResponse)
        {
            _commandResponse = commandResponse;
            byte[] responseBytes = _commandResponse.GetActualResponseBytes();

            _responseCode =  responseBytes[0];

            _idm = new byte[8];
            Buffer.BlockCopy(responseBytes, 1, _idm, 0, 8);

            _pmm = new byte[8];
            Buffer.BlockCopy(responseBytes, 9, _pmm, 0, 8);

            if(responseBytes.Length > 17){
                _requestData = new byte[2];
                Buffer.BlockCopy(responseBytes, 17, _requestData, 0, 2);
            }else{
                _requestData = new byte[0];
            }
        }

        public byte ResponseCode()
        {
            return _responseCode;
        }

        public byte[] IDm()
        {
            return _idm;
        }

        public byte[] PMm()
        {
            return _pmm;
        }

        public byte[] RequestData()
        {
            return _requestData;
        }

        public void Dump()
        {
            Debug.WriteLine("PollingCommandResponse");
            Debug.WriteLine("ResponseCode=" + ByteUtil.ByteToString(ResponseCode()));
            Debug.WriteLine("IDm=" + ByteUtil.ByteArrayToString(IDm()));
            Debug.WriteLine("PMm=" + ByteUtil.ByteArrayToString(PMm()));
            Debug.WriteLine("ResponseData=" + ByteUtil.ByteArrayToString(RequestData()));
        }
    }
}
