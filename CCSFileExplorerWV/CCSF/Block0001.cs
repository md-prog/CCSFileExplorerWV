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
     * Header Block
     * 
     * 0x00 DWORD 0xCCCC0001
     * 0x04 DWORD SIZE IN DWORD, SO IN BYTES IT WILL BE * 4
     * 0x08 DWORD BLOCK_ID : SIZE MEASURE STARTS HERE
     * 0x12 DWORD DATA
     * ...
     * 0xXX DWORD ANOTHER BLOCK STARTS
     */ 
    public class Block0001 : Block
    {
        public string Name;
        public List<uint> Unknown;

        public Block0001(Stream s)
        {
            Size = Block.ReadUInt32(s);
            ID = Block.ReadUInt32(s); // CCSF: 0x43 0x43 0x53 0x46: 0x46534343

            uint size = Size - 1; // Size includes ID field
            Data = new byte[size * 4];
            s.Read(Data, 0, (int)(size * 4));
            Name = ReadFixedLenString(Data, 0, 0x20);

            Unknown = new List<uint>();
            for(var i=0;i< size - 8;i++)
            {
                Unknown.Add(BitConverter.ToUInt32(Data, i * 4 + 0x20));
            }
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
