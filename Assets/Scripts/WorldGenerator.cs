using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    // Storing the collection of all the game objects we will
    // be using to generate the map with
    [Range(0, 100)]
    public int RateOfSoilVariety;
    public GameObject[] Ground;
    public GameObject[] WaterTypes;
    public GameObject[] ForestTrees;
    public GameObject[] RockyGround;
    public GameObject[] RockyTransitions;
    public GameObject[] GrassPaths;
    public GameObject[] StoneTypes;
    public GameObject[] CopperTypes;
    public GameObject PlayerHouse;
    public GameObject FenceObstacle;
    public GameObject Boundaries;
    public static List<GameObject> SumOfInteractableWorldObjects = new List<GameObject>();
	
    // This is where we will store all the map segments into one
    // whole game world as we create each segment
    public static int MapWidth = 100;
    public static int MapHeight = 40;
        
    // We need to constantly keep track of where to draw next
    // after a segment is completed
    public static int CurrentPositionX;
    public static int CurrentPositionY;
    private int _sortingLayerGrass;
    private int _sortingLayerTrees;
        
    public static int[,] GameWorld = new int[MapHeight, MapWidth];
    public GameObject NewTerrainNotification;

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

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            { 
                if ((x < 61) && (y == MapHeight / 2))
                {
                    GameWorld[y, x] = 2; // path / road
                } else if ((x == 75) && (y == MapHeight / 2))
                {
                    GameWorld[y, x] = 3; // house of the player
                } else if (x % 20 == 0 && x != 0 && x != 2 && x < 61 && x != 7)
                {
                    GameWorld[y, x] = 9; // Boundaries of biomes
                }
                    
                // Drawing the path head and tail
                if ((x == 0 && y == MapHeight / 2))
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
                    GameWorld[y, x] = 18;
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
                if ((x < 61) && (y == MapHeight / 2 - 1 || y == MapHeight / 2 + 1))
                {
                    if (x > 20 && x % 13 == 0 && x != 0)
                    {
                        for (int i = MapHeight / 2 - Random.Range(3, 7); i < MapHeight / 2; i++)
                        {
                            GameWorld[i, x] = 7; // path going down
                        }

                        for (int i = MapHeight / 2 + 1; i < MapHeight / 2 + Random.Range(4, 7); i++)
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
                
                if ((x >= 0 && x < 20) && y == 18) // rockBottom
                {
                    GameWorld[y, x] = 90;
                }
                if ((x >= 0 && x < 20) && y == 22) // rockTop
                {
                    GameWorld[y, x] = 91;
                }

                if ((x == 20 && (y >= 0 && y < 18)) || ((x == 20) && (y >= 23 && y < 40))) // rockLeft
                {
                    GameWorld[y, x] = 92;
                }

                if (x == 20 && y == 22) // rockTopLeft
                {
                    GameWorld[y, x] = 93;
                }

                if (x == 20 && y == 18) // rockBottomLeft
                {
                    GameWorld[y, x] = 94;
                }
            }
        }

        for (int waterX = 0; waterX < MapWidth; waterX++)
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

        for (int y = 0; y < MapHeight; y++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX - 0.64f, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        for (int x = 0; x < MapWidth; x++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        
        // We need to start at 0 after we iterate on the map so that
        // its easier to calculate the Boundaries
        currentX = 0;
        currentY = 0;
        
        for (int x = 0; x < MapWidth; x++)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY - 0.64f), Quaternion.identity, GameObject.FindGameObjectWithTag("Boundaries").transform);
            currentX += 0.64f;
        }
        for (int y = MapHeight; y > 0; y--)
        {
            Instantiate(Boundaries,
                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Boundaries").transform);
            currentY += 0.64f;
        }

        currentX = 0;
        currentY = 0;
        _sortingLayerGrass = MapHeight;
        _sortingLayerTrees = MapHeight * 2;

        // We begin instantiating every object on the map so
        // that we can visualize it with randomized sprites
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                switch (GameWorld[y, x])
                {
                    case 31: // Dirt top left
                        var dirtTopLeft = Instantiate(Ground[6],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtTopLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 32: // Dirt bottom left
                        var dirtBottomLeft = Instantiate(Ground[7],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtBottomLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 33: // Dirt split left
                        var dirtSplitLeft = Instantiate(Ground[8],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtSplitLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 34: // Dirt split top
                        var dirtSplitTop = Instantiate(Ground[9],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtSplitTop.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 35: // Dirt split bottom
                        var dirtBottomSplit = Instantiate(Ground[10],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtBottomSplit.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 36: // Dirt top right
                        var dirtTopRight = Instantiate(Ground[11],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtTopRight.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 37: // Dirt bottom left edge
                        var dirtBottomLeftEdge = Instantiate(Ground[13],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtBottomLeftEdge.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 38: // Dirt top left edge
                        var dirtTopLeftEdge = Instantiate(Ground[14],
                            new Vector2(currentX, currentY), Quaternion.identity,
                            GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirtTopLeftEdge.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
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
                                GameObject.FindGameObjectWithTag("Greener Ground").transform);
                            invertedGrass.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        }
                        else
                        {
                            var grass = Instantiate(Ground[pickedGrass],
                                new Vector2(currentX, currentY), Quaternion.identity,
                                GameObject.FindGameObjectWithTag("Greener Ground").transform);
                            grass.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        }
                        break;
                    case 2: // Path
                        var path = Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Grass Paths").transform);
                        path.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        SumOfInteractableWorldObjects.Add(path);
                        break;
                    case 3: // Player House
                        var house = Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        house.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        /*Instantiate(PlayerHouse,
                            new Vector2(currentX, currentY), Quaternion.identity);*/
                        break;
                    case 4: // Stone
                        // We are placing the grass and then the fence with its transparent
                        // backGround on top, so that they blend together
                        var groundStone = Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        var rockStone = Instantiate(StoneTypes[Random.Range(0, StoneTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Stones").transform);
                        groundStone.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        rockStone.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                        rockStone.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(rockStone);
                        SumOfInteractableWorldObjects.Add(rockStone);
                        break;
                    case 5: // Copper
                        var groundCopper = Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        var copper = Instantiate(CopperTypes[Random.Range(0, CopperTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Coppers").transform);
                        groundCopper.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        copper.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                        copper.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(copper);
                        SumOfInteractableWorldObjects.Add(copper);
                        break;
                    case 6: // Path up
                        var pathUp = Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Grass Paths").transform);
                        pathUp.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        SumOfInteractableWorldObjects.Add(pathUp);
                        break;
                    case 7: // Path down
                        var pathDown = Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Grass Paths").transform);
                        pathDown.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        SumOfInteractableWorldObjects.Add(pathDown);
                        break; // x 18.92 y 17.87408
                    case 8: // Dirt
                        var dirt = Instantiate(Ground[5],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        dirt.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        SumOfInteractableWorldObjects.Add(dirt);
                        break;
                    case 9: // Boundaries / fences
                        if (Random.Range(0, 101) > 12)
                        {
                            var grassFence = Instantiate(Ground[0],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Greener Ground").transform);
                            grassFence.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                            SumOfInteractableWorldObjects.Add(grassFence);
                        }
                        else
                        {
                            var grassFence = Instantiate(Ground[Random.Range(0, 2)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Greener Ground").transform);
                            var fence = Instantiate(FenceObstacle,
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Fences").transform);
                            grassFence.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                            fence.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                            fence.tag = "PlacedBlock";
                            UiButtonController.PlacedBlocks.Add(fence);
                            SumOfInteractableWorldObjects.Add(fence);
                            fence.AddComponent<HitPointsController>();
                        }
                        break;
                    case 0: // Trees
                        var grassTree = Instantiate(Ground[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Greener Ground").transform);
                        grassTree.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        var tree = Instantiate(ForestTrees[Random.Range(0, ForestTrees.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Forest Trees").transform);
                        tree.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingLayerTrees + 1;
                        tree.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(tree);
                        SumOfInteractableWorldObjects.Add(tree);
                        break;
                    case 14: // Rocky Ground for the natural resources
                        var groundRock = Instantiate(RockyGround[Random.Range(0, RockyGround.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        groundRock.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 15: // Stone only in the forest
                        var grassStone = Instantiate(Ground[Random.Range(0, 2)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        var stone = Instantiate(StoneTypes[Random.Range(0, StoneTypes.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Stones").transform);
                        grassStone.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        stone.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                        stone.tag = "PlacedBlock";
                        UiButtonController.PlacedBlocks.Add(stone);
                        SumOfInteractableWorldObjects.Add(stone);
                        break;
                    case 16: // Water tiles for "ponds" or small pools of water
                        var water1 = Instantiate(WaterTypes[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Water Pools").transform);
                        UiButtonController.PlacedWaterBlocks.Add(water1);
                        SumOfInteractableWorldObjects.Add(water1);
                        water1.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 18: // Water tiles for "ponds" or small pools of water
                        var water2 = Instantiate(WaterTypes[2],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Water Pools").transform);
                        UiButtonController.PlacedWaterBlocks.Add(water2);
                        SumOfInteractableWorldObjects.Add(water2);
                        water2.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 19: // Water tiles for "ponds" or small pools of water
                        var water3 = Instantiate(WaterTypes[1],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Water Pools").transform);
                        water3.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                        UiButtonController.PlacedWaterBlocks.Add(water3);
                        SumOfInteractableWorldObjects.Add(water3);
                        water3.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 90: // rockBottom
                        var rockBottom = Instantiate(RockyTransitions[0],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        rockBottom.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 91: // rockTop
                        var rockTop = Instantiate(RockyTransitions[3],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        rockTop.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 92: // rockLeft
                        var rockLeft = Instantiate(RockyTransitions[2],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        rockLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 93: // rockTopLeft
                        var rockTopLeft = Instantiate(RockyTransitions[4],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        rockTopLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                    case 94: // rockBottomLeft
                        var rockBottomLeft = Instantiate(RockyTransitions[1],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Rocky Ground").transform);
                        rockBottomLeft.GetComponent<SpriteRenderer>().sortingOrder = _sortingLayerGrass;
                        break;
                }

                currentX += 0.64f;
                _sortingLayerGrass--;
            }

            currentX = 0;
            currentY += 0.64f;
            _sortingLayerTrees--;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Invoke("RegenerateWorld", 0.5f);
        }
    }

    public void RegenerateWorld()
    {
        NewTerrainNotification.SetActive(true);
        
        float currentX = 0;
        float currentY = 0;
        
        _sortingLayerGrass = MapHeight;
        _sortingLayerTrees = MapHeight * 2;
        
        List<GameObject> currentObjectsInTheWorld = SumOfInteractableWorldObjects.ToList();
        var player = GameObject.FindGameObjectWithTag("Player");
        
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                bool isItSafeToInstantiate = true;
                foreach (var obj in currentObjectsInTheWorld)
                {
                    if ((obj.transform.position.x == currentX && obj.transform.position.y == currentY) ||
                        (obj.transform.position.x == player.transform.position.x && obj.transform.position.y == player.transform.position.y))
                    {
                        isItSafeToInstantiate = false;
                        break;
                    }
                }

                if (isItSafeToInstantiate)
                {
                    // Adding stone in the rocky forest biome
                    if (((x > 0 && x < 39) && ((y < 19) || (y > 22))) && Random.Range(0, 101) > 96)
                    {
                        if (Random.Range(0, 101) > 50)
                        {
                            var rockStone = Instantiate(StoneTypes[Random.Range(0, StoneTypes.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Stones").transform);
                            rockStone.tag = "PlacedBlock";
                            rockStone.name += "Q";
                            rockStone.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                            UiButtonController.PlacedBlocks.Add(rockStone);
                            SumOfInteractableWorldObjects.Add(rockStone);
                        }
                        else
                        {
                            var copper = Instantiate(CopperTypes[Random.Range(0, CopperTypes.Length)],
                                new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Coppers").transform);
                            copper.tag = "PlacedBlock";
                            copper.name += "Q";
                            copper.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingLayerGrass + 1;
                            UiButtonController.PlacedBlocks.Add(copper);
                            SumOfInteractableWorldObjects.Add(copper);
                        }
                    }

                    if (((x > 20 && x < 80) && ((y < 19) || (y > 22))) && Random.Range(0, 101) > 98)
                    {
                        var tree = Instantiate(ForestTrees[Random.Range(0, ForestTrees.Length)],
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Forest Trees").transform);
                        tree.tag = "PlacedBlock";
                        tree.name += "Q";
                        tree.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingLayerTrees + 1;
                        UiButtonController.PlacedBlocks.Add(tree);
                        SumOfInteractableWorldObjects.Add(tree);
                    }
                }
                
                currentX += 0.64f;
                _sortingLayerGrass--;
            }
            
            currentX = 0;
            currentY += 0.64f;
            _sortingLayerTrees--;
        }
    }

    public void HideNewTerrainNotification()
    {
        NewTerrainNotification.SetActive(false);
    }
}