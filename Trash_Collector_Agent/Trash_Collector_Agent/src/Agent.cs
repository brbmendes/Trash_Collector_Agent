using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Agent
    {
        /// <summary>
        /// Capacity agent internal trash
        /// </summary>
        Int32 capacityInternalTrash;

        /// <summary>
        /// Current agent internal trash
        /// </summary>
        Int32 currentInternalTrash;

        /// <summary>
        /// Capacity agent internal battery
        /// </summary>
        Int32 capacityInternalBattery;

        /// <summary>
        /// Current agent internal battery
        /// </summary>
        Int32 currentInternalBattery;

        /// <summary>
        /// Agent old position;
        /// </summary>
        public Dictionary<String, Int32> oldPosition;

        public Position lastPosition { get; set; }
        public Position currentPosition { get; set; }

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        /// <summary>
        /// List of recharger points
        /// </summary>
        List<Recharger> rechargers;

        /// <summary>
        /// Next Position Map dictionary
        /// </summary>
        public Dictionary<String, Int32> nextPosition;

        Int32 positionX { get; set; }
        Int32 positionY { get; set; }



        public Agent(Int32 internalTrash, Int32 internalBattery)
        {
            this.capacityInternalTrash = internalTrash;
            this.currentInternalTrash = internalTrash;
            this.capacityInternalBattery = internalBattery;
            this.currentInternalBattery = internalBattery;
            oldPosition = new Dictionary<String, Int32>();
            initializeDictionaryNextPosition();
        }

        public Int32 getX()
        {
            return this.positionX;
        }

        public Int32 getY()
        {
            return this.positionY;
        }

        public Int32 usedInternalTrash()
        {
            return this.currentInternalTrash;
        }

        public void cleanInternalTrash()
        {
            this.currentInternalTrash = this.capacityInternalTrash;
        }

        public Int32 usedInternalBattery()
        {
            return this.currentInternalBattery;
        }

        public void collectTrash()
        {
            this.currentInternalTrash--;
        }

        public void rechargeBattery()
        {
            this.currentInternalBattery = this.capacityInternalBattery;
        }

        public void set(Int32 x, Int32 y)
        {
            this.positionX = x;
            this.positionY = y;
        }

        public void consumeBattery()
        {
            this.currentInternalBattery--;
        }

        public void showPosition()
        {
            Console.WriteLine(String.Format("[{0},{1}]", this.positionX, this.positionY));
        }

        public void setTrashDeposits(List<Trash_deposit> trashDeposits)
        {
            this.trashDeposits = trashDeposits;
        }

        public void setRechargers(List<Recharger> rechargers)
        {
            this.rechargers = rechargers;
        }

        /// <summary>
        /// Ambiente em torno do agente
        /// </summary>
        Dictionary<String[,], Object> recognizedEnvironment = new Dictionary<string[,], Object>();

        public void initializeDictionaryNextPosition()
        {
            this.nextPosition = new Dictionary<String, Int32>();
            this.nextPosition.Add("D", 1);
            this.nextPosition.Add("#", 2);
            this.nextPosition.Add("R", 3);
            this.nextPosition.Add("T", 4);
            this.nextPosition.Add("-", 5);
        }

        public void recognizingEnvironment(String[,] map, Int32 agentPositionX, Int32 agentPositionY)
        {
            String[,] north = new String[agentPositionX - 1, agentPositionY];
            String[,] south = new String[agentPositionX + 1, agentPositionY];
            String[,] east = new String[agentPositionX, agentPositionY + 1];
            String[,] west = new String[agentPositionX, agentPositionY - 1];
            String[,] northEast = new String[agentPositionX - 1, agentPositionY + 1];
            String[,] northWest = new String[agentPositionX - 1, agentPositionY - 1];
            String[,] southEast = new String[agentPositionX + 1, agentPositionY + 1];
            String[,] southWest = new String[agentPositionX + 1, agentPositionY - 1];

            this.recognizedEnvironment.Add(north, map.GetValue(agentPositionX - 1, agentPositionY));
            this.recognizedEnvironment.Add(south, map.GetValue(agentPositionX + 1, agentPositionY));
            this.recognizedEnvironment.Add(east, map.GetValue(agentPositionX, agentPositionY + 1));
            this.recognizedEnvironment.Add(west, map.GetValue(agentPositionX, agentPositionY - 1));
            this.recognizedEnvironment.Add(northEast, map.GetValue(agentPositionX - 1, agentPositionY + 1));
            this.recognizedEnvironment.Add(northWest, map.GetValue(agentPositionX - 1, agentPositionY - 1));
            this.recognizedEnvironment.Add(southEast, map.GetValue(agentPositionX + 1, agentPositionY + 1));
            this.recognizedEnvironment.Add(southWest, map.GetValue(agentPositionX + 1, agentPositionY - 1));
        }

        public void clean(String[,] map)
        {
            this.lastPosition = new Position(this.getX(), this.getY());
            this.currentPosition = new Position(this.getX(), this.getY());

            Boolean right = true;
            while(this.currentPosition.Line <= Environment.sizeEnv - 1)
            {
                //this.capacityInternalTrash = 0;
                Console.WriteLine("\n");
                Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                Environment.showEnv();
                if (right)
                {
                    if(this.currentPosition.Column == Environment.sizeEnv - 1) // se for a última coluna
                    {
                        if (this.currentPosition.Line == Environment.sizeEnv) break;
                        // ... verifica posicao abaixo
                        if(map.GetValue(currentPosition.Line+1, currentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if(this.currentPosition.Line == Environment.sizeEnv) break;
                            right = false;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }
                        else if(map.GetValue(currentPosition.Line + 1, currentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.currentPosition.Line, this.currentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.currentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = Astar.PathFindAStar(map, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if (this.currentPosition.Line == Environment.sizeEnv) break;
                            right = false;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }
                        else{
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if (this.currentPosition.Line == Environment.sizeEnv) break;
                            right = false;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }
                         
                        //this.collectTrash();
                        //this.lastPosition.Line = this.currentPosition.Line;
                        //this.currentPosition.Line += 1;
                        //if(this.currentPosition.Line == Environment.sizeEnv) break;
                        //right = false;
                        //this.lastPosition.Column = this.currentPosition.Column;
                    }
                    else
                    {
                        // ... verifica o lado direito
                        if (this.currentPosition.Column == Environment.sizeEnv) break;
                        if(map.GetValue(currentPosition.Line, currentPosition.Column+1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column += 1;
                        }
                        else if (map.GetValue(currentPosition.Line, currentPosition.Column + 1).ToString().Trim() == "D" && this.currentInternalTrash == 0) // se for sujeira e estiver cheio
                        {
                            Node begin = new Node(this.currentPosition.Line, this.currentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.currentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = Astar.PathFindAStar(map, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column += 1;
                            //Console.ReadKey();
                        }
                        else // não é sujeira
                        {
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column += 1;
                        }
                        
                        
                    }
                }
                else 
                {

                    if(this.currentPosition.Column == 0){
                        // ... verifica posicao abaixo
                        if (this.currentPosition.Line == Environment.sizeEnv) break;
                        if(map.GetValue(currentPosition.Line+1, currentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if(this.currentPosition.Line == Environment.sizeEnv) break;
                            right = true;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }
                        else if (map.GetValue(currentPosition.Line+1, currentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.currentPosition.Line, this.currentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.currentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = Astar.PathFindAStar(map, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            Environment.staticMoveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if (this.currentPosition.Line == Environment.sizeEnv) break;
                            right = true;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }
                        else 
                        {
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.currentPosition.Line += 1;
                            if (this.currentPosition.Line == Environment.sizeEnv) break;
                            right = true;
                            this.lastPosition.Column = this.currentPosition.Column;
                        }

                        //this.collectTrash();
                        //this.lastPosition.Line = this.currentPosition.Line;
                        //this.currentPosition.Line += 1;
                        //if(this.currentPosition.Line == Environment.sizeEnv) break;
                        //right = true;
                        //this.lastPosition.Column = this.currentPosition.Column;
                    } else {
                        // ... verifica o lado esquerdo
                        if (this.currentPosition.Column == Environment.sizeEnv) break;
                        if(map.GetValue(currentPosition.Line, currentPosition.Column-1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column -= 1;
                        }
                        else if (map.GetValue(currentPosition.Line, currentPosition.Column-1).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.currentPosition.Line, this.currentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.currentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = Astar.PathFindAStar(map, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            Environment.staticMoveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            Environment.staticMoveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column -= 1;
                        }
                        else
                        {
                            this.lastPosition.Line = this.currentPosition.Line;
                            this.lastPosition.Column = this.currentPosition.Column;
                            this.currentPosition.Column -= 1;
                        }

                        //this.lastPosition.Line = this.currentPosition.Line;
                        //this.lastPosition.Column = this.currentPosition.Column;
                        //this.currentPosition.Column -= 1;
                    }                
                }
                Environment.posicionaAgente(map, currentPosition, lastPosition);
                Console.WriteLine("\n");
            }
        }




        public Node move(String[,] map)
        {
            #region A* IMPLEMENTADO

            // Pontos da atual posição do agente, e cria posição.
            Position agentPosition = new Position(this.getX(), this.getY());

            // Cria nodo Begin;
            Node begin = new Node(this.getX(), this.getY());


            #region ALTERNAR ENTRE AS LINHAS COMENTADAS PARA VER CADA UM DOS DESTINOS.

            // Seta três posições diferentes como alvo do A*
            //Position nearestTrash = this.calculateNearestTrash(agentPosition);
            //Position nearestRecharger = this.calculateNearestRecharger(agentPosition);
            Position endOfMatrix = new Position(Environment.sizeEnv - 1, Environment.sizeEnv - 1);

            // Define três posições diferentes como nodo destino.
            //Node endNearestTrash = new Node(nearestTrash.Line, nearestTrash.Column);
            //Node endNearestRecharger = new Node(nearestRecharger.Line, nearestRecharger.Column);
            Node endMatrix = new Node(endOfMatrix.Line, endOfMatrix.Column);

            //Node nodoDestino = Astar.PathFindAStar(map, begin, endNearestTrash);
            //Node nodoDestino = Astar.PathFindAStar(map, begin, endNearestRecharger);
            Node nodoDestino = Astar.PathFindAStar(map, begin, endMatrix);
            #endregion

            return nodoDestino;
            #endregion
        }

        public Node newAStar(String[,] map, Node end)
        {
            

            // Pontos da atual posição do agente, e cria posição.
            Position agentPosition = new Position(this.getX(), this.getY());

            // Cria nodo Begin;
            Node begin = new Node(this.getX(), this.getY());

            Node nodoDestino = Astar.PathFindAStar(map, begin, end);
           

            return nodoDestino;
           
        }

        private Position calculateNearestTrash(Position agentPosition)
        {
            Position nearestTrash = new Position(0, 0);
            Int32 absolutePosition = Int32.MaxValue;

            foreach (Trash_deposit trash in this.trashDeposits)
            {
                Position tempTrash = new Position(trash.getX(), trash.getY());
                Int32 localAbsolutePosition = calculateAbsolutePosition(agentPosition, tempTrash);
                if (localAbsolutePosition < absolutePosition)
                {
                    nearestTrash = tempTrash;
                    absolutePosition = localAbsolutePosition;
                }
            }
            return nearestTrash;
        }

        private Position calculateNearestRecharger(Position agentPosition)
        {
            Position nearestRecharger = new Position(0, 0);
            Int32 absolutePosition = Int32.MaxValue;

            foreach (Recharger recharger in this.rechargers)
            {
                Position tempRecharger = new Position(recharger.getX(), recharger.getY());
                Int32 localAbsolutePosition = calculateAbsolutePosition(agentPosition, tempRecharger);
                if (localAbsolutePosition < absolutePosition)
                {
                    nearestRecharger = tempRecharger;
                    absolutePosition = localAbsolutePosition;
                }
            }
            return nearestRecharger;
        }

        private int calculateAbsolutePosition(Position elem1, Position elem2)
        {
            int absoluteX = Math.Abs(elem1.Line - elem2.Line);
            int absoluteY = Math.Abs(elem1.Column - elem2.Column);

            return absoluteX + absoluteY;
        }
    }
}
