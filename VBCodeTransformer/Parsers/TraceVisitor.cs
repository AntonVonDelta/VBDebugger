using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VBCodeTransformer.Parsers.Models;

namespace VBCodeTransformer.Parsers {
    public class TraceVisitor : VisualBasic6ParserBaseVisitor<object> {
        private readonly string _filename;
        private readonly TokenStreamRewriter _rewriter;

        // Stores the line number where the source code really starts, excluding vb attributes
        // First line at index 0
        private int _sourceCodeStartingLine = -1;


        public TraceVisitor(string filename, CommonTokenStream tokens) {
            _filename = filename;
            _rewriter = new TokenStreamRewriter(tokens);
        }

        public string GetNewSourceCode() {
            var sourceCode = _rewriter.GetText();
            var lines = Regex.Matches(sourceCode, @"^(.*?)\r?$", RegexOptions.Multiline);
            var result = "";

            for (int i = 0; i < lines.Count; i++) {
                var line = lines[i].Groups[1].Value;

                if (i < _sourceCodeStartingLine) {
                    result += $"{line}\r\n";
                    continue;
                }

                result += line.Replace(CodeTemplates.LineMacro, $"{i - _sourceCodeStartingLine + 1}");
                result += "\r\n";
            }

            return result;
        }



        public override object VisitModuleOptions([NotNull] VisualBasic6Parser.ModuleOptionsContext context) {
            _sourceCodeStartingLine = context.Start.Line - 1;

            return base.VisitModuleOptions(context);
        }

        public override object VisitModuleBody([NotNull] VisualBasic6Parser.ModuleBodyContext context) {
            if (_sourceCodeStartingLine == -1)
                _sourceCodeStartingLine = context.Start.Line - 1;

            return base.VisitModuleBody(context);
        }

        public override object VisitSubStmt([NotNull] VisualBasic6Parser.SubStmtContext context) {
            var scopeName = context.ambiguousIdentifier().GetText();
            var scopeStartColumn = context.Start.Column;
            var printableArguments = Util.GetPrintableProcedureArguments(context.argList());
            var serializedProcedureArguments = Util.PrepareSerializedVariables(printableArguments);

            var newPreambleCode = CodeTemplates.GetFunctionPreamble(_filename, scopeName, serializedProcedureArguments, scopeStartColumn);
            var newPostambleCode = CodeTemplates.GetFunctionPostamble(_filename, scopeName, scopeStartColumn);
            var procedureVisitor = new ProcedureVisitor(_filename, _sourceCodeStartingLine, printableArguments, _rewriter);

            _rewriter.InsertAfter(context.argList().Stop, $"\r\n{newPreambleCode}");
            _rewriter.InsertBefore(context.END_SUB().Symbol, $"{newPostambleCode}");

            procedureVisitor.VisitSubStmt(context);

            return base.VisitSubStmt(context);
        }
    }
}
