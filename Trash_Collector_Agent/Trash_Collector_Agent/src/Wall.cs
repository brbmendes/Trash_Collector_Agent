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
        int positionX { get; set; }
        int positionY { get; set; }


        public Wall(int positionX, int positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }

        public Int32 getX()
        {
            return this.positionX;
        }

        public Int32 getY()
        {
            return this.positionY;
        }

        public void showPosition()
        {
            Console.WriteLine(String.Format("[{0},{1}]", this.positionX, this.positionY));
        }
    }
}
