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
    [SerializeField] TMP_Text healthTextElement;
    [SerializeField] TMP_Text creditsTextElement;
    
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameOverScoreTextElement;
    [SerializeField] TMP_Text gameOverCreditsTextElement;
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


    private void Start()
    {
        creditsTextElement.text = "Credits: "+ SaveDataManager.instance.saveData.playerCredits;
        gameOverCreditsTextElement.text = "Credits: "+ SaveDataManager.instance.saveData.playerCredits;
        
        SaveDataManager.instance.onCreditsChanged.AddListener(UpdateCreditsText);
        
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

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthTextElement.text = "Health: " + currentHealth + " / " + maxHealth;
    }

    public void ActivateEndGameUI(int finalScore)
    {
        gameOverPanel.SetActive(true);
        gameOverScoreTextElement.text = "Score: " + finalScore;
    }
    
    public void ReturnToMenu(int index)
    {
        SceneManager.LoadScene(index);
    }

    void UpdateCreditsText(int currentCredits)
    {
        creditsTextElement.text = "Credits: "+ currentCredits;
        gameOverCreditsTextElement.text = "Credits: "+ currentCredits;
    }

    
}
