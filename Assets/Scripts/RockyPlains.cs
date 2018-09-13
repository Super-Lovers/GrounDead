using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            for (int y = WorldGenerator.currentPositionY; y < WorldGenerator.currentPositionY + height; y++)
            {
                for (int x = WorldGenerator.currentPositionX; x < WorldGenerator.currentPositionX + width; x++)
                {
                    if (rng.Next(0, 101) > 90) // Iron
                    {
                        WorldGenerator.gameWorld[y, x] = 4;
                    } else if (rng.Next(0, 101) > 95) // Gold
                    {
                        WorldGenerator.gameWorld[y, x] = 5;
                    }
                    else
                    {
                        WorldGenerator.gameWorld[y, x] = 14;
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

            GenerateRockyPlains(height, width, WorldGenerator.gameWorld);
            //PrintMap(WorldGenerator.currentPositionY + height, WorldGenerator.currentPositionX + width, WorldGenerator.gameWorld);
        }
    }
}
