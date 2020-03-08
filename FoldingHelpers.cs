using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScaleTo16x16
{
    class FoldingHelpers
    {
        public static void MoveIfExists(string from, string to)
        {
            if (File.Exists(from)) File.Move(from, to);
        }
    }
}
