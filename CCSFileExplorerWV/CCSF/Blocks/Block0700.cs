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
     * ANIMATION
     */
    public class Block0700: Block
    {
        public uint unk1;
        public uint unk2;
        public uint unk3;
        public Block0700(Stream s)
        {
            type = 0xCCCC0700;
            uint size = StreamHelper.ReadUInt32(s) * 4;
            byte[] buff = new byte[size];
            s.Read(buff, 0, (int)size);
            MemoryStream m = new MemoryStream();
            m.Write(buff, 12, buff.Length - 12);
            m.Seek(0, 0);
            subBlocks = Block.ReadBlocks(m);
            unk1 = BitConverter.ToUInt32(buff, 0);
            unk2 = BitConverter.ToUInt32(buff, 4);
            unk3 = BitConverter.ToUInt32(buff, 8);
        }

        public override TreeNode ToNode()
        {
            TreeNode result = new TreeNode(type.ToString("X8") + " @0x" + offset.ToString("X8") + " +");
            result.Nodes.Add("Unknown 1 : 0x" + unk1.ToString("X8"));
            result.Nodes.Add("Unknown 2 : 0x" + unk2.ToString("X8"));
            result.Nodes.Add("Unknown 3 : 0x" + unk3.ToString("X8"));
            return result;
        }
    }
}
