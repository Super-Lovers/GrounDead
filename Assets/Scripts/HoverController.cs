using UnityEngine;
using UnityEngine.EventSystems;

public class HoverController : MonoBehaviour
{
    public static float BlockClickedX;
    public static float BlockClickedY;
	
    private SpriteRenderer _spriteRenderer;
    private Color _initialColors;
    private Renderer _renderer;
    private Transform _transform;
    //private BoxCollider2D _collider2D;
	
    //
    // INVENTORY GAME TEXTURES
    //
    private GameObject[] _actionsUi;
    private GameObject[] _pickUi;
    private bool _canBuild = true;
    private bool _isOutOfRange;

    public Material RedFlash;
    public Material GreenFlash;
    public Material GrassMaterial;
    public Material Tree1;
    public Material Tree2;
    public Material Tree3;
    public Material Tree4;
    public Material Tree5;
	
    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialColors = _spriteRenderer.color;
        _renderer = GetComponent<Renderer>();
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _transform = GetComponent<Transform>();
    }

    private void OnMouseEnter()
    {
        Vector3 hoverPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 0));
        float playerPosX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        float playerPosY = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        
        if ((playerPosX + 0.64f < hoverPosition.x || playerPosX - 0.64f > hoverPosition.x) ||
            playerPosY + 0.64f < hoverPosition.y || playerPosY - 0.64f > hoverPosition.y)
        {
            _isOutOfRange = true;
        }
        else
        {
            _isOutOfRange = false;
        }
        
        if (!EventSystem.current.IsPointerOverGameObject() && _isOutOfRange == false)
        {
            BlockClickedX = _transform.position.x;
            BlockClickedY = _transform.position.y;

            // This statement checks if the player has encountered the
            // error where the grass beneath a tree is highlighted instead
            // of the tree itself, and redirects the mouse selector which will
            // still give feedback to the player for the actual, intended, object.
            if (_transform.name == "grass1(Clone)" || _transform.name == "grass2(Clone)" || _transform.name == "grass3(Clone)" || _transform.name == "grass4(Clone)" || _transform.name == "grass5(Clone)" || _transform.name == "grass1Inverted(Clone)")
            {
                foreach (var block in UiButtonController.PlacedBlocks)
                {
                    if (BlockClickedX == block.transform.position.x &&
                        BlockClickedY == block.transform.position.y)
                    {
                        if (block.GetComponent<SpriteRenderer>() != null)
                        {
                            block.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.7f);
                            block.GetComponent<SpriteRenderer>().material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0.4f);
                        }
                        else
                        {
                            block.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0.7f);
                            block.GetComponentInChildren<SpriteRenderer>().material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0.4f);
                        }
                    }
                }
            }
            
            _spriteRenderer.color = new Color(0, 0, 0, 0.7f);
            _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0.4f);
        }
    }

    private void OnMouseExit()
    {
        foreach (var block in UiButtonController.PlacedBlocks)
        {
            if (BlockClickedX == block.transform.position.x &&
                BlockClickedY == block.transform.position.y)
            {
                if (block.GetComponent<SpriteRenderer>() != null)
                {
                    block.GetComponent<SpriteRenderer>().color = _initialColors;
                    block.GetComponent<SpriteRenderer>().material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 1f);
                }
                else
                {
                    block.GetComponentInChildren<SpriteRenderer>().color = _initialColors;
                    block.GetComponentInChildren<SpriteRenderer>().material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 1f);
                }
            }
        }
        
        _spriteRenderer.color = _initialColors;
        _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 1f);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)  && _isOutOfRange == false) // If player right clicks (1), left click (0)
        {
            PlayerController.PlayMode = "Creative";
            if (gameObject.tag == "PlacedBlock")
            {
                _canBuild = false;
            }
            else
            {
                _canBuild = true;
            }
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                foreach (var ui in _pickUi)
                {
                    ui.SetActive(false);
                }
				
                foreach (var ui in _actionsUi)
                {
                    ui.SetActive(false);
                    ui.SetActive(true);
                    var actionsUiPos = ui.transform.position;
				
                    if (ui.name == "BuildButton" && _canBuild)
                    {
                        actionsUiPos.x = Input.mousePosition.x + 18;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                    }
                    else
                    {
                        actionsUiPos.x = Input.mousePosition.x + 18;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                        if (ui.name == "BuildButton")
                        {
                            ui.SetActive(false);
                        }
                    }
                    if (ui.name == "DestroyButton")
                    {
                        actionsUiPos.x = Input.mousePosition.x + 110;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                    }
                    else if (ui.name == "CloseButton")
                    {
                        actionsUiPos.x = Input.mousePosition.x + 169;
                        actionsUiPos.y = Input.mousePosition.y + 129;
                    }
                    else if (ui.name == "Panel")
                    {
                        actionsUiPos.x = Input.mousePosition.x;
                        actionsUiPos.y = Input.mousePosition.y;
                    }

                    ui.transform.position = actionsUiPos;
                }
            }
        }
    }
}