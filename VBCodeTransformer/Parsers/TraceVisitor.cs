using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBCodeTransformer.Parsers
{
    public class TraceVisitor : VisualBasic6ParserBaseVisitor<object>
    {
        private readonly string _filename;
        private readonly TokenStreamRewriter _rewriter;

        public TraceVisitor(string filename, CommonTokenStream tokens)
        {
            _filename = filename;
            _rewriter = new TokenStreamRewriter(tokens);
        }

        public string GetNewSourceCode()
        {
            return _rewriter.GetText();
        }

        public override object VisitSubStmt([NotNull] VisualBasic6Parser.SubStmtContext context)
        {
            var scopeName = context.ambiguousIdentifier().GetText();
            var scopeStartLine = context.Start.Line;
            var scopeStartColumn = context.Start.Column;
            var newPreambleCode = CodeTemplates.GetFunctionPreamble(_filename, scopeName, scopeStartLine, scopeStartColumn);
            var newPostambleCode = CodeTemplates.GetFunctionPostamble(_filename, scopeName, scopeStartLine, scopeStartColumn);

            _rewriter.InsertAfter(context.argList().Stop, $"\r\n{newPreambleCode}");
            _rewriter.InsertBefore(context.END_SUB().Symbol, $"{newPostambleCode}");

            return base.VisitSubStmt(context);
        }

        public override object VisitEndStmt([NotNull] VisualBasic6Parser.EndStmtContext context)
        {
            return base.VisitEndStmt(context);
        }
    }
}
