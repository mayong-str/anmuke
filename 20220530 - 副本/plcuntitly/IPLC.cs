using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    internal interface IPLC
    {
        Plc Plc(CpuType cpu, string ip, Int16 rack, Int16 slot);
        void Open();
         byte[] ReadBytes(DataType dataType, int db, int startByteAdr, int count);
        void WriteBytes(DataType dataType, int db, int startByteAdr, byte[] value);
        void close();
    }
}
