// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text creditsTitle;

    private void Start()
    {
        creditsTitle.text = "Credits :" + SaveDataManager.instance.saveData.playerCredits;
        
        SaveDataManager.instance.onCreditsChanged.AddListener(UpdateCreditsUI);
        
    }

    void UpdateCreditsUI(int credits)
    {
        creditsTitle.text = "Credits :" + credits;
    }
    

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    
}
