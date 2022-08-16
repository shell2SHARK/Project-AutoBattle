using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {
        public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;
        }

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;
            public string charType;

            public GridBox(int x, int y, bool ocupied, int index, string charType)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;
                this.charType = charType;
            }
        }

        public struct CharacterSkills
        {
            string Name;
            float damage;
            float damageMultiplier;
        }

        public enum CharacterClass : uint
        {
            Paladino = 1,
            Guerreiro = 2,
            Clerico = 3,
            Arqueiro = 4
        }

    }
}
