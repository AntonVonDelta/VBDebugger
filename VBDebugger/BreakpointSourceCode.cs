using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBDebugger {
    class BreakpointSourceCode {
        public string Filepath { get; set; }

        // The line number hard coded in the source code
        public int InCodeBreakpointLineNumber { get; set; }
    }
}
