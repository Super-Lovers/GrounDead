using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtonController : MonoBehaviour
{
    private GameObject[] _actionsUi;
    private GameObject[] _pickUi;
	
    // Blocks for instantiation on the map
    public GameObject Wood;
    static public List<GameObject> PlacedBlocks = new List<GameObject>();
	
    // *******************************
    // SECTION FOR DISPLAYING ACTIONS ON GROUND
    // *******************************

    private bool _showWood = true;
    private bool _showStone = true;

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
            }
            else
            {
                _showWood = true;
                _showStone = true;
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
                actionsUiPos.y = Input.mousePosition.y + 50;
            }
            else if (ui.name == "BlockStoneWall" && _showStone)
            {
                ui.SetActive(true);
                actionsUiPos.x = Input.mousePosition.x + 156;
                actionsUiPos.y = Input.mousePosition.y + 50;
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

                    if (nameOfBlock == "stone")
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
    }

    public void CloseButtonOnClick()
    {
        foreach (var ui in _actionsUi)
        {
            ui.SetActive(false);
        }
    }
	
    // *******************************
    // SECTION FOR PICKING BLOCKS
    // *******************************

    public void ClosePickingBlocks()
    {
        foreach (var ui in _pickUi)
        {
            ui.SetActive(false);
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
        ClosePickingBlocks();
        
        foreach (var ui in _actionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
}