using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtonController : MonoBehaviour
{
    private GameObject[] _actionsUi;
    private GameObject[] _pickUi;
	
    // Blocks for instantiation on the map
    public GameObject Wood;
    public GameObject Stone;
    public GameObject Platform;
    public GameObject Spikes;
    public GameObject Fence;
    public GameObject Torch;
    public GameObject PopUp;
    static public List<GameObject> PlacedBlocks = new List<GameObject>();
    static public List<GameObject> PlacedWaterBlocks = new List<GameObject>();

    public GameObject WoodUi;
    public GameObject StoneUi;
    public GameObject CopperUi;
    public GameObject ApplesUi;
    public GameObject GatheredMaterials;
	
    // *******************************
    // SECTION FOR DISPLAYING ACTIONS ON GROUND
    // *******************************

    private bool _showWood = true;
    private bool _showStone = true;
    private bool _showPlatform = true;
    private bool _showSpike = true;
    private bool _showFence = true;
    private bool _showTorch = true;

    private AudioSource _cameraAudioSource;
    public AudioClip Mining;
    public AudioClip TreeFalling;
    public AudioClip StructurePlacement;
    public GameObject NotificationDamage;
    public static List<GameObject> PlacedStructures = new List<GameObject>();

    public Font NotificationFonts;
    
    private void Start()
    {
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _cameraAudioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    public void BuildBlockButton()
    {
        var objectOnTopOfGround = GameObject.FindGameObjectsWithTag("PlacedBlock");
        
        foreach (var block in objectOnTopOfGround)
        {
            if (HoverController.BlockClickedX == block.transform.position.x &&
                HoverController.BlockClickedY == block.transform.position.y)
            {
                _showWood = false;
                _showStone = false;
                _showPlatform = false;
                _showSpike = false;
                _showFence = false;
                _showTorch = false;
            }
            else
            {
                _showWood = true;
                _showStone = true;
                _showPlatform = true;
                _showSpike = true;
                _showFence = true;
                _showTorch = true;
            }
        }
        
        foreach (var ui in _pickUi)
        {
            ui.SetActive(false);
            var actionsUiPos = ui.transform.position;
				
            if (ui.name == "BlockWoodWall" && _showWood)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 25;
                actionsUiPos.y = Input.mousePosition.y + 100;
            }
            else if (ui.name == "BlockStoneWall" && _showStone)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 64 + 25;
                actionsUiPos.y = Input.mousePosition.y + 100;
            }
            else if (ui.name == "BlockPlatform" && _showPlatform)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 128 + 25;
                actionsUiPos.y = Input.mousePosition.y + 100;
            }
            else if (ui.name == "BlockSpike" && _showSpike)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 25;
                actionsUiPos.y = Input.mousePosition.y + 30;
            }
            else if (ui.name == "BlockFence" && _showFence)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 64 + 25;
                actionsUiPos.y = Input.mousePosition.y + 30;
            }
            else if (ui.name == "BlockTorch" && _showTorch)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 128 + 25;
                actionsUiPos.y = Input.mousePosition.y + 30;
            }
            else if (ui.name == "CloseButton")
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 220;
                actionsUiPos.y = Input.mousePosition.y + 161;
            }
            else if (ui.name == "Panel")
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x;
                actionsUiPos.y = Input.mousePosition.y;
            }

            ui.transform.position = actionsUiPos;
        }
    }

    private void StopWoodShineAnimation()
    {
        WoodUi.GetComponent<Animator>().SetBool("shineWood", false);
    }

    private void StopStoneShineAnimation()
    {
        StoneUi.GetComponent<Animator>().SetBool("shineStone", false);
    }

    private void StopCopperShineAnimation()
    {
        CopperUi.GetComponent<Animator>().SetBool("shineCopper", false);
    }

    private void StopApplesShineAnimation()
    {
        ApplesUi.GetComponent<Animator>().SetBool("shineApples", false);
    }

    public void DestroyBlockButton()
    {
        foreach (GameObject block in PlacedBlocks.ToArray())
        {
            if (HoverController.BlockClickedX == block.transform.position.x &&
                HoverController.BlockClickedY == block.transform.position.y)
            {
                string nameOfBlock = "";
                for (int i = 0; i < block.name.Length; i++)
                {
                    nameOfBlock += block.name[i];
                    if (nameOfBlock == "tree")
                    {
                        // Total wood gathered
                        var gatheredWood = Random.Range(4, 9);
                        PlayerPrefs.SetFloat("Wood", PlayerController.Wood += gatheredWood);
                        
                        // Notification for feedback
                        var notification = Instantiate(GatheredMaterials, Camera.main.WorldToScreenPoint(block.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
                        notification.GetComponentInChildren<Text>().text = "+" + gatheredWood + " Wood";
                        notification.GetComponentInChildren<Text>().font = NotificationFonts;
                        
                        WoodUi.GetComponent<Animator>().SetBool("shineWood", true);
                        Invoke("StopWoodShineAnimation", 1);
                        
                        if (Random.Range(0, 101) < 36)
                        {
                            PlayerController.Apples += 1;
                            PlayerPrefs.SetFloat("Apples", PlayerController.Apples);
                            GameObject.FindGameObjectWithTag("PlayerApples").GetComponent<Text>().text = PlayerPrefs.GetFloat("Apples").ToString();
                            
                            ApplesUi.GetComponent<Animator>().SetBool("shineApples", true);
                            
                            var notificationApple = Instantiate(GatheredMaterials, Camera.main.WorldToScreenPoint(
                                new Vector3(block.transform.position.x, block.transform.position.y - 0.3f, block.transform.position.z)), Quaternion.identity, GameObject.Find("Canvas").transform);
                            notificationApple.GetComponentInChildren<Text>().text = "+" + 1 + " Apple";
                            notificationApple.GetComponentInChildren<Text>().font = NotificationFonts;
                            
                            Invoke("StopApplesShineAnimation", 1);
            
                            // Player Statistics
                            MenuController.TotalApplesCollected++;
                            MenuController.TotalGatheringScore += 100;
                        }
                        
                        GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerPrefs.GetFloat("Wood").ToString();
            
                        // Player Statistics
                        MenuController.TotalTreesChopped++;
                        MenuController.TotalGatheringScore += 100;
                        
                        MenuController.UpdateScore();
                        
                        //Playing a sound effect
                        _cameraAudioSource.PlayOneShot(TreeFalling);
                        break;
                    }

                    if (nameOfBlock == "stone" || nameOfBlock == "fence")
                    {
                        var gatheredStone = Random.Range(2, 5);
                        var notification = Instantiate(GatheredMaterials, Camera.main.WorldToScreenPoint(block.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
                        notification.GetComponentInChildren<Text>().text = "+" + gatheredStone + " Stone";
                        notification.GetComponentInChildren<Text>().font = NotificationFonts;
                        
                        StoneUi.GetComponent<Animator>().SetBool("shineStone", true);
                        Invoke("StopStoneShineAnimation", 1);
                        
                        PlayerPrefs.SetFloat("Stone", PlayerController.Stone += gatheredStone);
                        GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
                        _cameraAudioSource.PlayOneShot(Mining);

                        MenuController.TotalStoneMined++;
                        MenuController.TotalGatheringScore += 100;
                        
                        MenuController.UpdateScore();
                        
                        break;
                    }
                    if (nameOfBlock == "copper")
                    {
                        
                        var notification = Instantiate(GatheredMaterials, Camera.main.WorldToScreenPoint(block.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
                        notification.GetComponentInChildren<Text>().text = "+" + 3 + " Copper";
                        notification.GetComponentInChildren<Text>().font = NotificationFonts;
                        
                        CopperUi.GetComponent<Animator>().SetBool("shineCopper", true);
                        Invoke("StopCopperShineAnimation", 1);
                        
                        PlayerPrefs.SetFloat("Copper", PlayerController.Copper += 3);
                        GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text = PlayerPrefs.GetFloat("Copper").ToString();
                        _cameraAudioSource.PlayOneShot(Mining);
                        
                        // Player Statistics
                        MenuController.TotalCopperMined++;
                        MenuController.TotalGatheringScore += 100;
                        
                        MenuController.UpdateScore();
                        
                        break;
                    }
                }
                
                Destroy(block);
                PlacedBlocks.Remove(block);
                PlacedStructures.Remove(block);
                WorldGenerator.SumOfInteractableWorldObjects.Remove(block);
        
                PlayerController.PlayMode = "Survival";
                foreach (var ui in _actionsUi)
                {
                    var uiPos = ui.transform.position;
                    uiPos.x += 1000;
                    ui.transform.position = uiPos;
                }
            }
        }
        
        // This loop checks if we are destroying a platform block and then
        // turns on the waters' collider below it
        foreach (GameObject block in PlacedWaterBlocks.ToArray())
        {
            if (HoverController.BlockClickedX == block.transform.position.x &&
                HoverController.BlockClickedY == block.transform.position.y)
            {
                string nameOfBlock = "";
                for (int i = 0; i < block.name.Length; i++)
                {
                    nameOfBlock += block.name[i];
                    if (nameOfBlock == "water")
                    {
                        block.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
        }
    }

    public void CloseButtonOnClick()
    {
        
        PlayerController.PlayMode = "Survival";
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
	
    // *******************************
    // SECTION FOR PICKING BLOCKS
    // *******************************

    public void ClosePickingBlocks()
    {
        foreach (var ui in _pickUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }

    public void BuildWoodWall()
    {
        if (PlayerPrefs.GetFloat("Wood") > 5)
        {
            // Updating the player's inventory
            PlayerPrefs.SetFloat("Wood", PlayerController.Wood -= 6);
            GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerPrefs.GetFloat("Wood").ToString();
            
            var woodWall = Instantiate(Wood, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(woodWall);
            
            woodWall.GetComponent<HitPointsController>().HitPoints = 100;
            //ZombieController._playerDetectorList.Add(woodWall);
            
            // Notification feedback for building a wall
            var notification = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(woodWall.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notification.GetComponentInChildren<Text>().text = "-" + 6 + " Wood";
            notification.GetComponentInChildren<Text>().font = NotificationFonts;
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalWoodenWallsBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildStonewall()
    {
        if (PlayerPrefs.GetFloat("Stone") > 5)
        {
            PlayerPrefs.SetFloat("Stone", PlayerController.Stone -= 6);
            GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
            
            var stoneWall = Instantiate(Stone, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(stoneWall);
            
            stoneWall.GetComponent<HitPointsController>().HitPoints = 200;
            
            var notification = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(stoneWall.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notification.GetComponentInChildren<Text>().text = "-" + 6 + " Stone";
            notification.GetComponentInChildren<Text>().font = NotificationFonts;
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalStoneWallsBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildPlatform()
    {
        if (PlayerPrefs.GetFloat("Wood") > 7)
        {
            // Updating the player's inventory
            PlayerPrefs.SetFloat("Wood", PlayerController.Wood -= 8);
            GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text =
                PlayerPrefs.GetFloat("Wood").ToString();
            
            var platform = Instantiate(Platform, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(platform);
            
            foreach (GameObject block in PlacedWaterBlocks.ToArray())
            {
                if (platform.transform.position.x == block.transform.position.x &&
                    platform.transform.position.y == block.transform.position.y)
                {
                    string nameOfBlock = "";
                    for (int i = 0; i < block.name.Length; i++)
                    {
                        nameOfBlock += block.name[i];
                        if (nameOfBlock == "water")
                        {
                            ObjectivesController.HasPlacedWoodenPlatform = true;
                            block.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
            }
            
            var notification = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(platform.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notification.GetComponentInChildren<Text>().text = "-" + 8 + " Wood";
            notification.GetComponentInChildren<Text>().font = NotificationFonts;
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalPlatformsBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildSpikes()
    {
        if (PlayerPrefs.GetFloat("Stone") > 5 && PlayerPrefs.GetFloat("Wood") > 9)
        {
            PlayerPrefs.SetFloat("Stone", PlayerController.Stone -= 6);
            GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
            
            PlayerPrefs.SetFloat("Wood", PlayerController.Wood -= 10);
            GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerPrefs.GetFloat("Wood").ToString();
            
            var spikes = Instantiate(Spikes, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(spikes);
            
            spikes.GetComponent<HitPointsController>().HitPoints = 20;
            
            var notificationStone = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(spikes.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationStone.GetComponentInChildren<Text>().text = "-" + 6 + " Stone";
            notificationStone.GetComponentInChildren<Text>().font = NotificationFonts;
            var notificationWood = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(
                new Vector3(spikes.transform.position.x, spikes.transform.position.y - 0.3f, spikes.transform.position.z)), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationWood.GetComponentInChildren<Text>().text = "-" + 10 + " Wood";
            notificationWood.GetComponentInChildren<Text>().font = NotificationFonts;
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalSpikeTrapsBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildFence()
    {
        if (PlayerPrefs.GetFloat("Stone") > 11 && PlayerPrefs.GetFloat("Copper") > 5)
        {
            PlayerPrefs.SetFloat("Stone", PlayerController.Stone -= 12);
            GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
            
            PlayerPrefs.SetFloat("Copper", PlayerController.Copper -= 6);
            GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text = PlayerPrefs.GetFloat("Copper").ToString();

            var fence = Instantiate(Fence, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(fence);
            
            fence.GetComponent<HitPointsController>().HitPoints = Random.Range(40, 61);
            
            var notificationStone = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(fence.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationStone.GetComponentInChildren<Text>().text = "-" + 12 + " Stone";
            notificationStone.GetComponentInChildren<Text>().font = NotificationFonts;
            var notificationCopper = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(
                new Vector3(fence.transform.position.x, fence.transform.position.y - 0.3f, fence.transform.position.z)), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationCopper.GetComponentInChildren<Text>().text = "-" + 6 + " Copper";
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalElectricFencesBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildTorch()
    {
        if (PlayerPrefs.GetFloat("Gun Powder") > 0 && PlayerPrefs.GetFloat("Wood") > 0)
        {
            PlayerPrefs.SetFloat("Wood", PlayerController.Wood -= 1);
            GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerPrefs.GetFloat("Wood").ToString();
            
            PlayerPrefs.SetFloat("Gun Powder", PlayerController.GunPowder -= 1);
            GameObject.FindGameObjectWithTag("PlayerGunPowder").GetComponent<Text>().text = PlayerPrefs.GetFloat("Gun Powder").ToString();
        
            ClosePickingBlocks();

            var torch = Instantiate(Torch, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            
            UpdateObject(torch);
            torch.GetComponent<HitPointsController>().HitPoints = Random.Range(40, 61);
            
            var notificationWood = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(torch.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationWood.GetComponentInChildren<Text>().text = "-" + 1 + " Wood";
            notificationWood.GetComponentInChildren<Text>().font = NotificationFonts;
            var notificationGunPowder = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(
                new Vector3(torch.transform.position.x, torch.transform.position.y - 0.3f, torch.transform.position.z)), Quaternion.identity, GameObject.Find("Canvas").transform);
            notificationGunPowder.GetComponentInChildren<Text>().text = "-" + 1 + " Gun Powder";
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            // Player Statistics
            MenuController.TotalTorchesBuilt++;
            MenuController.TotalBuildingScore += 100;
            
            MenuController.UpdateScore();
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }

    public void AddBullets()
    {
        if (PlayerPrefs.GetFloat("Gun Powder") > 5 && PlayerPrefs.GetFloat("Copper") > 5)
        {
            ObjectivesController.CraftedBullets = true;
            
            PlayerPrefs.SetFloat("Gun Powder", PlayerController.GunPowder -= 5);
            PlayerPrefs.SetFloat("Copper", PlayerController.Copper -= 5);
            PlayerPrefs.SetFloat("Bullets", PlayerController.Bullets += 20);
            
            
            GameObject.FindGameObjectWithTag("PlayerGunPowder").GetComponent<Text>().text = PlayerPrefs.GetFloat("Gun Powder").ToString();
            GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text = PlayerPrefs.GetFloat("Copper").ToString();
            GameObject.FindGameObjectWithTag("PlayerBullets").GetComponent<Text>().text = "Bullets: " + PlayerController.Bullets;
        }
        
        //Debug.Log("Added a bullet: " + PlayerController.Bullets);
    }

    private void UpdateObject(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sortingOrder = Mathf.Abs(Mathf.RoundToInt(HoverController.ClickedBlock.transform.position.y));
        if (PlacedStructures.Count > 0)
        {
            foreach (var structure in PlacedStructures)
            {
                if (structure.transform.position.x == obj.transform.position.x &&
                    structure.GetComponent<SpriteRenderer>().sortingOrder > obj.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    obj.GetComponent<SpriteRenderer>().sortingOrder += 3;
                } else if (structure.transform.position.x == obj.transform.position.x &&
                           structure.GetComponent<SpriteRenderer>().sortingOrder < obj.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    obj.GetComponent<SpriteRenderer>().sortingOrder -= 3;
                } else if (structure.transform.position.x == obj.transform.position.x &&
                           structure.GetComponent<SpriteRenderer>().sortingOrder == obj.GetComponent<SpriteRenderer>().sortingOrder &&
                           structure.transform.position.y > obj.transform.position.y)
                {
                    obj.GetComponent<SpriteRenderer>().sortingOrder += 4;
                } else if (structure.transform.position.x == obj.transform.position.x &&
                           structure.GetComponent<SpriteRenderer>().sortingOrder == obj.GetComponent<SpriteRenderer>().sortingOrder &&
                           obj.transform.position.y > structure.transform.position.y)
                {
                    obj.GetComponent<SpriteRenderer>().sortingOrder -= 4;
                }
            }
        }
        obj.AddComponent<HitPointsController>();
        obj.tag = "PlacedBlock";
		
        PlacedBlocks.Add(obj);
        PlacedStructures.Add(obj);
        WorldGenerator.SumOfInteractableWorldObjects.Add(obj);
    }
}