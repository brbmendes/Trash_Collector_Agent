using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Node
    {
        public String id { get; set; }
        public Int32 numNeighbors;
        public Int32 line { get; set; }
        public Int32 column { get; set; }
        public Int32 Fcost;
        public Int32 Gcost;
        public Int32 Hcost;
        public Node father { get; set; }
        public List<Node> neighbors { get; set; }

        public Node(Int32 line, Int32 column)
        {
            this.line = line;
            this.column = column;
            this.father = null;
            this.Fcost = 0;
            this.Gcost = 0;
            this.Hcost = 0;
            this.id = String.Format("x{0}-y{1}", line, column);
        }

        public void initializeNeighbors(String[,] map)
        {
            Int32 lineActualNode = this.line;
            Int32 columnActualNode = this.column;
            this.neighbors = new List<Node>();

            if (lineActualNode == 0)
            {
                if (columnActualNode == 0)
                {
                    //if (Espaco.sala[lineActualNode][columnActualNode + 1] == '-') {
                    Node east = new Node(lineActualNode, columnActualNode + 1);
                    neighbors.Add(east);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node south = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(south);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode + 1] == '-') {
                    Node southEast = new Node(lineActualNode + 1, columnActualNode + 1);
                    neighbors.Add(southEast);
                    //}
                }
                else if (columnActualNode == Environment.sizeEnv - 1)
                {
                    //if (Espaco.sala[lineActualNode][columnActualNode - 1] == '-') {
                    Node oeste = new Node(lineActualNode, columnActualNode - 1);
                    neighbors.Add(oeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node sul = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(sul);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode - 1] == '-') {
                    Node sudoeste = new Node(lineActualNode + 1, columnActualNode - 1);
                    neighbors.Add(sudoeste);
                    //}
                }
                else
                {
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node sul = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(sul);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode + 1] == '-') {
                    Node sudeste = new Node(lineActualNode + 1, columnActualNode + 1);
                    neighbors.Add(sudeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode - 1] == '-') {
                    Node sudoeste = new Node(lineActualNode + 1, columnActualNode - 1);
                    neighbors.Add(sudoeste);
                    //}
                }
            }
            else if (lineActualNode == Environment.sizeEnv - 1)
            {
                if (columnActualNode == 0)
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode + 1] == '-') {
                    Node leste = new Node(lineActualNode, columnActualNode + 1);
                    neighbors.Add(leste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode + 1] == '-') {
                    Node nordeste = new Node(lineActualNode - 1, columnActualNode + 1);
                    neighbors.Add(nordeste);
                    //}
                }
                else if (columnActualNode == Environment.sizeEnv - 1)
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode - 1] == '-') {
                    Node oeste = new Node(lineActualNode, columnActualNode - 1);
                    neighbors.Add(oeste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode - 1] == '-') {
                    Node noroeste = new Node(lineActualNode - 1, columnActualNode - 1);
                    neighbors.Add(noroeste);
                    //}
                }
                else
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode + 1] == '-') {
                    Node nordeste = new Node(lineActualNode - 1, columnActualNode + 1);
                    neighbors.Add(nordeste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode - 1] == '-') {
                    Node noroeste = new Node(lineActualNode - 1, columnActualNode - 1);
                    neighbors.Add(noroeste);
                    //}
                }
            }
            else
            {
                if (columnActualNode == 0)
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node sul = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(sul);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode + 1] == '-') {
                    Node leste = new Node(lineActualNode, columnActualNode + 1);
                    neighbors.Add(leste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode + 1] == '-') {
                    Node nordeste = new Node(lineActualNode - 1, columnActualNode + 1);
                    neighbors.Add(nordeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode + 1] == '-') {
                    Node sudeste = new Node(lineActualNode + 1, columnActualNode + 1);
                    neighbors.Add(sudeste);
                    //}
                }
                else if (columnActualNode == Environment.sizeEnv - 1)
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node sul = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(sul);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode - 1] == '-') {
                    Node oeste = new Node(lineActualNode, columnActualNode - 1);
                    neighbors.Add(oeste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode - 1] == '-') {
                    Node noroeste = new Node(lineActualNode - 1, columnActualNode - 1);
                    neighbors.Add(noroeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode - 1] == '-') {
                    Node sudoeste = new Node(lineActualNode + 1, columnActualNode - 1);
                    neighbors.Add(sudoeste);
                    //}
                }
                else
                {
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode] == '-') {
                    Node norte = new Node(lineActualNode - 1, columnActualNode);
                    neighbors.Add(norte);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode] == '-') {
                    Node sul = new Node(lineActualNode + 1, columnActualNode);
                    neighbors.Add(sul);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode + 1] == '-') {
                    Node leste = new Node(lineActualNode, columnActualNode + 1);
                    neighbors.Add(leste);
                    //}
                    //if (Espaco.sala[lineActualNode][columnActualNode - 1] == '-') {
                    Node oeste = new Node(lineActualNode, columnActualNode - 1);
                    neighbors.Add(oeste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode + 1] == '-') {
                    Node nordeste = new Node(lineActualNode - 1, columnActualNode + 1);
                    neighbors.Add(nordeste);
                    //}
                    //if (Espaco.sala[lineActualNode - 1][columnActualNode - 1] == '-') {
                    Node noroeste = new Node(lineActualNode - 1, columnActualNode - 1);
                    neighbors.Add(noroeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode + 1] == '-') {
                    Node sudeste = new Node(lineActualNode + 1, columnActualNode + 1);
                    neighbors.Add(sudeste);
                    //}
                    //if (Espaco.sala[lineActualNode + 1][columnActualNode - 1] == '-') {
                    Node sudoeste = new Node(lineActualNode + 1, columnActualNode - 1);
                    neighbors.Add(sudoeste);
                    //}
                }
            }
        }

        public void setNumNeighbors()
        {
            this.numNeighbors = this.neighbors.Count;
        }

        public void calculateFCost()
        {
            this.Fcost = this.Gcost + this.Hcost;
        }

        public void calculateGCost(Node father, Int32 movementCost)
        {
            this.Gcost = father.Gcost + movementCost;
        }

        public void calculateGCost(Int32 Gcost)
        {
            this.Gcost = Gcost;
        }

        public void calculateHCost(Node begin, Node end)
        {
            Int32 absoluteX = Math.Abs(begin.line - end.line);
            Int32 absoluteY = Math.Abs(begin.column - end.column);

            if(absoluteX > absoluteY)
            {
                this.Hcost = 14 * absoluteY + 10 * (absoluteX - absoluteY);
            }
            else
            {
                this.Hcost = 14 * absoluteX + 10 * (absoluteY - absoluteX);
            }
        }

        public Int32 getFCost()
        {
            return this.Fcost;
        }


    }


}

