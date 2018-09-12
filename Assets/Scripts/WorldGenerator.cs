using Boo.Lang;
using SurvivalConcept;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    // Storing the collection of all the game objects we will
    // be using to generate the map with
    public GameObject[] ground;
    public GameObject[] forestTrees;
    public GameObject[] rockyGround;
    public GameObject[] grassPaths;
    public GameObject[] stoneTypes;
    public GameObject[] goldTypes;
    public GameObject playerHouse;
    public GameObject fenceObstancle;
	
    // This is where we will store all the map segments into one
    // whole game world as we create each segment
    private static int mapWidth = 120;
    private static int mapHeight = 40;
        
    // We need to constantly keep track of where to draw next
    // after a segment is completed
    public static int currentPositionX = 0;
    public static int currentPositionY = 0;
        
    public static int[,] gameWorld = new int[mapHeight, mapWidth];

    void Start () {
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
            }
        }

        float currentX = 0;
        float currentY = 0;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                switch (gameWorld[y, x])
                {
                    case 1: // Grass
                        // The random range generator will help us pick
                        // objects from the arrays randomly so the environment
                        // always turns out unique
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 2: // Path
                        Instantiate(grassPaths[0],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 3: // Player House
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        Instantiate(playerHouse,
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 4: // Iron
                        // We are placing the grass and then the fence with its transparent
                        // background on top, so that they blend together
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        Instantiate(stoneTypes[Random.RandomRange(0, stoneTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 5: // Gold
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        Instantiate(goldTypes[Random.RandomRange(0, goldTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 6: // Path up
                        Instantiate(grassPaths[2],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 7: // Path down
                        Instantiate(grassPaths[1],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 8: // Dirt
                        Instantiate(ground[Random.RandomRange(2, ground.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 9: // Boundaries / fences
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        Instantiate(fenceObstancle,
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 0: // Trees
                        Instantiate(forestTrees[Random.RandomRange(0, forestTrees.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 14:
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                }

                currentX += 0.64f;
            }

            currentX = 0;
            currentY += 0.64f;
        }
    }
	
    void Update () {
		
    }
}