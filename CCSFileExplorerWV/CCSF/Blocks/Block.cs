using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public abstract class Block
    {
        public List<Block> subBlocks = new List<Block>();
        public uint type = 0;
        public uint offset = 0;
        public abstract TreeNode ToNode();

        public static List<Block> ReadBlocks(Stream s)
        {
            List<Block> result = new List<Block>(); 
            byte[] buff = new byte[4];
            bool error = false;
            uint pos;
            while (s.Position < s.Length)
            {
                pos = (uint)s.Position;
                s.Read(buff, 0, 4);
                uint type = BitConverter.ToUInt32(buff, 0);
                Block b = null;
                switch (type)
                {
                    case 0xCCCC0001:
                        b = new Block0001(s);
                        break;
                    case 0xCCCC0002:
                        b = new Block0002(s);
                        break;
                    case 0xCCCC0005:
                        b = new Block0005(s);
                        break;
                    case 0xCCCC0100:
                        b = new Block0100(s);
                        break;
                    case 0xCCCC0102:
                        b = new Block0102(s);
                        break;
                    case 0xCCCC0108:
                        b = new Block0108(s);
                        break;
                    case 0xCCCC0200:
                        b = new Block0200(s);
                        break;
                    case 0xCCCC0300:
                        b = new Block0300(s);
                        break;
                    case 0xCCCC0400:
                        b = new Block0400(s);
                        break;
                    case 0xCCCC0500:
                        b = new Block0500(s);
                        break;
                    case 0xCCCC0502:
                        b = new Block0502(s);
                        break;
                    case 0xCCCC0600:
                        b = new Block0600(s);
                        break;
                    case 0xCCCC0601:
                        b = new Block0601(s);
                        break;
                    case 0xCCCC0603:
                        b = new Block0603(s);
                        break;
                    case 0xCCCC0609:
                        b = new Block0609(s);
                        break;
                    case 0xCCCC0700:
                        b = new Block0700(s);
                        break;
                    case 0xCCCC0800:
                        b = new Block0800(s);
                        break;
                    case 0xCCCC0900:
                        b = new Block0900(s);
                        break;
                    case 0xCCCC0A00:
                        b = new Block0A00(s);
                        break;
                    case 0xCCCC0B00:
                        b = new Block0B00(s);
                        break;
                    case 0xCCCC0C00:
                        b = new Block0C00(s);
                        break;
                    case 0xCCCC0E00:
                        b = new Block0E00(s);
                        break;
                    case 0xCCCC1100:
                        b = new Block1100(s);
                        break;
                    case 0xCCCC1200:
                        b = new Block1200(s);
                        break;
                    case 0xCCCC1300:
                        b = new Block1300(s);
                        break;
                    case 0xCCCC1400:
                        b = new Block1400(s);
                        break;
                    case 0xCCCC1900:
                        b = new Block1900(s);
                        break;
                    case 0xCCCC1901:
                        b = new Block1901(s);
                        break;
                    case 0xCCCC2000:
                        b = new Block2000(s);
                        break;
                    case 0xCCCCFF01:
                        b = new BlockFF01(s);
                        break;
                    default:
                        error = true;
                        b = new ErrorBlock("Error at 0x" + s.Position.ToString("X8") + " read type:0x" + type.ToString("X8"));
                        break;
                }
                b.offset = pos;
                result.Add(b);
                if (error)
                    break;
            }
            return result;
        }

        public static uint[] validBlockTypes = new uint[] { 
            0xCCCC0001, 0xCCCC0002, 0xCCCC0005, 0xCCCC0100,
            0xCCCC0102, 0xCCCC0108, 0xCCCC0200, 0xCCCC0300,
            0xCCCC0400, 0xCCCC0500, 0xCCCC0502, 0xCCCC0600,
            0xCCCC0601, 0xCCCC0603, 0xCCCC0609, 0xCCCC0700,
            0xCCCC0800, 0xCCCC0900, 0xCCCC0A00, 0xCCCC0B00,
            0xCCCC0C00, 0xCCCC0E00, 0xCCCC1100, 0xCCCC1200,
            0xCCCC1300, 0xCCCC1400, 0xCCCC1900, 0xCCCC1901,
            0xCCCC2000, 0xCCCCFF01
        };

        public static bool isValidBlockType(uint u)
        {
            foreach (uint vu in validBlockTypes)
                if (u == vu)
                    return true;
            return false;
        }
    }
}




