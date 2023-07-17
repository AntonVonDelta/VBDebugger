using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBDebugger.Debugger
{
    public class InstructionException
    {
        public long Number { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }

        public static bool Equals(InstructionException obj1, InstructionException obj2)
        {
            if (obj1 == obj2) return true;

            return obj1.Number == obj2.Number && 
                obj1.Source == obj2.Source && 
                obj1.Description == obj2.Description;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode() + Source.GetHashCode() + Description.GetHashCode();
        }
    }
}
