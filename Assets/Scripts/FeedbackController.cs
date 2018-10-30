using UnityEngine;

public class FeedbackController : MonoBehaviour
{
	private RectTransform _rectTransform;
	
	private void Start()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update ()
	{
		var currentPos = _rectTransform.localPosition;
		currentPos = new Vector3(currentPos.x, currentPos.y + 1f, currentPos.z);
		_rectTransform.localPosition = currentPos;
		
		Invoke("RemoveObject", 0.7f);
	}

	void RemoveObject()
	{
		Destroy(gameObject);
	}
}
