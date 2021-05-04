using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SetScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI entryNameText;
    [SerializeField]
    private TextMeshProUGUI entryScoreText;

    public void SetBoard(ScoreBoardEntryData scoreBoardEntryData)
    {
        entryNameText.text = scoreBoardEntryData.entryName;
        entryScoreText.text = scoreBoardEntryData.entryScore.ToString();
    }

}
