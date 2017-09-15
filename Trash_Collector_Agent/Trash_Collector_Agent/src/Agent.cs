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
        /// Last Position agent
        /// </summary>
        public Position LastPosition { get; set; }

        /// <summary>
        /// Current position agent
        /// </summary>
        public Position CurrentPosition { get; set; }

        /// <summary>
        /// Next position agent
        /// </summary>
        public Position NextPosition { get; set; }

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

        /// <summary>
        /// Position agent
        /// </summary>
        public Position XY { get; set; }

        /// <summary>
        /// Environment
        /// </summary>
        public Environment environment { get; set; }

        /// <summary>
        /// Astar algorithm
        /// </summary>
        public Astar aStar { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalTrash"></param>
        public Agent(Int32 internalTrash)
        {
            this.XY = new Position(0, 0);
            initializePositions(XY);
            this.updatePosition(XY, XY, XY);
            this.capacityInternalTrash = internalTrash;
            this.currentInternalTrash = internalTrash;
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
            while (this.CurrentPosition.Line <= environment.Size - 1)
            {
                //this.capacityInternalTrash = 0;
                Console.WriteLine("\n");
                Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                environment.showEnvironment();

                if(environment.Size % 2 == 0)
                {
                    if(this.CurrentPosition.Line == 0 && this.CurrentPosition.Column == 0)
                    {
                        // Chegou na posicao final
                        Console.WriteLine("Fim do programa.");
                        break;
                    }
                }
                else
                {
                    if (this.CurrentPosition.Line == environment.Size - 1 && this.CurrentPosition.Column == environment.Size - 1)
                    {
                        // Chegou na posicao final
                        Console.WriteLine("Fim do programa.");
                        break;
                    }
                }

                if (right)
                {
                    if (this.CurrentPosition.Column == environment.Size - 1) // se for a última coluna
                    {
                        if (this.CurrentPosition.Line == environment.Size) break;
                        // ... verifica posicao abaixo
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                        {
                            Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for sujeira
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column - 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();

                            // se for sujeira e lixeira NÃO estiver cheia
                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = false;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                            {

                                Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                if (!foundPathToNearestTrash)
                                {
                                    System.Environment.Exit(1);
                                }
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
                        else
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                    }
                    else // NÃO É A ÚLTIMA COLUNA, E ESTÁ ANDANDO PARA A DIREITA.
                    {
                        // ... verifica o lado direito
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1) == "D" && this.currentInternalTrash > 0)
                        {

                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1) == "D" && this.currentInternalTrash == 0) // se for sujeira e estiver cheio
                        {
                            Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1) == "#" || environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1) == "T") // se achou parede ou lixeira
                        {

                            if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1) == "T" && CurrentPosition.Column + 1 == environment.Size - 1)
                            {
                                if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1) == "D" && this.currentInternalTrash > 0)
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column + 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = false;
                                }
                                // se for sujeira e estiver cheio
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1) == "D" && this.currentInternalTrash == 0)
                                {

                                    Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                    if (!foundPathToNearestTrash)
                                    {
                                        System.Environment.Exit(1);
                                    }
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column + 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = false;
                                }
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1) == "T")
                                {
                                    if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = false;
                                    }
                                    else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                                    {

                                        Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            System.Environment.Exit(1);
                                        }
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = false;
                                    }
                                    else
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = false;
                                    }
                                }
                                else
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = false;
                                }
                            }
                            else
                            {
                                Position targetPosition = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                                while (environment.Map.GetValue(targetPosition.Line, targetPosition.Column) == "#" || environment.Map.GetValue(targetPosition.Line, targetPosition.Column) == "T")
                                { // enquanto achar parede ou lixeira, incrementa posicao
                                    targetPosition.Column++;
                                }
                                if (environment.Map.GetValue(targetPosition.Line, targetPosition.Column) == "D") // achou lixo
                                {
                                    Boolean movedRightDiagonal = false;
                                    if (environment.Map.GetValue(targetPosition.Line - 1, targetPosition.Column) != "-")
                                    {
                                        targetPosition.Column = targetPosition.Column + 1;
                                        movedRightDiagonal = true;
                                    }
                                    Position futurePositionRobot = new Position(targetPosition.Line - 1, targetPosition.Column);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = this.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        System.Environment.Exit(1);
                                    }

                                    Console.WriteLine("\n");
                                    Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                                    environment.showEnvironment();

                                    if (this.currentInternalTrash > 0) // se não estiver cheio
                                    {
                                        if (movedRightDiagonal)
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column - 1);
                                            this.updateCurrentPosition(pos);
                                        }
                                        else
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column);
                                            this.updateCurrentPosition(pos);
                                        }
                                    }
                                    else  // se estiver cheio
                                    {
                                        Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            System.Environment.Exit(1);
                                        }
                                        if (movedRightDiagonal)
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column - 1);
                                            this.updateCurrentPosition(pos);
                                        }
                                        else
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column);
                                            this.updateCurrentPosition(pos);
                                        }
                                    }
                                }
                                else
                                {
                                    Position futurePositionRobot = new Position(targetPosition.Line, targetPosition.Column);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = this.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        System.Environment.Exit(1);
                                    }
                                }
                            }
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

                    if (this.CurrentPosition.Column == 0)
                    {
                        // ... verifica posicao abaixo

                        // se for sujeira e lixeira NÃO estiver cheia
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for sujeira e lixeira ESTIVER cheia
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                        {

                            Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for lixeira
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();

                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = true;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                            {

                                Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                if (!foundPathToNearestTrash)
                                {
                                    System.Environment.Exit(1);
                                }
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
                    }
                    else
                    {
                        // ... verifica o lado esquerdo
                        if (this.CurrentPosition.Column == environment.Size) break;
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1) == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1) == "D" && this.currentInternalTrash == 0)
                        {
                            Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1) == "#" || environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1) == "T") // se achou parede ou lixeira
                        {
                            // verifica se é lixeira, e está na grudada na parede externa
                            if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1) == "T" && CurrentPosition.Column - 1 == 0)
                            {
                                if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1) == "D" && this.currentInternalTrash > 0)
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = true;
                                }
                                // se for sujeira e estiver cheio
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1) == "D" && this.currentInternalTrash == 0)
                                {

                                    Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                    if (!foundPathToNearestTrash)
                                    {
                                        System.Environment.Exit(1);
                                    }
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = true;
                                }
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1) == "T")
                                {
                                    if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash > 0)
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = true;
                                    }
                                    else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column) == "D" && this.currentInternalTrash == 0)
                                    {

                                        Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            System.Environment.Exit(1);
                                        }
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = true;
                                    }
                                    else
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = true;
                                    }
                                }
                                else
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = true;
                                }
                            }
                            else
                            {
                                Position targetPosition = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column - 1);
                                while (environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1) == "#" || environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1) == "T")
                                { // enquanto achar parede ou lixeira, decrementa posicao
                                    targetPosition.Column--;
                                }
                                if (environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1) == "D") // achou lixo
                                {
                                    Boolean movedLeftDiagonal = false;
                                    if (environment.Map.GetValue(targetPosition.Line - 1, targetPosition.Column - 1) != "-")
                                    {
                                        targetPosition.Column = targetPosition.Column - 1;
                                        movedLeftDiagonal = true;
                                    }
                                    Position futurePositionRobot = new Position(targetPosition.Line - 1, targetPosition.Column - 1);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = this.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        System.Environment.Exit(1);
                                    }

                                    Console.WriteLine("\n");
                                    Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                                    environment.showEnvironment();

                                    if (this.currentInternalTrash > 0) // se não estiver cheio
                                    {
                                        if (movedLeftDiagonal)
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column + 1);
                                            this.updateCurrentPosition(pos);
                                        }
                                        else
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column);
                                            this.updateCurrentPosition(pos);
                                        }
                                    }
                                    else  // se estiver cheio
                                    {
                                        Boolean foundPathToNearestTrash = this.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            System.Environment.Exit(1);
                                        }
                                        if (movedLeftDiagonal)
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column + 1);
                                            this.updateCurrentPosition(pos);
                                        }
                                        else
                                        {
                                            this.collectTrash();
                                            this.updateLastPosition(this.CurrentPosition);
                                            Position pos = new Position(this.CurrentPosition.Line + 1, this.CurrentPosition.Column);
                                            this.updateCurrentPosition(pos);
                                        }
                                    }
                                }
                                else
                                {
                                    Position futurePositionRobot = new Position(targetPosition.Line, targetPosition.Column-1);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = this.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        System.Environment.Exit(1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
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

        private Boolean locateNearestTrashAndCleanTrash()
        {
            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
            Position nearestTrash = this.calculateNearestTrash(this.CurrentPosition);
            Node end = new Node(nearestTrash.Line, nearestTrash.Column);
            Node targetNode;
            targetNode = aStar.PathFindAStar(environment, begin, end);
            if (targetNode == null)
            {
                Console.WriteLine("Way to nearest trash is impossible or is blocked.");
                Console.WriteLine("Robot position: [{0},{1}]", this.CurrentPosition.Line, this.CurrentPosition.Column);
                Console.WriteLine("Target position: [{0},{1}]", end.XY.Line, end.XY.Column);
                return false;
            }
            List<Node> listFathers = environment.createFatherList(targetNode);
            List<Node> cloneListFathers = listFathers.ToList<Node>();
            Console.WriteLine("Going to trashDeposit located in [{0}][{1}]", end.XY.Line, end.XY.Column);
            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
            environment.moveAgentAroundEnvironment(this, listFathers, targetNode);
            this.cleanInternalTrash();
            cloneListFathers.Reverse();
            Console.WriteLine("Returning from trashDeposit located in [{0}][{1}]", end.XY.Line, end.XY.Column);
            environment.moveAgentAroundEnvironment(this, cloneListFathers, targetNode);
            return true;
        }

        private Boolean locatePathToBlankSpotAfterWall(Position targetPosition)
        {
            Node begin = new Node(this.CurrentPosition.Line, this.CurrentPosition.Column);
            Node end = new Node(targetPosition.Line, targetPosition.Column);
            Node targetNode;
            targetNode = aStar.PathFindAStar(environment, begin, end);
            if (targetNode == null)
            {
                Console.WriteLine("Way to nearest blank space after wall is impossible or is blocked.");
                Console.WriteLine("Robot position: [{0},{1}]", this.CurrentPosition.Line, this.CurrentPosition.Column);
                Console.WriteLine("Target position: [{0},{1}]", end.XY.Line, end.XY.Column);
                return false;
            }
            List<Node> listFathers = environment.createFatherList(targetNode);
            Console.WriteLine("Going to blank space located in [{0}][{1}]", end.XY.Line, end.XY.Column);
            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
            environment.moveAgentAroundEnvironment(this, listFathers, targetNode);
            return true;
        }
    }
}
