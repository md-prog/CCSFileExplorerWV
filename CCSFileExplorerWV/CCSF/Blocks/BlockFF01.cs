using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public class BlockFF01 : Block
    {
        public List<uint> unknown;
        public BlockFF01(Stream s)
        {
            type = 0xCCCCFF01;
            uint size = StreamHelper.ReadUInt32(s) * 4;
            byte[] buff = new byte[size];
            s.Read(buff, 0, (int)size);
            unknown = new List<uint>();
            for (int i = 0; i < size / 4; i++)
                unknown.Add(BitConverter.ToUInt32(buff, i * 4));
        }

        public override TreeNode ToNode()
        {
            TreeNode result = new TreeNode(type.ToString("X8") + " @0x" + offset.ToString("X8"));
            foreach (uint u in unknown)
                result.Nodes.Add("0x" + u.ToString("X8"));
            return result;
        }
    }
}
