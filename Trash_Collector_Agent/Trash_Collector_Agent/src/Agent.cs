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

        public void set(Int32 x, Int32 y){
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
        Dictionary<String[,], Object> recognizedEnvironment = new Dictionary<string[,],Object>();

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

            this.recognizedEnvironment.Add(north, map.GetValue(agentPositionX - 1,agentPositionY));
            this.recognizedEnvironment.Add(south, map.GetValue(agentPositionX + 1, agentPositionY));
            this.recognizedEnvironment.Add(east, map.GetValue(agentPositionX, agentPositionY + 1));
            this.recognizedEnvironment.Add(west, map.GetValue(agentPositionX, agentPositionY - 1));
            this.recognizedEnvironment.Add(northEast, map.GetValue(agentPositionX - 1, agentPositionY + 1));
            this.recognizedEnvironment.Add(northWest, map.GetValue(agentPositionX - 1, agentPositionY - 1));
            this.recognizedEnvironment.Add(southEast, map.GetValue(agentPositionX + 1, agentPositionY + 1));
            this.recognizedEnvironment.Add(southWest, map.GetValue(agentPositionX + 1, agentPositionY - 1));
        }

        public Node move(String[,] map)
        {
            
            // Pontos da atual posição do agente.
            Int32 agentPositionX = this.getX();
            Int32 agentPositionY = this.getY();
            Position agentPosition = new Position(agentPositionX, agentPositionY);

            Node begin = new Node(agentPositionX, agentPositionY);
            Position lixeiraMaisProxima = this.calculaLixeiraMaisProxima(agentPosition);
            Position fimDaMatriz = new Position(Environment.sizeEnv - 1, Environment.sizeEnv - 1);
            //Node end = new Node(lixeiraMaisProxima.Line, lixeiraMaisProxima.Column);
            Node end = new Node(fimDaMatriz.Line, fimDaMatriz.Column);

            Node nodoDestino = Astar.PathFindAStar(map, begin, end);
            return nodoDestino;
            //if(nodoDestino != null)
            //{
                
            //    Console.WriteLine("Caminho encontrado.");
            //} 
            
            /*
            // Reconhece ambiente ao redor.
            recognizingEnvironment(map, agentPositionX, agentPositionY);


            //Pega próxima posição do mapa e verifica qual caratere é. 
            String nextPositionMap = map.GetValue(agentPositionX, agentPositionY + 1).ToString().Trim();
            int nextPositionMapValue = this.nextPosition[nextPositionMap];

            // Move de acordo com o caractere.
            switch (nextPositionMapValue)
            {
                case 1: // Achou lixo
                    if (this.usedInternalTrash() > 0)
                    {
                        #region atualiza antiga posicao do agente
                        this.oldPosition.Remove("x");
                        this.oldPosition.Remove("y");
                        this.oldPosition.Add("x", this.getX());
                        this.oldPosition.Add("y", this.getY());
                        #endregion

                        #region Coleta lixo
                        this.collectTrash();
                        #endregion

                        #region Seta "- " onde o agente estava, e "A " na posição a frente
                        map.SetValue("- ", agentPositionX, agentPositionY);
                        this.set(agentPositionX, agentPositionY + 1);
                        map.SetValue("A ", this.getX(), this.getY());
                        #endregion

                        #region Consome bateria por andar
                        this.consumeBattery();
                        #endregion

                    }
                    //else
                    //{
                    //    // USAR ALGORITMO A* PARA ACHAR O deposito de lixo MAIS PRÓXIMO E ANDAR ATÉ ELE.
                    //    // DESCARREGAR LIXO
                    //    // USAR ALGORITMO A* PARA retonrar até ao ponto que estava
                    //}
                    break;
                case 2: // Achou parede
                    // USAR ALGORITMO A* para desviar.
                    break;
                case 3: // Achou Carregador
                    // USAR ALGORITMO A* PARA andar para o próximo ponto na mesma linha, ou na coluna abaixo caso não tenha linha pro lado e seja final da matriz
                    break;
                case 4: // Achou depósito de lixo
                    // USAR ALGORITMO A* PARA andar para o próximo ponto na mesma linha, ou na coluna abaixo caso não tenha linha pro lado e seja final da matriz
                    break;
                case 5:
                    #region atualiza antiga posicao do agente
                    this.oldPosition.Remove("x");
                    this.oldPosition.Remove("y");
                    this.oldPosition.Add("x", this.getX());
                    this.oldPosition.Add("y", this.getY());
                    #endregion

                    #region Seta "- " onde o agente estava, e "A " na posição a frente
                    map.SetValue("- ", agentPositionX, agentPositionY);
                    this.set(agentPositionX, agentPositionY + 1);
                    map.SetValue("A ", this.getX(), this.getY());
                    #endregion

                    #region Consome bateria por andar
                    this.consumeBattery();
                    #endregion
                    break;
            }
            */

        }

        private Position calculaLixeiraMaisProxima(Position agentPosition)
        {
        Position lixeiraMaisProxima = new Position(0,0);
        Int32 posicaoAbsoluta = Int32.MaxValue;
        
        foreach(Trash_deposit trash in this.trashDeposits)
        {
            Position tempTrash = new Position(trash.getX(), trash.getY());
            Int32 posicaoAbsolutaLocal = calculaPosicaoAbsoluta(agentPosition, tempTrash);
            if(posicaoAbsolutaLocal < posicaoAbsoluta)
            {
                lixeiraMaisProxima = tempTrash;
                posicaoAbsoluta = posicaoAbsolutaLocal;
            }
        }
        return lixeiraMaisProxima;
        }

        private Position calculaRecarregadorMaisProximo(Position agentPosition)
        {
            Position recarregadorMaisProximo = new Position(0, 0);
            Int32 posicaoAbsoluta = Int32.MaxValue;

            foreach (Recharger recharger in this.rechargers)
            {
                Position tempRecharger = new Position(recharger.getX(), recharger.getY());
                Int32 posicaoAbsolutaLocal = calculaPosicaoAbsoluta(agentPosition, tempRecharger);
                if (posicaoAbsolutaLocal < posicaoAbsoluta)
                {
                    recarregadorMaisProximo = tempRecharger;
                    posicaoAbsoluta = posicaoAbsolutaLocal;
                }
            }
            return recarregadorMaisProximo;
        }

        private int calculaPosicaoAbsoluta(Position elem1, Position elem2)
        {
            int valorAbsolutoX = Math.Abs(elem1.Line - elem2.Line);
            int valorAbsolutoY = Math.Abs(elem1.Column - elem2.Column);

            return valorAbsolutoX + valorAbsolutoY;
        }
    }
}
