using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    public class SiemensPLC : IPLC
    {
        Plc plc;
        public Plc Plc(CpuType cpu, string ip, short rack, short slot)
        {
            return new Plc(cpu, ip, rack, slot);
        }

        public void Open()
        {
            plc.Open();
        }

        public void close()
        {
            plc.Close();
        }

        public byte[] ReadBytes(DataType dataType, int db, int startByteAdr, int count)
        {
            return plc.ReadBytes(dataType, db, startByteAdr, count);
        }

        public void WriteBytes(DataType dataType, int db, int startByteAdr, byte[] value)
        {
            plc.WriteBytes(dataType, db, startByteAdr, value);  
        }
    }
}
