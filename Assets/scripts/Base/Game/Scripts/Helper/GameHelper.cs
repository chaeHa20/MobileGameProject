using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameHelper
{
    public static eTeam getOppositeTeam(eTeam sourceTeam)
    {
        switch (sourceTeam)
        {
            case eTeam._1: return eTeam._2;
            case eTeam._2: return eTeam._1;
            default: return eTeam._1;
        }
    }

    public static Vector3 toVector3(in Vector2 v, in float y = 0.0f)
    {
        return new Vector3(v.x, y, v.y);
    }

    public static Vector2 toVector2(in Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static void setEmission(MeshRenderer meshRenderer, Color color, float intensity)
    {
        if (null == meshRenderer)
            return;

        var material = meshRenderer.material;
        material.SetColor("_EmissionColor", color * intensity);
    }

    public static System.Numerics.BigInteger calcUpgradeCurrency(BigMoney initUpgradeCurrency, BigMoney increaseUpgradeCurrency, int level)
    {
        return initUpgradeCurrency.value + (level - 1) * increaseUpgradeCurrency.value;
    }

    public static bool isEnableUpgrade(int level, int maxLevel, BigMoney curCurrency, BigMoney initUpgradeCurrency, BigMoney increaseUpgradeCurrency)
    {
        if (level >= maxLevel)
            return false;

        var upgradeCurrency = calcUpgradeCurrency(initUpgradeCurrency, increaseUpgradeCurrency, level);
        return curCurrency.value >= upgradeCurrency;
    }

    public static bool tryGetNeededCurrencyToUpgrade(int curLevel, int maxLevel, int upgradeLevel, BigMoney initUpgradeCurrency,
                                                     BigMoney increaseUpgradeCurrency, out BigMoney needCurrency)
    {
        needCurrency = null;

        if (curLevel >= maxLevel)
            return false;

        System.Numerics.BigInteger needUpgradeCurrency = 0;
        for (int level = curLevel + 1; level <= upgradeLevel; ++level)
        {
            if (level > maxLevel)
                break;

            needUpgradeCurrency += calcUpgradeCurrency(initUpgradeCurrency, increaseUpgradeCurrency, level);
        }

        needCurrency = new BigMoney(needUpgradeCurrency);

        return true;
    }

    /// <param name="callback">success, needCurrency, upgradeLevel</param>
    public static bool tryGetUpgradeLevelWithCurrentCurrency(int curLevel, int maxLevel, BigMoney curCurrency,
                                            BigMoney initUpgradeCurrency, BigMoney increaseUpgradeCurrency,
                                            out BigMoney needCurrency, out int upgradeLevel)
    {
        needCurrency = null;
        upgradeLevel = 0;

        if (curLevel >= maxLevel)
            return false;

        System.Numerics.BigInteger needUpgradeCurrency = 0;
        for (var level = curLevel + 1; level <= maxLevel; ++level)
        {
            var upgradeCurrency = calcUpgradeCurrency(initUpgradeCurrency, increaseUpgradeCurrency, level);
            if (needUpgradeCurrency + upgradeCurrency > curCurrency.value)
            {
                break;
            }

            needUpgradeCurrency += upgradeCurrency;
            upgradeLevel = level;
        }

        if (upgradeLevel > maxLevel)
            upgradeLevel = maxLevel;

        if (0 >= needUpgradeCurrency)
            return false;

        needCurrency = new BigMoney(needUpgradeCurrency);

        return true;
    }

    public static void ignoreApplicationPauseAction(bool isIgnore)
    {
        GameNotificationHelper.getInstance().isNotSendNotification = isIgnore;
        GameUIHelper.instance.isNotShowPauseMsg = isIgnore;
        GameSimulator.isNotShowOfflineGold = isIgnore; // TODO : 2024-08-14 by pms
    }
}
