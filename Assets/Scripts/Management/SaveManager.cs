using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private int killsRecord = 0;
    [SerializeField] private int gamesPlayed = 0;

    public int KillsRecord { get => killsRecord; private set { killsRecord = value; } }
    public int GamesPlayed { get => gamesPlayed; private set { gamesPlayed = value; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        KillsRecord = PlayerPrefs.GetInt("maxKills");
        GamesPlayed = PlayerPrefs.GetInt("gamesPlayed");
    }

    public void UpdateKillsRecord(int kills)
    {
        if (kills >= KillsRecord)
        {
            killsRecord = kills;
            PlayerPrefs.SetInt("maxKills", KillsRecord);
        }
    }

    public void UpdateGamesRecord()
    {
        gamesPlayed++;
        PlayerPrefs.SetInt("gamesPlayed", GamesPlayed);
    }
}
