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
        public Int32 Size { get; set; }

        /// <summary>
        /// Map declaration
        /// </summary>
        public String[,] Map;

        /// <summary>
        /// Agent
        /// </summary>
        public Agent robot;

        /// <summary>
        /// List of trash deposit points
        /// </summary>
        List<Trash_deposit> trashDeposits;

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
        /// Environment constructor method
        /// </summary>
        /// <param name="size">Size of environment</param>
        /// <param name="robot">Agent</param>
        /// <param name="qtdTrashDeposits">Quantity of trash deposits</param>
        /// <param name="percentDirty">Percentual of dirty environment</param>
        public Environment(Int32 size, Agent robot, Int32 qtdTrashDeposits, Int32 percentDirty)
        {
            this.Size = size;
            this.Map = new String[this.Size, this.Size];
            this.robot = robot;
            this.qtdTrashDeposits = qtdTrashDeposits;
            this.percentDirty = percentDirty;
            this.walls = new HashSet<Wall>();
            this.dirties = new List<Dirty>();
            this.trashDeposits = new List<Trash_deposit>();
        }

        /// <summary>
        /// Initialize map with empty blocks
        /// </summary>
        public void initializeMap()
        {
            
            for (Int32 i = 0; i < this.Map.GetLength(0); i++)
            {
                for (Int32 j = 0; j < this.Map.GetLength(1); j++)
                {
                    this.Map.SetValue("-", i, j);
                }
            }
            
        }

        /// <summary>
        /// Initialize Agent start position;
        /// </summary>
        public void positioningAgent(Agent robot)
        {
            this.Map.SetValue("A", robot.CurrentPosition.Line, robot.CurrentPosition.Column);
        }

        /// <summary>
        /// Show environment on screen
        /// </summary>
        public void showEnvironment()
        {
            for (Int32 i = 0; i < this.Size; i++)
            {
                for (Int32 j = 0; j < this.Size; j++)
                {
                    Console.Write(String.Format(" {0} ", this.Map.GetValue(i, j)));
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
            Double oneThirdSize = (Math.Truncate((this.Size/3)-0.6)+1);
            Double twoThirdsSize = (0.66666666667 * this.Size) + 0.77777777779;

            #region left quadrant - HORIZONTAL
            while(y < oneThirdSize)
            {
                this.Map.SetValue("#",0+2,y);
                Position wallPosition = new Position(0 + 2,y);
                this.walls.Add(new Wall(wallPosition));
                y++;
            }
            
            y = 2;
            
            while (y < oneThirdSize)
            {
                this.Map.SetValue("#",this.Size-3,y);
                Position wallPosition = new Position(this.Size - 3, y);
                this.walls.Add(new Wall(wallPosition));
                y++;
            }

            #endregion


            #region right quadrant - HORIZONTAL
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            while(y < this.Size -2)
            {
                this.Map.SetValue("#",0+2,y);
                Position wallPosition = new Position(0 + 2, y);
                this.walls.Add(new Wall(wallPosition));
                y++;
            }

            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));
            while(y < this.Size -2)
            {
                this.Map.SetValue("#",this.Size-3,y);
                Position wallPosition = new Position(this.Size - 3, y);
                this.walls.Add(new Wall(wallPosition));
                y++;
            }
            
            #endregion	
            
            
            #region left quadrant - VERTICAL

            y = 3;
            while (y < oneThirdSize)
            {
                x = 2;
                do
                {
                    x++;
                    this.Map.SetValue("#", x, y);
                    Position wallPosition = new Position(x, y);
                    this.walls.Add(new Wall(wallPosition));
                }
                while(x < this.Size-3);
                y++;
                    
            }
            
            #endregion


            #region right quadrant - VERTICAL
            y = Convert.ToInt32(Math.Truncate(twoThirdsSize));

            while(y < this.Size-3)
            {
                x = 2;
                do
                {
                    x++;
                    this.Map.SetValue("#", x, y);
                    Position wallPosition = new Position(x, y);
                    this.walls.Add(new Wall(wallPosition));
                }
                while(x < this.Size-3);
                y++;
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
            while (this.trashDeposits.Count != leftTrashDeposits)
            {
                Position p = new Position(rnd.Next(3, this.Size - 3), rnd.Next(0, 3));
                Trash_deposit deposit = new Trash_deposit(p);
                if (deposit.XY.Column != 1)
                {
                    if (this.Map.GetValue(deposit.XY.Line, deposit.XY.Column).ToString() == "-")
                    {
                        this.Map.SetValue("T", deposit.XY.Line, deposit.XY.Column);
                        this.trashDeposits.Add(deposit);
                    }
                }
            }
            
            #endregion  

            #region right quadrant TrashDeposits
            while (this.trashDeposits.Count != leftTrashDeposits+rightTrashDeposits)
            {
                Position p = new Position(rnd.Next(3, this.Size - 3), rnd.Next(this.Size - 3, this.Size));
                Trash_deposit deposit = new Trash_deposit(p);
                if (deposit.XY.Column != this.Size - 2)
                {
                    if (this.Map.GetValue(deposit.XY.Line, deposit.XY.Column).ToString() == "-")
                    {
                        this.Map.SetValue("T", deposit.XY.Line, deposit.XY.Column);
                        this.trashDeposits.Add(deposit);
                    }
                }
            }
            #endregion

            // Passing trashDeposits list to Agent.
            this.robot.setTrashDeposits(this.trashDeposits);
        }

        public void buildDirtyEnvironment()
        {
            Double percentDirtyConverted = this.percentDirty * 0.01;
            Int32 freeBlocks = this.Map.Length - this.walls.Count - this.qtdTrashDeposits;
            Int32 numberOfDirties = Convert.ToInt32(freeBlocks * percentDirtyConverted);
            Random rnd = new Random();

            while (this.dirties.Count != numberOfDirties)
            {
                Position p = new Position(rnd.Next(0, this.Size), rnd.Next(0, this.Size));
                Dirty dirty = new Dirty(p);
                if (this.Map.GetValue(dirty.XY.Line, dirty.XY.Column).ToString() == "-")
                {
                    this.Map.SetValue("D", dirty.XY.Line, dirty.XY.Column);
                    this.dirties.Add(dirty);
                }
            }
        }

        public List<Node> createFatherList(Node node)
        {
            List<Node> list = new List<Node>();
            list.Add(node);
            privateCreateFatherList(node, list);
            list.Reverse();
            return list;
        }


        private Node privateCreateFatherList(Node node, List<Node> path)
        {
            if (node.Father == null)
            {
                return null;
            }
            path.Add(node.Father);
            return privateCreateFatherList(node.Father, path);
        }

        

        public void moveAgentAroundEnvironment(Agent robot, List<Node> listPosition, Node target)
        {

            listPosition.RemoveAt(0); // remove a primeira posição da lista, pois é a mesma que a posição atual do agente
            int count = 0;
            while (listPosition.Count != 0)
            {
                if(count == 0)
                {
                    Node nextPosition = listPosition.First();
                    listPosition.RemoveAt(0);
                    Boolean hasNextNextPosition = false;
                    if(listPosition.Count > 0)
                    {
                        Node nextNextPosition = listPosition.First();
                        Position lastPosition = robot.CurrentPosition;
                        robot.updatePosition(lastPosition, nextPosition.XY, nextNextPosition.XY);
                        hasNextNextPosition = true;
                    }
                    else
                    {
                        robot.updateLastPosition(robot.CurrentPosition);
                        robot.updateCurrentPosition(nextPosition.XY);
                    }                    
                    if (nextPosition.XY.Line == target.XY.Line && nextPosition.XY.Column == target.XY.Column)
                    {
                        this.printInfo(hasNextNextPosition, target);
                        if (this.Map.GetValue(target.XY.Line, target.XY.Column).ToString() == "-")
                        {
                            robot.updateCurrentPosition(target.XY);
                            printAgent(robot);
                        }
                        this.showEnvironment();
                    }
                    else
                    {
                        count++;
                        this.printAgent(robot);
                        this.printInfo(true, target);
                        this.showEnvironment();
                    }
                } 
                else
                {
                    Node nextPosition = listPosition.First();
                    listPosition.RemoveAt(0);
                    Boolean hasNextNextPosition = false;
                    if (listPosition.Count > 0)
                    {
                        Node nextNextPosition = listPosition.First();
                        Position lastPosition = robot.CurrentPosition;
                        robot.updatePosition(lastPosition, nextPosition.XY, nextNextPosition.XY);
                        hasNextNextPosition = true;
                    }
                    else
                    {
                        robot.updateLastPosition(robot.CurrentPosition);
                        robot.updateCurrentPosition(nextPosition.XY);
                    }

                    if (nextPosition.XY.Line == target.XY.Line && nextPosition.XY.Column == target.XY.Column)
                    {
                        this.printInfo(hasNextNextPosition, target);
                        if (this.Map.GetValue(target.XY.Line, target.XY.Column).ToString() == "-")
                        {
                            robot.updateCurrentPosition(target.XY);
                            printAgent(robot);
                        }
                        this.showEnvironment();
                    }
                    else
                    {
                        this.printAgent(robot);
                        this.printInfo(true, target);
                        this.showEnvironment();
                    }
                }
                
            }
            
        }

        public void printAgent(Agent robot)
        {
            this.Map.SetValue("-", robot.LastPosition.Line, robot.LastPosition.Column);
            this.Map.SetValue("A", robot.CurrentPosition.Line, robot.CurrentPosition.Column);
        }

        public void printInfo(Boolean hasNext, Node target)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Agent last position: \t[{0}][{1}]", robot.LastPosition.Line, robot.LastPosition.Column);
            Console.WriteLine("Agent current position: [{0}][{1}]", robot.CurrentPosition.Line, robot.CurrentPosition.Column);
            if (hasNext)
            {
                Console.WriteLine("Agent next position: \t[{0}][{1}]", robot.NextPosition.Line, robot.NextPosition.Column);
            }
            Console.WriteLine("Agent target: \t\t[{0}][{1}]", target.XY.Line, target.XY.Column);
        }

        public void printPath(Position pos)
        {
            this.Map.SetValue("P", pos.Line, pos.Column);
        }

    }
}
