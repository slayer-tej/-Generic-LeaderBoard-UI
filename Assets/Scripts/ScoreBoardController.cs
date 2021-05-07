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
    private int _maxEntriesAtaTime;
    [SerializeField]
    private Button _buttonLeaderBoard;
    [SerializeField]
    private Button _buttonPrev;
    [SerializeField]
    private Button _buttonNext;
    [SerializeField]
    private Transform highScoresHolder;

    private int maxScoreBoardEntries;
    private GameObject m_scoreBoardEntryObject;
    private ScoreBoardSaveData savedScores;
    private bool isScorePanelActive = false;
    private ServicePool servicePool;
    private List<SetScore> pooledItems = new List<SetScore>();
    private int _entryCurr;
    private string SavePath => $"{Application.dataPath}/HighScores.json";

    private void Start()
    {
        savedScores = GetSavedScores();
        maxScoreBoardEntries = savedScores.highScores.Count;
        Debug.Log(maxScoreBoardEntries);
        servicePool = gameObject.GetComponent<ServicePool>();
        _buttonLeaderBoard.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        isScorePanelActive = isScorePanelActive ? false : true;
        highScoresHolder.gameObject.SetActive(isScorePanelActive);
        if (isScorePanelActive)
        {
            UpdateScores(0);
        }
        else
        {
            _entryCurr = 0;
            ResetButtons();
        }
    }

    public void UpdateScores(int change)
    {
        RetutnToPool();
        _entryCurr += change;
        if (_entryCurr <= 0)
        {
            _buttonPrev.interactable = false;
        }
        else if(_entryCurr >= maxScoreBoardEntries - _maxEntriesAtaTime)
        {
            _buttonNext.interactable = false;
        }
        else
        {
            ResetButtons();
        }
        UpdateUI(savedScores);
    }

    #region Add Entries Automatically
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
        //SaveScores(savedScores);
    }
    #endregion

    private void UpdateUI(ScoreBoardSaveData savedScores)
    {
        int entriesToDisplay = _entryCurr + 10;
        for (int i = _entryCurr; i < entriesToDisplay; i++)
        {
                GameObject scoreCard = CreateGameObject();
                SetScore setScore = scoreCard.GetComponent<SetScore>();
                setScore.Enable();
                setScore.SetBoard(savedScores.highScores[i]);
                pooledItems.Add(setScore);
        }
        Debug.Log(_entryCurr);
    }

    private void RetutnToPool()
    {
        foreach (SetScore Item in pooledItems)
        {
            servicePool.ReturnItem(Item.gameObject);
            Item.Disable();
        }
    }

    private void ResetButtons()
    {
        _buttonNext.interactable = true;
        _buttonPrev.interactable = true;
    }

    private GameObject CreateGameObject()
    {
        m_scoreBoardEntryObject = servicePool.GetObject();
        return m_scoreBoardEntryObject;
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
            return JsonUtility.FromJson<ScoreBoardSaveData>(json);
        }
    }

}
