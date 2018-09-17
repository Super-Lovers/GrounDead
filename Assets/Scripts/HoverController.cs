using UnityEngine;

public class HoverController : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	private Color _initialColors;
	private Renderer _renderer;
	private Transform _transform;
	
	//
	// INVENTORY GAME TEXTURES
	//
	public Texture2D Hammer;
	private GameObject[] _actionsUI;
	
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_initialColors = _spriteRenderer.color;
		_renderer = GetComponent<Renderer>();
		_transform = GetComponent<Transform>();
		_actionsUI = GameObject.FindGameObjectsWithTag("ActionUI");
;	}

	private void OnMouseEnter()
	{
		_spriteRenderer.color = new Color(0, 0, 0, 0.7f);
		_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 0.4f);
	}

	private void OnMouseExit()
	{
		_spriteRenderer.color = _initialColors;
		_renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, 1f);
	}

	private void OnMouseDown()
	{
		foreach (var ui in _actionsUI)
		{
			ui.SetActive(true);
			var actionsUIPos = ui.transform.position;
			if (ui.name == "Button")
			{
				actionsUIPos.x = Input.mousePosition.x + 25;
				actionsUIPos.y = Input.mousePosition.y + 25;
			} else if (ui.name == "CloseButton")
			{
				actionsUIPos.x = Input.mousePosition.x + 163;
				actionsUIPos.y = Input.mousePosition.y + 75;
			} else if (ui.name == "Panel")
			{
				actionsUIPos.x = Input.mousePosition.x;
				actionsUIPos.y = Input.mousePosition.y;
			}
			ui.transform.position = actionsUIPos;
		}
	}
}
