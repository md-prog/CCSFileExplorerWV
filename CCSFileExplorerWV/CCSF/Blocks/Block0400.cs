using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CCSFileExplorerWV.CCSF.Blocks
{
    /**
     * Palette
     */
    public class Block0400 : Block
    {
        public List<uint> unknown;
        public Block0400(Stream s)
        {
            uint type = 0xCCCC0400;
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
            TreeNode result = new TreeNode(type.ToString("X8"));
            return result;
        }
    }

    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}
