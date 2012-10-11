using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDKforNFCWrapperLibrary.FeliCa
{
    public class FeliCaException : Exception
    {
        public FeliCaException()
        {
        }

        public FeliCaException(string message)
            : base(message)
        {
        }
        public FeliCaException(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
