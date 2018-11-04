using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour {
	public GameObject Score1;
	public GameObject Score2;
	public GameObject Score3;
	public GameObject Score4;
	public GameObject Score5;
	// Used for comparing with existing, new score.
	public static List<long> ScoreboardList = new List<long>();

	// Use this for initialization
	void Start ()
	{
		string path = "Assets/Resources/player_scores.txt";
		StreamReader reader = new StreamReader(path);

		using (reader)
		{
			int lineNumber = 0;

			string line = reader.ReadLine();

			while (line != null)
			{
				//Debug.Log(line);
				if (lineNumber == 0)
				{
					Score1.GetComponentInChildren<Text>().text = line;
				} else if (lineNumber == 1)
				{
					Score2.GetComponentInChildren<Text>().text = line;
				} else if (lineNumber == 2)
				{
					Score3.GetComponentInChildren<Text>().text = line;
				} else if (lineNumber == 3)
				{
					Score4.GetComponentInChildren<Text>().text = line;
				} else if (lineNumber == 4)
				{
					Score5.GetComponentInChildren<Text>().text = line;
				}
				lineNumber++;
				line = reader.ReadLine();
			}
		}
	}
}
