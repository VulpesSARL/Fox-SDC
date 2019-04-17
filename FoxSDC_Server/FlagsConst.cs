using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    public class Consts
    {
        public const Int64 MaxFilesize = 0x1900000000;
        public const Int64 MaxFileChunk = 0xA00000;
        public const string NullGUID = "00000000-0000-0000-0000-000000000000";
        public const string NullUCID = "00000000000000000000000000000000";
    }

    public class FlagsConst
    {
        public const int Document_Type_Package = 0x0;
        public const int Document_Type_Max = 0x0;
    }
}
