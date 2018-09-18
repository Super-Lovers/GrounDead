using UnityEngine;

public class UIButtonController : MonoBehaviour
{
	private GameObject[] _actionsUi;
	
	void Start ()
	{
		_actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
	}

	public void BuildBlockButton()
	{
		Debug.Log("Build");
	}

	public void DestroyBlockButton()
	{
		Debug.Log("Destroy");
	}

	public void CloseButtonOnClick()
	{
		foreach (var ui in _actionsUi)
		{
			ui.SetActive(false);
		}
	}
}
