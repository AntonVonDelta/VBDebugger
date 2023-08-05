using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VBCodeTransformer.Parsers.Models;

namespace VBCodeTransformer.Parsers {
    public class ProcedureVisitor : VisualBasic6ParserBaseVisitor<object> {
        private readonly List<VariableModel> _definedVariables = new List<VariableModel>();
        private readonly TokenStreamRewriter _rewriter;
        private readonly string _filename;
        private readonly int _sourceCodeStartingLine;
        private string _scopeName;

        public ProcedureVisitor(string filename, int sourceCodeStartingLine, List<VariableModel> arguments, TokenStreamRewriter rewriter) {
            _filename = filename;
            _sourceCodeStartingLine = sourceCodeStartingLine;
            _rewriter = rewriter;

            _definedVariables.AddRange(arguments);
        }

        public override object VisitSubStmt([NotNull] VisualBasic6Parser.SubStmtContext context) {
            _scopeName = context.ambiguousIdentifier().GetText();

            return base.VisitSubStmt(context);
        }

        public override object VisitVariableSubStmt([NotNull] VisualBasic6Parser.VariableSubStmtContext context) {
            VariableModel variable;

            if (context.asTypeClause() == null) return null;

            variable = new VariableModel() {
                Name = context.ambiguousIdentifier().GetText(),
                Type = context.asTypeClause().type_().GetText()
            };

            _definedVariables.Add(variable);

            return base.VisitVariableSubStmt(context);
        }

        public override object VisitICS_B_ProcedureCall([NotNull] VisualBasic6Parser.ICS_B_ProcedureCallContext context) {
            if (context.argsCall() != null) {
                var startColumn = context.certainIdentifier().Start.Column;
                var parametersOrFunctionCalls = Util.GetProcedureCallArguments(context.argsCall());
                var printableParameters = _definedVariables
                    .Where(el => parametersOrFunctionCalls.Any(b => string.Compare(el.Name, b, true) == 0)).ToList();
                var serializedProcedureArguments = Util.PrepareSerializedVariables(printableParameters);

                var procedureCallPreamble = CodeTemplates.GetProcedureCallPreamble(_filename, _scopeName, serializedProcedureArguments, startColumn);

                _rewriter.InsertBefore(context.certainIdentifier().Start, procedureCallPreamble);
            } else {
                //foreach (var arg in context.argsCall())

            }


            return base.VisitICS_B_ProcedureCall(context);
        }

        public override object VisitLineLabel([NotNull] VisualBasic6Parser.LineLabelContext context) {
            var startColumn = context.ambiguousIdentifier().Start.Column;
            var lineLabelCheckpoint = CodeTemplates.GetLineLabelCheckpoint(_filename, _scopeName, startColumn);

            _rewriter.InsertAfter(context.Stop, lineLabelCheckpoint);

            return base.VisitLineLabel(context);
        }
    }
}
