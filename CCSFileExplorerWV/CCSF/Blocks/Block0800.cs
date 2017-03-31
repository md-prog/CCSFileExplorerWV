using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CCSFileExplorerWV.CCSF.Blocks
{
    public class Block0800 : Block
    {
        public List<uint> unknown;
        public Block0800(Stream s)
        {
            type = 0xCCCC0800;
            long length = s.Length;
            uint u;
            unknown = new List<uint>();
            while (s.Position < length)
            {
                u = StreamHelper.ReadUInt32(s);
                if (!Block.isValidBlockType(u))
                    unknown.Add(u);
                else
                {
                    s.Seek(-4, SeekOrigin.Current);
                    break;
                }
            }
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
