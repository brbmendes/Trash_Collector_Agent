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
        public Int32 capacityInternalTrash { get; set; }

        /// <summary>
        /// Current agent internal trash
        /// </summary>
        public Int32 currentInternalTrash { get; set; }

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
        public List<Trash_deposit> trashDeposits { get; set; }

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
                    if (this.CurrentPosition.Line == environment.Size - 1 && this.CurrentPosition.Column == 0)
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
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = false;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                        {
                            Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                Console.ReadKey();
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
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column - 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();

                            // se for sujeira e lixeira NÃO estiver cheia
                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = false;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                            {

                                Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                if (!foundPathToNearestTrash)
                                {
                                    Console.ReadKey();
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
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString() == "D" && this.currentInternalTrash > 0)
                        {

                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString() == "D" && this.currentInternalTrash == 0) // se for sujeira e estiver cheio
                        {
                            Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                Console.ReadKey();
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.updateLastPosition(this.CurrentPosition);
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateCurrentPosition(pos);
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString() == "#" || environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString() == "T") // se achou parede ou lixeira
                        {

                            if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column + 1).ToString() == "T" && CurrentPosition.Column + 1 == environment.Size - 1)
                            {
                                if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1).ToString() == "D" && this.currentInternalTrash > 0)
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column + 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = false;
                                }
                                // se for sujeira e estiver cheio
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1).ToString() == "D" && this.currentInternalTrash == 0)
                                {

                                    Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                    if (!foundPathToNearestTrash)
                                    {
                                        Console.ReadKey();
                                        System.Environment.Exit(1);
                                    }
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column + 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = false;
                                }
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column + 1).ToString() == "T")
                                {
                                    if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = false;
                                    }
                                    else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                                    {

                                        Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            Console.ReadKey();
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
                                while (environment.Map.GetValue(targetPosition.Line, targetPosition.Column).ToString() == "#" || environment.Map.GetValue(targetPosition.Line, targetPosition.Column).ToString() == "T")
                                { // enquanto achar parede ou lixeira, incrementa posicao
                                    targetPosition.Column++;
                                }
                                if (environment.Map.GetValue(targetPosition.Line, targetPosition.Column).ToString().ToString() == "D") // achou lixo
                                {
                                    Boolean movedRightDiagonal = false;
                                    if (environment.Map.GetValue(targetPosition.Line - 1, targetPosition.Column).ToString() != "-")
                                    {
                                        targetPosition.Column = targetPosition.Column + 1;
                                        movedRightDiagonal = true;
                                    }
                                    Position futurePositionRobot = new Position(targetPosition.Line - 1, targetPosition.Column);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = aStar.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        Console.ReadKey();
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
                                        Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            Console.ReadKey();
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
                                    Boolean foundPathToNearestBlankSpaceAfterWall = aStar.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        Console.ReadKey();
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
                        if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.CurrentPosition.Line += 1;
                            if (this.CurrentPosition.Line == environment.Size) break;
                            right = true;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                        }
                        // se for sujeira e lixeira ESTIVER cheia
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                        {

                            Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                Console.ReadKey();
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
                        else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "T")
                        {
                            Position pos = new Position(this.CurrentPosition.Line, this.CurrentPosition.Column + 1);
                            this.updateLastPosition(this.CurrentPosition);
                            this.CurrentPosition = pos;
                            Console.WriteLine("\n");
                            Console.WriteLine("Current internal trash capacity: {0}", this.currentInternalTrash);
                            environment.printAgent(this);
                            environment.showEnvironment();

                            if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                            {
                                this.collectTrash();
                                this.LastPosition.Line = this.CurrentPosition.Line;
                                this.CurrentPosition.Line += 1;
                                if (this.CurrentPosition.Line == environment.Size) break;
                                right = true;
                                this.LastPosition.Column = this.CurrentPosition.Column;
                            }
                            // se for sujeira e lixeira ESTIVER cheia
                            else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                            {

                                Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                if (!foundPathToNearestTrash)
                                {
                                    Console.ReadKey();
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
                        if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString() == "D" && this.currentInternalTrash > 0)
                        {
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString() == "D" && this.currentInternalTrash == 0)
                        {
                            Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                            if (!foundPathToNearestTrash)
                            {
                                Console.ReadKey();
                                System.Environment.Exit(1);
                            }
                            this.collectTrash();
                            this.LastPosition.Line = this.CurrentPosition.Line;
                            this.LastPosition.Column = this.CurrentPosition.Column;
                            this.CurrentPosition.Column -= 1;
                        }
                        else if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString() == "#" || environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString() == "T") // se achou parede ou lixeira
                        {
                            // verifica se é lixeira, e está na grudada na parede externa
                            if (environment.Map.GetValue(CurrentPosition.Line, CurrentPosition.Column - 1).ToString() == "T" && CurrentPosition.Column - 1 == 0)
                            {
                                if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1).ToString() == "D" && this.currentInternalTrash > 0)
                                {
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = true;
                                }
                                // se for sujeira e estiver cheio
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1).ToString() == "D" && this.currentInternalTrash == 0)
                                {

                                    Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                    if (!foundPathToNearestTrash)
                                    {
                                        Console.ReadKey();
                                        System.Environment.Exit(1);
                                    }
                                    Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column - 1);
                                    this.collectTrash();
                                    this.updateLastPosition(this.CurrentPosition);
                                    this.updateCurrentPosition(pos);
                                    if (this.CurrentPosition.Line == environment.Size) break;
                                    right = true;
                                }
                                else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column - 1).ToString() == "T")
                                {
                                    if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash > 0)
                                    {
                                        Position pos = new Position(CurrentPosition.Line + 1, CurrentPosition.Column);
                                        this.collectTrash();
                                        this.updateLastPosition(this.CurrentPosition);
                                        this.updateCurrentPosition(pos);
                                        if (this.CurrentPosition.Line == environment.Size) break;
                                        right = true;
                                    }
                                    else if (environment.Map.GetValue(CurrentPosition.Line + 1, CurrentPosition.Column).ToString() == "D" && this.currentInternalTrash == 0)
                                    {

                                        Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            Console.ReadKey();
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
                                while (environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1).ToString() == "#" || environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1).ToString() == "T")
                                { // enquanto achar parede ou lixeira, decrementa posicao
                                    targetPosition.Column--;
                                }
                                if (environment.Map.GetValue(targetPosition.Line, targetPosition.Column - 1).ToString() == "D") // achou lixo
                                {
                                    Boolean movedLeftDiagonal = false;
                                    if (environment.Map.GetValue(targetPosition.Line - 1, targetPosition.Column - 1).ToString() != "-")
                                    {
                                        targetPosition.Column = targetPosition.Column - 1;
                                        movedLeftDiagonal = true;
                                    }
                                    Position futurePositionRobot = new Position(targetPosition.Line - 1, targetPosition.Column - 1);
                                    Boolean foundPathToNearestBlankSpaceAfterWall = aStar.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        Console.ReadKey();
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
                                        Boolean foundPathToNearestTrash = aStar.locateNearestTrashAndCleanTrash();
                                        if (!foundPathToNearestTrash)
                                        {
                                            Console.ReadKey();
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
                                    Boolean foundPathToNearestBlankSpaceAfterWall = aStar.locatePathToBlankSpotAfterWall(futurePositionRobot);
                                    if (!foundPathToNearestBlankSpaceAfterWall)
                                    {
                                        Console.ReadKey();
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
    }
}
