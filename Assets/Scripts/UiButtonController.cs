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
                actionsUiPos.x = Input.mousePosition.x + 286;
                actionsUiPos.y = Input.mousePosition.y + 133;
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
                        PlayerPrefs.SetFloat("Wood", PlayerController.Wood++);
                        GameObject.FindGameObjectWithTag("PlayerWood").GetComponent<Text>().text = PlayerController.Wood.ToString();
                        
                        //Playing a sound effect
                        _cameraAudioSource.PlayOneShot(TreeFalling);
                        break;
                    }

                    if (nameOfBlock == "stone" || nameOfBlock == "fence")
                    {
                        PlayerPrefs.SetFloat("Stone", PlayerController.Stone++);
                        GameObject.FindGameObjectWithTag("PlayerStone").GetComponent<Text>().text = PlayerController.Stone.ToString();
                        _cameraAudioSource.PlayOneShot(Mining);
                        break;
                    }
                    if (nameOfBlock == "gold")
                    {
                        PlayerPrefs.SetFloat("Gold", PlayerController.Gold++);
                        GameObject.FindGameObjectWithTag("PlayerGold").GetComponent<Text>().text = PlayerController.Gold.ToString();
                        _cameraAudioSource.PlayOneShot(Mining);
                        break;
                    }
                }
                
                Destroy(block);
                PlacedBlocks.Remove(block);
        
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
        var woodWall = Instantiate(Wood, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
        woodWall.GetComponent<SpriteRenderer>().sortingOrder = 40;
        woodWall.tag = "PlacedBlock";
		
        PlacedBlocks.Add(woodWall);
        
        ClosePickingBlocks();
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
	
    public void BuildStonewall()
    {
        var stoneWall = Instantiate(Stone, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
        stoneWall.GetComponent<SpriteRenderer>().sortingOrder = 40;
        stoneWall.tag = "PlacedBlock";
		
        PlacedBlocks.Add(stoneWall);
        
        ClosePickingBlocks();
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
	
    public void BuildPlatform()
    {
        var platform = Instantiate(Platform, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
        platform.GetComponent<SpriteRenderer>().sortingOrder = 39;
        platform.tag = "PlacedBlock";
        
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
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
	
    public void BuildSpikes()
    {
        var spikes = Instantiate(Spikes, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
        spikes.GetComponent<SpriteRenderer>().sortingOrder = 40;
        spikes.tag = "PlacedBlock";
		
        PlacedBlocks.Add(spikes);
        
        ClosePickingBlocks();
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
	
    public void BuildFence()
    {
        var fence = Instantiate(Fence, new Vector2(HoverController.BlockClickedX, HoverController.BlockClickedY),  Quaternion.identity);
        fence.GetComponent<SpriteRenderer>().sortingOrder = 40;
        fence.tag = "PlacedBlock";
		
        PlacedBlocks.Add(fence);
        
        ClosePickingBlocks();
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
}