using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ScintillaNET;

namespace VBDebugger {
    public static class Util {
        public enum ScintillaConstants {
            VISIBLE_STRICT = 0x04,
            VISIBLE_SLOP = 0x01
        }


        public static void SetVisiblePolicy(this Scintilla control, ScintillaConstants policy, int slop) {
            control.DirectMessage(ScintillaNET.NativeMethods.SCI_SETVISIBLEPOLICY, new IntPtr((int)policy), new IntPtr(7));
        }

        /// <summary>
        /// Apply visible policy
        /// </summary>
        public static void EnsureLineVisible(this Scintilla control, int line) {
            control.DirectMessage(ScintillaNET.NativeMethods.SCI_ENSUREVISIBLEENFORCEPOLICY, new IntPtr(line), IntPtr.Zero);
        }
    }
}
