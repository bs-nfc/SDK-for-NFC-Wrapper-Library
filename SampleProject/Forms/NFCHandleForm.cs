using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDKforNFCWrapperLibrary.Forms;
using SampleProject.Utils;
using System.Diagnostics;
using SDKforNFCWrapperLibrary.FeliCa;
using SDKforNFCWrapperLibrary.FeliCa.Command;
using SDKforNFCWrapperLibrary.FeliCa.Response;

namespace SampleProject.Forms
{
    public partial class NFCHandleForm : NFCForm
    {
        public NFCHandleForm()
        {
            InitializeComponent();
        }

        private void NFCHandleForm_Shown(object sender, EventArgs e)
        {
            StartNfcPolling(false, false, true);
        }

        private void NFCHandleForm_DiscoverNfcFTag(object sender, NfcFEventArgs e)
        {
            Debug.WriteLine("Discover Tag");

            FeliCaLibWrapper felicaLib = new FeliCaLibWrapper();
            PollingCommand command = new PollingCommand(PollingCommand.SYSTEM_CODE_WILDCARD, PollingCommand.REQUEST_CODE_REQUEST_SYSTEM_CODE);
            try
            {
                CommandResponse response = felicaLib.PerformCommand(command);
                PollingCommnadResponse pollingResponse = new PollingCommnadResponse(response);
                pollingResponse.Dump();
            }
            catch (FeliCaException)
            {
                DispatchErrorEvent();
                Close();
            }
        }

        private void NFCHandleForm_NfcError(object sender, NfcErrorEventArgs e)
        {
            string errorNum1 = Convert.ToString(e.Error[0], 16);
            string errorNum2 = Convert.ToString(e.Error[1], 16);
            MessageBox.Show(this, "Error(" + errorNum1 + "," + errorNum2 + ")");

            Close();
        }
    }
}
