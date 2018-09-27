using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverController : MonoBehaviour
{
    public static float BlockClickedX;
    public static float BlockClickedY;
	
    private SpriteRenderer _spriteRenderer;
    private Color _initialColors;
    private Renderer _renderer;
    private Transform _transform;
	
    //
    // INVENTORY GAME TEXTURES
    //
    private GameObject[] _actionsUi;
    private GameObject[] _pickUi;
    private GameObject _notification;
    private bool _canBuild = true;
    
    // Player play modes
    public static string PlayMode = "Creative";
	
    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialColors = _spriteRenderer.color;
        _renderer = GetComponent<Renderer>();
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _notification = GameObject.FindGameObjectWithTag("NotificationUi");
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerController.ClosePickingBlocks();
            PlayerController.CloseButtonOnClick();
            if (PlayMode == "Creative")
            {
                PlayMode = "Survival";
                _notification.GetComponentInChildren<Text>().text = "You are now in " + PlayMode + " mode";
            }
            else
            {
                PlayMode = "Creative";
                _notification.GetComponentInChildren<Text>().text = "You are now in " + PlayMode + " mode";
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            BlockClickedX = _transform.position.x;
            BlockClickedY = _transform.position.y;
			
            _spriteRenderer.color = new Color(0, 0, 0, 0.7f);
            _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0.4f);
        }
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = _initialColors;
        _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 1f);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && PlayMode == "Creative") // If player right clicks (1), left click (0)
        {
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
                        actionsUiPos.x = Input.mousePosition.x + 25;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                    }
                    else
                    {
                        actionsUiPos.x = Input.mousePosition.x + 25;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                        if (ui.name == "BuildButton")
                        {
                            ui.SetActive(false);
                        }
                    }
                    if (ui.name == "DestroyButton")
                    {
                        actionsUiPos.x = Input.mousePosition.x + 156;
                        actionsUiPos.y = Input.mousePosition.y + 50;
                    }
                    else if (ui.name == "CloseButton")
                    {
                        actionsUiPos.x = Input.mousePosition.x + 295;
                        actionsUiPos.y = Input.mousePosition.y + 133;
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