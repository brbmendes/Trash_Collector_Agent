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
        public Environment(Int32 size, List<Trash_deposit> trashDeposits, List<Recharger> rechargers, List<String[,]> walls)
        {
            this.size = size;
            this.trashDeposits = trashDeposits;
            this.rechargers = rechargers;
            this.walls = walls;
            this.map = new String[this.size, this.size];
        }

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
            Double doisTercosSize = (0.66666666667 * this.size) + 0.77777777779;
            Int32 internalYLeftSide = Convert.ToInt32(Math.Truncate((this.size/3)-0.6));
            Int32 internalYRightSide = Convert.ToInt32(Math.Truncate(doisTercosSize));

            //#region PAREDES EXTERNAS
            //map.SetValue("# ", 0 + 2, 0 + 2);
            //map.SetValue("# ", this.size - 2, 0 + 2);
            //map.SetValue("# ", 0 + 2, this.size - 2);
            //map.SetValue("# ", this.size - 2, this.size - 2);
            //#endregion PAREDES EXTERNAS

            // Os blocos superiores da parede serão nos pontos { [0+2,0+2] , [this.size-2,0+2] }
            // os blocos inferiores da parede serão nos pontos { [0+2,this.size-2] , [this.size-2,this.size-2] }

            #region quadrante esquerdo - HORIZONTAL

            while(y < ((Math.Truncate((this.size/3)-0.6)+1)))
            {
                this.map.SetValue("# ",0+2,y);
                y++;
                //this.showEnvironment();
            }
            
            y = 2;
            
            while(y < ((Math.Truncate((this.size/3)-0.6)+1)))
            {
                this.map.SetValue("# ",this.size-3,y);
                y++;
                //this.showEnvironment();
            }

            #endregion
            
            
            #region quadrante direito - HORIZONTAL
            //y = 8;
            y = Convert.ToInt32(Math.Truncate(doisTercosSize));

            while(y < size -2)
            {
                this.map.SetValue("# ",0+2,y);
                y++;
                //this.showEnvironment();
            }

            //y = 8;
            y = Convert.ToInt32(Math.Truncate(doisTercosSize));

            while(y < size -2)
            {
                this.map.SetValue("# ",size-3,y);
                y++;
                //this.showEnvironment();
            }
            
            #endregion	
            
            
            #region quadrante esquerdo - VERTICAL
            
            y = 3;

            while(y <= Math.Truncate((this.size/3)-0.6))
            {
                x = 2;
                do
                {
                    x++;
                    //this.map.SetValue("# ",x,internalYLeftSide);
                    this.map.SetValue("# ", x, y);
                    //this.showEnvironment();
                }
                while(x < size-3);
                y++;
                //this.showEnvironment();
                    
            }
            
            #endregion
            
            
            #region quadrante direito - VERTICAL

            //y = 8;
            y = Convert.ToInt32(Math.Truncate(doisTercosSize));

            while(y < size-3)
            {
                x = 2;
                internalYRightSide = Convert.ToInt32(Math.Truncate(doisTercosSize));
                do
                {
                    x++;
                    //map.SetValue("# ", x, internalYRightSide);
                    this.map.SetValue("# ", x, y);
                    //this.showEnvironment();
                }
                while(x < size-3);
                y++;
                //this.showEnvironment();
            }

            #endregion

        }
    }
}
