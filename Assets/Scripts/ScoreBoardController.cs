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
    private int maxScoreBoardEntries = 100;
    [SerializeField]
    private Button button;
    [SerializeField]
    private LoadAssetBundles m_loadAssetBundle;
    [SerializeField]
    private Transform highScoresHolder;
    private UnityEngine.Object m_scoreBoardEntryObject;
    private ScoreBoardSaveData savedScores;
    private bool isScorePanelActive = false;
    private int EntryIndex = 0;

    private string SavePath => $"{Application.dataPath}/HighScores.json";

    private void Start()
    {
        savedScores =  GetSavedScores();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        isScorePanelActive = isScorePanelActive ? false : true;
        highScoresHolder.gameObject.SetActive(isScorePanelActive);
        if (isScorePanelActive)
        {
            UpdateUI(savedScores);
        }
    }

    public void AddEntry(ScoreBoardEntryData scoreBoardEntryData)
    {
        ScoreBoardSaveData savedScores = GetSavedScores();
        bool scoreAdded = false;
        for(int i = 0;i < savedScores.highScores.Count; i++)
        {
            if (scoreBoardEntryData.entryScore > savedScores.highScores[i].entryScore)
            {
                savedScores.highScores.Insert(i, scoreBoardEntryData);
                scoreAdded = true;
                break;
            }
        }
        if(!scoreAdded && savedScores.highScores.Count < maxScoreBoardEntries)
        {
            savedScores.highScores.Add(scoreBoardEntryData);
        }
        if(savedScores.highScores.Count > maxScoreBoardEntries)
        {
            savedScores.highScores.RemoveRange(maxScoreBoardEntries, savedScores.highScores.Count-maxScoreBoardEntries);
        }
        UpdateUI(savedScores);
        //SaveScores(savedScores);
    }

    private void UpdateUI(ScoreBoardSaveData savedScores)
    {
       foreach (Transform child in highScoresHolder)
        {
            Destroy(child.gameObject);
        }
       int LimitEntries = EntryIndex + 10;

        
        for(int i = EntryIndex; i < LimitEntries; i++)
        {
            if (EntryIndex < LimitEntries && LimitEntries < savedScores.highScores.Count)
            {
                m_scoreBoardEntryObject = m_loadAssetBundle.GetBundleObject();
                GameObject scoreCard = Instantiate(m_scoreBoardEntryObject, highScoresHolder) as GameObject;
                scoreCard.GetComponent<SetScore>().SetBoard(savedScores.highScores[EntryIndex]);
                EntryIndex++;
                Debug.Log(EntryIndex);
            }
            else
            {
                Debug.Log("Total Count " + savedScores.highScores.Count);
            }

        }


        //foreach (ScoreBoardEntryData highscore in savedScores.highScores)

        //{   if(EntryIndex < LimitEntries)
        //    {
        //        m_scoreBoardEntryObject = m_loadAssetBundle.GetBundleObject();
        //        GameObject scoreCard = Instantiate(m_scoreBoardEntryObject, highScoresHolder) as GameObject;
        //        scoreCard.GetComponent<SetScore>().SetBoard(highscore);
        //        EntryIndex++;
        //        Debug.Log(EntryIndex);
        //    }
        //    else
        //    {
        //        break;
        //    }

        //}
    }

    private ScoreBoardSaveData GetSavedScores()
    {
        if (!File.Exists(SavePath)){
            File.Create(SavePath).Dispose();
            return new ScoreBoardSaveData();
        }
        using (StreamReader stream = new StreamReader(SavePath))
        {
            string json = stream.ReadToEnd();
            return JsonUtility.FromJson<ScoreBoardSaveData>(json);
        }
    }

}
