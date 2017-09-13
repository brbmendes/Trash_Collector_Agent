using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    public class Position
    {
        public Int32 Line { get; set; }
        public Int32 Column { get; set; }

        public Position(Int32 line, Int32 column)
        {
            this.Line = line;
            this.Column = column;
        }
    }
}
