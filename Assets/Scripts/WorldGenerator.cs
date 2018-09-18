using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    // Storing the collection of all the game objects we will
    // be using to generate the map with
    public GameObject[] Ground;
    public GameObject[] WaterTypes;
    public GameObject[] ForestTrees;
    public GameObject[] RockyGround;
    public GameObject[] GrassPaths;
    public GameObject[] StoneTypes;
    public GameObject[] GoldTypes;
    public GameObject PlayerHouse;
    public GameObject FenceObstacle;
    public GameObject Boundaries;
	
    // This is where we will store all the map segments into one
    // whole game world as we create each segment
    private static int mapWidth = 100;
    private static int mapHeight = 40;
        
    // We need to constantly keep track of where to draw next
    // after a segment is completed
    public static int CurrentPositionX;
    public static int CurrentPositionY;
        
    public static int[,] GameWorld = new int[mapHeight, mapWidth];

    void Start () {
        RockyPlains.GenerateRockyPlains(20, 20, 5);
        CurrentPositionX = 20;
        ForestGenerator.GenerateForest(20, 20, 30);
        CurrentPositionX = 40;
        ForestGenerator.GenerateForest(20, 20, 20);
        CurrentPositionX = 60;
            
        // Then we are generating the small but well-populated
        // woods in both sides of the house
        ForestGenerator.GenerateForest(20, 14, 10);
        CurrentPositionX = 60;
        CurrentPositionY = 27;
        ForestGenerator.GenerateForest(20, 13, 10);
        CurrentPositionX = 40;
        CurrentPositionY = 20;
        ForestGenerator.GenerateForest(20, 20, 40);
        CurrentPositionX = 20;
        ForestGenerator.GenerateForest(20, 20, 30);
        CurrentPositionX = 0;
        RockyPlains.GenerateRockyPlains(20, 20, 15);

        // Creating the river with the fish spawners
        CurrentPositionX = 20;
        CurrentPositionY = 0;
        RiverGenerator.GenerateRiver(20, 40, 40);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if ((x < 63) && (y == mapHeight / 2))
                {
                    GameWorld[y, x] = 2; // path / road
                } else if ((x == 75) && (y == mapHeight / 2))
                {
                    GameWorld[y, x] = 3; // house of the player
                } else if (x % 20 == 0 && x != 0 && x != 2 && x < 61 && x != 7)
                {
                    GameWorld[y, x] = 9; // Boundaries of biomes
                }
                    
                // Drawing the path head and tail
                if ((x == 0 && y == mapHeight / 2) || (x == 62 && y == mapHeight / 2))
                {
                    GameWorld[y, x] = 1; // path endings can be grass for simplicity
                }
                    
                // Drawing the zombie's side of the game world
                if (x > 79)
                {
                    GameWorld[y, x] = 8;
                }

                if (x > 60 && y > 13 && y < 27 && GameWorld[y, x] != 2 && GameWorld[y, x] != 1 && GameWorld[y, x] != 3)
                {
                    GameWorld[y, x] = 8;
                }
                
                // Water to represent the ocean
                if (x > 90)
                {
                    GameWorld[y, x] = 16;
                }
                
                if (x == 90)
                {
                    GameWorld[y, x] = 17;
                }
                
                // Adding stone in the rocky forest biome
                if (((x > 20 && x < 40) && ((y < 19) || (y > 21))) && Random.Range(0, 101) > 96 && GameWorld[y, x] != 0)
                {
                    GameWorld[y, x] = 15;
                }
                
                // Adding stone in the house region
                if (((x > 60 && x < 80) && ((y < 15) || (y > 26))) && Random.Range(0, 101) > 99 && GameWorld[y, x] != 0)
                {
                    GameWorld[y, x] = 15;
                }
                
                // We also create the Boundaries of the
                // game world in this code block
                if ((x < 63) && (y == mapHeight / 2 - 1 || y == mapHeight / 2 + 1))
                {
                    if (x > 20 && x % 13 == 0 && x != 0)
                    {
                        for (int i = mapHeight / 2 - Random.Range(3, 7); i < mapHeight / 2; i++)
                        {
                            GameWorld[i, x] = 7; // path going down
                        }

                        for (int i = mapHeight / 2 + 1; i < mapHeight / 2 + Random.Range(4, 7); i++)
                        {
                            GameWorld[i, x] = 6; // path going down
                        }
                    }
                    else
                    {
                        GameWorld[y, x] = 9; // Boundaries of biomes
                    }
                }
            }
        }

        // In this section I draw the Boundaries of the map's width and height
        // so that the player can never leave the world and break the game
        float currentX = 0;
        float currentY = 0;

        for (int y = 0; y < mapHeight; y++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX - 0.64f, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        for (int x = 0; x < mapWidth; x++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        
        // We need to start at 0 after we iterate on the map so that
        // its easier to calculate the Boundaries
        currentX = 0;
        currentY = 0;
        
        for (int x = 0; x < mapWidth; x++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY - 0.64f), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        for (int y = mapHeight; y > 0; y--)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        currentX = 0;
        currentY = 0;
        int sortingLayerGrass = mapHeight;
        int sortingLayerTrees = mapHeight * 2;

        // We begin instantiating every object on the map so
        // that we can visualize it with randomized sprites
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                switch (GameWorld[y, x])
                {
                    case 1: // Grass
                        // The random range generator will help us pick
                        // objects from the arrays randomly so the environment
                        // always turns out unique
                        var grass = Instantiate(Ground[Random.Range(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        grass.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        
                        /*{
                            var grassPosition = grass.transform.position;
                            grassPosition = new Vector3(grass.transform.position.x - 0.01f, grass.transform.position.y + 0.15f);
                            grass.transform.position = grassPosition;
                        }*/
                            break;
                        case 2: // Path
                            Instantiate(GrassPaths[0],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                            break;
                        case 3: // Player House
                            Instantiate(Ground[Random.Range(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            Instantiate(PlayerHouse,
                                new Vector2(currentX, currentY), Quaternion.identity);
                            break;
                        case 4: // Stone
                            // We are placing the grass and then the fence with its transparent
                            // backGround on top, so that they blend together
                            Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                            var rockStone = Instantiate(StoneTypes[Random.Range(0, StoneTypes.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Stones").transform);
                            rockStone.tag = "PlacedBlock";
                            UiButtonController.PlacedBlocks.Add(rockStone);
                            break;
                        case 5: // Gold
                            Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                            var gold = Instantiate(GoldTypes[Random.Range(0, GoldTypes.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Golds").transform);
                            gold.tag = "PlacedBlock";
                            UiButtonController.PlacedBlocks.Add(gold);
                            break;
                        case 6: // Path up
                            Instantiate(GrassPaths[2],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                            break;
                        case 7: // Path down
                            Instantiate(GrassPaths[1],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                            break; // x 18.92 y 17.87408
                        case 8: // Dirt
                            Instantiate(Ground[Random.Range(2, Ground.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            break;
                        case 9: // Boundaries / fences
                            if (Random.Range(0, 101) > 12)
                            {
                                var grassFence = Instantiate(Ground[Random.Range(0, 2)],
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                                grassFence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                                
                                /*{
                                    var grassPosition = grassFence.transform.position;
                                    grassPosition = new Vector3(grassFence.transform.position.x - 0.01f, grassFence.transform.position.y + 0.15f);
                                    grassFence.transform.position = grassPosition;
                                }*/
                            }
                            else
                            {
                                var grassFence = Instantiate(Ground[Random.Range(0, 2)],
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                                var fence = Instantiate(FenceObstacle,
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Fences").transform);
                                grassFence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                                fence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass + 1;
                                
                                /*{
                                    var grassPosition = grassFence.transform.position;
                                    grassPosition = new Vector3(grassFence.transform.position.x - 0.01f, grassFence.transform.position.y + 0.15f);
                                    grassFence.transform.position = grassPosition;
                                }*/
                            }
                            break;
                        case 0: // Trees
                            var grassTree = Instantiate(Ground[Random.Range(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            var tree = Instantiate(ForestTrees[Random.Range(0, ForestTrees.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Forest Trees").transform);
                            grassTree.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                            tree.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerTrees + 1;
                            tree.tag = "PlacedBlock";
                            UiButtonController.PlacedBlocks.Add(tree);
                                
                            /*{
                                var grassPosition = grassTree.transform.position;
                                grassPosition = new Vector3(grassTree.transform.position.x - 0.01f, grassTree.transform.position.y + 0.15f);
                                grassTree.transform.position = grassPosition;
                            }*/
                                break;
                            case 14: // Rocky Ground for the natural resources
                                Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                                break;
                            case 15: // Stone only in the forest
                                var grassStone = Instantiate(Ground[Random.Range(0, 2)],
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                                var stone = Instantiate(StoneTypes[Random.Range(0, StoneTypes.Length)],
                                    new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Stones").transform);
                                grassStone.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                                stone.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass + 1;
                                stone.tag = "PlacedBlock";
                                UiButtonController.PlacedBlocks.Add(stone);
                                    
                            /*{
                                var grassPosition = grassStone.transform.position;
                                grassPosition = new Vector3(grassStone.transform.position.x - 0.01f, grassStone.transform.position.y + 0.15f);
                                grassStone.transform.position = grassPosition;
                            }*/
                        break;
                    case 16: // Water tiles for "ponds" or small pools of water
                        Instantiate(WaterTypes[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
                        break;
                    case 17: // Water tiles for "ponds" or small pools of water
                        Instantiate(WaterTypes[1],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
                        break;
                }

                currentX += 0.64f;
            }

            currentX = 0;
            currentY += 0.64f;
            sortingLayerTrees--;
            sortingLayerGrass--;
        }
    }
}