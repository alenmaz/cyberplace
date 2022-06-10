using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Agava.YandexGames.Utility;

public class YandexAdvertisment : MonoBehaviour
{
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        yield return YandexGamesSdk.WaitForInitialization();
    }

    public void ShowAdvertisment()
    {
        InterestialAd.Show();
    }

    public void ShowVideoAdvertisment()
    {
        VideoAd.Show();
    }
}
