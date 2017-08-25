using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment env;
            int size = 12;

            env = new Environment(size);
            env.initializeMap();
            env.showEnvironment();

            Console.ReadKey();


        }
    }
}
