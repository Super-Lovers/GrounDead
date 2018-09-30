using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
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
