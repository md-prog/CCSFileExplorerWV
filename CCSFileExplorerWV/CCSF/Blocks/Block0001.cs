using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public class Block0001 : Block
    {
        public string name;
        public List<uint> unknown;
        public Block0001(Stream s)
        {
            type = 0xCCCC0001;
            uint size = StreamHelper.ReadUInt32(s) * 4;
            byte[] buff = new byte[size];
            s.Read(buff, 0, (int)size);
            int pos = 0;
            name = "";
            while (buff[pos] != 0)
                name += (char)buff[pos++];
            unknown = new List<uint>();
            for (int i = 0; i < (size - 0x20) / 4; i++)
                unknown.Add(BitConverter.ToUInt32(buff, 0x20 + i * 4));
        }

        public override TreeNode ToNode()
        {
            TreeNode result = new TreeNode(type.ToString("X8") + " @0x" + offset.ToString("X8"));
            result.Nodes.Add(name);
            foreach (uint u in unknown)
                result.Nodes.Add("0x" + u.ToString("X8"));
            return result;
        }
    }
}
