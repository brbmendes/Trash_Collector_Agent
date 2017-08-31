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
        /// List of wall points
        /// </summary>
        List<String[,]> walls;

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        /// <summary>
        /// List of recharger points
        /// </summary>
        List<Recharger> rechargers;

        /// <summary>
        /// Quantity of trashDeposits
        /// </summary>
        Int32 qtdTrashDeposits;

        /// <summary>
        /// Quantity of Rechargers
        /// </summary>
        Int32 qtdRechargers;


        #region CONSTRUTOR TEMPORARIO PARA TESTAR O MAPA
        public Environment(Int32 size)
        {
            this.size = size;
            this.map = new String[this.size, this.size];
        }
        #endregion

        /// <summary>
        /// Environment constructor method
        /// </summary>
        /// <param name="size">Size of environment</param>
        /// <param name="trashDeposits">List of trash deposit points</param>
        /// <param name="rechargers">List of recharger points</param>
        /// <param name="walls">List of wall points</param>
        public Environment(Int32 size, Int32 qtdTrashDeposits, Int32 qtdRechargers)
        {
            this.size = size;
            this.trashDeposits = new List<Trash_deposit>();
            this.rechargers = new List<Recharger>();
            this.qtdTrashDeposits = qtdTrashDeposits;
            this.qtdRechargers = qtdRechargers;
            this.map = new String[this.size, this.size];
        }

        /// <summary>
        /// Initialize map with empty blocks
        /// </summary>
        public void initializeMap()
        {
            for (Int32 i = 0; i < this.map.GetLength(0); i++)
            {
                for (Int32 j = 0; j < this.map.GetLength(1); j++)
                {
                    this.map.SetValue("- ", i, j);
                }
            }
        }

        /// <summary>
        /// Show environment on screen
        /// </summary>
        public void showEnvironment()
        {
            for (Int32 i = 0; i < this.size; i++)
            {
                for (Int32 j = 0; j < this.size; j++)
                {
                    Console.Write(this.map.GetValue(i, j));
                }
                Console.WriteLine("\n");
            }
        }

        /// <summary>
        /// Build the walls
        /// </summary>
        public void buildWalls()
        {
            Int32 x = 2;
            Int32 y = 2;
            Double oneThirdSize = (Math.Truncate((this.size/3)-0.6)+1);
            Double twoThirdsSize = (0.66666666667 * this.size) + 0.77777777779;

            #region left quadrant - HORIZONTAL
            //while(y < ((Math.Truncate((this.size/3)-0.6)+1)))
            while(y < oneThirdSize)
            {
                this.map.SetValue("# ",0+2,y);
                y++;
                //this.showEnvironment();
            }
            
            y = 2;
            
            //while(y < ((Math.Truncate((this.size/3)-0.6)+1)))
            while (y < oneThirdSize)
            {
                this.map.SetValue("# ",this.size-3,y);
                y++;
                //this.showEnvironment();
            }

            #endregion


            #region right quadrant - HORIZONTAL
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            while(y < size -2)
            {
                this.map.SetValue("# ",0+2,y);
                y++;
                //this.showEnvironment();
            }

            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));
            while(y < size -2)
            {
                this.map.SetValue("# ",this.size-3,y);
                y++;
                //this.showEnvironment();
            }
            
            #endregion	
            
            
            #region left quadrant - VERTICAL

            y = 3;
            //while(y <= Math.Truncate((this.size/3)-0.6))
            while (y < oneThirdSize)
            {
                x = 2;
                do
                {
                    x++;
                    //this.map.SetValue("# ",x,internalYLeftSide);
                    this.map.SetValue("# ", x, y);
                    //this.showEnvironment();
                }
                while(x < this.size-3);
                y++;
                //this.showEnvironment();
                    
            }
            
            #endregion


            #region right quadrant - VERTICAL
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            while(y < this.size-3)
            {
                x = 2;
                do
                {
                    x++;
                    this.map.SetValue("# ", x, y);
                    //this.showEnvironment();
                }
                while(x < this.size-3);
                y++;
                //this.showEnvironment();
            }

            #endregion

        }

        public void buildTrashDeposits(Int32 qtdTrashDeposits)
        {
            Int32 countAux = qtdTrashDeposits;
            #region left quadrant TrashDeposits
            // size = 16
            // carregadores ficam entre os pontos [0+3,0] , [0+3,0+2] , [this.size-3,0] , [this.size-3,0+2]
            Double oneThirdSize = (Math.Truncate((this.size / 3) - 0.6) + 1);
            Double twoThirdsSize = (0.66666666667 * this.size) + 0.77777777779;



            ////    ACHO QUE A VERSÃO COM DO-WHILE ABAIXO ESTÁ CERTA. TEM QUE TESTAR E DEBUGAR
            //Random rnd = new Random();
            //for (Int32 i = 0; i < qtdTrashDeposits ; i++)
            //{
            //    Trash_deposit t = new Trash_deposit(rnd.Next(3,this.size-2),rnd.Next(0,3));
            //    if(this.map.GetValue(t.getX(),t.getY()) == "-")
            //    {
            //        this.trashDeposits.Add(t);
            //    }
                
            //}

            Random rnd = new Random();
            while(countAux > 0)
            {
                Trash_deposit t;
                do
                {
                    t = new Trash_deposit(rnd.Next(3, this.size - 2), rnd.Next(0, 3));
                }
                while (this.map.GetValue(t.getX(), t.getY()) != "-");
                this.trashDeposits.Add(t);
                countAux--;
            }


            #endregion  

            #region right quadrant TrashDeposits
            // carregadores ficam entre os pontos [0+3,this.size-2] , [0+3,this.size] , [this.size-3,this.size-2] , [this.size-3,this.size]

            #endregion
        }

        public void buildRechargers(int qtd)
        {
            #region left quadrant Rechargers
            // size = 16
            // carregadores ficam entre os pontos [0+3,0] , [0+3,0+2] , [this.size-3,0] , [this.size-3,0+2]
            Int32 y = 0;
            Double oneThirdSize = (Math.Truncate((this.size / 3) - 0.6) + 1);

            #endregion

            #region right quadrant Rechargers
            // size = 16
            // carregadores ficam entre os pontos [0+3,this.size-2] , [0+3,this.size] , [this.size-3,this.size-2] , [this.size-3,this.size]
            
            Double twoThirdsSize = (0.66666666667 * this.size) + 0.77777777779;
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            #endregion
        }
    }
}
