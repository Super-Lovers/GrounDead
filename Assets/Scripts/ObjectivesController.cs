using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour {
    Dictionary<string, string> currentTasks = new Dictionary<string, string>();

    private GameObject _taskTitle;
    private GameObject _taskDescription;
    public GameObject TasksWindow;
    public GameObject CompletedTaskPopUp;

    // Task dependencies for completing
    private int _currentTask = 1;
    public static bool WalkedRight;
    public static bool WalkedLeft;
    public static bool WalkedUp;
    public static bool WalkedDown;
    public static bool HasAttackedMelee;
    public static bool HasAttackedRange;
    public static bool HasSwitchedWeapons;
    public static bool HasPlacedWoodenPlatform;
    public static bool CraftedBullets;
    public static bool HealedSelf;
    public static bool IsTutorialComplete;

    private int _currentWoodWallsBuilt;
	
    // Zombies and Day/Night Cycle variables
    public int NumberOfZombiesToSpawnLeft;
    public int NumberOfZombiesToSpawnRight;
    private int _numberOfZombiesToSpawnOriginal;
    public float ChanceOfZombieToSpawn;
    public static int CurrentZombiesAlive;
    public static int CurrentDay;
    public GameObject ZombieBasic;
    public GameObject ZombieCop;
    public GameObject ZombieBoss;
    private bool _isItDay = true;
    public GameObject Camera;
	
    // Use this for initialization
    void Start ()
    {
        _numberOfZombiesToSpawnOriginal = NumberOfZombiesToSpawnLeft + NumberOfZombiesToSpawnRight;
		
        // Tasks for the player to accomplish
        currentTasks.Add("Learn To Move", "Using the W, S, A, D keys, try to move around to become comfortable with the controls.");
        currentTasks.Add("Learn To Defend Yourself", "You press the <space> bar to attack using your melee weapon and the <left mouse button> to use your rifle, try it! \n\nYou can also try switch between your melee knife and axe using the <E> key!");
        currentTasks.Add("Collect Wood", "Collect wood by right clicking on a tree and selecting the red hammer.");
        currentTasks.Add("Place Wooden Wall", "Place a wooden wall by right clicking a ground tile, selecting the green hammer and clicking on the wooden wall.");
        currentTasks.Add("Place Wooden Platform", "Place a wooden platform by right clicking on a *water* tile, selecting the green hammer and clicking on the wooden platform.");
        currentTasks.Add("Craft Rifle Bullets", "If your rifle is out of bullets, you can easily craft them with 5 gun powder and 5 copper by clicking on the + button on the top-right corner next to your total bullets interface.");
        currentTasks.Add("How to Heal Yourself", "You're hurt from one of the zombies, you can use the apples collected so far from the trees to heal yourself using the <Q> button!");
		
        _taskTitle = GameObject.FindGameObjectWithTag("Task Title");
        _taskDescription = GameObject.FindGameObjectWithTag("Task Description");
		
        // Start the first task for the player to complete.
        _taskTitle.GetComponent<Text>().text = "Learn To Move";
        _taskDescription.GetComponent<Text>().text = currentTasks["Learn To Move"];
		
        CompletedTaskPopUp.SetActive(false);
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (WalkedRight && WalkedLeft && WalkedDown && WalkedUp && _currentTask == 1)
        {
            CompleteTask("Learn To Move");
			
            // We dont want the player to complete a task before it even starts, so we reset
            // the variables that check if he fired/meleed or not.
            HasAttackedMelee = false;
            HasAttackedRange = false;
        }
		
        if (HasAttackedMelee && HasAttackedRange && HasSwitchedWeapons && _currentTask == 2)
        {
            CompleteTask("Learn To Defend Yourself");
            ShowTasksWindow();
        }
		
        if (PlayerController.Wood > 200 && _currentTask == 3)
        {
            CompleteTask("Collect Wood");
            _currentWoodWallsBuilt = MenuController.TotalWoodenWallsBuilt;
        }
		
        // We also check for the current walls built
        // because the player might have built a wooden wall already
        // and we want to check from that point forward so the calculations would not go south.
        if (MenuController.TotalWoodenWallsBuilt > _currentWoodWallsBuilt &&
            _currentTask == 4) // We only want to finish the next quest if the previous one is first done.
        {
            CompleteTask("Place Wooden Wall");
        }
        if (HasPlacedWoodenPlatform && _currentTask == 5) // We only want to finish the next quest if the previous one is first done.
        {
            CompleteTask("Place Wooden Platform");
        }
        
        if (CraftedBullets && _currentTask == 6) // We only want to finish the next quest if the previous one is first done.
        {
            CompleteTask("Craft Rifle Bullets");
        }
        

        if (PlayerController.PlayerHealth < 270 && HealedSelf == false && IsTutorialComplete)
        {
            TasksWindow.SetActive(true);
            _taskTitle.GetComponent<Text>().text = "How to Heal Yourself";
            _taskDescription.GetComponent<Text>().text = currentTasks["How to Heal Yourself"];
        }
        
        if (HealedSelf && _currentTask == 7 && PlayerController.PlayerHealth < 270) // We only want to finish the next quest if the previous one is first done.
        {
            CompleteTask("How to Heal Yourself");
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnZombies();
            
            // When the zombies are spawned, the wave counter is increased
            CurrentDay++;
            GameObject.FindGameObjectWithTag("PlayerWave").GetComponent<Text>().text = "Day: " + CurrentDay;
        }
    }

    private void CompleteTask(string taskKey)
    {
        CompletedTaskPopUp.SetActive(true);
		
        switch (taskKey)
        {
            case "Learn To Move":
                currentTasks.Remove(taskKey);
					
                // Once this task is completed, we change the text of the current
                // task title to the next one.
                _taskTitle.GetComponent<Text>().text = "Learn To Defend Yourself";
                _taskDescription.GetComponent<Text>().text = currentTasks["Learn To Defend Yourself"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Learn To Defend Yourself":
                currentTasks.Remove(taskKey);
						
                _taskTitle.GetComponent<Text>().text = "Collect Wood";
                _taskDescription.GetComponent<Text>().text = currentTasks["Collect Wood"];
						
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Collect Wood":
                currentTasks.Remove(taskKey);

                _taskTitle.GetComponent<Text>().text = "Place Wooden Wall";
                _taskDescription.GetComponent<Text>().text = currentTasks["Place Wooden Wall"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Place Wooden Wall":
                currentTasks.Remove(taskKey);
					
                _taskTitle.GetComponent<Text>().text = "Place Wooden Platform";
                _taskDescription.GetComponent<Text>().text = currentTasks["Place Wooden Platform"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Place Wooden Platform":
                currentTasks.Remove(taskKey);

                _taskTitle.GetComponent<Text>().text = "Craft Rifle Bullets";
                _taskDescription.GetComponent<Text>().text = currentTasks["Craft Rifle Bullets"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Craft Rifle Bullets":
                currentTasks.Remove(taskKey);
					
                _taskTitle.GetComponent<Text>().text = "Tutorial is Completed";
                _taskDescription.GetComponent<Text>().text = "Well done, you can now start playing the game by building your base and preparing yourself for the night! You will get helpful insight from here again!";
					
                Invoke("HideCompletedPopUp", 1f);
                // Begin the game day/night cycle when the tutorial is complete.
                InvokeRepeating("UpdateWorldTime", 20, 20);
                _currentTask++;
                IsTutorialComplete = true;
                break;
            case "How to Heal Yourself":
                currentTasks.Remove(taskKey);

                _taskTitle.GetComponent<Text>().text = "No Tasks Currently.";
                _taskDescription.GetComponent<Text>().text = "As of now, there are no tasks for you to complete.";
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            default:
                Debug.Log("Invalid task name to complete.");
                break;
        }
    }

    public void SpawnZombies()
    {
        Camera.GetComponent<BoxCollider2D>().enabled = true;
	    
        // Separating the zombie spawn areas
        // from the free exploration areas of the player
        int segmentStart = 0;
        int segmentEnd = 20;
        int segmentBeachEnd = 10;

        
        // We are starting to generate zombies one block down the top and left border
        float currentX = 0.64f;
        float currentY = 24.96f;
        
        for (int y = 39; y > 1; y--)
        {
            for (int x = segmentStart; x < segmentEnd; x++)
            {
	            
                if (NumberOfZombiesToSpawnLeft > 0 && Random.Range(0, 101) < ChanceOfZombieToSpawn)
                {
                    var randomZombie = Random.Range(0, 3);
                    GameObject spawnedZombie;
                    if (randomZombie == 0)
                    {
                        spawnedZombie = Instantiate(ZombieBasic,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie";
                    }
                    else if (randomZombie == 1)
                    {
                        spawnedZombie = Instantiate(ZombieBoss,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie Boss";
                    }
                    else if (randomZombie == 2)
                    {
                        spawnedZombie = Instantiate(ZombieCop,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie Cop";
                    }
                    else
                    {
                        spawnedZombie = Instantiate(ZombieBasic,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie";
                    }
                
                    for (int i = 0; i < UiButtonController.PlacedBlocks.Count - 1; i++)
                    {
                        bool isItSafeToSpawn;
                        if (spawnedZombie.transform.position.x != UiButtonController.PlacedBlocks[i].transform.position.x &&
                            spawnedZombie.transform.position.y != UiButtonController.PlacedBlocks[i].transform.position.y)
                        {
                            isItSafeToSpawn = true;
                        }
                        else
                        {
                            isItSafeToSpawn = false;
                        }

                        if (isItSafeToSpawn == false)
                        {
                            Destroy(spawnedZombie);
                        }
                    }
                }
                
                /*if (NumberOfZombiesToSpawnLeft > 0 && Random.Range(0, 101) < ChanceOfZombieToSpawn)
                {
                    var spawnedZombie = Instantiate(ZombieBasic,
                        new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                    spawnedZombie.transform.tag = "Zombie";
                
                    for (int i = 0; i < UiButtonController.PlacedBlocks.Count - 1; i++)
                    {
                        bool isItSafeToSpawn;
                        if (spawnedZombie.transform.position.x != UiButtonController.PlacedBlocks[i].transform.position.x &&
                            spawnedZombie.transform.position.y != UiButtonController.PlacedBlocks[i].transform.position.y)
                        {
                            isItSafeToSpawn = true;
                            NumberOfZombiesToSpawnLeft--;
                        }
                        else
                        {
                            isItSafeToSpawn = false;
                        }

                        if (isItSafeToSpawn == false)
                        {
                            Destroy(spawnedZombie);
                        }
                    }
                }
                */
                currentX += 0.64f;
            }

            currentX = 0;
            currentY -= 0.64f;
        }
        
        // Resetting the coordinates of the borders for generating the zombies.
        // We are starting to generate zombies one block down the top and right border.
        
        currentX = 53.36f;
        currentY = 24.96f;
        
        for (int y = 39; y > 1; y--)
        {
            for (int x = segmentStart; x < segmentBeachEnd; x++)
            {
                if (NumberOfZombiesToSpawnRight > 0 && Random.Range(0, 101) < ChanceOfZombieToSpawn)
                {
                    var randomZombie = Random.Range(0, 3);
                    GameObject spawnedZombie;
                    if (randomZombie == 0)
                    {
                        spawnedZombie = Instantiate(ZombieBasic,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie";
                    }
                    else if (randomZombie == 1)
                    {
                        spawnedZombie = Instantiate(ZombieBoss,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie Boss";
                    }
                    else if (randomZombie == 2)
                    {
                        spawnedZombie = Instantiate(ZombieCop,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie Cop";
                    }
                    else
                    {
                        spawnedZombie = Instantiate(ZombieBasic,
                            new Vector2(currentX, currentY), Quaternion.identity, GameObject.FindGameObjectWithTag("Zombies").transform);
                        spawnedZombie.transform.tag = "Zombie";
                    }
                
                    for (int i = 0; i < UiButtonController.PlacedBlocks.Count - 1; i++)
                    {
                        bool isItSafeToSpawn;
                        if (spawnedZombie.transform.position.x != UiButtonController.PlacedBlocks[i].transform.position.x &&
                            spawnedZombie.transform.position.y != UiButtonController.PlacedBlocks[i].transform.position.y)
                        {
                            isItSafeToSpawn = true;
                        }
                        else
                        {
                            isItSafeToSpawn = false;
                        }

                        if (isItSafeToSpawn == false)
                        {
                            Destroy(spawnedZombie);
                        }
                    }
                }
                currentX -= 0.64f;
            }

            currentX = 53.36f;
            currentY -= 0.64f;
        }
        
        // When the zombies are spawned, the day counter is increased
        CurrentDay++;
        GameObject.FindGameObjectWithTag("PlayerWave").GetComponent<Text>().text = "Day: " + CurrentDay;
        NumberOfZombiesToSpawnLeft = (int)_numberOfZombiesToSpawnOriginal / 2 + 1;
        NumberOfZombiesToSpawnRight = (int)_numberOfZombiesToSpawnOriginal / 2 + 1;
	    
        // Slowly increasing the rate of zombies spawning!
        ChanceOfZombieToSpawn += 0.4f;
    }

    private void HideCompletedPopUp()
    {
        CompletedTaskPopUp.SetActive(false);
        TasksWindow.SetActive(false);

        if (_currentTask == 7 || _currentTask == 8)
        {
            TasksWindow.SetActive(false);
        }
        else
        {
            Invoke("ShowTasksWindow", 1);
        }
    }

    private void ShowTasksWindow()
    {
        TasksWindow.SetActive(true);
    }

    public void UpdateWorldTime()
    {
        StartCoroutine("UpdateWorldTimeSlowly");
    }
    private IEnumerator UpdateWorldTimeSlowly()
    {
        bool spawnZombies = false;
        for (int i = 1; i < 5; i++) {
            if (_isItDay)
            {
                gameObject.GetComponentInChildren<Light>().transform.Rotate(i * 5, 0, 0);
                yield return new WaitForSeconds(1f);
                spawnZombies = true;
            }
            else
            {
                gameObject.GetComponentInChildren<Light>().transform.Rotate(i * -5, 0, 0);
                yield return new WaitForSeconds(1f);
                spawnZombies = false;
            }
        }

        if (spawnZombies)
        {
            SpawnZombies();
        }

        _isItDay = !_isItDay;
    }
}