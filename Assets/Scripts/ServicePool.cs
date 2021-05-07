using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ServicePool : PoolingService<GameObject>
{
    private GameObject m_scoreBoardEntryObject;
    [SerializeField]
    private LoadAssetBundles m_loadAssetBundle;
    [SerializeField]
    private Transform highScoresHolder;

    public GameObject GetObject()
    {
        return GetItem();
    }

    protected override GameObject CreateItem()
    {
        m_scoreBoardEntryObject = m_loadAssetBundle.GetBundleObject();
        GameObject ScoreCard = Instantiate(m_scoreBoardEntryObject, highScoresHolder);
        return ScoreCard;
    }

}
