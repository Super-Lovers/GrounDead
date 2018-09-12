using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SurvivalConcept
{
    public class RockyPlains
    {
        static Random rng = new Random();
        static int totalSteps = 0;
        static int numberOfSteps;
        
        static void GenerateRockyPlains(int height, int width, int[,] map)
        {
            for (int y = Program.currentPositionY; y < Program.currentPositionY + height; y++)
            {
                for (int x = Program.currentPositionX; x < Program.currentPositionX + width; x++)
                {
                    if (rng.Next(0, 101) > 90) // Iron
                    {
                        Program.gameWorld[y, x] = 4;
                    } else if (rng.Next(0, 101) > 95) // Gold
                    {
                        Program.gameWorld[y, x] = 5;
                    }
                    else
                    {
                        Program.gameWorld[y, x] = 1;
                    }
                }
            }
        }

        public static void GenerateRockyPlains(int mapWidth, int mapHeight, int numberOfObjects)
        {
            int width = mapWidth;
            int height = mapHeight;
            numberOfSteps = numberOfObjects;

            // The map properties
            int[,] map = new int[height, width];

            GenerateRockyPlains(height, width, Program.gameWorld);
            //PrintMap(Program.currentPositionY + height, Program.currentPositionX + width, Program.gameWorld);

            Program.currentPositionX += 20;
        }
    }
}
