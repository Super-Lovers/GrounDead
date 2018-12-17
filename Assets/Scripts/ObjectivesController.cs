using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour {
    public static Dictionary<string, string> CurrentTasks = new Dictionary<string, string>();

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
    public int MaxNumberOfZombies;
    public int NumberOfZombiesToSpawnLeft;
    public int NumberOfZombiesToSpawnRight;
    private int _numberOfZombiesToSpawnOriginal;
    public float ChanceOfZombieToSpawn;
    public static int CurrentZombiesAlive;
    public static int CurrentDay;
    public GameObject ZombieBasic;
    public GameObject ZombieCop;
    public GameObject ZombieBoss;
    public static bool IsItDay = true;
    public GameObject Camera;
    public GameObject NewWaveNotification;
    private GameObject _worldGenerator;
	
    // Use this for initialization
    void Start ()
    {
        _worldGenerator = GameObject.Find("God (World generator)");
        _taskTitle = GameObject.FindGameObjectWithTag("Task Title");
        _taskDescription = GameObject.FindGameObjectWithTag("Task Description");
        
        if (IsTutorialComplete == false)
        {
            WalkedRight = false;
            WalkedLeft = false;
            WalkedUp = false;
            WalkedDown = false;
            HasAttackedMelee = false;
            HasAttackedRange = false;
            HasSwitchedWeapons = false;
            HasPlacedWoodenPlatform = false;
            CraftedBullets = false;
            
            CurrentTasks.Clear();
            // Tasks for the player to accomplish
            CurrentTasks.Add("Learn To Move", "Using the W, S, A, D keys, try to move around to become comfortable with the controls.");
            CurrentTasks.Add("Learn To Defend Yourself", "You press the <space> bar to attack using your melee weapon and the <left mouse button> to use your rifle, try it!");
            CurrentTasks.Add("Collect Wood", "Collect wood by right clicking on a tree or a tile beneath it and by selecting the red hammer.");
            CurrentTasks.Add("Place Wooden Wall", "Place a wooden wall by right clicking a ground tile, selecting the green hammer and clicking on the wooden wall.");
            CurrentTasks.Add("Place Wooden Platform", "Place a wooden platform by right clicking on a *water* tile, selecting the green hammer and clicking on the wooden platform.");
            CurrentTasks.Add("Craft Rifle Bullets", "If your rifle is out of bullets, you can easily craft them with 5 gun powder and 5 copper by clicking on the + button on the top-right corner next to your total bullets interface.");
            CurrentTasks.Add("How to Heal Yourself", "You're hurt from one of the zombies, you can use the apples collected so far from the trees to heal yourself using the <Q> button!");
		
            // Start the first task for the player to complete.
            _taskTitle.GetComponent<Text>().text = "Learn To Move";
            _taskDescription.GetComponent<Text>().text = CurrentTasks["Learn To Move"];
        }
        else
        {
            InvokeRepeating("UpdateWorldTime", 30, 30);
            TasksWindow.SetActive(false);
        }
        CompletedTaskPopUp.SetActive(false);
        
        _numberOfZombiesToSpawnOriginal = NumberOfZombiesToSpawnLeft + NumberOfZombiesToSpawnRight;
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (IsTutorialComplete == false)
        {
            if (WalkedRight && WalkedLeft && WalkedDown && WalkedUp && _currentTask == 1)
            {
                CompleteTask("Learn To Move");
			
                // We dont want the player to complete a task before it even starts, so we reset
                // the variables that check if he fired/meleed or not.
                HasAttackedMelee = false;
                HasAttackedRange = false;
            }
		
            if (HasAttackedMelee && HasAttackedRange && _currentTask == 2)
            {
                CompleteTask("Learn To Defend Yourself");
                ShowTasksWindow();
            }
		
            
            if (PlayerController.Wood > 20 && _currentTask == 3)
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
        }
        

        if (PlayerController.PlayerHealth < 270 && HealedSelf == false && IsTutorialComplete)
        {
            _currentTask = 7;
            TasksWindow.SetActive(true);
            _taskTitle.GetComponent<Text>().text = "How to Heal Yourself";
            _taskDescription.GetComponent<Text>().text = CurrentTasks["How to Heal Yourself"];
        }
        
        if (HealedSelf && _currentTask == 7 && PlayerController.PlayerHealth < 270) // We only want to finish the next quest if the previous one is first done.
        {
            CompleteTask("How to Heal Yourself");
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnZombies();
            
            GameObject.FindGameObjectWithTag("PlayerWave").GetComponent<Text>().text = "Day: " + CurrentDay;
        }
    }

    private void CompleteTask(string taskKey)
    {
        MenuController.TotalTutorialsScore += 200;
        MenuController.UpdateScore();
        CompletedTaskPopUp.SetActive(true);
		
        switch (taskKey)
        {
            case "Learn To Move":
                CurrentTasks.Remove(taskKey);
					
                // Once this task is completed, we change the text of the current
                // task title to the next one.
                _taskTitle.GetComponent<Text>().text = "Learn To Defend Yourself";
                _taskDescription.GetComponent<Text>().text = CurrentTasks["Learn To Defend Yourself"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Learn To Defend Yourself":
                CurrentTasks.Remove(taskKey);
						
                _taskTitle.GetComponent<Text>().text = "Collect Wood";
                _taskDescription.GetComponent<Text>().text = CurrentTasks["Collect Wood"];
						
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Collect Wood":
                CurrentTasks.Remove(taskKey);

                _taskTitle.GetComponent<Text>().text = "Place Wooden Wall";
                _taskDescription.GetComponent<Text>().text = CurrentTasks["Place Wooden Wall"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Place Wooden Wall":
                CurrentTasks.Remove(taskKey);
					
                _taskTitle.GetComponent<Text>().text = "Place Wooden Platform";
                _taskDescription.GetComponent<Text>().text = CurrentTasks["Place Wooden Platform"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Place Wooden Platform":
                CurrentTasks.Remove(taskKey);

                _taskTitle.GetComponent<Text>().text = "Craft Rifle Bullets";
                _taskDescription.GetComponent<Text>().text = CurrentTasks["Craft Rifle Bullets"];
					
                Invoke("HideCompletedPopUp", 1f);
                _currentTask++;
                break;
            case "Craft Rifle Bullets":
                CurrentTasks.Remove(taskKey);
					
                _taskTitle.GetComponent<Text>().text = "Tutorial is Completed";
                _taskDescription.GetComponent<Text>().text = "Well done, you can now start playing the game by building your base and preparing yourself for the night! You will get helpful insight from here again!";
					
                Invoke("HideCompletedPopUp", 1f);
                // Begin the game day/night cycle when the tutorial is complete.
                InvokeRepeating("UpdateWorldTime", 30, 30);
                _currentTask++;
                IsTutorialComplete = true;
                break;
            case "How to Heal Yourself":
                CurrentTasks.Remove(taskKey);

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
        // When the zombies are spawned, the wave counter is increased
        CurrentDay++;

        // After a few days have passed, the world resources will be regenerated
        if (CurrentDay % 3 == 0)
        {
            var worldGeneratorScript = _worldGenerator.GetComponent<WorldGenerator>();
            worldGeneratorScript.HideNewTerrainNotification();
            worldGeneratorScript.RegenerateWorld();
        }
        
        MenuController.UpdateScore();
        
        Camera.GetComponent<BoxCollider2D>().enabled = true;
	    
        // Separating the zombie spawn areas
        // from the free exploration areas of the player
        int segmentStart = 0;
        int segmentEnd = 20;
        int segmentBeachEnd = 10;

        
        // We are starting to generate zombies one block down the top and left border
        float currentX = 0.64f;
        float currentY = 24.96f;

        while (NumberOfZombiesToSpawnLeft > 0)
        {
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
                            else
                            {
                                // We set every new zombie's sound settings according to the current
                                // sound settings of the player set.
                                spawnedZombie.GetComponent<AudioSource>().volume = UiButtonController.CurrentVolume;
                                
                                NumberOfZombiesToSpawnLeft--;
                                break;
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
        }
        
        // Resetting the coordinates of the borders for generating the zombies.
        // We are starting to generate zombies one block down the top and right border.
        currentX = 53.36f;
        currentY = 24.96f;

        while (NumberOfZombiesToSpawnRight > 0)
        {
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
                            else
                            {
                                spawnedZombie.GetComponent<AudioSource>().volume = UiButtonController.CurrentVolume;
                                
                                NumberOfZombiesToSpawnRight--;
                                break;
                            }
                        }
                    }
                    currentX -= 0.64f;
                }

                currentX = 53.36f;
                currentY -= 0.64f;
            }
        }

        if (_numberOfZombiesToSpawnOriginal < MaxNumberOfZombies)
        {
            if (Random.Range(0, 101) < 70)
            {
                _numberOfZombiesToSpawnOriginal += 1;
                //Debug.Log("Increase zombie max");
                //Debug.Log(_numberOfZombiesToSpawnOriginal);
            }
        }
        else
        {
            _numberOfZombiesToSpawnOriginal = MaxNumberOfZombies;
        }
        NumberOfZombiesToSpawnLeft = (int)_numberOfZombiesToSpawnOriginal / 2;
        NumberOfZombiesToSpawnRight = (int)_numberOfZombiesToSpawnOriginal / 2;
        
        // When the zombies are spawned, the day counter is increased
        GameObject.FindGameObjectWithTag("PlayerWave").GetComponent<Text>().text = "Day: " + CurrentDay;
        // Slowly increasing the rate of zombies spawning!
        //ChanceOfZombieToSpawn += 0.1f;
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
            if (IsItDay)
            {
                gameObject.GetComponentInChildren<Light>().transform.Rotate(i * 5, 0, 0);
                yield return new WaitForSeconds(1f);
                spawnZombies = true;
            }
            else
            {
                GameObject[] zombiesAlive = GameObject.FindGameObjectsWithTag("Zombie");
                GameObject[] zombiesCopsAlive = GameObject.FindGameObjectsWithTag("Zombie Cop");
                GameObject[] zombiesBossesAlive = GameObject.FindGameObjectsWithTag("Zombie Boss");

                foreach (var zombie in zombiesAlive)
                {
                    Destroy(zombie);
                }
                foreach (var zombie in zombiesCopsAlive)
                {
                    Destroy(zombie);
                }
                foreach (var zombie in zombiesBossesAlive)
                {
                    Destroy(zombie);
                }

                gameObject.GetComponentInChildren<Light>().transform.Rotate(i * -5, 0, 0);
                yield return new WaitForSeconds(1f);
                spawnZombies = false;
            }
        }

        if (spawnZombies)
        {
            SpawnZombies();
            
            NewWaveNotification.SetActive(true);
            NewWaveNotification.GetComponentInChildren<Text>().text = "WAVE: " + CurrentDay;
            Invoke("HideWaveNotification", 3f);
        }

        CameraController.ChangedTheme = false;
        IsItDay = !IsItDay;
    }

    private void HideWaveNotification()
    {
        NewWaveNotification.SetActive(false);
    }
}