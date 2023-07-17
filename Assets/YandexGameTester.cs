using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class YandexGameTester : MonoBehaviour
{
    public void ShowFullscreenAd()
    {
        YandexGame.FullscreenShow();
    }

    public void ShowRewardedAd()
    {
        YandexGame.RewVideoShow(1);
    }
}
