// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int gameScore;
    [SerializeField] private float gameTimer;
    [SerializeField] private int pointsPerSecond;
     
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
        
        int finalGameScore = gameScore + (finalGameTimeInSeconds * pointsPerSecond);
        
        return finalGameScore;
    }
    
}
