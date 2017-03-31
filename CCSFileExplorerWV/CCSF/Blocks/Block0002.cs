using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public class Block0002: Block
    {
        public List<string> filenames;
        public List<string> objnames;
        public Block0002(Stream s)
        {
            type = 0xCCCC0002;
            uint size = StreamHelper.ReadUInt32(s) * 4 + 8;
            byte[] buff = new byte[size];
            s.Read(buff, 0, (int)size);
            uint filecount = BitConverter.ToUInt32(buff, 0) - 1;
            uint objcount = BitConverter.ToUInt32(buff, 4) - 1;
            int pos = 0x28;
            filenames = new List<string>();
            objnames = new List<string>();
            for (int i = 0; i < filecount; i++)
            {
                filenames.Add(StreamHelper.ReadString(buff, pos));
                pos += 0x20;
            }
            pos += 0x20;
            for (int i = 0; i < filecount; i++)
            {
                objnames.Add(StreamHelper.ReadString(buff, pos));
                pos += 0x20;
            }
        }

        public override TreeNode ToNode()
        {
            TreeNode result = new TreeNode(type.ToString("X8") + " @0x" + offset.ToString("X8"));
            TreeNode t1 = new TreeNode("File Names");
            foreach (string name in filenames)
                t1.Nodes.Add(name);
            TreeNode t2 = new TreeNode("Object Names");
            foreach (string name in objnames)
                t2.Nodes.Add(name);
            result.Nodes.Add(t1);
            result.Nodes.Add(t2);
            return result;
        }
    }
}
