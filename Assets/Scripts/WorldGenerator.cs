﻿using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    // Storing the collection of all the game objects we will
    // be using to generate the map with
    [Range(0, 100)]
    public int RateOfSoilVariety;
    public GameObject[] Ground;
    public GameObject[] WaterTypes;
    public GameObject[] ForestTrees;
    public GameObject[] RockyGround;
    public GameObject[] GrassPaths;
    public GameObject[] StoneTypes;
    public GameObject[] CopperTypes;
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
        ForestGenerator.GenerateForest(20, 14, 30);
        CurrentPositionX = 60;
        CurrentPositionY = 27;
        ForestGenerator.GenerateForest(20, 13, 30);
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
                if ((x < 61) && (y == mapHeight / 2))
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
                if ((x == 0 && y == mapHeight / 2))
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
                    GameWorld[y, x] = 16;
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
                if ((x < 61) && (y == mapHeight / 2 - 1 || y == mapHeight / 2 + 1))
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

                // Left Zombie border
                if (x == 61 && (y >= 14 && y <= 21))
                {
                    GameWorld[y, x] = 33; // Dirt split left
                }

                if (x == 61 && (y >= 22 && y <= 26))
                {
                    GameWorld[y, x] = 33; // Dirt split left
                }

                if ((x > 61 && x < 80) && y == 26)
                {
                    GameWorld[y, x] = 34; // Dirt split top
                }
                if ((x > 61 && x < 80) && y == 14)
                {
                    GameWorld[y, x] = 35; // Dirt split bottom
                }

                if (x == 80 && (y > 26 && y < 40))
                {
                    GameWorld[y, x] = 33; // Dirt split left
                }

                if (x == 80 && (y >= 0 && y < 14))
                {
                    GameWorld[y, x] = 33; // Dirt split left
                }
                
                if (x == 79 && y == 13)
                {
                    //GameWorld[y, x] = 36; // Dirt top right
                }
                
                if (x == 61 && y == 14)
                {
                    GameWorld[y, x] = 31; // Dirt top left
                }
                
                if (x == 61 && y == 26)
                {
                    GameWorld[y, x] = 32; // Dirt bottom left
                }
                
                if (x == 80 && y == 26)
                {
                    GameWorld[y, x] = 37; // Dirt bottom left
                }

                if (x == 80 && y == 14)
                {
                    GameWorld[y, x] = 38; // Dirt bottom left
                }
            }
        }

        for (int waterX = 0; waterX < mapWidth; waterX++)
        {
            for (int waterY = 39; waterY > 0; waterY--)
            {
                if (waterY < 39)
                {
                    if (GameWorld[waterY + 1, waterX] != 16 && GameWorld[waterY, waterX] == 16)
                    {
                        GameWorld[waterY + 1, waterX] = 19;
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
                    case 31: // Dirt top left
                        var dirtTopLeft = Instantiate(Ground[6],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtTopLeft.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        break;
                    case 32: // Dirt bottom left
                        var dirtBottomLeft = Instantiate(Ground[7],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtBottomLeft.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        break;
                    case 33: // Dirt split left
                        var dirtSplitLeft = Instantiate(Ground[8],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtSplitLeft.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        break;
                    case 34: // Dirt split top
                        var dirtSplitTop = Instantiate(Ground[9],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtSplitTop.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        break;
                    case 35: // Dirt split bottom
                        var dirtBottomSplit = Instantiate(Ground[10],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtBottomSplit.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        break;
                    case 36: // Dirt top right
                        var dirtTopRight = Instantiate(Ground[11],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtTopRight.GetComponent<SpriteRenderer>().sortingOrder = 26;
                        break;
                    case 37: // Dirt bottom left edge
                        var dirtBottomLeftEdge = Instantiate(Ground[13],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtBottomLeftEdge.GetComponent<SpriteRenderer>().sortingOrder = 5;
                        break;
                    case 38: // Dirt top left edge
                        var dirtTopLeftEdge = Instantiate(Ground[14],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindWithTag("Greener Ground").transform);
                        dirtTopLeftEdge.GetComponent<SpriteRenderer>().sortingOrder = 5;
                        break;
                    case 1: // Grass 1
                        // The random range generator will help us pick
                        // objects from the arrays randomly so the environment
                        // always turns out unique
                        
                        int pickedGrass = 0;
                        if (Random.Range(0, 100) > RateOfSoilVariety)
                        {
                            int rng = Random.Range(0, 100);
                            if (rng > 0 && rng < 1)
                            {
                                pickedGrass = 0;
                            }
                            else if (rng > 1 && rng < 50)
                            {
                                pickedGrass = 2;
                            }
                            else if (rng > 50 && rng < 75)
                            {
                                pickedGrass = 3;
                            }
                            else if (rng > 75 && rng < 100)
                            {
                                pickedGrass = 4;
                            }
                        }

                        if (pickedGrass == 0 && Random.Range(0, 100) < 61)
                        {
                            var invertedGrass = Instantiate(Ground[15],
                                new Vector2(currentX, currentY), Quaternion.identity,
                                GameObject.FindWithTag("Greener Ground").transform);
                            invertedGrass.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        }
                        else
                        {
                            var grass = Instantiate(Ground[pickedGrass],
                                new Vector2(currentX, currentY), Quaternion.identity,
                                GameObject.FindWithTag("Greener Ground").transform);
                            grass.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        }
                        break;
                    case 2: // Path
                        Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break;
                    case 3: // Player House
                        Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        /*Instantiate(PlayerHouse,
                            new Vector2(currentX, currentY), Quaternion.identity);*/
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
                    case 5: // Copper
                        Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Rocky Ground").transform);
                        var copper = Instantiate(CopperTypes[Random.Range(0, CopperTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Coppers").transform);
                        copper.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(copper);
                        break;
                    case 6: // Path up
                        Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break;
                    case 7: // Path down
                        Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Grass Paths").transform);
                        break; // x 18.92 y 17.87408
                    case 8: // Dirt
                        Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        break;
                    case 9: // Boundaries / fences
                        if (Random.Range(0, 101) > 12)
                        {
                            var grassFence = Instantiate(Ground[0],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            grassFence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        }
                        else
                        {
                            var grassFence = Instantiate(Ground[Random.Range(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                            var fence = Instantiate(FenceObstacle,
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Fences").transform);
                            grassFence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                            fence.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass + 1;
                            fence.tag = "PlacedBlock";
                            UiButtonController.PlacedBlocks.Add(fence);
                            fence.AddComponent<HitPointsController>();
                        }
                        break;
                    case 0: // Trees
                        var grassTree = Instantiate(Ground[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Greener Ground").transform);
                        var tree = Instantiate(ForestTrees[Random.Range(0, ForestTrees.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Forest Trees").transform);
                        grassTree.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass;
                        tree.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerTrees + 1;
                        tree.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(tree);
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
                        break;
                    case 16: // Water tiles for "ponds" or small pools of water
                        var water1 = Instantiate(WaterTypes[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
                        UiButtonController.PlacedWaterBlocks.Add(water1);
                        break;
                    case 18: // Water tiles for "ponds" or small pools of water
                        var water2 = Instantiate(WaterTypes[2],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
                        UiButtonController.PlacedWaterBlocks.Add(water2);
                        break;
                    case 19: // Water tiles for "ponds" or small pools of water
                        var water3 = Instantiate(WaterTypes[1],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindWithTag("Water Pools").transform);
                        water3.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerGrass + 1;
                        UiButtonController.PlacedWaterBlocks.Add(water3);
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