using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSFileExplorerWV.CCSF
{
    public class VertexConnection
    {
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public byte Connect { get; set; }

        public VertexConnection(byte[] b)
        {
            Unknown1 = b[0];
            Unknown2 = b[1];
            Unknown3 = b[2];
            Connect = b[3];
        }

        public VertexConnection(uint b)
        {
            Unknown1 = (byte) (0xFF & b);
            b = b >> 8;
            Unknown2 = (byte)(0xFF & b);
            b = b >> 8;
            Unknown3 = (byte)(0xFF & b);
            b = b >> 8;
            Connect = (byte)(0xFF & b);
        }
    }
}
