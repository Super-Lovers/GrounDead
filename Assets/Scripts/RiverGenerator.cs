using System;

public class RiverGenerator
{
    static Random rng = new Random();
    static int totalSteps = 0;
    static int _numberOfSteps;
        
    static void SendAgent(int height, int width, int totalSteps, int _numberOfSteps)
    {
        int randomPointInMapX = rng.Next(WorldGenerator.CurrentPositionX, WorldGenerator.CurrentPositionX + width);
        int randomPointInMapY = rng.Next(WorldGenerator.CurrentPositionY, WorldGenerator.CurrentPositionY + height);

        while (totalSteps < _numberOfSteps)
        {
            while (_numberOfSteps > 0)
            {
                int randomDirection = rng.Next(1, 5);

                if (randomDirection == 1)
                {
                    if (randomPointInMapX + 1 >= WorldGenerator.CurrentPositionX + width)
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
                    if (randomPointInMapY + 1 >= WorldGenerator.CurrentPositionY + height)
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

                if (WorldGenerator.GameWorld[randomPointInMapY, randomPointInMapX] == 1)
                {
                    WorldGenerator.GameWorld[randomPointInMapY, randomPointInMapX] = 16;
                    _numberOfSteps--;
                    totalSteps++;
                }
            }
        }
    }

    public static void GenerateRiver(int mapWidth, int mapHeight, int numberOfObjects)
    {
        // The map properties
        int width = mapWidth;
        int height = mapHeight;
        _numberOfSteps = numberOfObjects;

        SendAgent(height, width, totalSteps, _numberOfSteps);
        SendAgent(height, width, totalSteps, _numberOfSteps);
        SendAgent(height, width, totalSteps, _numberOfSteps);
        SendAgent(height, width, totalSteps, _numberOfSteps);
    }
}