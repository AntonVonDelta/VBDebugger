using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VBDebugger {
    public static class Util {
        public static void HighlightLine(this RichTextBox richTextBox, int index, Color color) {
            richTextBox.WordWrap = false;

            var currentLine = richTextBox.GetCurentLine();
            int firstVisibleChar = richTextBox.GetCharIndexFromPosition(new Point(0, 1));
            int firstVisibleLineIndex = richTextBox.GetLineFromCharIndex(firstVisibleChar);
            int lastVisibleChar = richTextBox.GetCharIndexFromPosition(new Point(0, richTextBox.ClientSize.Height));
            int lastVisibleLineIndex = richTextBox.GetLineFromCharIndex(lastVisibleChar);

            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = richTextBox.BackColor;

            var lines = richTextBox.Lines;
            if (index < 0 || index >= lines.Length)
                return;
            var start = richTextBox.GetFirstCharIndexFromLine(index);  // Get the 1st char index of the appended text
            var length = lines[index].Length;

            richTextBox.Select(start, length);                 // Select from there to the end
            richTextBox.SelectionBackColor = color;


            if (index >= firstVisibleLineIndex && index <= lastVisibleLineIndex) {
                richTextBox.Select(firstVisibleChar, 0);
                richTextBox.ScrollToCaret();
            }
        }

        public static void ScrollToLine(this RichTextBox richTextBox, int index) {
            var start = richTextBox.GetFirstCharIndexFromLine(index);

            richTextBox.Select(start, 0);
            richTextBox.ScrollToCaret();
        }

        public static int GetCurentLine(this RichTextBox richTextBox) {
            int cursorPosition = richTextBox.SelectionStart;

            return richTextBox.GetLineFromCharIndex(cursorPosition);
        }
    }
}
