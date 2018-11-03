using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {


	public Sprite HoveredButtonImage;
	public Sprite ClickedButtonImage;
	public Sprite ExitedButtonImage;

	public GameObject ButtonStart;
	public GameObject ButtonExit;

	private void Start()
	{
		// Unpause if the player is back in the main menu.
		Time.timeScale = 1;
	}

	public void StartHoverButton()
	{
		ButtonStart.GetComponent<Image>().sprite = HoveredButtonImage;
		var buttonTextPos = ButtonStart.GetComponentInChildren<Text>().transform.position;
		buttonTextPos.y -= 4;
		ButtonStart.GetComponentInChildren<Text>().transform.position = buttonTextPos;
	}
	public void StartClickedButton()
	{
		ButtonStart.GetComponent<Image>().sprite = ClickedButtonImage;
	}
	public void StartExitedButton()
	{
		ButtonStart.GetComponent<Image>().sprite = ExitedButtonImage;
		var buttonTextPos = ButtonStart.GetComponentInChildren<Text>().transform.position;
		buttonTextPos.y += 4;
		ButtonStart.GetComponentInChildren<Text>().transform.position = buttonTextPos;
	}
	
	public void QuitHoverButton()
	{
		ButtonExit.GetComponent<Image>().sprite = HoveredButtonImage;
		var buttonExitTextPos = ButtonExit.GetComponentInChildren<Text>().transform.position;
		buttonExitTextPos.y -= 4;
		ButtonExit.GetComponentInChildren<Text>().transform.position = buttonExitTextPos;
	}
	public void QuitClickedButton()
	{
		ButtonExit.GetComponent<Image>().sprite = ClickedButtonImage;
	}
	public void QuitExitedButton()
	{
		ButtonExit.GetComponent<Image>().sprite = ExitedButtonImage;
		var buttonExitTextPos = ButtonExit.GetComponentInChildren<Text>().transform.position;
		buttonExitTextPos.y += 4;
		ButtonExit.GetComponentInChildren<Text>().transform.position = buttonExitTextPos;
	}

	public void StartTheGame()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void QuitTheGame()
	{
		Application.Quit();
	}

	public void ReturnToStartMenu()
	{
		MenuController.ResetGameWorld();
		
		SceneManager.LoadScene("StartMenuScene");
	}
}
