using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCSFileExplorerWV.CCSF.Blocks
{
    public class ErrorBlock : Block
    {
        public string error;

        public ErrorBlock(string message)
        {
            error = message;
        }

        public override TreeNode ToNode()
        {
            TreeNode result = new TreeNode(error);
            return result;
        }
    }
}
