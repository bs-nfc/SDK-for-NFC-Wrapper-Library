using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDKforNFCWrapperLibrary.Properties;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace SDKforNFCWrapperLibrary.Forms
{
    public partial class NFCForm : Form
    {
        [DllImport("User32.dll")]
        extern static UInt32 RegisterWindowMessage(string lpString);

        private const string MSG_STR_OF_FIND = "find";

        private const string MSG_STR_OF_ENABLE = "enable";

        private const UInt32 DEVICE_TYPE_NFC_14443A_18092_106K = 0x00000001;

        private const UInt32 DEVICE_TYPE_NFC_18092_212K = 0x00000002;

        private const UInt32 DEVICE_TYPE_NFC_18092_424K = 0x00000004;

        private const UInt32 DEVICE_TYPE_NFC_14443B_106K = 0x00000008;

        private static felica_nfc_dll_wrapper FeliCaNfcDllWrapperClass = new felica_nfc_dll_wrapper();

        private UInt32 cardFindMessage;

        private UInt32 cardEnableMessage;

        public delegate void DiscoverNfcFTagEventHandler(object sender, NfcFEventArgs e);

        public delegate void DiscoverNfcATagEventHandler(object sender, NfcAEventArgs e);

        public delegate void DiscoverNfcBTagEventHandler(object sender, NfcBEventArgs e);

        public delegate void NfcErrorHandler(object sender, NfcErrorEventArgs e);

        public event DiscoverNfcFTagEventHandler DiscoverNfcFTag;

        public event DiscoverNfcATagEventHandler DiscoverNfcATag;

        public event DiscoverNfcBTagEventHandler DiscoverNfcBTag;

        public event NfcErrorHandler NfcError;

        public NFCForm()
        {
            InitializeComponent();
        }

        private void NFCForm_Load(object sender, EventArgs e)
        {
            // デザインモードではNFCを動作させない
            if (DesignMode)
                return;

            string configPath = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            Console.WriteLine("PATH");
            Console.WriteLine(configPath);
            Console.WriteLine(configPath);
            Console.WriteLine(configPath);
            Console.WriteLine(configPath);

            if (Settings.Default.EnableNativeNfcLog)
            {
                string nativeNfcLog = Settings.Default.NativeNfcLog;

                Debug.WriteLine("Enable Nfc Log");
                string fileName = Directory.GetCurrentDirectory() + @"\" + nativeNfcLog;
                FeliCaNfcDllWrapperClass.FeliCaLibNfcStartLogging(fileName);
            }

            Debug.WriteLine("RegisterWindowMessage, message=" + MSG_STR_OF_FIND);
            cardFindMessage = RegisterWindowMessage(MSG_STR_OF_FIND);
            if (cardFindMessage == 0)
            {
                Console.Error.WriteLine("Failed to register window message, param=" + MSG_STR_OF_FIND);
                return;
            }

            Debug.WriteLine("RegisterWindowMessage, message=" + MSG_STR_OF_ENABLE);
            cardEnableMessage = RegisterWindowMessage(MSG_STR_OF_ENABLE);
            if (cardEnableMessage == 0)
            {
                Console.Error.WriteLine("Failed to register window message, param=" + MSG_STR_OF_ENABLE);
                return;
            }

            bool result;

            Debug.WriteLine("initialize nfc library");
            result = FeliCaNfcDllWrapperClass.FeliCaLibNfcInitialize();
            if (!result)
            {
                Console.Error.WriteLine("Failed to nfc initialize library");
                DispatchErrorEvent();
                DisposeNfc();
                return;
            }

            StringBuilder portName = new StringBuilder("USB0");
            Debug.WriteLine("open nfc usb port, port name=" + portName.ToString());
            result = FeliCaNfcDllWrapperClass.FeliCaLibNfcOpen(portName);
            if (!result)
            {
                Console.Error.WriteLine("Failed to nfc open, port_name=" + portName);
                DispatchErrorEvent();
                DisposeNfc();
                return;
            }

            Debug.WriteLine("set poll callback parameters, msf_str_of_find=" + MSG_STR_OF_FIND + ", msg_str_of_enable=" + MSG_STR_OF_ENABLE);
            result = FeliCaNfcDllWrapperClass.FeliCaLibNfcSetPollCallbackParameters(this.Handle, MSG_STR_OF_FIND, MSG_STR_OF_ENABLE);
            if (!result)
            {
                Console.Error.WriteLine("Failed to set poll callback parameters, msf_str_of_find=" + MSG_STR_OF_FIND + ", msg_str_of_enable=" + MSG_STR_OF_ENABLE);
                DispatchErrorEvent();
                DisposeNfc();
                return;
            }
        }

        protected void StartNfcPolling(bool enableNfcA, bool enableNfcB, bool enableNfcF)
        {
            Debug.WriteLine("Start Polling");

            UInt32 targetDevice = 0;
            if (enableNfcA)
            {
                Debug.WriteLine("Enable 14443 Type A");
                targetDevice = targetDevice | DEVICE_TYPE_NFC_14443A_18092_106K;
            }
            if (enableNfcB)
            {
                Debug.WriteLine("Enable 14443 Type B");
                targetDevice = targetDevice | DEVICE_TYPE_NFC_14443B_106K;
            }
            if (enableNfcF)
            {
                Debug.WriteLine("Enable FeliCa 212K");
                targetDevice = targetDevice | DEVICE_TYPE_NFC_18092_212K;
                Debug.WriteLine("Enable FeliCa 424K");
                targetDevice = targetDevice | DEVICE_TYPE_NFC_18092_424K;
            }

            bool result = FeliCaNfcDllWrapperClass.FeliCaLibNfcStartPollMode(targetDevice);
            if (!result)
            {
                DispatchErrorEvent();
                DisposeNfc();
                return;
            }
        }

        protected void StopNfcPolling()
        {
            Debug.WriteLine("Stop Polling");

            bool result = FeliCaNfcDllWrapperClass.FeliCaLibNfcStopPollMode();
            if (!result)
            {
                DispatchErrorEvent();
                DisposeNfc();
                return;
            }
        }

        private void NFCReaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeNfc();

            if (Settings.Default.EnableNativeNfcLog)
                FeliCaNfcDllWrapperClass.FeliCaLibNfcStopLogging();
        }

        private void DispatchErrorEvent()
        {
            UInt32[] errorInfo = new UInt32[2] { 0, 0 };
            FeliCaNfcDllWrapperClass.FeliCaLibNfcGetLastError(errorInfo);
            Console.Error.WriteLine("Last error");
            Console.Error.WriteLine(errorInfo[0]);
            Console.Error.WriteLine(errorInfo[1]);

            NfcError(this, new NfcErrorEventArgs(errorInfo));
        }

        private void DisposeNfc()
        {
            FeliCaNfcDllWrapperClass.FeliCaLibNfcStopPollMode();
            FeliCaNfcDllWrapperClass.FeliCaLibNfcClose();
            FeliCaNfcDllWrapperClass.FeliCaLibNfcUninitialize();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == cardFindMessage)
            {
                OnReceiveFoundCardMessage(ref m);
            }
            else if (m.Msg == cardEnableMessage)
            {
                OnReceiveCardEnabledMessage(ref m);
            }

            base.WndProc(ref m);
            return;
        }

        protected virtual void OnReceiveFoundCardMessage(ref Message m)
        {
            IntPtr pDevInfo = m.LParam;
            IntPtr pDeviceData = ParseDeviceData(pDevInfo);
            DEVICE_INFO dev_info = (DEVICE_INFO)Marshal.PtrToStructure(pDevInfo, typeof(DEVICE_INFO));

            switch (dev_info.target_device)
            {
                case DEVICE_TYPE_NFC_18092_212K:
                    DispatchNfcFEvent(pDeviceData);
                    break;
                case DEVICE_TYPE_NFC_18092_424K:
                    DispatchNfcFEvent(pDeviceData);
                    break;
                case DEVICE_TYPE_NFC_14443A_18092_106K:
                    DispatchNfcAEvent(pDeviceData);
                    break;
                case DEVICE_TYPE_NFC_14443B_106K:
                    DispatchNfcBEvent(pDeviceData);
                    break;
            }

            FeliCaNfcDllWrapperClass.FeliCaLibNfcStopDevAccess(0x00);
        }

        private IntPtr ParseDeviceData(IntPtr devInfo)
        {
            if (IntPtr.Size == 8)
            {
                return (IntPtr)((Int64)devInfo + (Int64)Marshal.OffsetOf(typeof(DEVICE_INFO), "dev_info"));
            }
            else
            {
                return (IntPtr)((Int32)devInfo + (Int32)Marshal.OffsetOf(typeof(DEVICE_INFO), "dev_info"));
            }
        }

        private void DispatchNfcFEvent(IntPtr pDeviceData)
        {
            DEVICE_DATA_NFC_18092_212_424K deviceData_F =
                (DEVICE_DATA_NFC_18092_212_424K)Marshal.PtrToStructure(pDeviceData,
                typeof(DEVICE_DATA_NFC_18092_212_424K));
            DiscoverNfcFTag(this, new NfcFEventArgs(deviceData_F));
        }

        private void DispatchNfcAEvent(IntPtr pDeviceData)
        {
            DEVICE_DATA_NFC_14443A_18092_106K deviceData_A =
                (DEVICE_DATA_NFC_14443A_18092_106K)Marshal.PtrToStructure(pDeviceData,
                typeof(DEVICE_DATA_NFC_14443A_18092_106K));
            DiscoverNfcATag(this, new NfcAEventArgs(deviceData_A));
        }

        private void DispatchNfcBEvent(IntPtr pDeviceData)
        {
            DEVICE_DATA_NFC_14443B_106K deviceData_B =
                (DEVICE_DATA_NFC_14443B_106K)Marshal.PtrToStructure(pDeviceData,
                typeof(DEVICE_DATA_NFC_14443B_106K));
            DiscoverNfcBTag(this, new NfcBEventArgs(deviceData_B));
        }

        protected virtual void OnReceiveCardEnabledMessage(ref Message m)
        {
        }
    }
}
