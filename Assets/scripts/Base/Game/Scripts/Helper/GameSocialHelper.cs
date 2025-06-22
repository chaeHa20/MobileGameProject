using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameSocialHelper : SocialHelper
{
    public static GameSocialHelper getInstance()
    {
        return getInstance<GameSocialHelper>();
    }

    public bool isLoggedIn()
    {
        return isLoggedIn(mediaType);
    }

    public Sprite getProfile()
    {
        var media = getMedia(mediaType);

        if (null == media || null == media.user || null == media.user.profile)
            return null;// GameResourceHelper.getInstance().getSprite((int)eResource.User001);
        else
            return media.user.profile;
    }

    public string getName()
    {
        var media = getMedia(mediaType);
        if (null == media || null == media.user)
            return "Unknown";

        return media.user.name;
    }
}
