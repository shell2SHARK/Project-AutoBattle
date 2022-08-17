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
        //Recebe os valores em X e Y do script Program.cs e os armazena aqui
        public int xLength;
        public int yLength;

        public Grid(int Lines, int Columns)
        {
            xLength = Lines;
            yLength = Columns;           

            for (int i = 0; i < Lines; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    // Para cada coluna uma box é adicionada vazia
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j),"");
                    grids.Add(newBox);
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield()
        {           
            int checkListGrid = 0;

            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                { 
                    // Se a coluna estiver ocupada identifica se é um humano ou inimigo ali
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
                        //Se não estiver ocupada apenas adiciona uma box vazia
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
