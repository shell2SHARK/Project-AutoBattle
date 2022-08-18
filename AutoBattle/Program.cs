using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Marcador do turno atual
            int currentTurn = 0;
            // Valores de largura e altura do campo
            int valueLineX;
            int valueLineY;         
            // Para comparar a classe com o inimigo e não vir chars iguais para os dois jogadores
            int classHMNSelected = 0;
            // Campo de batalha do jogo
            Grid grid; 
            // Aloca e armazena as posições dos personagens no campo
            GridBox PlayerCurrentLocation;
            GridBox EnemyCurrentLocation;
            // Armazena os dados de cada classe escolhidos pelos jogadores
            Character PlayerCharacter;
            Character EnemyCharacter;
            // Guarda os jogadores escolhidos dentro de uma lista
            List<Character> AllPlayers = new List<Character>();
            // Atributos de cada classe podem ser alterados por aqui
            var paladinSkills = new CharacterSkills("Paladino", 100, 30, 3);
            var warriorSkills = new CharacterSkills("Guerreiro", 120, 40, 2);
            var clericSkills = new CharacterSkills("Clerico", 90, 20, 2);
            var archerSkills = new CharacterSkills("Arqueiro", 90, 10, 3);
            Setup(); 

            void Setup()
            {
                // Caso o usuário escolha um tamanho aceitável para o campo, ele é desenhado
                Console.WriteLine("Bem vindo ao Auto Battle!\nEscolha a quantidade de linhas:");
                valueLineX = Int32.Parse(Console.ReadLine());

                if (valueLineX > 1)
                {
                    Console.WriteLine("Agora a quantidade de colunas:");
                    valueLineY = Int32.Parse(Console.ReadLine());

                    if (valueLineY > 1)
                    {
                        grid = new Grid(valueLineX, valueLineY); 
                        GetPlayerChoice();
                    }
                    else
                    {
                        Console.WriteLine("Quantidade insuficiente,escolha novamente...");
                        Setup();
                    }
                }
                else
                {
                    Console.WriteLine("Quantidade insuficiente,escolha novamente...");
                    Setup();
                }                                
            }

            void GetPlayerChoice()
            {
                //Pede ao jogador para escolher uma possível classe de personagem para jogar
                Console.WriteLine("Escolha sua classe:");
                Console.WriteLine("[1] Paladino, [2] Guerreiro, [3] Clerico, [4] Arqueiro");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));                       
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }                
            }

            void CreatePlayerCharacter(int classIndex)
            {               
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Humano escolheu a classe: {characterClass}");
                PlayerCharacter = new Character(characterClass);
                
                // Os valores de skill são enviados para cada Character separadamente
                switch (classIndex)
                {
                    case 1:
                        PlayerCharacter.charSkillsValue = paladinSkills;
                        PlayerCharacter.playerType = "HMN";
                        classHMNSelected = 1;
                        break;
                    case 2:
                        PlayerCharacter.charSkillsValue = warriorSkills;
                        PlayerCharacter.playerType = "HMN";
                        classHMNSelected = 2;
                        break;
                    case 3:
                        PlayerCharacter.charSkillsValue = clericSkills;
                        PlayerCharacter.playerType = "HMN";
                        classHMNSelected = 3;
                        break;
                    case 4:
                        PlayerCharacter.charSkillsValue = archerSkills;
                        PlayerCharacter.playerType = "HMN";
                        classHMNSelected = 4;
                        break;
                }

                CreateEnemyCharacter();
            }

            void CreateEnemyCharacter()
            {
                // Escolhe um inimigo aleatório para o inimigo
                // Caso o inimigo escolha o mesmo personagem do jogador, o método é chamado novamente
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);

                if (randomInteger != classHMNSelected)
                {
                    CharacterClass enemyClass = (CharacterClass)randomInteger;
                    Console.WriteLine($"Inimigo escolheu a classe: {enemyClass}");
                    EnemyCharacter = new Character(enemyClass);
                }
                else
                {
                    CreateEnemyCharacter();
                }

                // Os valores de skill são enviados para cada Character separadamente
                switch (randomInteger)
                {
                    case 1:
                        EnemyCharacter.charSkillsValue = paladinSkills;
                        EnemyCharacter.playerType = "ENM";
                        break;
                    case 2:
                        EnemyCharacter.charSkillsValue = warriorSkills;
                        EnemyCharacter.playerType = "ENM";
                        break;
                    case 3:
                        EnemyCharacter.charSkillsValue = clericSkills;
                        EnemyCharacter.playerType = "ENM";
                        break;
                    case 4:
                        EnemyCharacter.charSkillsValue = archerSkills;
                        EnemyCharacter.playerType = "ENM";
                        break;
                }

                StartGame();
            }

            void StartGame()
            {
                //Preenche as variáveis de personagens e alvos
                EnemyCharacter.target = PlayerCharacter;
                PlayerCharacter.target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);

                foreach(var chars in AllPlayers)
                {
                    //Para cada personagem, os valores de skills são atribuidos a eles
                    chars.DefineSkills();
                }

                AlocatePlayerCharacter();
                StartTurn();
            }

            void StartTurn(){

                if (currentTurn == 0)
                {
                    // Escolhe aleatoriamente um jogador no campo para começar no primeiro turno
                    Random rand = new Random();
                    var shuffle = AllPlayers.OrderBy(item => Guid.NewGuid()).ToList();
                    AllPlayers.Clear();

                    foreach (var value in shuffle)
                    {
                        AllPlayers.Add(value);                    
                    }

                    Console.WriteLine($"O {AllPlayers[0].charSkillsValue.name} vai começar primeiro.");                   
                }

                foreach (Character character in AllPlayers)
                {
                    // Adiciona a quantidade de Boxes criadas no campo para os personagens diretamente
                    character.gridBoxesTotal = grid.grids.Count;
                    character.StartTurn(grid);
                }

                currentTurn++;
                Console.WriteLine($"---- FIM DO {currentTurn}° TURNO ----");
                HandleTurn();
            }

            void HandleTurn()
            {
                // Caso um personagem morra e outro fique de pé, o método de vencedor é chamado de cada personagem
                if(PlayerCharacter.health <= 0 && EnemyCharacter.health > 0)
                {
                    EnemyCharacter.EndGame();
                    return;
                } 
                else if (EnemyCharacter.health <= 0 && PlayerCharacter.health > 0)
                {
                    PlayerCharacter.EndGame();
                    return;
                }
                else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Clique em qualquer tecla para começar o próximo turno...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }

            void AlocatePlayerCharacter()
            {
                // Sorteia uma posição aleatória dentro do campo para começar
                int random = GetRandomInt(0,grid.grids.Count);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                //Se o campo não estiver ocupado pelo inimigo, o adiciona como seu
                if (!RandomLocation.ocupied)
                {
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.charType = PlayerCharacter.playerType;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                } 
                else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                // Sorteia uma posição aleatória dentro do campo para começar
                int random = GetRandomInt(0, grid.grids.Count);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                //Se o campo não estiver ocupado pelo inimigo, o adiciona como seu
                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.charType = EnemyCharacter.playerType;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.DrawBattlefield();
                }
                else
                {
                    AlocateEnemyCharacter();
                } 
            }
        }
    }
}
