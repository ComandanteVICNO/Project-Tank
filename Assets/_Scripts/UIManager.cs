// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TMP_Text scoreTextElement;
    [SerializeField] TMP_Text timerTextElement;
    
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameOverScoreTextElement;
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


    public void UpdateScoreText(int score)
    {
        scoreTextElement.text = "Score: " + score;
    }

    public void UpdateTimer(float timer)
    {
        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerTextElement.text = formattedTime;
    }

    public void EndGame()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreTextElement.text = "Score: " + GameManager.instance.GetGameScore();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
