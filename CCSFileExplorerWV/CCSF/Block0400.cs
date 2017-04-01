using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CCSFileExplorerWV
{
    /**
     * Palette
     */
    public class Block0400 : Block
    {
        public List<uint> unknown;
        public Block0400(Stream s)
        {
            BlockID = 0xCCCC0400;
            long length = s.Length;
            uint u;
            unknown = new List<uint>();
            while (s.Position < length)
            {
                u = Block.ReadUInt32(s);
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
            TreeNode result = new TreeNode(BlockID.ToString("X8"));
            return result;
        }

        public override void WriteBlock(Stream s)
        {
            WriteUInt32(s, BlockID);
            WriteUInt32(s, (uint)(Data.Length / 4 + 51));
            WriteUInt32(s, ID);
            s.Write(Data, 0, Data.Length);
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
