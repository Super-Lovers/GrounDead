using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SurvivalConcept
{
    public class Program
    {
        // This is where we will store all the map segments into one
        // whole game world as we create each segment
        private static int mapWidth = 120;
        private static int mapHeight = 40;
        
        // We need to constantly keep track of where to draw next
        // after a segment is completed
        public static int currentPositionX = 0;
        public static int currentPositionY = 0;
        
        public static int[,] gameWorld = new int[mapHeight, mapWidth];
        
        static void Main(string[] args)
        {
            
            RockyPlains.GenerateRockyPlains(20, 20, 5);
            ForestGenerator.GenerateForest(20, 20, 30);
            ForestGenerator.GenerateForest(20, 20, 40);
            
            // Then we are generating the small but well-populated
            // woods in both sides of the house
            ForestGenerator.GenerateForest(20, 14, 10);
            currentPositionX -= 20;
            currentPositionY += 27;
            ForestGenerator.GenerateForest(20, 13, 10);
            currentPositionX -= 40;
            currentPositionY -= 7;
            ForestGenerator.GenerateForest(20, 20, 40);
            currentPositionX -= 40;
            ForestGenerator.GenerateForest(20, 20, 30);
            currentPositionX -= 40;
            RockyPlains.GenerateRockyPlains(20, 20, 5);

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // We also create the boundaries of the
                    // game world in this code block
                    if ((x < 63) && (y == mapHeight / 2 - 1 || y == mapHeight / 2 + 1))
                    {
                        if (x % 13 == 0 && x != 0)
                        {
                            if (y == mapHeight / 2 - 1)
                            {
                                gameWorld[y, x] = 6; // path going up
                            }
                            else
                            {
                                gameWorld[y, x] = 7; // path going down
                            }
                        }
                        else
                        {
                            gameWorld[y, x] = 9; // boundaries of biomes
                        }
                    } else if ((x < 63) && (y == mapHeight / 2))
                    {
                        gameWorld[y, x] = 2; // path / road
                    } else if ((x == 75) && (y == mapHeight / 2))
                    {
                        gameWorld[y, x] = 3; // house of the player
                    } else if (x % 20 == 0 && x != 0 && x != 2 && x < 61 && x != 7)
                    {
                        gameWorld[y, x] = 9; // boundaries of biomes
                    }
                    
                    // Drawing the path head and tail
                    if ((x == 0 && y == mapHeight / 2) || (x == 62 && y == mapHeight / 2))
                    {
                        gameWorld[y, x] = 1; // path endings can be grass for simplicity
                    }
                    
                    // Drawing the zombie's side of the game world
                    if (x > 79)
                    {
                        gameWorld[y, x] = 8;
                    }

                    if (x > 60 && y > 13 && y < 27 && gameWorld[y, x] != 2 && gameWorld[y, x] != 1 && gameWorld[y, x] != 3)
                    {
                        gameWorld[y, x] = 8;
                    }
                    Console.Write(gameWorld[y, x]);
                }
                Console.WriteLine();
            }

            //StreamReader reader = new StreamReader("file.txt");
            
            // Use using to automatically close the stream or file
            // without using the Close method
            /*
            using (reader)
            {
                int lineNumber = 0;
                // Read the first line, then enter the loop
                // and continue reading until you reach an
                // empty line
                string line = reader.ReadLine();

                while (line != null)
                {
                    lineNumber++;
                    Console.WriteLine("Line {0}: {1}", lineNumber, line);
                    line = reader.ReadLine();
                }
            }
            */
        }
    }
}
