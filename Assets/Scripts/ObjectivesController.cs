using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour {
	Dictionary<string, string> currentTasks = new Dictionary<string, string>();

	private GameObject _taskTitle;
	private GameObject _taskDescription;
	public GameObject TasksWindow;
	public GameObject CompletedTaskPopUp;

	private int _currentWoodWallsBuilt;
	
	// Use this for initialization
	void Start () {
		// Tasks for the player to accomplish
		currentTasks.Add("Collect Wood", "Collect wood by right clicking on a tree and clicking on the red hammer.");
		currentTasks.Add("Place Wooden Wall", "Place a wooden wall by right clicking a ground tile, selecting the green hammer and clicking on the wooden wall.");
		
		_taskTitle = GameObject.FindGameObjectWithTag("Task Title");
		_taskDescription = GameObject.FindGameObjectWithTag("Task Description");
		
		// Start the first task for the player to complete.
		_taskTitle.GetComponent<Text>().text = "Collect Wood";
		_taskDescription.GetComponent<Text>().text = currentTasks["Collect Wood"];
		
		CompletedTaskPopUp.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerController.Wood > 200 && currentTasks.ContainsKey("Collect Wood"))
		{
			CompleteTask("Collect Wood");
			_currentWoodWallsBuilt = MenuController.TotalWoodenWallsBuilt;
		}
		// We also check for the current walls built
		// because the player might have built a wooden wall already
		// and we want to check from that point forward so the calculations would not go south.
		if (MenuController.TotalWoodenWallsBuilt > _currentWoodWallsBuilt &&
		    currentTasks.ContainsKey("Place Wooden Wall") &&
		    !currentTasks.ContainsKey("Collect Wood")) // We only want to finish the next quest if the previous one is first done.
		{
			CompleteTask("Place Wooden Wall");
		}
	}

	private void CompleteTask(string taskKey)
	{
		CompletedTaskPopUp.SetActive(true);
		
		switch (taskKey)
		{
				case "Collect Wood":
					currentTasks.Remove(taskKey);

					// Once this task is completed, we change the text of the current
					// task title to the next one.
					_taskTitle.GetComponent<Text>().text = "Place Wooden Wall";
					_taskDescription.GetComponent<Text>().text = currentTasks["Place Wooden Wall"];
					
					Invoke("HideCompletedPopUp", 1f);
					break;
				case "Place Wooden Wall":
					currentTasks.Remove(taskKey);
					
					_taskTitle.GetComponent<Text>().text = "No tasks for now.";
					_taskDescription.GetComponent<Text>().text = "";
					
					Invoke("HideCompletedPopUp", 1f);
					break;
				default:
					Debug.Log("Invalid task name to complete.");
					break;
		}
	}

	private void HideCompletedPopUp()
	{
		CompletedTaskPopUp.SetActive(false);
		TasksWindow.SetActive(false);
		
		Invoke("ShowTasksWindow", 1);
	}

	private void ShowTasksWindow()
	{
		TasksWindow.SetActive(true);
	}
}