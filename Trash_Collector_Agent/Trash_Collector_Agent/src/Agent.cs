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
        /// Agent old position;
        /// </summary>
        public Dictionary<String, Int32> oldPosition;

        public Position LastPosition { get; set; }
        public Position CurrentPosition { get; set; }
        public Position NextPosition { get; set; }

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        public Position XY { get; set; }

        public Environment environment { get; set; }

        public Astar aStar { get; set; }

        public Agent(Int32 internalTrash)
        {
            this.XY = new Position(0, 0);
            initializePositions(XY);
            this.updatePosition(XY, XY, XY);
            this.capacityInternalTrash = internalTrash;
            this.currentInternalTrash = internalTrash;
            oldPosition = new Dictionary<String, Int32>();
        }

        private void initializePositions(Position pos)
        {
            this.LastPosition = pos;
            this.CurrentPosition = pos;
            this.NextPosition = pos;
        }

        public void updatePosition(Position currentPosition, Position nextPosition, Position nextNextPosition)
        {
            this.LastPosition.Line = currentPosition.Line;
            this.LastPosition.Column = currentPosition.Column;
            this.CurrentPosition.Line = nextPosition.Line;
            this.CurrentPosition.Column = nextPosition.Column;
            this.NextPosition.Line = nextNextPosition.Line;
            this.NextPosition.Column = nextNextPosition.Column;
        }

        public void updatePosition(Position currentPosition, Position nextPosition)
        {
            this.LastPosition.Line = currentPosition.Line;
            this.LastPosition.Column = currentPosition.Column;
            this.CurrentPosition.Line = nextPosition.Line;
            this.CurrentPosition.Column = nextPosition.Column;
        }

        public Int32 usedInternalTrash()
        {
            return this.currentInternalTrash;
        }

        public void cleanInternalTrash()
        {
            this.currentInternalTrash = this.capacityInternalTrash;
        }

        public void collectTrash()
        {
            this.currentInternalTrash--;
        }

        public void setTrashDeposits(List<Trash_deposit> trashDeposits)
        {
            this.trashDeposits = trashDeposits;
        }
        /// <summary>
        /// Ambiente em torno do agente
        /// </summary>
        Dictionary<String[,], Object> recognizedEnvironment = new Dictionary<string[,], Object>();

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

            Boolean right = true;
            while(this.CurrentPosition.Line <= environment.Size - 1)
            {
                //this.capacityInternalTrash = 0;
                Console.WriteLine("\n");
                Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                environment.showEnvironment();
                if (right)
                {
                    if(this.CurrentPosition.Column == environment.Size - 1) // se for a última coluna
                    {
                        if (this.CurrentPosition.Line == environment.Size) break;
                        // ... verifica posicao abaixo
                        if(map.GetValue(CurrentPosition.Line+1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if(this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else if(map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = aStar.PathFindAStar(environment, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else{
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                         
                        //this.collectTrash();
                        //this.lastPosition.Line = this.CurrentPosition.Line;
                        //this.CurrentPosition.Line += 1;
                        //if(this.CurrentPosition.Line == environment.Size) break;
                        //right = false;
                        //this.lastPosition.Column = this.CurrentPosition.Column;
                    }
                    else
                    {
                        // ... verifica o lado direito
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if(map.GetValue(CurrentPosition.Line, CurrentPosition.Column+1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column += 1;
                        }
                        else if (map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString().Trim() == "D" && this.currentInternalTrash == 0) // se for sujeira e estiver cheio
                        {
                            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = aStar.PathFindAStar(environment, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column += 1;
                            //Console.ReadKey();
                        }
                        else // não é sujeira
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column += 1;
                        }
                        
                        
                    }
                }
                else 
                {

                    if(this.CurrentPosition.Column == 0){
                        // ... verifica posicao abaixo
                        if (this.CurrentPosition.Line == environment.Size) break;
                        if(map.GetValue(CurrentPosition.Line+1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if(this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else if (map.GetValue(CurrentPosition.Line+1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = aStar.PathFindAStar(environment, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            environment.moveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else 
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }

                        //this.collectTrash();
                        //this.lastPosition.Line = this.CurrentPosition.Line;
                        //this.CurrentPosition.Line += 1;
                        //if(this.CurrentPosition.Line == environment.Size) break;
                        //right = true;
                        //this.lastPosition.Column = this.CurrentPosition.Column;
                    } else {
                        // ... verifica o lado esquerdo
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if(map.GetValue(CurrentPosition.Line, CurrentPosition.Column-1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (map.GetValue(CurrentPosition.Line, CurrentPosition.Column-1).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
                            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
                            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
                            Node destinyNode;
                            destinyNode = aStar.PathFindAStar(environment, begin, end);
                            List<Node> listFathers = Environment.staticCreateFatherList(destinyNode);
                            List<Node> cloneListFathers = listFathers.ToList<Node>();
                            Console.WriteLine("Going to destiny - RUNNUNG A*");
                            environment.moveAgentAroundEnvironment(this, listFathers, destinyNode);
                            this.cleanInternalTrash();
                            cloneListFathers.Reverse();
                            Console.WriteLine("Returning from destiny - RUNNUNG A*");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.moveAgentAroundEnvironment(this, cloneListFathers, destinyNode);
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }

                        //this.lastPosition.Line = this.CurrentPosition.Line;
                        //this.lastPosition.Column = this.CurrentPosition.Column;
                        //this.CurrentPosition.Column -= 1;
                    }                
                }
                environment.positioningAgent(this);
                Console.WriteLine("\n");
            }
        }




        public Node move(String[,] map)
        {
            #region A* IMPLEMENTADO

            // Pontos da atual posição do agente, e cria posição.
            Position agentPosition = new Position(this.XY.Line, this.XY.Column);

            // Cria nodo Begin;
            Node begin = new Node(this.XY.Line, this.XY.Column);


            #region ALTERNAR ENTRE AS LINHAS COMENTADAS PARA VER CADA UM DOS DESTINOS.

            // Seta três posições diferentes como alvo do A*
            //Position nearestTrash = this.calculateNearestTrash(agentPosition);
            Position endOfMatrix = new Position(environment.Size - 1, environment.Size - 1);

            // Define três posições diferentes como nodo destino.
            //Node endNearestTrash = new Node(nearestTrash.Line, nearestTrash.Column);
            Node endMatrix = new Node(endOfMatrix.Line, endOfMatrix.Column);

            //Node nodoDestino = Astar.PathFindAStar(map, begin, endNearestTrash);
            Node nodoDestino = aStar.PathFindAStar(environment, begin, endMatrix);
            #endregion

            return nodoDestino;
            #endregion
        }

        public Node newAStar(String[,] map, Node end)
        {
            

            // Pontos da atual posição do agente, e cria posição.
            Position agentPosition = new Position(this.XY.Line, this.XY.Column);

            // Cria nodo Begin;
            Node begin = new Node(this.XY.Line, this.XY.Column);

            Node nodoDestino = aStar.PathFindAStar(environment, begin, end);
           

            return nodoDestino;
           
        }

        private Position calculateNearestTrash(Position agentPosition)
        {
            Position nearestTrash = new Position(0, 0);
            Int32 absolutePosition = Int32.MaxValue;

            foreach (Trash_deposit trash in this.trashDeposits)
            {
                Position tempTrash = new Position(trash.XY.Line, trash.XY.Column);
                Int32 localAbsolutePosition = calculateAbsolutePosition(agentPosition, tempTrash);
                if (localAbsolutePosition < absolutePosition)
                {
                    nearestTrash = tempTrash;
                    absolutePosition = localAbsolutePosition;
                }
            }
            return nearestTrash;
        }

        private int calculateAbsolutePosition(Position elem1, Position elem2)
        {
            int absoluteX = Math.Abs(elem1.Line - elem2.Line);
            int absoluteY = Math.Abs(elem1.Column - elem2.Column);

            return absoluteX + absoluteY;
        }
    }
}
