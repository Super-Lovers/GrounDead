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
    static public List<GameObject> PlacedBlocks = new List<GameObject>();
    static public List<GameObject> PlacedWaterBlocks = new List<GameObject>();

    public GameObject WoodUi;
    public GameObject StoneUi;
    public GameObject CopperUi;
    public GameObject ApplesUi;
	
    // *******************************
    // SECTION FOR DISPLAYING ACTIONS ON GROUND
    // *******************************

    private bool _showWood = true;
    private bool _showStone = true;
    private bool _showPlatform = true;
    private bool _showSpike = true;
    private bool _showFence = true;

    private AudioSource _cameraAudioSource;
    public AudioClip Mining;
    public AudioClip TreeFalling;
    public AudioClip StructurePlacement;
    
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
            }
            else
            {
                _showWood = true;
                _showStone = true;
                _showPlatform = true;
                _showSpike = true;
                _showFence = true;
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
                        WoodUi.GetComponent<Animator>().SetBool("shineWood", true);
                        Invoke("StopWoodShineAnimation", 1);
                        
                        PlayerPrefs.SetFloat("Wood", PlayerController.Wood += Random.Range(4, 9));
                        
                        if (Random.Range(0, 101) < 36)
                        {
                            PlayerPrefs.SetFloat("Apples", PlayerController.Apples += 1);
                            GameObject.FindGameObjectWithTag("PlayerApples").GetComponent<Text>().text = PlayerPrefs.GetFloat("Apples").ToString();
                            
                            ApplesUi.GetComponent<Animator>().SetBool("shineApples", true);
                            Invoke("StopApplesShineAnimation", 1);
                        }
                        
                        GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerPrefs.GetFloat("Wood").ToString();
                        
                        //Playing a sound effect
                        _cameraAudioSource.PlayOneShot(TreeFalling);
                        break;
                    }

                    if (nameOfBlock == "stone" || nameOfBlock == "fence")
                    {
                        StoneUi.GetComponent<Animator>().SetBool("shineStone", true);
                        Invoke("StopStoneShineAnimation", 1);
                        
                        PlayerPrefs.SetFloat("Stone", PlayerController.Stone += Random.Range(2, 5));
                        GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
                        _cameraAudioSource.PlayOneShot(Mining);
                        break;
                    }
                    if (nameOfBlock == "copper")
                    {
                        CopperUi.GetComponent<Animator>().SetBool("shineCopper", true);
                        Invoke("StopCopperShineAnimation", 1);
                        
                        PlayerPrefs.SetFloat("Copper", PlayerController.Copper += 3);
                        GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text = PlayerPrefs.GetFloat("Copper").ToString();
                        _cameraAudioSource.PlayOneShot(Mining);
                        break;
                    }
                }
                
                Destroy(block);
                PlacedBlocks.Remove(block);
        
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
            woodWall.GetComponent<SpriteRenderer>().sortingOrder = 40;
            woodWall.AddComponent<HitPointsController>();
            woodWall.tag = "PlacedBlock";
            woodWall.GetComponent<HitPointsController>().HitPoints = 100;
		
            PlacedBlocks.Add(woodWall);
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
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
            stoneWall.GetComponent<SpriteRenderer>().sortingOrder = 40;
            stoneWall.AddComponent<HitPointsController>();
            stoneWall.GetComponent<HitPointsController>().HitPoints = 200;
            stoneWall.tag = "PlacedBlock";
		
            PlacedBlocks.Add(stoneWall);
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
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
            platform.GetComponent<SpriteRenderer>().sortingOrder = 39;
            platform.tag = "PlacedBlock";
            platform.AddComponent<HitPointsController>();
        
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
                            block.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
            }
		
            PlacedBlocks.Add(platform);
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
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
            spikes.GetComponent<SpriteRenderer>().sortingOrder = 40;
            spikes.AddComponent<HitPointsController>();
            spikes.GetComponent<HitPointsController>().HitPoints = 20;
            spikes.tag = "PlacedBlock";
		
            PlacedBlocks.Add(spikes);
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
	
    public void BuildFence()
    {
        if (PlayerPrefs.GetFloat("Stone") > 11 && PlayerPrefs.GetFloat("Copper") > 5)
        {
            /*
            PlayerPrefs.SetFloat("Stone", PlayerController.Stone -= 1);
            PlayerPrefs.SetFloat("Copper", PlayerController.Copper -= 2);
            GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text =
                PlayerPrefs.GetFloat("Stone").ToString();
            GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text =
                PlayerPrefs.GetFloat("Copper").ToString();
            */
            
            PlayerPrefs.SetFloat("Stone", PlayerController.Stone -= 12);
            GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerPrefs.GetFloat("Stone").ToString();
            
            PlayerPrefs.SetFloat("Copper", PlayerController.Copper -= 6);
            GameObject.FindGameObjectWithTag("PlayerCopper").GetComponent<Text>().text = PlayerPrefs.GetFloat("Copper").ToString();

            var fence = Instantiate(Fence, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
            fence.GetComponent<SpriteRenderer>().sortingOrder = 40;
            fence.AddComponent<HitPointsController>();
            fence.GetComponent<HitPointsController>().HitPoints = Random.Range(40, 61);
            fence.tag = "PlacedBlock";
		
            PlacedBlocks.Add(fence);
        
            ClosePickingBlocks();
        
            PlayerController.PlayMode = "Survival";
            foreach (var ui in _actionsUi)
            {
                var uiPos = ui.transform.position;
                uiPos.x += 1000;
                ui.transform.position = uiPos;
            }
            
            _cameraAudioSource.PlayOneShot(StructurePlacement);
        }
    }
}