using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;

        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;           

            for (int i = 0; i < Lines; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j),"");
                    grids.Add(newBox);
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield(int Lines, int Columns)
        {
            int checkListGrid = 0;

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {                 
                    if (grids[checkListGrid].ocupied)
                    {
                        if (grids[checkListGrid].charType == "HMN")
                        {
                            Console.Write("[H]\t");
                        }
                        else
                        {
                            Console.Write("[I]\t");
                        }
                        
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }

                    checkListGrid++;
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

    }
}
