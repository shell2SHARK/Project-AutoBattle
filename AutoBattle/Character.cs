using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        //Total de Boxes recebido pelo script Program.cs
        public int gridBoxesTotal;
        // Atributos do personagem
        public float health;
        public float baseDamage;
        public float damageMultiplier { get; set; }
        public string name { get; set; }
        // Se é humano ou inimigo
        public string playerType;
        // Recebe os valores da posição na box atual
        public GridBox currentBox;
        // Atributos de skill recebidos do script Program.cs
        public CharacterSkills charSkillsValue;
        // Alvo inimigo setado pelo script Program.cs
        public Character target { get; set; } 
        // A identificação da classe é recebida aqui
        public Character(CharacterClass characterClass)
        {
            
        }

        public void DefineSkills()
        {
            // Define cada valor de skill vindos de charSkillsValue
            name = charSkillsValue.name;
            health = charSkillsValue.life;
            baseDamage = charSkillsValue.damage;
            damageMultiplier = charSkillsValue.damageMultiplier;
        }

        public bool TakeDamage(float amount)
        {
            health -= amount;           
            return false;      
        }

        public void EndGame()
        {     
            Console.WriteLine($"Player {name} venceu o {target.name}!");
            Console.WriteLine("Obrigado por Jogar!");
            Environment.Exit(0);
        }

        public void StartTurn(Grid battlefield)
        {
            if (CheckCloseTargets(battlefield)) 
            {
                // Se o personagem tem vida e seu inimigo também, o ataque é realizado
                if (health > 0 && target.health > 0)//alterado
                {
                    Attack(target);

                    // Chance aleatória do personagem mudar de posição no campo após realizar ataque
                    var rand = new Random();
                    int chanceToKnockback = rand.Next(0, 100);

                    if (chanceToKnockback > 85)
                    {
                        Console.WriteLine($"Player {name} tomou um empurrão do adversário!");
                        int knockbackChar = rand.Next(0, gridBoxesTotal);
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.index] = currentBox;
                        currentBox = battlefield.grids.Find(x => x.index == knockbackChar);
                        currentBox.ocupied = true;
                        currentBox.charType = playerType;
                        battlefield.grids[currentBox.index] = currentBox;
                        battlefield.DrawBattlefield();
                    }
                }
                else
                {
                    return;
                }               
            }
            else
            {
                //Se não tem um alvo próximo, calcula a menor distancia entre o próximo alvo para se mover até lá
                if (this.currentBox.xIndex > target.currentBox.xIndex)
                {
                    Console.WriteLine($"Player {name} andou pra esquerda");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.index == currentBox.index - 1);
                    currentBox.ocupied = true;
                    currentBox.charType = playerType;
                    battlefield.grids[currentBox.index] = currentBox;
                    battlefield.DrawBattlefield();
                    return;
                }
                else if(currentBox.xIndex < target.currentBox.xIndex)
                {
                    Console.WriteLine($"Player {name} andou pra direita");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.index == currentBox.index + 1);
                    currentBox.ocupied = true;
                    currentBox.charType = playerType;
                    battlefield.grids[currentBox.index] = currentBox;
                    battlefield.DrawBattlefield();
                    return;
                }
                else if (this.currentBox.yIndex > target.currentBox.yIndex)
                {
                    Console.WriteLine($"Player {name} andou pra cima");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.index == currentBox.index - battlefield.yLength);
                    currentBox.ocupied = true;
                    currentBox.charType = playerType;
                    battlefield.grids[currentBox.index] = currentBox;
                    battlefield.DrawBattlefield();
                    return;
                }
                else if(this.currentBox.yIndex < target.currentBox.yIndex)
                {
                    Console.WriteLine($"Player {name} andou pra baixo");
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.index] = currentBox;
                    currentBox = battlefield.grids.Find(x => x.index == currentBox.index + battlefield.yLength);
                    currentBox.ocupied = true;
                    currentBox.charType = playerType;
                    battlefield.grids[currentBox.index] = currentBox;
                    battlefield.DrawBattlefield();
                    return;
                }
            }
        }

        // Verifica se nas posições X e Y existe algum inimigo perto o suficiente para ser atacado ou não
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = battlefield.grids.Find(x => x.index == currentBox.index - 1).ocupied;
            bool right = battlefield.grids.Find(x => x.index == currentBox.index + 1).ocupied;
            bool up = battlefield.grids.Find(x => x.index == currentBox.index + battlefield.yLength).ocupied;
            bool down = battlefield.grids.Find(x => x.index == currentBox.index - battlefield.yLength).ocupied;

            if (left || right || up || down)
            {
                return true;
            }

            return false; 
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            int damageValue = rand.Next(0, (int)baseDamage);
            target.TakeDamage(damageValue * damageMultiplier);
            Console.WriteLine($"Player {name} está atacando {target.name} e deu {damageValue * damageMultiplier} de dano!\n");
            Console.WriteLine($"Health {name} | {health} --- Health {target.name} | {target.health}");
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}
