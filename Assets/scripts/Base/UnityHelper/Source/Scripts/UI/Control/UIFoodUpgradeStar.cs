using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIFoodUpgradeStar : UIComponent
{
    [SerializeField]
    public class UpgradeStar
    {
        public GameObject gameObject;
        public Image m_baseIcon;
        public Image m_clearIcon;

        public void initStar()
        {
            gameObject.SetActive(true);
            m_baseIcon.gameObject.SetActive(true);
            m_baseIcon.gameObject.SetActive(false);
        }
        public void clearStar()
        {
            gameObject.SetActive(true);
            m_baseIcon.gameObject.SetActive(false);
            m_baseIcon.gameObject.SetActive(true);
        }
    }

    [SerializeField] List<UpgradeStar> m_firstPhase = new List<UpgradeStar>();
    [SerializeField] List<UpgradeStar> m_secondPhase = new List<UpgradeStar>();

    public void upgradeStar(int rewardCount)
    {
        foreach (var star in m_firstPhase)
            star.gameObject.SetActive(false);
        foreach (var star in m_secondPhase)
            star.gameObject.SetActive(false);

        if (rewardCount <= m_firstPhase.Count)
        {
            foreach (var star in m_firstPhase)
                star.initStar();

            for (int index = 0; index < rewardCount; index++)
            {
                m_firstPhase[index].clearStar();
            }
        }
        else
        {
            foreach (var star in m_secondPhase)
                star.initStar();

            var secondPhaseStarCount = rewardCount - m_firstPhase.Count;
            for (int index = 0; index < secondPhaseStarCount; index++)
            {
                m_secondPhase[index].clearStar();
            }
        }
    }
}
