// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class SaveDataStructure
{
    public int playerCredits = 0;
}


public class SaveDataManager : MonoBehaviour
{
    
        public static SaveDataManager instance;
        [SerializeField] private string saveFileName = "data.json";
        public SaveDataStructure saveData;
        private string saveFilePath;
        
        [Serializable]
        public class CreditsChangeEvent: UnityEvent<int>{}
        
        public CreditsChangeEvent onCreditsChanged;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
            
            LoadData();
            
        }

        #region Save / Load

        [ContextMenu("Save Data")]
        public void SaveData()
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game data saved to: " + saveFilePath);
        }

        [ContextMenu("Load Data")]
        public void LoadData()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                SaveDataStructure data = JsonUtility.FromJson<SaveDataStructure>(json);
                Debug.Log("Game data loaded from: " + saveFilePath);
                saveData = data;
                
            }
            else
            {
                Debug.Log("No save file found! Creating new data...");
                saveData = new SaveDataStructure();
            }
        }

        #endregion


        public void UpdatePlayerCredits(int playerCredits)
        {
            saveData.playerCredits += playerCredits;
            onCreditsChanged.Invoke(saveData.playerCredits);
            SaveData();
        }


}


