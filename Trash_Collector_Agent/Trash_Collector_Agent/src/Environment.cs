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
        /// Agent
        /// </summary>
        public Agent agent;

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        /// <summary>
        /// List of recharger points
        /// </summary>
        List<Recharger> rechargers;

        /// <summary>
        /// List of dirty points
        /// </summary>
        List<Dirty> dirties;

        /// <summary>
        /// Map of walls
        /// </summary>
        HashSet<Wall> walls;

        /// <summary>
        /// Quantity of trashDeposits
        /// </summary>
        Int32 qtdTrashDeposits;

        /// <summary>
        /// Percentual of dirty environment
        /// </summary>
        Int32 percentDirty;

        /// <summary>
        /// Quantity of Rechargers
        /// </summary>
        Int32 qtdRechargers;


        /// <summary>
        /// Next Position Map dictionary
        /// </summary>
        Dictionary<String, Int32> nextPosition;

        //#region CONSTRUTOR TEMPORARIO PARA TESTAR O MAPA
        //public Environment(Int32 size, Int32 qtdTrashDeposits, Int32 qtdRechargers)
        //{
        //    this.size = size;
        //    this.map = new String[this.size, this.size];
        //    this.qtdTrashDeposits = qtdTrashDeposits;
        //    this.qtdRechargers = qtdRechargers;
        //    this.rechargers = new List<Recharger>();
        //    this.trashDeposits = new List<Trash_deposit>();
        //}
        //#endregion

        /// <summary>
        /// Environment constructor method
        /// </summary>
        /// <param name="size">Size of environment</param>
        /// <param name="qtdTrashDeposits">Quantity of trash deposits</param>
        /// <param name="qtdRechargers">Quantity of rechargers</param>
        /// <param name="percentDirty">Percentual of dirty environment</param>
        /// <param name="sizeInternalTrash">Size of agent internal trash</param>
        /// <param name="capacityInternalBattery">Capacity of agent internal battery</param>
        public Environment(Int32 size, Int32 qtdTrashDeposits, Int32 qtdRechargers, Int32 percentDirty, Int32 sizeInternalTrash, Int32 capacityInternalBattery)
        {
            this.size = size;
            this.map = new String[this.size, this.size];
            this.agent = new Agent(sizeInternalTrash, capacityInternalBattery);
            this.qtdRechargers = qtdRechargers;
            this.qtdTrashDeposits = qtdTrashDeposits;
            this.percentDirty = percentDirty;
            this.walls = new HashSet<Wall>();
            this.dirties = new List<Dirty>();
            this.rechargers = new List<Recharger>();
            this.trashDeposits = new List<Trash_deposit>();
            initializeDictionaryNextPosition();
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
        /// Initialize Agent in position [0,0]
        /// </summary>
        public void positioningAgent()
        {
            this.agent.set(0,0);
            this.map.SetValue("A ", 0, 0);
            this.agent.oldPosition.Add("x", 0);
            this.agent.oldPosition.Add("y", 0);
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
                this.walls.Add(new Wall(0 + 2, y));
                y++;
                //this.showEnvironment();
            }
            
            y = 2;
            
            //while(y < ((Math.Truncate((this.size/3)-0.6)+1)))
            while (y < oneThirdSize)
            {
                this.map.SetValue("# ",this.size-3,y);
                this.walls.Add(new Wall(this.size - 3, y));
                y++;
                //this.showEnvironment();
            }

            #endregion


            #region right quadrant - HORIZONTAL
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            while(y < size -2)
            {
                this.map.SetValue("# ",0+2,y);
                this.walls.Add(new Wall(0 + 2, y));
                y++;
                //this.showEnvironment();
            }

            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));
            while(y < size -2)
            {
                this.map.SetValue("# ",this.size-3,y);
                this.walls.Add(new Wall(this.size - 3, y));
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
                    this.walls.Add(new Wall(x, y));
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
                    this.walls.Add(new Wall(x, y));
                    //this.showEnvironment();
                }
                while(x < this.size-3);
                y++;
                //this.showEnvironment();
            }

            #endregion

        }

        public void buildTrashDeposits()
        {
            Random rnd = new Random();
            Int32 leftTrashDeposits = 1;
            Int32 rightTrashDeposits = 1;
         
            if(this.qtdTrashDeposits <= 0)
            {
                leftTrashDeposits = 1;
                rightTrashDeposits = 1;
            }
            else if (this.qtdTrashDeposits == 1)
            {
                if(rnd.Next(0, 2) == 0)
                {
                    leftTrashDeposits = 1;
                    rightTrashDeposits = 0;
                }
                else
                {
                    leftTrashDeposits = 0;
                    rightTrashDeposits = 1;
                }
            }
            else
            {
                leftTrashDeposits = this.qtdTrashDeposits / 2;
                rightTrashDeposits = this.qtdTrashDeposits - leftTrashDeposits;
            }
            
            #region left quadrant TrashDeposits
            // size = 16
            // carregadores ficam entre os pontos [0+3,0] , [0+3,0+2] , [this.size-3,0] , [this.size-3,0+2]
            while (this.trashDeposits.Count != leftTrashDeposits)
            {
                Trash_deposit t = new Trash_deposit(rnd.Next(3, this.size - 3), rnd.Next(0, 3));
                if (this.map.GetValue(t.getX(), t.getY()).ToString().Trim() == "-")
                {
                    this.map.SetValue("T ",t.getX(), t.getY());
                    this.trashDeposits.Add(t);
                }
            }
            
            #endregion  

            #region right quadrant TrashDeposits
            // carregadores ficam entre os pontos [0+3,this.size-2] , [0+3,this.size] , [this.size-3,this.size-2] , [this.size-3,this.size]
            while (this.trashDeposits.Count != leftTrashDeposits+rightTrashDeposits)
            {
                Trash_deposit t = new Trash_deposit(rnd.Next(3, this.size - 3), rnd.Next(this.size-2, this.size));
                if (this.map.GetValue(t.getX(), t.getY()).ToString().Trim() == "-")
                {
                    this.map.SetValue("T ", t.getX(), t.getY());
                    this.trashDeposits.Add(t);
                }
            }
            #endregion
        }

        public void buildRechargers()
        {
            Random rnd = new Random();
            Int32 leftRechargers = 1;
            Int32 rightRechargers = 1;

            if (this.qtdRechargers <= 0)
            {
                leftRechargers = 1;
                rightRechargers = 1;
            }
            else if (this.qtdRechargers == 1)
            {
                if (rnd.Next(0, 2) == 0)
                {
                    leftRechargers = 1;
                    rightRechargers = 0;
                }
                else
                {
                    leftRechargers = 0;
                    rightRechargers = 1;
                }
            }
            else
            {
                rightRechargers = this.qtdRechargers / 2;
                leftRechargers = this.qtdRechargers - rightRechargers;
                
            }
            #region left quadrant Rechargers
            // size = 16
            // carregadores ficam entre os pontos [0+3,0] , [0+3,0+2] , [this.size-3,0] , [this.size-3,0+2]
            while (this.rechargers.Count != leftRechargers)
            {
                Recharger r = new Recharger(rnd.Next(3, this.size - 3), rnd.Next(0, 3));
                if (this.map.GetValue(r.getX(), r.getY()).ToString().Trim() == "-")
                {
                    this.map.SetValue("R ", r.getX(), r.getY());
                    this.rechargers.Add(r);
                }
            }
            #endregion

            #region right quadrant Rechargers
            // carregadores ficam entre os pontos [0+3,this.size-2] , [0+3,this.size] , [this.size-3,this.size-2] , [this.size-3,this.size]
            while (this.rechargers.Count != leftRechargers + rightRechargers)
            {
                Recharger r = new Recharger(rnd.Next(3, this.size - 3), rnd.Next(this.size - 2, this.size));
                if (this.map.GetValue(r.getX(), r.getY()).ToString().Trim() == "-")
                {
                    this.map.SetValue("R ", r.getX(), r.getY());
                    this.rechargers.Add(r);
                }
            }
            #endregion
        }

        public void buildDirtyEnvironment()
        {
            Double percentDirtyConverted = this.percentDirty * 0.01;
            Int32 freeBlocks = this.map.Length - this.walls.Count - this.qtdRechargers - this.qtdTrashDeposits;
            Int32 numberOfDirties = Convert.ToInt32(freeBlocks * percentDirtyConverted);
            Random rnd = new Random();

            while (this.dirties.Count != numberOfDirties)
            {
                Dirty d = new Dirty(rnd.Next(0, this.size), rnd.Next(0, this.size));
                if (this.map.GetValue(d.getX(), d.getY()).ToString().Trim() == "-")
                {
                    this.map.SetValue("D ", d.getX(), d.getY());
                    this.dirties.Add(d);
                    //this.showEnvironment();
                }
            }
        }


        public void initializeDictionaryNextPosition()
        {
            this.nextPosition = new Dictionary<String, Int32>();
            this.nextPosition.Add("D", 1);
            this.nextPosition.Add("#", 2);
            this.nextPosition.Add("R", 3);
            this.nextPosition.Add("T", 4);
            this.nextPosition.Add("-", 5);
        }

        public void move()
        {
            // Pontos da atual posição do agente.
            Int32 agentPositionX = this.agent.getX();
            Int32 agentPositionY = this.agent.getY();


            //Pega próxima posição do mapa e verifica qual caratere é. 
            String nextPositionMap = this.map.GetValue(agentPositionX, agentPositionY + 1).ToString().Trim();
            int nextPositionMapValue = this.nextPosition[nextPositionMap];

            // Move de acordo com o caractere.
            switch (nextPositionMapValue)
            {
                case 1: // Achou lixo
                    if (this.agent.usedInternalTrash() > 0)
                    {
                        #region atualiza antiga posicao do agente
                        this.agent.oldPosition.Remove("x");
                        this.agent.oldPosition.Remove("y");
                        this.agent.oldPosition.Add("x", this.agent.getX());
                        this.agent.oldPosition.Add("y", this.agent.getY());
                        #endregion

                        #region Coleta lixo
                        this.agent.collectTrash();
                        #endregion

                        #region Seta "- " onde o agente estava, e "A " na posição a frente
                        this.map.SetValue("- ", agentPositionX, agentPositionY);
                        this.agent.set(agentPositionX, agentPositionY + 1);
                        this.map.SetValue("A ", this.agent.getX(), this.agent.getY());
                        #endregion

                        #region Consome bateria por andar
                        this.agent.consumeBattery();
                        #endregion

                    }
                    //else
                    //{
                    //    // USAR ALGORITMO A* PARA ACHAR O deposito de lixo MAIS PRÓXIMO E ANDAR ATÉ ELE.
                    //    // DESCARREGAR LIXO
                    //    // USAR ALGORITMO A* PARA retonrar até ao ponto que estava
                    //}
                    break;
                case 2: // Achou parede
                    // USAR ALGORITMO A* para desviar.
                    break;
                case 3: // Achou Carregador
                    // USAR ALGORITMO A* PARA andar para o próximo ponto na mesma linha, ou na coluna abaixo caso não tenha linha pro lado e seja final da matriz
                    break;
                case 4: // Achou depósito de lixo
                    // USAR ALGORITMO A* PARA andar para o próximo ponto na mesma linha, ou na coluna abaixo caso não tenha linha pro lado e seja final da matriz
                    break;
                case 5:
                    #region atualiza antiga posicao do agente
                    this.agent.oldPosition.Remove("x");
                    this.agent.oldPosition.Remove("y");
                    this.agent.oldPosition.Add("x", this.agent.getX());
                    this.agent.oldPosition.Add("y", this.agent.getY());
                    #endregion
                    
                    #region Seta "- " onde o agente estava, e "A " na posição a frente
                    this.map.SetValue("- ", agentPositionX, agentPositionY);
                    this.agent.set(agentPositionX, agentPositionY + 1);
                    this.map.SetValue("A ", this.agent.getX(), this.agent.getY());
                    #endregion

                    #region Consome bateria por andar
                    this.agent.consumeBattery();
                    #endregion
                    break;
            }

            
        }
    }
}
