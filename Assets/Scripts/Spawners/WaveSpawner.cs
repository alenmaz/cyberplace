using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Waves[] _waves;
    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;
    [SerializeField] private ObjectPooler _pooler;

    public int EnemiesInWave { get => _waves[_currentWaveIndex].WaveSettings.Length; }

    public int WavesMax { get => _waves.Length; }

    public int GetTotalEnemies()
    {
        int total = 0;
        foreach(var wave in _waves)
            total += wave.WaveSettings.Length;
        return total;
    }

    private void Start()
    {
        _enemiesLeftToSpawn = _waves[0].WaveSettings.Length;
        LaunchWave();
    }

    private IEnumerator SpawnEnemyInWave()
    {
        if(_enemiesLeftToSpawn > 0)
        {
            yield return new WaitForSeconds(_waves[_currentWaveIndex]
                .WaveSettings[_currentEnemyIndex]
                .SpawnDelay);
            var instance = _pooler.GetInstance(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].Type);
            instance.transform.position = _waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].NeededSpawner.transform.position;
            instance.gameObject.SetActive(true);
            _enemiesLeftToSpawn--;
            _currentEnemyIndex++;
            StartCoroutine(SpawnEnemyInWave());
        }
        else
        {
            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _enemiesLeftToSpawn = _waves[_currentWaveIndex].WaveSettings.Length;
                _currentEnemyIndex = 0;
            }
        }
    }
    public void LaunchWave()
    {
        StartCoroutine(SpawnEnemyInWave());
    }
}

[System.Serializable]
public class Waves 
{
    [SerializeField] private WaveSettings[] _waveSettings;
    public WaveSettings[] WaveSettings { get => _waveSettings; }
}

[System.Serializable]
public class WaveSettings
{
    [SerializeField] private ObjectType _type;
    [SerializeField] private GameObject _neededSpawner;
    [SerializeField] private float _spawnDelay;

    public ObjectType Type { get => _type; }
    public GameObject NeededSpawner { get => _neededSpawner; }
    public float SpawnDelay { get => _spawnDelay; }
}
