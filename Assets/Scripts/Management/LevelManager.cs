using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelSettings
{
    [SerializeField] private GameObject map;
    [SerializeField] private Transform teleportPoint;

    public Transform TeleportPoint { get => teleportPoint; }

    public GameObject SwitchMap(GameObject previousMap)
    {
        if (map != null)
        {
            if (previousMap != null) previousMap.SetActive(false);
            map.SetActive(true);
            return map;
        }
        return previousMap;
    }

    public void TeleportObjectToStart(GameObject obj)
    {
        if (teleportPoint != null) obj.transform.position = teleportPoint.position;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelSettings[] levelSettings;
    [SerializeField] private GameObject blackoutScreen;
    [SerializeField] private GameObject currentMap;

    public GameObject CurrentMap { get => currentMap; }

    public void ApplyLevelSetting(int currentWave, GameObject player)
    {
        if(currentWave < 0 && currentWave >= levelSettings.Length)
        {
            Debug.LogError("Wave index was outside of bounds of settings array. Check if there are settings for all waves in Level Manager.");
            return;
        }
        currentMap = levelSettings[currentWave].SwitchMap(currentMap);
        levelSettings[currentWave].TeleportObjectToStart(player);
    }
}
