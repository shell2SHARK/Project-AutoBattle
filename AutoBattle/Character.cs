using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string name { get; set; }
        public float health;
        public float baseDamage;
        public float damageMultiplier { get; set; }
        public int playerIndex;
        public GridBox currentBox;
        public Character target { get; set; } 

        public Character(CharacterClass characterClass)
        {

        }

        public bool TakeDamage(float amount)
        {
            health -= amount;           
            return false;      
        }

        public void EndGame()
        {
            Console.WriteLine($"Player {target.name} venceu o {name}!");
        }

        public void StartTurn(Grid battlefield)//alterado
        {
            if (CheckCloseTargets(battlefield)) 
            {
                Attack(target);
                return;
            }
            else
            {                   
                // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if (this.currentBox.xIndex > target.currentBox.xIndex)
                {
                    Console.WriteLine($"Player {name} andou pra esquerda");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.Index == currentBox.Index - 1);
                    currentBox.ocupied = true;
                    currentBox.charType = name;
                    battlefield.grids[currentBox.Index] = currentBox;
                    battlefield.DrawBattlefield(5, 5);
                    return;
                }
                else if(currentBox.xIndex < target.currentBox.xIndex)
                {
                    Console.WriteLine($"Player {name} andou pra direita");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.Index == currentBox.Index + 1);
                    currentBox.ocupied = true;
                    currentBox.charType = name;
                    battlefield.grids[currentBox.Index] = currentBox;
                    battlefield.DrawBattlefield(5, 5);
                    return;
                }

                if (this.currentBox.yIndex > target.currentBox.yIndex)
                {
                    Console.WriteLine($"Player {name} andou pra cima");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght);
                    currentBox.ocupied = true;
                    currentBox.charType = name;
                    battlefield.grids[currentBox.Index] = currentBox;
                    battlefield.DrawBattlefield(5, 5);
                    return;
                }
                else if(this.currentBox.yIndex < target.currentBox.yIndex)
                {
                    Console.WriteLine($"Player {name} andou pra baixo");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght);
                    currentBox.ocupied = true;
                    currentBox.charType = name;
                    battlefield.grids[currentBox.Index] = currentBox;
                    battlefield.DrawBattlefield(5, 5);
                    return;
                }              
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            if (left || right || up || down)
            {
                Console.WriteLine("Hora da batalha!");
                return true;
            }

            return false; 
        }

        public void Attack (Character target)
        {
            var rand = new Random();
            int damageValue = rand.Next(0, (int)baseDamage);
            target.TakeDamage(damageValue);
            Console.WriteLine($"Player {name} está atacando {target.name} e deu {damageValue} de dano!\n");
            Console.WriteLine($"Health {name} | {health} --- Health {target.name} | {target.health}");
        }
    }
}
