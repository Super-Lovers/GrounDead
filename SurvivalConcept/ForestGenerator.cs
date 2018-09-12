using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SurvivalConcept
{
    public class ForestGenerator
    {
        static Random rng = new Random();
        static int totalSteps = 0;
        static int numberOfSteps;
        
        static void GenerateMap(int height, int width, int[,] map)
        {
            for (int y = Program.currentPositionY; y < Program.currentPositionY + height; y++)
            {
                for (int x = Program.currentPositionX; x < Program.currentPositionX + width; x++)
                {
                    Program.gameWorld[y, x] = 1;
                }
            }
        }

        static void GenerateForest(int height, int width, int[,] map, int totalSteps, int numberOfSteps, Random rng)
        {
            int randomPointInMapX = rng.Next(Program.currentPositionX, Program.currentPositionX + width);
            int randomPointInMapY = rng.Next(Program.currentPositionY, Program.currentPositionY + height);

            while (totalSteps < numberOfSteps)
            {
                while (numberOfSteps > 0)
                {
                    int randomDirection = rng.Next(1, 5);

                    if (randomDirection == 1)
                    {
                        if (randomPointInMapX + 1 >= Program.currentPositionX + width)
                        {
                            break;
                        }
                        randomPointInMapX++;
                    }
                    else if (randomDirection == 2)
                    {
                        if (randomPointInMapX - 1 <= 0)
                        {
                            break;
                        }
                        randomPointInMapX--;
                    }
                    else if (randomDirection == 3)
                    {
                        if (randomPointInMapY + 1 >= Program.currentPositionY + height)
                        {
                            break;
                        }
                        randomPointInMapY++;
                    }
                    else if (randomDirection == 4)
                    {
                        if (randomPointInMapY - 1 <= 0)
                        {
                            break;
                        }
                        randomPointInMapY--;
                    }

                    if (Program.gameWorld[randomPointInMapY, randomPointInMapX] == 1)
                    {
                        Program.gameWorld[randomPointInMapY, randomPointInMapX] = 0;
                        numberOfSteps--;
                        totalSteps++;
                    }
                }
                /*if (totalSteps > 80)
                {
                    Console.WriteLine(totalSteps + " trees were made until the walker collided with a wall");
                }*/
            }
        }

        static void PrintMap(int height, int width, int[,] map)
        {
            // Print the complete map to the console and map data file
            StreamWriter writer = new StreamWriter("forestGeneration.txt", false);
            using (writer)
            {
                for (int y = Program.currentPositionY; y < Program.currentPositionY + height; y++)
                {
                    for (int x = Program.currentPositionX; x < Program.currentPositionX + width; x++)
                    {
                        //Console.Write(map[y, x]);
                        //writer.Write(map[y, x]);
                        
                        // Now we need to add each tile to the game world list...
                        //Program.gameWorld[y, x] = map[y, x];
                    }
                    //Console.WriteLine();
                    //writer.WriteLine();
                }
            }
        }

        public static void GenerateForest(int mapWidth, int mapHeight, int numberOfObjects)
        {
            int width = mapWidth;
            int height = mapHeight;
            numberOfSteps = numberOfObjects;

            // The map properties
            int[,] map = new int[height, width];

            GenerateMap(height, width, Program.gameWorld);
            GenerateForest(height, width, Program.gameWorld, totalSteps, numberOfSteps, rng);
            GenerateForest(height, width, Program.gameWorld, totalSteps, numberOfSteps, rng);
            GenerateForest(height, width, Program.gameWorld, totalSteps, numberOfSteps, rng);
            GenerateForest(height, width, Program.gameWorld, totalSteps, numberOfSteps, rng);
            //PrintMap(Program.currentPositionY + height, Program.currentPositionX + width, Program.gameWorld);

            Program.currentPositionX += 20;
        }
    }
}
