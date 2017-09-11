using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Astar
    {
        //private String[,] map;
        private static List<Node> openList;
        private static List<Node> closedList;
        private static Dictionary<String, Node> initializedNodes;

        // True se encontrou o caminho
        // False otherwise
        public static Boolean PathFindAStar(String[,] map, Node begin, Node end)
        {
            //this.map = map;
            Int32 movementCost = 10;
            openList = new List<Node>();
            closedList = new List<Node>();
            initializedNodes = new Dictionary<string, Node>();

            // Calculate HCost
            begin.calculateHCost(begin, end);

            openList.Add(begin);


            while (true)
            {
                // se a lista aberta estiver vazia, não pudemos encontrar um caminho
                if (openList.Count == 0)
                {
                    Console.WriteLine("Caminho não encontrado.");
                    return false;
                }

                // ordena lista
                openList = openList.OrderBy(item => item.getFCost()).ToList();
                Console.WriteLine("Lista aberta: \n");
                foreach (Node nod in openList)
                {
                    Console.WriteLine(nod.id);
                }

                // pega o nó com menor custo F da lista
                Node current = openList.First();
                openList.Remove(current);

                // adiciona na lista fechada.
                closedList.Add(current);


                // Verifica se o current é o destino
                if (current.line == end.line && current.column == end.column)
                {
                    return true;
                }

                // Inicializa e carrega os vizinhos
                current.initializeNeighbors(map);

                // verifica se o vizinho já foi inicializado. se já foi, copia os valores para o vizinho atual.
                foreach (Node initialized in current.neighbors)
                {
                    if (initializedNodes.ContainsKey(initialized.id))
                    {
                        Node temp;
                        initializedNodes.TryGetValue(initialized.id, out temp);
                        copyValues(temp, initialized);
                    }
                }


                // processa todos os nós vizinhos
                foreach (Node neighbor in current.neighbors)
                {
                    Boolean estaNaListaFechada = false;
                    Boolean ehParedeOuSujeira = false;
                    // se o vizinho é parede ou sujeira, ou seja, está bloqueado
                    if (map[neighbor.line, neighbor.column].ToString().Trim() == "#" || map[neighbor.line, neighbor.column].ToString().Trim() == "D")
                    {
                        // muda flag ehParedeOuSujeira
                        ehParedeOuSujeira = true;
                    }

                    // se o vizinho está na lista fechada
                    if (closedList.Any(item => item.id == neighbor.id))
                    {
                        // muda flag estaNaListaFechada
                        estaNaListaFechada = true;
                    }

                    if (estaNaListaFechada == true || ehParedeOuSujeira == true)
                    {
                        // não faz nada e vai para o próximo vizinho.
                    }
                    else
                    {
                        // Calcula custo de moviment ( muda para 14 se estiver na diagonal )
                        if (current.line != neighbor.line && current.column != neighbor.column)
                        {
                            movementCost = 14;
                        }

                        // se o vizinho está na lista aberta, tenta melhorar o caminho até ele através do atual
                        if (openList.Any(item => item.id == neighbor.id))
                        {

                            Int32 betterGCost = current.Gcost + movementCost;
                            if (betterGCost < neighbor.Gcost)
                            {
                                neighbor.father = current;
                                neighbor.calculateGCost(betterGCost);
                                neighbor.calculateFCost();
                            }
                        }
                        else // se ele chegou aqui, é porque não está na lista aberta.
                        {
                            neighbor.father = current;
                            neighbor.calculateGCost(current.Gcost + movementCost);
                            neighbor.calculateHCost(neighbor, end);
                            neighbor.calculateFCost();
                            openList.Add(neighbor);
                        }
                        if (!initializedNodes.ContainsKey(neighbor.id))
                        {
                            initializedNodes.Add(neighbor.id, neighbor);
                        }
                    }

                }
            }
        }

        private static void copyValues(Node source, Node destiny)
        {
            destiny.father = source.father;
            destiny.Fcost = source.Fcost;
            destiny.Gcost = source.Gcost;
            destiny.Hcost = source.Hcost;
            destiny.neighbors = source.neighbors;
        }
    }
}
