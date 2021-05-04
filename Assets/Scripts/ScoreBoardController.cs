using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

[Serializable]
public struct ScoreBoardEntryData
{
    public string entryName;
    public int entryScore;
}

[Serializable]
public class ScoreBoardSaveData
{
    public List<ScoreBoardEntryData> highScores = new List<ScoreBoardEntryData>();
}


public class ScoreBoardController : MonoBehaviour
{
    [SerializeField]
    private int maxScoreBoardEntries = 5;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Transform highScoresHolder;
    private UnityEngine.Object m_scoreBoardEntryObject;
    private LoadAssetBundles m_loadAssetBundle;
    private string SavePath;
    private ScoreBoardEntryData data;
    

    private void Start()
    {
        ScoreBoardSaveData savedScores = GetSavedScores();
        m_loadAssetBundle = gameObject.GetComponent<LoadAssetBundles>();
        SavePath = Path.Combine(Application.persistentDataPath, "HighScores.Json");
        data.entryName = "N/A";
        data.entryScore = 0;
        if (savedScores == null)
        {
            savedScores = new ScoreBoardSaveData();
        }
        UpdateUI(savedScores);
        SaveScores(savedScores);
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        highScoresHolder.gameObject.SetActive(true);
    }

    public void AddEntry(ScoreBoardEntryData scoreBoardEntryData)
    {
        ScoreBoardSaveData savedScores = GetSavedScores();
        bool scoreAdded = false;
        for (int i = 0; i < savedScores.highScores.Count; i++)
        {
            if (scoreBoardEntryData.entryScore > savedScores.highScores[i].entryScore)
            {
                savedScores.highScores.Insert(i, scoreBoardEntryData);
                scoreAdded = true;
                break;
            }
        }
        if (!scoreAdded && savedScores.highScores.Count < maxScoreBoardEntries)
        {
            savedScores.highScores.Add(scoreBoardEntryData);
        }
        if (savedScores.highScores.Count > maxScoreBoardEntries)
        {
            savedScores.highScores.RemoveRange(maxScoreBoardEntries, savedScores.highScores.Count - maxScoreBoardEntries);
        }
        UpdateUI(savedScores);
        SaveScores(savedScores);
    }

    private void UpdateUI(ScoreBoardSaveData savedScores)
    {
        foreach (Transform child in highScoresHolder)
        {
            Destroy(child.gameObject);
        }
        foreach (ScoreBoardEntryData highscore in savedScores.highScores)
        {
            m_scoreBoardEntryObject = m_loadAssetBundle.GetBundleObject();

            GameObject scoreCard = Instantiate(m_scoreBoardEntryObject, highScoresHolder) as GameObject;
            scoreCard.GetComponent<SetScore>().SetBoard(highscore);
        }
    }

    private ScoreBoardSaveData GetSavedScores()
    {
        if (!File.Exists(SavePath))
        {
            File.Create(SavePath).Dispose();
            ScoreBoardSaveData newSavedData = new ScoreBoardSaveData();
            return newSavedData;
        }
        using (StreamReader stream = new StreamReader(SavePath))
        {
            string json = stream.ReadToEnd();
            return JsonUtility.FromJson<ScoreBoardSaveData>(json);
        }
    }

    private void SaveScores(ScoreBoardSaveData scoreBoardSaveData)
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            string json = JsonUtility.ToJson(scoreBoardSaveData, true);
            stream.Write(json);
        }
    }
}