using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	// First Panel
	public static int TotalHostilityScore;
	public static int TotalZombiesDefeated;
	public static int TotalZombieCopsDefeated;
	public static int TotalZombieCitizensDefeated;
	public static int TotalZombieBossesDefeated;
	public static int TotalBuildingsDestroyed;
	public static int TotalDaysSurvived;
	
	// Second Panel
	public static int TotalBuildingScore;
	public static int TotalWoodenWallsBuilt;
	public static int TotalStoneWallsBuilt;
	public static int TotalPlatformsBuilt;
	public static int TotalSpikeTrapsBuilt;
	public static int TotalElectricFencesBuilt;
	public static int TotalTorchesBuilt;
	
	// Third Panel
	public static int TotalGatheringScore;
	public static int TotalTreesChopped;
	public static int TotalStoneMined;
	public static int TotalCopperMined;
	public static int TotalApplesCollected;
	public static int TotalGunPowderCollected;
	
	// Tutorial part counter
	private int _tutorialPart = 0;

	public GameObject PartOne;
	public GameObject PartTwo;
	public GameObject PartThree;
	public GameObject PartFour;
	public GameObject PartFive;

	private void Start()
	{
		// *******************
		// Tutorial Segment
		// *******************
		if (gameObject.transform.tag != "Tutorial")
		{
			PartOne.SetActive(true);
			PartTwo.SetActive(false);
			PartThree.SetActive(false);
			PartFour.SetActive(false);
			PartFive.SetActive(false);
		}
		
		// First Panel
		GameObject.Find("Total Hostility Score").GetComponent<Text>().text =
			"Total Hostility Score: " + TotalHostilityScore;
		GameObject.Find("Total Zombies Defeated").GetComponent<Text>().text =
			"Total Zombies Defeated: " + TotalZombiesDefeated;
		GameObject.Find("Total Zombie Cops Defeated").GetComponent<Text>().text =
			"Total Zombie Cops Defeated: " + TotalZombieCopsDefeated;
		GameObject.Find("Total Zombie Citizens Defeated").GetComponent<Text>().text =
			"Total Zombie Citizens Defeated: " + TotalZombieCitizensDefeated;
		GameObject.Find("Total Zombie Bosses Defeated").GetComponent<Text>().text =
			"Total Zombie Bosses Defeated: " + TotalZombieBossesDefeated;
		GameObject.Find("Total Buildings Destroyed").GetComponent<Text>().text =
			"Total Buildings Destroyed: " + TotalBuildingsDestroyed;
		TotalDaysSurvived = PlayerController.CurrentDay;
		GameObject.Find("Total Days Survived").GetComponent<Text>().text =
			"Total Days Survived: " + TotalDaysSurvived;
		
		// Second Panel
		GameObject.Find("Total Building Score").GetComponent<Text>().text =
			"Total Building Score: " + TotalBuildingScore;
		GameObject.Find("Total Wooden Walls Built").GetComponent<Text>().text =
			"Total Wooden Walls Built: " + TotalWoodenWallsBuilt;
		GameObject.Find("Total Stone Walls Built").GetComponent<Text>().text =
			"Total Stone Walls Built: " + TotalStoneWallsBuilt;
		GameObject.Find("Total Platforms Built").GetComponent<Text>().text =
			"Total Platforms Built: " + TotalPlatformsBuilt;
		GameObject.Find("Total Spike Traps Built").GetComponent<Text>().text =
			"Total Spike Traps Built: " + TotalSpikeTrapsBuilt;
		GameObject.Find("Total Electric Fences Built").GetComponent<Text>().text =
			"Total Electric Fences Built: " + TotalElectricFencesBuilt;
		GameObject.Find("Total Torches Built").GetComponent<Text>().text =
			"Total Torches Built: " + TotalTorchesBuilt;
		
		// Third Panel
		GameObject.Find("Total Gathering Score").GetComponent<Text>().text =
			"Total Gathering Score: " + TotalGatheringScore;
		GameObject.Find("Total Trees Chopped").GetComponent<Text>().text =
			"Total Trees Chopped: " + TotalTreesChopped;
		GameObject.Find("Total Stone Mined").GetComponent<Text>().text =
			"Total Stone Mined: " + TotalStoneMined;
		GameObject.Find("Total Copper Mined").GetComponent<Text>().text =
			"Total Copper Collected: " + TotalCopperMined;
		GameObject.Find("Total Apples Collected").GetComponent<Text>().text =
			"Total Apples Collected: " + TotalApplesCollected;
		GameObject.Find("Total Gun Powder Collected").GetComponent<Text>().text =
			"Total Gun Powder Collected: " + TotalGunPowderCollected;
		
		// Final Score
		long finalScore = TotalHostilityScore + TotalBuildingScore + TotalGatheringScore + (TotalDaysSurvived * 100);
		GameObject.Find("Total Final Score Text").GetComponent<Text>().text =
			"Total Final Score: " + finalScore;
		
		/*
		UpdateStats("Total Hostility Score", TotalHostilityScore);
		UpdateStats("Total Zombies Defeated", TotalZombiesDefeated);
		UpdateStats("Total Zombie Cops Defeated", TotalZombieCopsDefeated);
		UpdateStats("Total Zombie Citizens Defeated", TotalZombieCitizensDefeated);
		UpdateStats("Total Zombie Bosses Defeated", TotalZombieBossesDefeated);
		UpdateStats("Total Buildings Destroyed", TotalBuildingsDestroyed);
		
		// Second Panel
		UpdateStats("Total Building Score", TotalBuildingScore);
		UpdateStats("Total Wooden Walls Built", TotalWoodenWallsBuilt);
		UpdateStats("Total Stone Walls Built", TotalStoneWallsBuilt);
		UpdateStats("Total Platforms Built:  ", TotalPlatformsBuilt);
		UpdateStats("Total Spike Traps Built", TotalSpikeTrapsBuilt);
		UpdateStats("Total Electric Fences Built", TotalElectricFencesBuilt);
		UpdateStats("Total Torches Built", TotalTorchesBuilt);
		
		// Third Panel
		UpdateStats("Total Gathering Score", TotalGatheringScore);
		UpdateStats("Total Trees Chopped", TotalTreesChopped);
		UpdateStats("Total Stone Mined", TotalStoneMined);
		UpdateStats("Total Copper Collected", TotalCopperMined);
		UpdateStats("Total Apples Collected", TotalApplesCollected);
		UpdateStats("Total Gun Powder Collected", TotalGunPowderCollected);*/
	}

	/*public static void UpdateStats(string gameObjectName, int score)
	{
		GameObject.Find(gameObjectName).GetComponent<Text>().text =
			gameObjectName + ":A " + score;
	}*/
	public void NextTutorial(int nextPart)
	{
		if (nextPart == 0)
		{
			PartOne.SetActive(true);
			PartTwo.SetActive(false);
			PartThree.SetActive(false);
			PartFour.SetActive(false);
			PartFive.SetActive(false);
		} else if (nextPart == 1)
		{
			PartOne.SetActive(false);
			PartTwo.SetActive(true);
			PartThree.SetActive(false);
			PartFour.SetActive(false);
			PartFive.SetActive(false);
		} else if (nextPart == 2)
		{
			PartOne.SetActive(false);
			PartTwo.SetActive(false);
			PartThree.SetActive(true);
			PartFour.SetActive(false);
			PartFive.SetActive(false);
		} else if (nextPart == 3)
		{
			PartOne.SetActive(false);
			PartTwo.SetActive(false);
			PartThree.SetActive(false);
			PartFour.SetActive(true);
			PartFive.SetActive(false);
		} else if (nextPart == 4)
		{
			PartOne.SetActive(false);
			PartTwo.SetActive(false);
			PartThree.SetActive(false);
			PartFour.SetActive(false);
			PartFive.SetActive(true);
		} else if (nextPart == 5)
		{
			SceneManager.LoadScene("SampleScene");
		}
	}

	public void StartTheGame()
	{
		SceneManager.LoadScene("TutorialScene");
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
