using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.DLT645.AddIn
{
    public enum DataType
    {
        None = 0,
        UInt8 = 1,
        UInt16 = 2,
        UInt24 = 3,
        UInt32 = 4,
        UInt48 = 5,
        UInt64 = 6,
        Int8 = 7,
        Int16 = 8,
        Int32 = 9,
        Int64 = 10,
        String = 11,
        Date = 12,
        Time = 13,
        DateTime = 14,
        BCD = 15
    }
}
