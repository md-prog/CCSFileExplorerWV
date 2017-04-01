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
     * Image
     */
    public class Block0300 : Block
    {
        public Block0300(Stream s)
        {
            Size = Block.ReadUInt32(s);
            ID = Block.ReadUInt32(s);

            uint size = Size - 1;

            MemoryStream m = new MemoryStream();
            uint u = 0, l = 0;
            while (!isValidBlockType(u = ReadUInt32(s)) && l++ < size)
            {
                m.Write(BitConverter.GetBytes(u), 0, 4);
            }
            if (isValidBlockType(u))
                s.Seek(-4, SeekOrigin.Current);

            Data = m.ToArray();
        }

        public override TreeNode ToNode()
        {
            return new TreeNode(BlockID.ToString("X8") + "ID:0x" + ID.ToString("X") + " Size: 0x" + Data.Length.ToString("X"));
        }

        public override void WriteBlock(Stream s)
        {
            WriteUInt32(s, BlockID);
            WriteUInt32(s, (uint)(Data.Length / 4 + 51));
            WriteUInt32(s, ID);
            s.Write(Data, 0, Data.Length);
        }
    }
}
