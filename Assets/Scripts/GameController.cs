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
    public GameObject gameoOverPanel;

    private void Awake()
    {
        Time.timeScale = 1;
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
        SceneManager.LoadScene(2);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameoOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
