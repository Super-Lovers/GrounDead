using Boo.Lang;
using SurvivalConcept;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    private float worldWidthBounds = 0;
    private float worldHeightBounds = 0;
    
    // Storing the collection of all the game objects we will
    // be using to generate the map with
    public GameObject[] ground;
    public GameObject[] waterTypes;
    public GameObject[] forestTrees;
    public GameObject[] rockyGround;
    public GameObject[] grassPaths;
    public GameObject[] stoneTypes;
    public GameObject[] goldTypes;
    public GameObject playerHouse;
    public GameObject fenceObstacle;
    public GameObject boundaries;
	
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
        currentPositionX = 20;
        ForestGenerator.GenerateForest(20, 20, 30);
        currentPositionX = 40;
        ForestGenerator.GenerateForest(20, 20, 20);
        currentPositionX = 60;
            
        // Then we are generating the small but well-populated
        // woods in both sides of the house
        ForestGenerator.GenerateForest(20, 14, 10);
        currentPositionX = 60;
        currentPositionY = 27;
        ForestGenerator.GenerateForest(20, 13, 10);
        currentPositionX = 40;
        currentPositionY = 20;
        ForestGenerator.GenerateForest(20, 20, 40);
        currentPositionX = 20;
        ForestGenerator.GenerateForest(20, 20, 30);
        currentPositionX = 0;
        RockyPlains.GenerateRockyPlains(20, 20, 15);

        // Creating the river with the fish spawners
        currentPositionX = 20;
        currentPositionY = 0;
        RiverGenerator.GenerateRiver(20, 40, 20);

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
                    
                // Adding stone in the rocky forest biome
                if (((x > 20 && x < 40) && ((y < 19) || (y > 21))) && Random.RandomRange(0, 101) > 96 && gameWorld[y, x] != 0)
                {
                    gameWorld[y, x] = 15;
                }
                
                // Adding stone in the house region
                if (((x > 60 && x < 80) && ((y < 15) || (y > 26))) && Random.RandomRange(0, 101) > 99 && gameWorld[y, x] != 0)
                {
                    gameWorld[y, x] = 15;
                }
            }
        }

        // In this section I draw the boundaries of the map's width and height
        // so that the player can never leave the world and break the game
        float currentX = 0;
        float currentY = 0;

        for (int y = 0; y < mapHeight; y++)
        {
            Instantiate(boundaries,
                new Vector2(currentX - 0.64f, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        for (int x = 0; x < mapWidth; x++)
        {
            Instantiate(boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        
        // We need to start at 0 after we iterate on the map so that
        // its easier to calculate the boundaries
        currentX = 0;
        currentY = 0;
        
        for (int x = 0; x < mapWidth; x++)
        {
            Instantiate(boundaries,
                new Vector2(currentX, currentY - 0.64f), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        for (int y = mapHeight; y > 0; y--)
        {
            Instantiate(boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        currentX = 0;
        currentY = 0;

        // We begin instantiating every object on the map so
        // that we can visualize it with randomized sprites
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
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        break;
                    case 2: // Path
                        Instantiate(grassPaths[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break;
                    case 3: // Player House
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        Instantiate(playerHouse,
                            new Vector2(currentX, currentY), Quaternion.identity);
                        break;
                    case 4: // Stone
                        // We are placing the grass and then the fence with its transparent
                        // background on top, so that they blend together
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                        Instantiate(stoneTypes[Random.RandomRange(0, stoneTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Stones").transform);
                        break;
                    case 5: // Gold
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                        Instantiate(goldTypes[Random.RandomRange(0, goldTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Golds").transform);
                        break;
                    case 6: // Path up
                        Instantiate(grassPaths[2],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break;
                    case 7: // Path down
                        Instantiate(grassPaths[1],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break;
                    case 8: // Dirt
                        Instantiate(ground[Random.RandomRange(2, ground.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        break;
                    case 9: // Boundaries / fences
                        if (Random.RandomRange(0, 101) > 70)
                        {
                            Instantiate(ground[Random.RandomRange(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        }
                        else
                        {
                            Instantiate(ground[Random.RandomRange(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            Instantiate(fenceObstacle,
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Fences").transform); 
                        }
                        break;
                    case 0: // Trees
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        Instantiate(forestTrees[Random.RandomRange(0, forestTrees.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Forest Trees").transform);
                        break;
                    case 14: // Rocky ground for the natural resources
                        Instantiate(rockyGround[Random.RandomRange(0, rockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                        break;
                    case 15: // Stone only in the forest
                        Instantiate(ground[Random.RandomRange(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                        Instantiate(stoneTypes[Random.RandomRange(0, stoneTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Stones").transform);
                        break;
                    case 16: // Water tiles for "ponds" or small pools of water
                        Instantiate(waterTypes[Random.RandomRange(0, waterTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
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