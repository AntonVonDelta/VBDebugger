using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBCodeTransformer.Parsers;
using VBCodeTransformer.Utils;

namespace VBCodeTransformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private VisualBasic6Parser.StartRuleContext GetTree(string input)
        {
            var charStream = new CaseInsensitiveStream(input);
            var lexer = new VisualBasic6Lexer(charStream);
            CommonTokenStream tokenStream;
            VisualBasic6Parser parser;

            tokenStream = new CommonTokenStream(lexer);
            parser = new VisualBasic6Parser(tokenStream);

            return parser.startRule();
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            var input = @"
                dim a as string

                sub main()

                end sub

                sub test(a as string)
                        MsgBox ""da""
                end sub
                function test2() as Integer

                end function
";
            var charStream = new CaseInsensitiveStream(input);
            var lexer = new VisualBasic6Lexer(charStream);
            CommonTokenStream tokenStream;
            VisualBasic6Parser parser;
            VisualBasic6Parser.StartRuleContext tree;

            tokenStream = new CommonTokenStream(lexer);
            parser = new VisualBasic6Parser(tokenStream);
            tree = parser.startRule();

            TraceVisitor traceVisitor = new TraceVisitor("Form1.frm", tokenStream);
            traceVisitor.Visit(tree);

            richTextBox1.Text = traceVisitor.GetNewSourceCode();
        }
    }
}
