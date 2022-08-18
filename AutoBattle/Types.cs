using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {     
        public struct GridBox
        {
            // Identifica posição em x e y do personagem
            public int xIndex;
            public int yIndex;
            public bool ocupied; // Identifica se a box atual está ocupada ou não por alguém
            public int index; // Posição do personagem em index no campo
            public string charType; // Tipo do personagem Humano ou Inimigo

            public GridBox(int x, int y, bool ocupied, int index, string charType)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.index = index;
                this.charType = charType;
            }
        }

        public struct CharacterSkills
        {
            //Atributos atuais de cada personagem
            public string name;
            public float life; 
            public float damage;
            public float damageMultiplier;

            public CharacterSkills(string nameChar, float lifeChar, float damageChar, float damageMultChar) 
            {
                name = nameChar;
                life = lifeChar;
                damage = damageChar;
                damageMultiplier = damageMultChar;
            }
        }

        public enum CharacterClass : uint
        {
            //Identificação de cada personagem
            Paladino = 1,
            Guerreiro = 2,
            Clerico = 3,
            Arqueiro = 4
        }
    }
}
