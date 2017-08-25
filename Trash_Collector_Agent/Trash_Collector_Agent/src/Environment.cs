using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Environment
    {
        /// <summary>
        /// Size of environment
        /// </summary>
        Int32 size;

        /// <summary>
        /// Map declaration
        /// </summary>
        String[,] map;

        /// <summary>
        /// List of trash deposits points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        /// <summary>
        /// List of rechargers points
        /// </summary>
        List<Recharger> rechargers;

        public Environment(Int32 size)
        {
            this.size = size;
            this.map = new String[this.size, this.size];
        }

        /// <summary>
        /// Environment constructor method
        /// </summary>
        /// <param name="size">Size of environment</param>
        /// <param name="trashDeposits">List of trash deposit points</param>
        /// <param name="rechargers">List of rechargers points</param>
        public Environment(Int32 size, List<Trash_deposit> trashDeposits, List<Recharger> rechargers)
        {
            this.size = size;
            this.trashDeposits = trashDeposits;
            this.rechargers = rechargers;
            this.map = new String[this.size,this.size];
            
        }

        public void initializeMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map.SetValue("- ", i, j);
                }
            }
        }

        /// <summary>
        /// Show environment on screen
        /// </summary>
        public void showEnvironment()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    Console.Write(map.GetValue(i,j));
                }
                Console.WriteLine("\n");
            }
        }
    }
}
