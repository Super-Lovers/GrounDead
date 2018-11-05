using UnityEngine;

public class ObjectVisibilityDetector : MonoBehaviour
{
	public Material DefaultMaterial;
	public Material GrassMap;
	private SpriteRenderer _spriteRenderer;
	
	private void Start()
	{
		_spriteRenderer = GetComponentInParent<SpriteRenderer>();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other == null)
		{
			_spriteRenderer.sortingOrder = 11;
			_spriteRenderer.material = GrassMap;
			_spriteRenderer.color = new Color(255, 255, 255, 1);
		}
		if (other.transform.tag == "Object Visibility Detector")
		{
			if (gameObject.layer == 14)
			{
				_spriteRenderer.sortingOrder = 40;
			}
			else
			{
				_spriteRenderer.sortingOrder = 100;
			}
			_spriteRenderer.material = DefaultMaterial;
			_spriteRenderer.color = new Color(0, 0, 0, 0.5f);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.tag == "Object Visibility Detector")
		{
			_spriteRenderer.sortingOrder = 11;
			_spriteRenderer.material = GrassMap;
			_spriteRenderer.color = new Color(255, 255, 255, 1);
		}
	}
}
