using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    private int score;

    private Sprite[] number;
    private Image[] score_digit;

	// Use this for initialization
	void Start () {

        number = Resources.LoadAll<Sprite>("Textures/Score/black_score");

        int points = PlayerPrefs.GetInt("score");
        score = (points != 0) ? points : 0;

        int childs = transform.childCount;
        score_digit = new Image[childs];
        
        for (int i = 0; i < childs; i++)
            score_digit[i] = transform.GetChild(i).GetComponent<Image>();

	}

    public void SumPoints(int points)
    {
        score += points;
        string str_score = score.ToString();
        int lenght = str_score.Length;

        for (int i = 0; i < str_score.Length; i++)
        {
            int digit = int.Parse(str_score.Substring((lenght - i) - 1, 1));
            score_digit[i].overrideSprite = number[digit];
        }
        
    }

    public int getScore()
    {
        return score;
    }
}
