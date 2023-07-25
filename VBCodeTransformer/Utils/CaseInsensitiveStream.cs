using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBCodeTransformer.Utils {
    public class CaseInsensitiveStream : Antlr4.Runtime.AntlrInputStream {

        public CaseInsensitiveStream(string sExpr)
           : base(sExpr) {
        }

        public override int La(int index) {
            if (index == 0) return 0;
            if (index < 0) index++;
            int pdx = p + index - 1;
            if (pdx < 0 || pdx >= n) return TokenConstants.Eof;
            var x1 = data[pdx];
            return char.ToUpper(x1);
        }
    }
}
