using System;

public class RockyPlains
{
    static Random rng = new Random();
        
    static void GenerateRockyPlains(int height, int width)
    {
        for (int y = WorldGenerator.CurrentPositionY; y < WorldGenerator.CurrentPositionY + height; y++)
        {
            for (int x = WorldGenerator.CurrentPositionX; x < WorldGenerator.CurrentPositionX + width; x++)
            {
                if (rng.Next(0, 101) > 90) // Iron
                {
                    WorldGenerator.GameWorld[y, x] = 4;
                } else if (rng.Next(0, 101) > 95) // Gold
                {
                    WorldGenerator.GameWorld[y, x] = 5;
                }
                else
                {
                    WorldGenerator.GameWorld[y, x] = 14;
                }
            }
        }
    }

    public static void GenerateRockyPlains(int mapWidth, int mapHeight, int numberOfObjects)
    {
        // The map properties
        int width = mapWidth;
        int height = mapHeight;

        GenerateRockyPlains(height, width);
    }
}