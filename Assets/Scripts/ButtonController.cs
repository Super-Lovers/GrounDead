using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {


	public Sprite HoveredButtonImage;
	public Sprite ClickedButtonImage;
	public Sprite ExitedButtonImage;

	public GameObject ButtonStart;
	public GameObject ButtonExit;

	public void StartHoverButton()
	{
		ButtonStart.GetComponent<UnityEngine.UI.Image>().sprite = HoveredButtonImage;
	}
	public void StartClickedButton()
	{
		ButtonStart.GetComponent<UnityEngine.UI.Image>().sprite = ClickedButtonImage;
	}
	public void StartExitedButton()
	{
		ButtonStart.GetComponent<UnityEngine.UI.Image>().sprite = ExitedButtonImage;
	}
	
	public void QuitHoverButton()
	{
		ButtonExit.GetComponent<UnityEngine.UI.Image>().sprite = HoveredButtonImage;
	}
	public void QuitClickedButton()
	{
		ButtonExit.GetComponent<UnityEngine.UI.Image>().sprite = ClickedButtonImage;
	}
	public void QuitExitedButton()
	{
		ButtonExit.GetComponent<UnityEngine.UI.Image>().sprite = ExitedButtonImage;
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
		SceneManager.LoadScene("StartMenuScene");
	}
}
