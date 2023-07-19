using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VBCodeTransformer.Parsers
{
    public class TraceVisitor : VisualBasic6ParserBaseVisitor<object>
    {
        class VariableModel
        {
            public string Name { get; set; }
            public string Type { get; set; }
        }

        private List<string> _printableVariableTypes = new List<string>{
            "string", "Integer", "Long", "Boolean" };
        private readonly string _filename;
        private readonly TokenStreamRewriter _rewriter;

        // Stores the line number where the source code really starts, excluding vb attributes
        // First line at index 0
        private int _sourceCodeStartingLine = -1;


        public TraceVisitor(string filename, CommonTokenStream tokens)
        {
            _filename = filename;
            _rewriter = new TokenStreamRewriter(tokens);
        }

        public string GetNewSourceCode()
        {
            var sourceCode = _rewriter.GetText();
            var lines = Regex.Matches(sourceCode, @"^(.*?)\r?$", RegexOptions.Multiline);
            var result = "";

            for (int i = _sourceCodeStartingLine; i < lines.Count; i++)
            {
                var line = lines[i].Groups[1].Value;

                result += line.Replace(CodeTemplates.LineMacro, $"{i - _sourceCodeStartingLine + 1}");
                result += "\r\n";
            }

            return result;
        }

        private List<VariableModel> GetPrintableProcedureArguments(VisualBasic6Parser.ArgListContext argListContext)
        {
            var result = GetProcedureArguments(argListContext);

            return result.Where(el => _printableVariableTypes.Contains(el.Type)).ToList();
        }

        private List<VariableModel> GetProcedureArguments(VisualBasic6Parser.ArgListContext argListContext)
        {
            var result = new List<VariableModel>();

            foreach (var arg in argListContext.arg())
            {
                VariableModel variable;
                if (arg.asTypeClause() == null) continue;
                if (arg.asTypeClause().type_().baseType() == null) continue;

                variable = new VariableModel()
                {
                    Name = arg.ambiguousIdentifier().GetText(),
                    Type = arg.asTypeClause().type_().baseType().GetText().ToLower()
                };

                result.Add(variable);
            }

            return result;
        }

        private string PrepareSerializedVariables(List<VariableModel> variables)
        {
            return string.Join(",", variables.Select(el => $"\"{el.Name}\", {el.Name}"));
        }

        public override object VisitModuleOptions([NotNull] VisualBasic6Parser.ModuleOptionsContext context)
        {
            _sourceCodeStartingLine = context.Start.Line - 1;

            return base.VisitModuleOptions(context);
        }

        public override object VisitModuleBody([NotNull] VisualBasic6Parser.ModuleBodyContext context)
        {
            if (_sourceCodeStartingLine == -1)
                _sourceCodeStartingLine = context.Start.Line - 1;

            return base.VisitModuleBody(context);
        }

        public override object VisitSubStmt([NotNull] VisualBasic6Parser.SubStmtContext context)
        {
            var scopeName = context.ambiguousIdentifier().GetText();
            var scopeStartColumn = context.Start.Column;
            var printableArguments = GetPrintableProcedureArguments(context.argList());
            var serializedProcedureArguments = PrepareSerializedVariables(printableArguments);

            var newPreambleCode = CodeTemplates.GetFunctionPreamble(_filename, scopeName, serializedProcedureArguments, scopeStartColumn);
            var newPostambleCode = CodeTemplates.GetFunctionPostamble(_filename, scopeName, scopeStartColumn);

            _rewriter.InsertAfter(context.argList().Stop, $"\r\n{newPreambleCode}");
            _rewriter.InsertBefore(context.END_SUB().Symbol, $"{newPostambleCode}");

            return base.VisitSubStmt(context);
        }
    }
}
