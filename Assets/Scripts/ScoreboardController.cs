using System;
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
    public static long CurrentScore;

    //private TextAsset _score;

    public TextAsset ScoresAsset;
    private string _scoresText;

    // Use this for initialization
    void Start ()
    {
        ScoreboardList.Clear();
		
        string path = Application.dataPath + "/StreamingAssets/player_scores.txt";
        StreamReader reader = new StreamReader(path);

        using (reader)
        {
            int lineNumber = 0;

            string line = reader.ReadLine();

            while (line != null)
            {
                ScoreboardList.Add(long.Parse(line));
                lineNumber++;
                line = reader.ReadLine();
            }
        }
		
        ScoreboardList.Sort();
        ScoreboardList.Reverse();

        // Updating the player's scoreboard with his new current score
        // and resetting it afterwards.
        for (int i = 0; i < ScoreboardList.Count; i++)
        {
            if (i == 0)
            {
                Score1.GetComponentInChildren<Text>().text = ScoreboardList[0].ToString();
            }
            if (i == 1)
            {
                Score2.GetComponentInChildren<Text>().text = ScoreboardList[1].ToString();
            }
            if (i == 2)
            {
                Score3.GetComponentInChildren<Text>().text = ScoreboardList[2].ToString();
            }
            if (i == 3)
            {
                Score4.GetComponentInChildren<Text>().text = ScoreboardList[3].ToString();
            }
            if (i == 4)
            {
                Score5.GetComponentInChildren<Text>().text = ScoreboardList[4].ToString();
            }
        }
    }
}