using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Astar
    {
        private Environment env { get; set; }
        //private String[,] map;
        private List<Node> openList;
        private List<Node> closedList;
        private Dictionary<String, Node> initializedNodes;

        public Astar(Environment environment)
        {
            this.env = environment;
        }

        // True se encontrou o caminho
        // False otherwise
        public Node PathFindAStar(String[,] map, Node begin, Node end)
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
                    return null;
                }

                // ordena lista
                openList = openList.OrderBy(item => item.getFCost()).ToList();


                // pega o nó com menor custo F da lista
                Node current = openList.First();
                openList.Remove(current);

                // adiciona na lista fechada.
                closedList.Add(current);

                // Verifica se o current é o destino
                if (current.XY.Line == end.XY.Line && current.XY.Column == end.XY.Column)
                {
                    return current;
                }

                // Inicializa e carrega os vizinhos
                current.initializeNeighbors(env);

                // verifica se o vizinho já foi inicializado. se já foi, copia os valores para o vizinho atual.
                foreach (Node initialized in current.Neighbors)
                {
                    if (initializedNodes.ContainsKey(initialized.Id))
                    {
                        Node temp;
                        initializedNodes.TryGetValue(initialized.Id, out temp);
                        copyValues(temp, initialized);
                    }
                }


                // processa todos os nós vizinhos
                foreach (Node neighbor in current.Neighbors)
                {
                    Boolean estaNaListaFechada = false;
                    Boolean ehParedeOuSujeira = false;
                    // se o vizinho é parede ou sujeira, ou seja, está bloqueado
                    // Tudo que não for "-" é caminho bloqueado, exceto se for o nodo destino.
                    if (map[neighbor.XY.Line, neighbor.XY.Column].ToString().Trim() == "#" || map[neighbor.XY.Line, neighbor.XY.Column].ToString().Trim() == "D" || map[neighbor.XY.Line, neighbor.XY.Column].ToString().Trim() == "R" || map[neighbor.XY.Line, neighbor.XY.Column].ToString().Trim() == "T")
                    {
                        if (end.XY.Line == neighbor.XY.Line && end.XY.Column == neighbor.XY.Column)
                        {
                                // Se o vizinho for o destino, não faz nada.
                            } else {
                                ehParedeOuSujeira = true;
                            }               
                    }

                    // se o vizinho está na lista fechada
                    if (closedList.Any(item => item.Id == neighbor.Id))
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
                        if (current.XY.Line != neighbor.XY.Line && current.XY.Column != neighbor.XY.Column)
                        {
                            movementCost = 14;
                        }
                            // se o vizinho está na lista aberta, tenta melhorar o caminho até ele através do atual
                            if (openList.Any(item => item.Id == neighbor.Id))
                            {

                                Int32 betterGCost = current.Gcost + movementCost;
                                if (betterGCost < neighbor.Gcost)
                                {
                                    neighbor.Father = current;
                                    neighbor.setGCost(betterGCost);
                                    neighbor.calculateFCost();
                                }
                            }
                            else // se ele chegou aqui, é porque não está na lista aberta.
                            {
                                neighbor.Father = current;
                                neighbor.setGCost(current.Gcost + movementCost);
                                neighbor.calculateHCost(neighbor, end);
                                neighbor.calculateFCost();
                                openList.Add(neighbor);
                            }
                            if (!initializedNodes.ContainsKey(neighbor.Id))
                            {
                                initializedNodes.Add(neighbor.Id, neighbor);
                            }
                        //}

                    }

                }
            }
        }

        public Node PathFindAStar(Environment env, Node begin, Node end)
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
                    return null;
                }

                // ordena lista
                openList = openList.OrderBy(item => item.getFCost()).ToList();


                // pega o nó com menor custo F da lista
                Node current = openList.First();
                openList.Remove(current);

                // adiciona na lista fechada.
                closedList.Add(current);

                // Verifica se o current é o destino
                if (current.XY.Line == end.XY.Line && current.XY.Column == end.XY.Column)
                {
                    return current;
                }

                // Inicializa e carrega os vizinhos
                current.initializeNeighbors(env);

                // verifica se o vizinho já foi inicializado. se já foi, copia os valores para o vizinho atual.
                foreach (Node initialized in current.Neighbors)
                {
                    if (initializedNodes.ContainsKey(initialized.Id))
                    {
                        Node temp;
                        initializedNodes.TryGetValue(initialized.Id, out temp);
                        copyValues(temp, initialized);
                    }
                }


                // processa todos os nós vizinhos
                foreach (Node neighbor in current.Neighbors)
                {
                    Boolean estaNaListaFechada = false;
                    Boolean ehParedeOuSujeira = false;
                    // se o vizinho é parede ou sujeira, ou seja, está bloqueado
                    // Tudo que não for "-" é caminho bloqueado, exceto se for o nodo destino.
                    if (env.Map[neighbor.XY.Line, neighbor.XY.Column] == "#"
                        || env.Map[neighbor.XY.Line, neighbor.XY.Column] == "D"
                        || env.Map[neighbor.XY.Line, neighbor.XY.Column] == "R"
                        || env.Map[neighbor.XY.Line, neighbor.XY.Column] == "T")
                    {
                        if (end.XY.Line == neighbor.XY.Line && end.XY.Column == neighbor.XY.Column)
                        {
                            // Se o vizinho for o destino, não faz nada.
                        }
                        else
                        {
                            ehParedeOuSujeira = true;
                        }
                    }

                    // se o vizinho está na lista fechada
                    if (closedList.Any(item => item.Id == neighbor.Id))
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
                        if (current.XY.Line != neighbor.XY.Line && current.XY.Column != neighbor.XY.Column)
                        {
                            movementCost = 14;
                        }
                        // se o vizinho está na lista aberta, tenta melhorar o caminho até ele através do atual
                        if (openList.Any(item => item.Id == neighbor.Id))
                        {

                            Int32 betterGCost = current.Gcost + movementCost;
                            if (betterGCost < neighbor.Gcost)
                            {
                                neighbor.Father = current;
                                neighbor.setGCost(betterGCost);
                                neighbor.calculateFCost();
                            }
                        }
                        else // se ele chegou aqui, é porque não está na lista aberta.
                        {
                            neighbor.Father = current;
                            neighbor.setGCost(current.Gcost + movementCost);
                            neighbor.calculateHCost(neighbor, end);
                            neighbor.calculateFCost();
                            openList.Add(neighbor);
                        }
                        if (!initializedNodes.ContainsKey(neighbor.Id))
                        {
                            initializedNodes.Add(neighbor.Id, neighbor);
                        }
                    }

                }
            }
        }

        private static void copyValues(Node source, Node destiny)
        {
            destiny.Father = source.Father;
            destiny.Fcost = source.Fcost;
            destiny.Gcost = source.Gcost;
            destiny.Hcost = source.Hcost;
            destiny.Neighbors = source.Neighbors;
        }
    }
}
