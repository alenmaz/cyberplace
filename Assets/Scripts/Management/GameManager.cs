using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game status")]
    public int BreakTime; //перерыв между волнами
    [SerializeField] private int KillsDone; //убийств в текущую волну
    [SerializeField] private int TotalKills; //общий счетчик убийств
    [SerializeField] private int KillsNeeded; //нужно убийств, чтобы закончить волну, берется из WaveSpawner
    [SerializeField] private int WavesDone; //сколько волн пройдено
    [SerializeField] private int WavesNeeded; //нужно волн, чтобы закончить забег, берется из WaveSpawner

    public int CurrentWave { get => WavesDone; }
    public bool isPlayerDead = false;
    public bool isPaused = false;
    public bool isQuitting = false;

    [Header("Dependencies")]
    [SerializeField] private AbilityManager AbilityManager;
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private YandexAdvertisment AdManager;
    [SerializeField] private SaveManager SaveManager;
    [SerializeField] private WaveSpawner EnemySpawner;
    [SerializeField] private GameObject Player;

    [Header("UI dependencies")]
    [SerializeField] private Bar[] WavesBars;
    [SerializeField] private TextMeshProUGUI[] KillsCounters;
    [SerializeField] private TextMeshProUGUI[] KillsRecords;

    [SerializeField] private GameObject AbilityPanel;
    [SerializeField] private GameObject GameEndPanel;
    [SerializeField] private GameObject GameWinPanel;
    [SerializeField] private GameObject GamePausePanel;

    [SerializeField] private TextMeshProUGUI MineCounter;
    [SerializeField] private TextMeshProUGUI BulletsCounter;
    [SerializeField] private Bar ReloadBar;

    private bool isEndOfWave;
    private bool isEndOfGame;

    [Header("Enemy drop bonuses")]
    [SerializeField] private GameObject[] Items;

    void Start()
    {
        KillsNeeded = EnemySpawner.EnemiesInWave;
        WavesNeeded = EnemySpawner.WavesMax;
        SaveManager = FindObjectOfType<SaveManager>();
        UpdateKillsRecords();

        UpdateWavesBars();
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<Player>().SetMineNumber(0);
        LevelManager.ApplyLevelSetting(WavesDone, Player);
    }

    void Update()
    {
        if (KillsNeeded <= KillsDone && !isEndOfWave)
        {
            isEndOfWave = true;
            WavesDone++;
            UpdateWavesBars();
            if (WavesNeeded <= WavesDone && !isEndOfGame)
            {
                isEndOfGame = true;
                KillsDone = 0;
                GameWinPanel.SetActive(true);
                if(SaveManager != null) SaveManager.UpdateGamesRecord();
                AdManager.ShowVideoAdvertisment();
            }
            else
            {
                AbilityManager.FillButtons(AbilityPanel.transform.Find("AbilityButtons").GetComponentsInChildren<Button>(), WavesDone - 1);
                StartCoroutine(NexWave());
            }
        }

        if (isPlayerDead) GameEndPanel.SetActive(true);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GamePausePanel.activeInHierarchy) GamePausePanel.SetActive(false);
            else GamePausePanel.SetActive(true);
        }
    }

    public void UpdateWavesBars()
    {
        foreach (var wavesBar in WavesBars)
            if(wavesBar != null)
            {
                wavesBar.SetMaxValue(WavesNeeded);
                wavesBar.SetValue(WavesDone);
            }
    }

    public void UpdateMinesCounter(int current)
    {
        MineCounter.text = $"{current}";
    }

    public void UpdateBulletsCounter(int current, int max)
    {
        BulletsCounter.text = $"{current}/{max}";
    }

    public void UpdateReloadSlider(bool isActive, float current, float max)
    {
        ReloadBar.gameObject.SetActive(isActive);
        ReloadBar.SetMaxValue(max);
        ReloadBar.SetValue(current);
    }

    public void UpdateKillsRecords()
    {
        foreach(var record in KillsRecords)
            if(record != null && SaveManager != null) record.text = SaveManager.KillsRecord.ToString();
    }

    /// <summary>
    /// Метод для обновления текста KillsCounter (счетчик общих убийств со всех волн)
    /// </summary>
    /// <param name="kills">Число на которое нужно обновить счетчик</param>
    public void UpdateKills(int kills)
    {
        TotalKills += kills;
        KillsDone += kills;
        if (SaveManager != null) SaveManager.UpdateKillsRecord(TotalKills);
        UpdateKillsRecords();
        foreach(var killsCounter in KillsCounters)
            if(killsCounter != null) killsCounter.text = $"Убито: {TotalKills}";
    }

    public int GetKills() => KillsDone;

    public int GetKillsToWinWave() => KillsNeeded;

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(BreakTime / 2);
        AbilityPanel.SetActive(true);
    }

    //запускает следующую волну после некоторого промежутка времени (перерыва)
    IEnumerator NexWave()
    {
        yield return new WaitForSeconds(2);
        AbilityPanel.SetActive(true);
        Player.GetComponent<Player>().SetMineNumber(WavesDone);
        LevelManager.ApplyLevelSetting(WavesDone, Player);
        yield return new WaitForSeconds(BreakTime);
        isEndOfWave = false;
        KillsDone = 0;
        KillsNeeded = EnemySpawner.EnemiesInWave;
        EnemySpawner.LaunchWave();
    }

    /// <summary>
    /// Спавнит предмет по id в точке, если в списке items есть предмет с таким id.
    /// </summary>
    /// <param name="position"></param>
    public void SpawnItemAt(Vector2 position, int itemID)
    {
        if (itemID >= 0 && itemID < Items.Length)
        {
            var item = Instantiate(Items[itemID], position, Quaternion.identity);
            if (LevelManager.CurrentMap != null) item.transform.SetParent(LevelManager.CurrentMap.transform);
        }
        else Debug.Log($"Couldn't spawn item with id {itemID}");
    }

    public void SpawnItemAt(Vector2 position, GameObject prefab)
    {
        if (prefab != null)
        {
            var item = Instantiate(prefab, position, Quaternion.identity);
            if (LevelManager.CurrentMap != null) item.transform.SetParent(LevelManager.CurrentMap.transform);
        }
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void LoadLevel(int index)
    {
        isQuitting = true;
        SceneManager.LoadScene(index);
    }

    public void RevivePlayer() => Player.SetActive(true);
}
