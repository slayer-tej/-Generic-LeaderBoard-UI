using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Asset : MonoBehaviour
{
    [SerializeField]
    private LoadAssetBundles m_loadAssetBundle;
    [SerializeField]
    private Transform scoreHolder;
    [SerializeField]
    private Button button;
    private string SavePath => $"{Application.dataPath}/HighScores.json";
    private ScoreBoardSaveData savedScores;


    // Start is called before the first frame update
    void Start()
    {
        savedScores = GetSavedScores();
        button.onClick.AddListener(OnClick);
        
    }

    private void OnClick()
    {
        scoreHolder.gameObject.SetActive(true);
        UpdateUI(savedScores);
    }

    private void UpdateUI(ScoreBoardSaveData savedScores)
    {
    }

    private ScoreBoardSaveData GetSavedScores()
    {
        if (!File.Exists(SavePath))
        {
            File.Create(SavePath).Dispose();
            return new ScoreBoardSaveData();
        }
        using (StreamReader stream = new StreamReader(SavePath))
        {
            string json = stream.ReadToEnd();
            Debug.Log("Successfully LOaded Data from JSon");
            return JsonUtility.FromJson<ScoreBoardSaveData>(json);
        }
    }
}
