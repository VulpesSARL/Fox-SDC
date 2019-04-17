using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    /*

    WARNING: this file is extremly sensitive, and even a slight change can break EVERYTHING!

    */

    public class PushData
    {
        public string Action;
        public object AdditionalDataO;
        public string AdditionalData1;
        public string AdditionalData2;
        public Int64 AdditionalData3;
        public DateTime TimeStampCheck;
        public string ReplyID;
    }

    public class PushDataRoot
    {
        public PushData Data;
        public byte[] Signature;
    }

    public class PushDataResponse
    {
        public Int64 Channel;
        public string MachineID;
        public string ReplyID;
        public object Data;
        public DateTime TimeStampCheck;
        public string Action;
    }
}
