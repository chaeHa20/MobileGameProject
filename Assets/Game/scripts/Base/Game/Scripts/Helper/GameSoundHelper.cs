using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class GameSoundHelper : SoundHelper
{
    public static GameSoundHelper getInstance()
    {
        return instance as GameSoundHelper;
    }

    public override void initialize()
    {
        base.initialize();

        GameLocalDataHelper.getInstance().requestGetGameOption((localGameOption) =>
        {
            setBgmVolume(localGameOption.isBgmOn ? 1.0f : 0.0f);
            setSfxVolume(localGameOption.isSfxOn ? 1.0f : 0.0f);
        });
    }

    protected override string getClipPath(int soundId)
    {
        SoundRow soundRow = GameTableHelper.instance.getRow<SoundRow>((int)eTable.Sound, soundId);
        return GameTableHelper.instance.getResourcePath(soundRow.resourceId);
    }

    protected override string getClipName(int soundId)
    {
        SoundRow soundRow = GameTableHelper.instance.getRow<SoundRow>((int)eTable.Sound, soundId);
        return GameTableHelper.instance.getResourceName(soundRow.resourceId);
    }

    protected override float getVolume(int soundId)
    {
        SoundRow soundRow = GameTableHelper.instance.getRow<SoundRow>((int)eTable.Sound, soundId);
        return soundRow.volume;
    }

    protected override bool isLoop(int soundId)
    {
        SoundRow soundRow = GameTableHelper.instance.getRow<SoundRow>((int)eTable.Sound, soundId);
        return soundRow.isLoop;
    }
}
