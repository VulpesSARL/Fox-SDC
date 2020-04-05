using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class EFIBios
    {
        public static NetDictIntString GetEFIBootDevices()
        {
            NetDictIntString dict = new NetDictIntString();

            if (ProgramAgent.CPP.GetEFIBootDevices(out dict.Dict) == false)
                dict.Dict = null;

            return (dict);
        }

        public static NetBool SetNextEFIBootDevice(string ID)
        {
            NetBool b = new NetBool();
            b.Data = false;

            int IDi;
            if (int.TryParse(ID, out IDi) == false)
                return (b);

            b.Data = ProgramAgent.CPP.SetEFINextBootDevice(IDi);

            return (b);
        }
    }
}
