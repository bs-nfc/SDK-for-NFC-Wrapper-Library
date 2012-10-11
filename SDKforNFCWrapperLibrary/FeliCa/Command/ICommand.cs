using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDKforNFCWrapperLibrary.FeliCa.Command
{
    public interface ICommand
    {
        byte[] ToByteArray();
        void Dump();
    }
}
