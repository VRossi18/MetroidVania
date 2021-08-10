using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int score;
    public Text scoreText;

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = $"x {score}";
        }
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = $"x {score}";

        PlayerPrefs.SetInt("score", score);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
