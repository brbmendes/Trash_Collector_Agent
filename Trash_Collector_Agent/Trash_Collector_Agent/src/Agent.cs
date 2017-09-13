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

        public void updatePosition(Position last, Position current, Position next)
        {
            this.LastPosition = last;
            this.CurrentPosition = current;
            this.NextPosition = next;
        }

        public void updateLastPosition(Position currentPosition)
        {
            this.LastPosition = currentPosition;
        }

        public void updateCurrentPosition(Position nextPosition)
        {
            this.CurrentPosition = nextPosition;
        }

        public void updateNextPosition(Position nextNextPosition)
        {
            this.NextPosition = nextNextPosition;
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

        public void clean()
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
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if(this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            this.locateNearestTrashAndCleanTrash();  
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for sujeira
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column - 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();

                            // se for sujeira e lixeira NÃO estiver cheia
                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = false;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                            {

                                this.locateNearestTrashAndCleanTrash();
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = false;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            else
                            {
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = false;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                        }
                        else{
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                    }
                    else
                    {
                        // ... verifica o lado direito
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            
                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString().Trim() == "D" && this.currentInternalTrash == 0) // se for sujeira e estiver cheio
                        {
                            this.locateNearestTrashAndCleanTrash();  
                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else // não é sujeira
                        {
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        
                        
                    }
                }
                else 
                {

                    if(this.CurrentPosition.Column == 0){
                        // ... verifica posicao abaixo
                        
                        // se for sujeira e lixeira NÃO estiver cheia
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if(this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for sujeira e lixeira ESTIVER cheia
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {

                            this.locateNearestTrashAndCleanTrash();               
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for lixeira
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();
                            
                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = true;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                            {

                                this.locateNearestTrashAndCleanTrash();
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
                            
                        }
                        else 
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                    } else {
                        // ... verifica o lado esquerdo
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString().Trim() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString().Trim() == "D" && this.currentInternalTrash == 0)
                        {
                            this.locateNearestTrashAndCleanTrash();
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
                environment.printAgent(this);
                Console.WriteLine("\n");
            }
        }




        public Node move()
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

        private void locateNearestTrashAndCleanTrash()
        {
            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
            Node target;
            target = aStar.PathFindAStar(environment, begin, end);
            List<Node> listFathers = environment.createFatherList(target);
            List<Node> cloneListFathers = listFathers.ToList<Node>();
            Console.WriteLine("Going to trashDeposit located in [{0}][{1}]", end.XY.Line, end.XY.Column);
            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
            environment.moveAgentAroundEnvironment(this, listFathers, target);
            this.cleanInternalTrash();
            cloneListFathers.Reverse();
            Console.WriteLine("Returning from trashDeposit located in [{0}][{1}]", end.XY.Line, end.XY.Column);
            environment.moveAgentAroundEnvironment(this, cloneListFathers, target);
        }
    }
}
