using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV
{
    public class Block0005 : Block
    {
        override public byte[] FullBlockData
        {
            get
            {
                MemoryStream m = new MemoryStream();
                m.Write(BitConverter.GetBytes(BlockID), 0, 4);
                m.Write(BitConverter.GetBytes(Size), 0, 4);
                m.Write(Data, 0, Data.Length);
                return m.ToArray();
            }
        }

        public Block0005(Stream s)
        {
            Size = Block.ReadUInt32(s);
            uint size = Size;
            ID = 0xFFFFFFFF;
            Data = new byte[size * 4];
            s.Read(Data, 0, (int)(size * 4));
        }

        public override TreeNode ToNode()
        {
            return new TreeNode(BlockID.ToString("X8") + "ID:0x" + ID.ToString("X") + " Size: 0x" + Data.Length.ToString("X"));
        }

        public override void WriteBlock(Stream s)
        {
            WriteUInt32(s, BlockID);
            WriteUInt32(s, (uint)(Data.Length / 4 + 1));
            WriteUInt32(s, 1);
            s.Write(Data, 0, Data.Length);
        }
    }
}
