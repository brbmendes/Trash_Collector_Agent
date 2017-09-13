using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trash_Collector_Agent.src
{
    class Program
    {
        public static void breakLines()
        {
            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine(System.Environment.NewLine);
        }

        static void Main(string[] args)
        {

            //// CONSTRUÇÃO DO AMBIENTE EM SEQUÊNCIA, PARA VER SE ESTÁ CRIANDO CERTO.
            //Environment env;
            //Agent robot;
            //Astar aStar;
            ////Node destinyNode;
            //int qtdTrashDeposits = 3;
            //int percentDirty = 15;
            //int internalTrash = 4;
            //for (int i = 12; i < 26; i++)
            //{
            //    robot = new Agent(internalTrash);
            //    env = new Environment(i, robot, qtdTrashDeposits, percentDirty);
            //    Console.WriteLine("Tamanho do ambiente = {0}x{0}", i);
            //    Console.WriteLine("Percentual de sujeira no ambiente = {0} %", percentDirty);
            //    Console.WriteLine("Lixeiras no ambiente = {0}", qtdTrashDeposits);
            //    Console.WriteLine("Lixeira interna agente = {0}", robot.usedInternalTrash());
            //    Console.WriteLine("\n");
            //    env.initializeMap();
            //    env.positioningAgent(robot);
            //    env.buildWalls();
            //    env.buildTrashDeposits();
            //    env.buildDirtyEnvironment();

            //    robot.environment = env;
            //    aStar = new Astar(env);
            //    robot.aStar = aStar;

            //    env.showEnvironment();
            //    Program.breakLines();
            //}

            #region DECLARAÇÃO DE VARIAVEIS
            Environment env;
            Agent robot;
            Node destinyNode;
            Astar aStar;
            int size = 18;
            int qtdTrashDeposits = 3;
            int percentDirty = 15;
            int internalTrash = 4;
            #endregion

            #region INSTANCIAÇÃO DO AGENTE
            robot = new Agent(internalTrash);
            #endregion

            #region INSTANCIAÇÃO DO AMBIENTE
            env = new Environment(size, robot, qtdTrashDeposits, percentDirty);
            #endregion

            #region CHAMADA DE METODOS PARA INICIALIZAR MAPA E MOSTRAR AMBIENTE
            Console.WriteLine("Tamanho do ambiente = {0}x{0}", size);
            Console.WriteLine("Percentual de sujeira no ambiente = {0} %", percentDirty);
            Console.WriteLine("Lixeiras no ambiente = {0}", qtdTrashDeposits);
            Console.WriteLine("Lixeira interna agente = {0}", robot.usedInternalTrash());
            Console.WriteLine("\n");

            env.initializeMap();
            env.positioningAgent(robot);
            env.buildWalls();
            env.buildTrashDeposits();
            env.buildDirtyEnvironment();

            robot.environment = env;
            aStar = new Astar(env);
            robot.aStar = aStar;

            env.showEnvironment();
            #endregion


            #region EXECUTA A*
            //// A* que limpa ambiente
            ////env.agent.clean(env.map);

            // A* só move
            destinyNode = robot.move();
            List<Node> listFathers = env.createFatherList(destinyNode);
            List<Node> listFathersReturning = listFathers.ToList();
            listFathersReturning.Reverse(); // Inverte para voltar ao nodo inicial.
            env.moveAgentAroundEnvironment(robot, listFathers, destinyNode);
            Program.breakLines();
            Console.WriteLine("Agente está retornando para posição inicial.");
            destinyNode = listFathersReturning.Last();
            env.moveAgentAroundEnvironment(robot, listFathersReturning, destinyNode);
            Program.breakLines();
            #endregion

            Console.ReadKey();
        }
    }
}