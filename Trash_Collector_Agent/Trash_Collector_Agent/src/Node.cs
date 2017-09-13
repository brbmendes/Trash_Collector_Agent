using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Node
    {
        public Position XY { get; set; }
        public String Id { get; set; }
        public Int32 Fcost;
        public Int32 Gcost;
        public Int32 Hcost;
        public Node Father { get; set; }
        public List<Node> Neighbors { get; set; }

        public Node(Int32 line, Int32 column)
        {
            this.XY = new Position(line, column);
            this.Father = null;
            this.Fcost = 0;
            this.Gcost = 0;
            this.Hcost = 0;
            this.Id = String.Format("[{0},{1}]", line, column);
        }

        public Node(Position position)
        {
            this.XY.Line = position.Line;
            this.XY.Column = position.Column;
            this.Father = null;
            this.Fcost = 0;
            this.Gcost = 0;
            this.Hcost = 0;
            this.Id = String.Format("[{0},{1}]", this.XY.Line, this.XY.Column);
        }

        public void initializeNeighbors(Environment environment)
        {
            this.Neighbors = new List<Node>();

            if (this.XY.Line == 0)
            {
                if (this.XY.Column == 0)
                {
                    Node east = new Node(this.XY.Line, this.XY.Column + 1);
                    Neighbors.Add(east);

                    Node south = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(south);

                    Node southEast = new Node(this.XY.Line + 1, this.XY.Column + 1);
                    Neighbors.Add(southEast);
                }
                else if (this.XY.Column == environment.Size - 1)
                {
                    Node oeste = new Node(this.XY.Line, this.XY.Column - 1);
                    Neighbors.Add(oeste);

                    Node sul = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(sul);

                    Node sudoeste = new Node(this.XY.Line + 1, this.XY.Column - 1);
                    Neighbors.Add(sudoeste);
                }
                else
                {
                    Node sul = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(sul);
                    
                    Node sudeste = new Node(this.XY.Line + 1, this.XY.Column + 1);
                    Neighbors.Add(sudeste);

                    Node sudoeste = new Node(this.XY.Line + 1, this.XY.Column - 1);
                    Neighbors.Add(sudoeste);
                }
            }
            else if (this.XY.Line == environment.Size - 1)
            {
                if (this.XY.Column == 0)
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node leste = new Node(this.XY.Line, this.XY.Column + 1);
                    Neighbors.Add(leste);

                    Node nordeste = new Node(this.XY.Line - 1, this.XY.Column + 1);
                    Neighbors.Add(nordeste);
                }
                else if (this.XY.Column == environment.Size - 1)
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node oeste = new Node(this.XY.Line, this.XY.Column - 1);
                    Neighbors.Add(oeste);

                    Node noroeste = new Node(this.XY.Line - 1, this.XY.Column - 1);
                    Neighbors.Add(noroeste);
                }
                else
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node nordeste = new Node(this.XY.Line - 1, this.XY.Column + 1);
                    Neighbors.Add(nordeste);

                    Node noroeste = new Node(this.XY.Line - 1, this.XY.Column - 1);
                    Neighbors.Add(noroeste);
                }
            }
            else
            {
                if (this.XY.Column == 0)
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node sul = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(sul);

                    Node leste = new Node(this.XY.Line, this.XY.Column + 1);
                    Neighbors.Add(leste);

                    Node nordeste = new Node(this.XY.Line - 1, this.XY.Column + 1);
                    Neighbors.Add(nordeste);

                    Node sudeste = new Node(this.XY.Line + 1, this.XY.Column + 1);
                    Neighbors.Add(sudeste);
                }
                else if (this.XY.Column == environment.Size - 1)
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node sul = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(sul);

                    Node oeste = new Node(this.XY.Line, this.XY.Column - 1);
                    Neighbors.Add(oeste);

                    Node noroeste = new Node(this.XY.Line - 1, this.XY.Column - 1);
                    Neighbors.Add(noroeste);

                    Node sudoeste = new Node(this.XY.Line + 1, this.XY.Column - 1);
                    Neighbors.Add(sudoeste);
                }
                else
                {
                    Node norte = new Node(this.XY.Line - 1, this.XY.Column);
                    Neighbors.Add(norte);

                    Node sul = new Node(this.XY.Line + 1, this.XY.Column);
                    Neighbors.Add(sul);

                    Node leste = new Node(this.XY.Line, this.XY.Column + 1);
                    Neighbors.Add(leste);

                    Node oeste = new Node(this.XY.Line, this.XY.Column - 1);
                    Neighbors.Add(oeste);

                    Node nordeste = new Node(this.XY.Line - 1, this.XY.Column + 1);
                    Neighbors.Add(nordeste);

                    Node noroeste = new Node(this.XY.Line - 1, this.XY.Column - 1);
                    Neighbors.Add(noroeste);

                    Node sudeste = new Node(this.XY.Line + 1, this.XY.Column + 1);
                    Neighbors.Add(sudeste);

                    Node sudoeste = new Node(this.XY.Line + 1, this.XY.Column - 1);
                    Neighbors.Add(sudoeste);
                }
            }
        }

        public void calculateFCost()
        {
            this.Fcost = this.Gcost + this.Hcost;
        }

        public Int32 getFCost()
        {
            return this.Fcost;
        }

        public void calculateGCost(Node father, Int32 movementCost)
        {
            this.Gcost = father.Gcost + movementCost;
        }

        public Int32 getGCost()
        {
            return this.Gcost;
        }

        public void setGCost(Int32 Gcost)
        {
            this.Gcost = Gcost;
        }

        public void calculateHCost(Node begin, Node end)
        {
            Int32 absoluteX = Math.Abs(begin.XY.Line - end.XY.Line);
            Int32 absoluteY = Math.Abs(begin.XY.Column - end.XY.Column);

            if(absoluteX > absoluteY)
            {
                this.Hcost = 14 * absoluteY + 10 * (absoluteX - absoluteY);
            }
            else
            {
                this.Hcost = 14 * absoluteX + 10 * (absoluteY - absoluteX);
            }
        }

        public Int32 getHCost()
        {
            return this.Hcost;
        }

        


    }


}

