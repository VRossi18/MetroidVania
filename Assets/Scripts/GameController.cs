using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int score;
    public Text scoreText;

    private void Awake()
    {
        instance = this;
        
    }

    void Update()
    {
        
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = $"x {score}";
    }
}
