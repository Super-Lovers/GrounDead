using UnityEngine;
using UnityEngine.UI;

public class CloseButtonController : MonoBehaviour
{
	private GameObject[] _actionsUi;
	
	void Start ()
	{
		_actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
	}

	public void CloseButtonOnClick()
	{
		foreach (var ui in _actionsUi)
		{
			ui.SetActive(false);
		}
	}
}
