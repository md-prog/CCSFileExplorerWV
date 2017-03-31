using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public static class StreamHelper
    {

        public static uint ReadUInt32(Stream s)
        {
            byte[] buff = new byte[4];
            s.Read(buff, 0, 4);
            return BitConverter.ToUInt32(buff, 0);
        }

        public static string ReadString(byte[] array, int pos)
        {
            string result = "";
            while (array[pos] != 0)
                result += (char)array[pos++];
            return result;
        }
    }

}
