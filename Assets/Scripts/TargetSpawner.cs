using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> spawnPositions;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int spawnCount;

    private readonly Dictionary<SpawnPoint, Target> _spawnPointsDict = new();
    private bool _canSpawn = false;
    private float _timer = 0;
    
    private void Awake()
    {
        foreach (var pos in spawnPositions)
        {
            _spawnPointsDict.Add(pos, null);
        }
    }

    public void ToggleSpawner(bool canSpawn)
    {
        _canSpawn = canSpawn;

        if (_canSpawn) return;

        foreach (var spawned in _spawnPointsDict)
        {
            if (spawned.Value != null) spawned.Value.Close();
            _spawnPointsDict[spawned.Key] = null;
        }
    }
    
    private void Update()
    {
        if (!_canSpawn) return;

        _timer += Time.deltaTime;
        if (_timer < 0.4f) return;
        _timer = 0;
        
        if (_spawnPointsDict.Values.Count(v => v != null) >= spawnCount) return;

        var availableList = _spawnPointsDict.Where(k => k.Value == null).Select(v => v.Key).ToList();
        var randomKey = availableList[Random.Range(0, availableList.Count)];

        _spawnPointsDict[randomKey] =
            Instantiate(targetPrefab, randomKey.transform.position, Quaternion.Euler(0, 90, 0))
                .GetComponentInChildren<Target>().Setup(randomKey.CanMove);
    }
}