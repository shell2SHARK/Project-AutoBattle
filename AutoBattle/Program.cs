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
            Grid grid = new Grid(5, 5);
            CharacterClass playerCharacterClass;
            GridBox PlayerCurrentLocation;
            GridBox EnemyCurrentLocation;
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            Setup(); 

            void Setup()
            {
                GetPlayerChoice();
            }

            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Bem vindo ao Auto Battle!\nEscolha sua classe:");
                Console.WriteLine("[1] Paladino, [2] Guerreiro, [3] Clerico, [4] Arqueiro");
                //store the player choice in a variable
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
                PlayerCharacter.health = 100;
                PlayerCharacter.baseDamage = 20;
                PlayerCharacter.playerIndex = 0;
                PlayerCharacter.name = "HMN";         
                CreateEnemyCharacter();
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Inimigo escolheu a classe: {enemyClass}");
                EnemyCharacter = new Character(enemyClass);
                EnemyCharacter.health = 100;
                EnemyCharacter.baseDamage = 20;
                EnemyCharacter.playerIndex = 1; 
                EnemyCharacter.name = "ENM";
                StartGame();
            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.target = PlayerCharacter;
                PlayerCharacter.target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                AlocatePlayers();
                StartTurn();
            }

            void StartTurn(){

                if (currentTurn == 0)
                {
                    Random rand = new Random();
                    var shuffle = AllPlayers.OrderBy(item => Guid.NewGuid()).ToList();
                    AllPlayers.Clear();

                    foreach (var value in shuffle)
                    {
                        AllPlayers.Add(value);                    
                    }

                    Console.WriteLine($"O {AllPlayers[0].name} vai começar primeiro.");
                }

                foreach(Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                Console.WriteLine($"---- FIM DO {currentTurn}° TURNO ----");
                HandleTurn();
            }

            void HandleTurn()
            {
                if(PlayerCharacter.health <= 0)
                {
                    PlayerCharacter.EndGame();
                    return;
                } 
                else if (EnemyCharacter.health <= 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    EnemyCharacter.EndGame();
                    Console.Write(Environment.NewLine + Environment.NewLine);
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

            void AlocatePlayers()
            {
                AlocatePlayerCharacter();
            }

            void AlocatePlayerCharacter()
            {
                int random = GetRandomInt(0,grid.grids.Count - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                if (!RandomLocation.ocupied)
                {
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.charType = PlayerCharacter.name;
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
                int random = GetRandomInt(0, grid.grids.Count - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.charType = EnemyCharacter.name;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.DrawBattlefield(5, 5);
                }
                else
                {
                    AlocateEnemyCharacter();
                } 
            }
        }
    }
}
