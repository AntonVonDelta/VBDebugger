using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private string _loadedFilePath = null;
        private string _loadedFileSource = "";

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
            if (_loadedFilePath == null) return;

            var input = _loadedFileSource;
            var charStream = new CaseInsensitiveStream(input);
            var lexer = new VisualBasic6Lexer(charStream);
            CommonTokenStream tokenStream;
            VisualBasic6Parser parser;
            VisualBasic6Parser.StartRuleContext tree;

            tokenStream = new CommonTokenStream(lexer);
            parser = new VisualBasic6Parser(tokenStream);
            tree = parser.startRule();

            TraceVisitor traceVisitor = new TraceVisitor(Path.GetFileName(_loadedFilePath), tokenStream);
            traceVisitor.Visit(tree);

            richTextBox1.Text = traceVisitor.GetNewSourceCode();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;

                _loadedFilePath = filePath;

                using (var stream = new StreamReader(filePath))
                {
                    _loadedFileSource = stream.ReadToEnd();

                    richTextBox1.Text = _loadedFileSource;
                }
            }
        }
    }
}
