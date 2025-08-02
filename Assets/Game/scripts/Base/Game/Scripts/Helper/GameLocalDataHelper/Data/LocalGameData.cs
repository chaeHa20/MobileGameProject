using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

[Serializable]
public class LocalGameData : LocalData
{
    [SerializeField] LocalGameOption m_gameOption = new LocalGameOption();    
    [SerializeField] long m_offlineTick = 0;
    [SerializeField] int m_playerThumbnailId = 0;

    public LocalGameOption gameOption { get => m_gameOption; set => m_gameOption = value; }
    public long offlineTick { get => m_offlineTick; set => m_offlineTick = value; }
    public int playerThumbnailId { get => m_playerThumbnailId; set => m_playerThumbnailId = value; }

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);
        m_playerThumbnailId = 1;
    }

    public override void checkValid()
    {
        base.checkValid();
    }
}
