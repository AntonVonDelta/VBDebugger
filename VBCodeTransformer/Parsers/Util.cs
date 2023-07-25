using Antlr4.Runtime.Tree.Xpath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBCodeTransformer.Parsers.Models;

namespace VBCodeTransformer.Parsers {
    public class Util {
        public static readonly List<string> PrintableVariableTypes = new List<string>{
            "string", "Integer", "Long", "Boolean" };


        public static string PrepareSerializedVariables(List<VariableModel> variables) {
            return string.Join(", ", variables.Select(el => $"\"{el.Name}\", {el.Name}"));
        }


        public static List<VariableModel> GetPrintableProcedureArguments(VisualBasic6Parser.ArgListContext argListContext) {
            var result = GetProcedureArguments(argListContext);

            return result.Where(el => PrintableVariableTypes.Contains(el.Type)).ToList();
        }

        public static List<VariableModel> GetProcedureArguments(VisualBasic6Parser.ArgListContext argListContext) {
            var result = new List<VariableModel>();

            foreach (var arg in argListContext.arg()) {
                VariableModel variable;
                if (arg.asTypeClause() == null) continue;

                variable = new VariableModel() {
                    Name = arg.ambiguousIdentifier().GetText(),
                    Type = arg.asTypeClause().type_().GetText().ToLower()
                };

                result.Add(variable);
            }

            return result;
        }



        public static List<string> GetProcedureCallArguments(VisualBasic6Parser.ArgsCallContext argsCallContext) {
            var result = new List<string>();

            foreach (var arg in argsCallContext.argCall()) {
                var valueStmt = arg.valueStmt();
                var variableOrCalls = valueStmt.GetRuleContexts<VisualBasic6Parser.ImplicitCallStmt_InStmtContext>()
                    .SelectMany(el => el.GetRuleContexts<VisualBasic6Parser.ICS_S_VariableOrProcedureCallContext>());

                foreach (var variableOrCall in variableOrCalls) {
                    result.Add(variableOrCall.ambiguousIdentifier().GetText());
                }
            }

            return result;
        }
    }
}
