using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    /// <summary>
    /// Class containing information about recharge points.
    /// </summary>
    class Wall
    {
        public Position XY { get; set; }


        public Wall(Position position)
        {
            this.XY = new Position(position.Line, position.Column);
        }

        public void showPosition()
        {
            Console.WriteLine(String.Format("[{0},{1}]", this.XY.Line, this.XY.Column));
        }
    }
}
