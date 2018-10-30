using UnityEngine;
using UnityEngine.UI;

public class BuildPopUpController : MonoBehaviour
{
	private GameObject _popUpUi;
	private GameObject _requirementOne;
	private GameObject _requirementTwo;
	
	// Resources to display that are required for each block
	public Sprite Wood;
	public Sprite Stone;
	public Sprite Copper;
	public Sprite GunPowder;

	private void Start()
	{
		_popUpUi = GameObject.Find("Pop-ups");
		_requirementOne = GameObject.Find("Requirement One");
		_requirementTwo = GameObject.Find("Requirement Two");

		var reqOnePos = _requirementOne.GetComponent<Image>().rectTransform.position;
		_requirementOne.GetComponent<Image>().rectTransform.position = reqOnePos;
		
		var reqTwoPos = _requirementTwo.GetComponent<Image>().rectTransform.position;
		_requirementTwo.GetComponent<Image>().rectTransform.position = reqTwoPos;
	}

	public void ShowWoodRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = Wood;
		_requirementOne.GetComponentInChildren<Text>().text = "6";
		_requirementTwo.GetComponent<Image>().sprite = null;
		_requirementTwo.GetComponentInChildren<Text>().text = "";
		if (PlayerController.Wood >= 6)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}
	public void ShowStoneRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = Stone;
		_requirementOne.GetComponentInChildren<Text>().text = "6";
		_requirementTwo.GetComponent<Image>().sprite = null;
		_requirementTwo.GetComponentInChildren<Text>().text = "";
		if (PlayerController.Stone >= 6)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}
	public void ShowPlatformRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = Wood;
		_requirementOne.GetComponentInChildren<Text>().text = "8";
		_requirementTwo.GetComponent<Image>().sprite = null;
		_requirementTwo.GetComponentInChildren<Text>().text = "";
		if (PlayerController.Wood >= 8)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}
	public void ShowSpikesRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = Stone;
		_requirementOne.GetComponentInChildren<Text>().text = "6";
		_requirementTwo.GetComponent<Image>().sprite = Wood;
		_requirementTwo.GetComponentInChildren<Text>().text = "10";
		if (PlayerController.Stone >= 6)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		if (PlayerController.Wood >= 10)
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(255, 255, 255, 1);
	}
	public void ShowFenceRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = Stone;
		_requirementOne.GetComponentInChildren<Text>().text = "12";
		_requirementTwo.GetComponent<Image>().sprite = Copper;
		_requirementTwo.GetComponentInChildren<Text>().text = "6";
		if (PlayerController.Stone >= 12)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		if (PlayerController.Copper >= 6)
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(255, 255, 255, 1);
	}
	public void ShowTorchRequirements()
	{
		_popUpUi.SetActive(true);
		var popUpUiPos = _popUpUi.transform.position;
		popUpUiPos.x = Input.mousePosition.x;
		popUpUiPos.y = Input.mousePosition.y;
		_popUpUi.transform.position = popUpUiPos;
		
		_requirementOne.GetComponent<Image>().sprite = GunPowder;
		_requirementOne.GetComponentInChildren<Text>().text = "1";
		_requirementTwo.GetComponent<Image>().sprite = Wood;
		_requirementTwo.GetComponentInChildren<Text>().text = "1";
		if (PlayerController.GunPowder >= 1)
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementOne.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		if (PlayerController.Wood >= 1)
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(0, 130, 0, 1);
		}
		else
		{
			_requirementTwo.GetComponentInChildren<Text>().color = new Color(190, 0, 0, 1);
		}
		_requirementTwo.GetComponent<Image>().color = new Color(255, 255, 255, 1);
	}

	public void HidePopUp()
	{
		_popUpUi.SetActive(false);
	}
}
