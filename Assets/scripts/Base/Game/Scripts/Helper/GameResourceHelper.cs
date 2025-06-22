using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class GameResourceHelper : ResourceHelper
{
    private Material m_outlineMaterial = null;

    public static GameResourceHelper getInstance()
    {
        return getInstance<GameResourceHelper>();
    }

    public T instantiate<T>(int resourceId) where T : Component
    {
        string resPath = GameTableHelper.instance.getResourcePath(resourceId);
        return instantiate<T>(resPath);
    }

    public Material loadMaterial(int resourceId)
    {
        string resPath = GameTableHelper.instance.getResourcePath(resourceId);
        return loadMaterial(resPath);
    }

    public Sprite loadSprite(int resourceId)
    {
        string resPath = GameTableHelper.instance.getResourcePath(resourceId);
        return loadSprite(resPath);
    }

    public void setSprite(Image image, int spriteId, bool isSetNativeSize)
    {
        image.sprite = getSprite(spriteId);
        if (isSetNativeSize)
            image.SetNativeSize();
    }

    public Sprite getSprite(int spriteId)
    {
        string resPath = GameTableHelper.instance.getResourcePath(spriteId);
        return getSprite(resPath);
    }

    public Color getGradeColor(eGrade grade)
    {
        return GameSettings.instance.common.gradeColor.getColor(grade);
    }

    public Color getUpgradeGradeColor(eGrade grade)
    {
        return GameSettings.instance.common.upgradeGradeColor.getColor(grade);
    }


    public Sprite getSprite(eBuyType buyType)
    {
        switch (buyType)
        {
            case eBuyType.Gem: return getGemSprite();
            default: return null;
        }
    }

    public Texture loadTexture(int resourceId)
    {
        string resPath = GameTableHelper.instance.getResourcePath(resourceId);
        return loadTexture(resPath);
    }

    public Material createMaterial(Material source, int mainTextureId)
    {
        var mat = new Material(source);
        mat.mainTexture = loadTexture(mainTextureId);
        return mat;
    }

    public Sprite getGoldSprite()
    {
        var goldRow = GameTableHelper.instance.getRow<ItemRow>((int)eTable.Item, Define.GOLD_ITEM_ID);
        return getSprite(goldRow.spriteId);
    }

    public Sprite getGemSprite()
    {
        var gemRow = GameTableHelper.instance.getRow<ItemRow>((int)eTable.Item, Define.GEM_ITEM_ID);
        return getSprite(gemRow.spriteId);
    }

    public Sprite getSprite(eCurrency currencyType)
    {
        int itemId = 0;
        switch (currencyType)
        {
            case eCurrency.Gold: itemId = Define.GOLD_ITEM_ID; break;
            case eCurrency.Gem: itemId = Define.GEM_ITEM_ID; break;
            default: return null;
        }

        var gemRow = GameTableHelper.instance.getRow<ItemRow>((int)eTable.Item, itemId);
        return getSprite(gemRow.spriteId);
    }
}
