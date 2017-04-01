using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV
{
    public class BlockDefault : Block
    {
        public BlockDefault(uint _type, uint _id, byte[] _data)
        {
            BlockID = _type;
            ID = _id;
            Data = _data;
        }

        public BlockDefault(Stream s)
        {
            Size = Block.ReadUInt32(s);
            uint size = Size - 1;
            ID = Block.ReadUInt32(s);
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
            WriteUInt32(s, ID);
            s.Write(Data, 0, Data.Length);
        }
    }
}
