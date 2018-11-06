using UnityEngine;
using UnityEngine.EventSystems;

public class HoverController : MonoBehaviour
{
    public static float BlockClickedX;
    public static float BlockClickedY;
    public static GameObject BlockHoveredOver;
	
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
    private bool _isOutOfRange = true;

    public Material RedFlash;
    public Material GreenFlash;
    public Material GrassMaterial;
    public Material Tree1;
    public Material Tree2;
    public Material Tree3;
    public Material Tree4;
    public Material Tree5;
    private GameObject _player;
    public static GameObject ClickedBlock;
	
    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialColors = _spriteRenderer.color;
        _renderer = GetComponent<Renderer>();
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _transform = GetComponent<Transform>();
        
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnMouseExit()
    {
        /*
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
        */

        _spriteRenderer.material = GrassMaterial;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Player Range" ||
            other.transform.tag == "Player" ||
            other.transform.tag == "Melee Weapon" ||
            other.transform.tag == "PlayerDetector" ||
            other.transform.tag == "PlacedBlock")
        {
            _isOutOfRange = false;
        }

        if (gameObject.layer == 14 &&
            (other.transform.tag == "Zombie Cop" || other.transform.tag == "Zombie"))
        {
            other.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        
        if (gameObject.layer == 14 && other.transform.tag == "PlayerDetector")
        {
            other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 10;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Player" ||
            other.transform.tag == "Melee Weapon" ||
            other.transform.tag == "PlayerDetector" ||
            other.transform.tag == "PlacedBlock")
        {
            _isOutOfRange = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player" ||
            other.transform.tag == "Melee Weapon" ||
            other.transform.tag == "PlayerDetector" ||
            other.transform.tag == "PlacedBlock")
        {
            _isOutOfRange = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player Range" ||
            other.transform.tag == _player.transform.tag ||
            other.transform.tag == "Melee Weapon" ||
            other.transform.tag == "PlayerDetector" ||
            other.transform.tag == "PlacedBlock" ||
            other.transform.tag == "Zombie Layer Increase Detector")
        {
            _isOutOfRange = true;
        }
        
        if (gameObject.layer == 14 &&
             (other.transform.tag == "Zombie Cop" || other.transform.tag == "Zombie"))
        {
            other.GetComponent<SpriteRenderer>().sortingOrder = 41;
        }

        if (gameObject.layer == 14 && other.transform.tag == "PlayerDetector")
        {
            other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = 11;
        }
    }

    private void OnMouseOver()
    {
        /*
        Vector3 hoverPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 0));
        float _playerPosX = _player.transform.position.x;
        float _playerPosY = _player.transform.position.y;
        */
        
        if (!EventSystem.current.IsPointerOverGameObject() && _isOutOfRange == false)
        {
            BlockClickedX = _transform.position.x;
            BlockClickedY = _transform.position.y;

            /*
            // This statement checks if the _player has encountered the
            // error where the grass beneath a tree is highlighted instead
            // of the tree itself, and redirects the mouse selector which will
            // still give feedback to the _player for the actual, intended, object.
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
                */

            _spriteRenderer.material = GreenFlash;
        }
        else
        {
            _spriteRenderer.material = RedFlash;
        }
        if (Input.GetMouseButtonDown(1)  && _isOutOfRange == false) // If _player right clicks (1), left click (0)
        {
            ClickedBlock = gameObject;
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