# Trash_Collector_Agent
Implementação de um programa em C# que simula o comportamento de um Agente coletor de lixo em um ambiente (uma matriz de tamanho nxn).

Este projeto foi criado a partir de um trabalho da disciplina de Inteligência Artificial do curso de Ciência da Computação da PUCRS, para compreender e aprofundar o conhecimento do Algoritmo A*.

> Objetivo: Percorrer a matriz linearmente, da esquerda para direita e de cima para baixo. Cada vez que achar sujeira ("D"), o agente remove a sujeira. Caso a lixeira interna do agente esteja cheia, ele deve se mover até o depósito de lixo mais próximo ("T") e depositar a sujeira lá, voltando a estar com sua lixeira interna vazia. Depois disto, o agente retorna à posição que estava e continua percorrendo a matriz até chegar no final dela. Ele deve desviar das paredes ("#") e também dos depósitos de lixo.

> A aplicação do algoritmo A* ocorre quando o agente procura o caminho mais próximo até o depósito de lixo e quando calcula o caminho mais curto para desviar da parede e dos depósitos de lixo.

> A*: Algoritmo de busca com informação que combina as técnicas de Best-First-Search com o algoritmo de Dijkstra. Dado um ponto de origem A e um ponto de destino B, o algoritmo procura entre todos os caminhos possíveis para ir de A até B aquele que deve ser a melhor solução possível (o caminho com menor tempo, ou com menor distância, etc).

## Manual de utilização: ##
  
###  Utilização do programa ###
>    Dentro do repositório, no diretório "Trash_Collector_Agent\Trash_Collector_Agent\Trash_Collector_Agent\bin\Debug" contém um arquivo chamado "Trash_Collector_Agent.cheating". 

>    Renomeie o arquivo para "Trash_Collector_Agent.exe"

>    Execute o arquivo
    
>    Ex: Trash_Collector_Agent\Trash_Collector_Agent\Trash_Collector_Agent\bin\Debug\Trash_Collector_Agent.exe

> Aperte qualquer tecla para o programa seguir para o próximo passo.
    
###  Rodando do programa: ###
>    Trash_Collector_Agent\Trash_Collector_Agent\Trash_Collector_Agent\bin\Debug\Trash_Collector_Agent.exe

>    Para mudar as configurações do programa é necessário baixar o projeto, alterar os valores no arquivo "Program.cs" e recompilar o programa.

![Rodando o programa](https://github.com/brbmendes/Trash_Collector_Agent/blob/master/Trash_Collector_Agent.jpg)
