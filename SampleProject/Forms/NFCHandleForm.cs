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
            string nfcId = Util.ToHexString(e.DeviceDataF.NFCID2);
            MessageBox.Show(this, nfcId, "Discover NFC Tag");
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
