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
     * TOC Block
     * 
     * 0x00 DWORD 0xCCCC0002
     * 0x04 DWORD SIZE IN DWORD, SO IN BYTES IT WILL BE * 4
     * 0x08 DWORD FILE_COUNT : SIZE MEASURE STARTS HERE
     * 0x12 DWORD OBJECT_COUNT : SIZE MEASURE STARTS HERE
     * 0x16 DWORD DATA
     * ...
     * 0xXX DWORD ANOTHER BLOCK STARTS
     */
    public class Block0002 : Block
    {
        public uint FileCount;
        public uint ObjCount;
        public List<string> filenames;
        public List<string> objnames;
        public List<ushort> indexes;

        override public byte[] FullBlockData
        {
            get
            {
                MemoryStream m = new MemoryStream();
                m.Write(BitConverter.GetBytes(BlockID), 0, 4);
                m.Write(BitConverter.GetBytes(Size), 0, 4);
                m.Write(BitConverter.GetBytes(FileCount + 1), 0, 4);
                m.Write(BitConverter.GetBytes(ObjCount + 1), 0, 4);
                m.Write(Data, 0, Data.Length);
                return m.ToArray();
            }
        }

        public Block0002(Stream s)
        {
            ID = 0xFFFFFFFF;
            Size = Block.ReadUInt32(s);
            FileCount = Block.ReadUInt32(s) - 1;
            ObjCount = Block.ReadUInt32(s) - 1;

            uint size = Size;
            Data = new byte[size * 4];
            s.Read(Data, 0, (int)(size * 4));
            
            int pos = 0x20; // 32 bytes - 8 words 0 padding
            filenames = new List<string>();
            for (int i = 0; i < FileCount; i++)
            {
                filenames.Add(ReadString(Data, pos));
                pos += 0x20;
            }
            pos += 0x20; // 32 bytes - 8 words 0 padding
            objnames = new List<string>();
            indexes = new List<ushort>();
            for (int i = 0; i < ObjCount; i++)
            {
                objnames.Add(ReadString(Data, pos));
                indexes.Add(BitConverter.ToUInt16(Data, pos + 0x1E));
                pos += 0x20;
            }
        }

        public override TreeNode ToNode()
        {
            return new TreeNode(BlockID.ToString("X8") + "ID:0x" + ID.ToString("X") + " Size: 0x" + Data.Length.ToString("X"));
        }

        public override void WriteBlock(Stream s)
        {
            WriteUInt32(s, BlockID);
            MemoryStream m = new MemoryStream();
            m.Write(new byte[0x20], 0, 0x20);
            foreach (string name in filenames)
                WriteString(m, name, 0x20);
            m.Write(new byte[0x20], 0, 0x20);
            for (int i = 0; i < ObjCount; i++)
            {
                WriteString(m, objnames[i], 0x1E);
                m.Write(BitConverter.GetBytes(indexes[i]), 0, 2);
            }
            WriteUInt32(m, 3);
            WriteUInt32(m, 0);
            WriteUInt32(s, (uint)(m.Length / 4));
            WriteUInt32(s, FileCount + 1);
            WriteUInt32(s, ObjCount + 1);
            s.Write(m.ToArray(), 0, (int)m.Length);
        }
    }
}
