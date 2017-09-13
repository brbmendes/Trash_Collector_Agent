using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    /// <summary>
    /// Class containing information about the trash deposits.
    /// </summary>
    public class Trash_deposit
    {
        public Position XY{ get; set; }

        public Trash_deposit(Position position)
        {
            this.XY = new Position(position.Line, position.Column);
        }

        public void showPosition()
        {
            Console.WriteLine(String.Format("[{0},{1}]", XY.Line, XY.Column));
        }
    }
}
