﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBCodeTransformer.Parsers
{
    public class CodeTemplates
    {
        public const string LineMacro = "__LINE__";

        public static string GetFunctionPreamble(string filename, string scopeName, string serializedArguments, int columnsShift)
        {
            var padding = new string(' ', columnsShift);
            var result =
                $"{padding}On Error GoTo debug_handler\r\n" +
                $"{padding}    DebugEnterProcedure \"{filename}\", \"{scopeName}\", {LineMacro}";

            if (serializedArguments.Any())
                result += $",{serializedArguments}";

            result += "\r\n";

            return result;
        }

        public static string GetFunctionPostamble(string filename, string scopeName, int columnsShift)
        {
            var padding = new string(' ', columnsShift);

            // The last padding is because we insert before END SUB and thus
            // after the spaces beween new line and the actual END SUB.
            // Then we would have pushed END SUB on a plain new line without its previous padding

            return
                $"\r\n" +
                $"{padding}    DebugLeaveProcedure \"{filename}\", \"{scopeName}\", {LineMacro}\r\n" +
                $"{padding}    Exit Sub\r\n" +
                $"{padding}debug_handler:\r\n" +
                $"{padding}    DebugLeaveProcedure \"{filename}\", \"{scopeName}\", {LineMacro}\r\n" +
                $"{padding}    Err.Raise Err.Number, Err.Source, Err.Description\r\n" +
                $"{padding}";
        }
    }
}
