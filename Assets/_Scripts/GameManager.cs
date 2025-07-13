// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int gameScore;
    [SerializeField] private float gameTimer;
     
     bool isGameOver;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        UIManager.instance.UpdateTimer(gameTimer);
    }


    public void AddScore(int scoreToAdd)
    {
        gameScore += scoreToAdd;
        UIManager.instance.UpdateScoreText(gameScore);
    }

    public void SubtractScore(int scoreToSubtract)
    {
        gameScore -= scoreToSubtract;
        UIManager.instance.UpdateScoreText(gameScore);
    }

    public int GetGameScore()
    {
        int finalGameTimeInSeconds = (int)gameTimer;
        
        int finalGameScore = gameScore + (finalGameTimeInSeconds * 5);
        
        Debug.Log("Game lasted: " + finalGameTimeInSeconds + "  seconds | With each second being 5 points, final time score was: " + (finalGameTimeInSeconds * 5));
        
        return finalGameScore;
    }
    
}
