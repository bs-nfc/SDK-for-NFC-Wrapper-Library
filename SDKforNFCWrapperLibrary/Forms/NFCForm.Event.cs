using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDKforNFCWrapperLibrary.Forms
{
    public partial class NFCForm
    {
        public class NfcFEventArgs : EventArgs
        {
            private DEVICE_DATA_NFC_18092_212_424K _deviceDataF;

            public NfcFEventArgs(DEVICE_DATA_NFC_18092_212_424K deviceDataF)
            {
                _deviceDataF = deviceDataF;
            }

            public DEVICE_DATA_NFC_18092_212_424K DeviceDataF
            {
                get
                {
                    return _deviceDataF;
                }
            }
        }

        public class NfcAEventArgs : EventArgs
        {
            private DEVICE_DATA_NFC_14443A_18092_106K _deviceDataA;

            public NfcAEventArgs(DEVICE_DATA_NFC_14443A_18092_106K deviceDataA)
            {
                _deviceDataA = deviceDataA;
            }

            public DEVICE_DATA_NFC_14443A_18092_106K DeviceDataA
            {
                get
                {
                    return _deviceDataA;
                }
            }
        }

        public class NfcBEventArgs : EventArgs
        {
            private DEVICE_DATA_NFC_14443B_106K _deviceDataB;

            public NfcBEventArgs(DEVICE_DATA_NFC_14443B_106K deviceDataB)
            {
                _deviceDataB = deviceDataB;
            }

            public DEVICE_DATA_NFC_14443B_106K DeviceDataB
            {
                get
                {
                    return _deviceDataB;
                }
            }
        }

        public class NfcErrorEventArgs : EventArgs
        {
            private UInt32[] _error;

            public NfcErrorEventArgs(UInt32[] error)
            {
                _error = error;
            }

            public UInt32[] Error
            {
                get
                {
                    return _error;
                }
            }
        }
    }
}
