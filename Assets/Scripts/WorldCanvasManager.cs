using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject hitMarkerPrefab;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float scaleFactor = 0.05f;
    [SerializeField] private float textScaleFactor = 0.05f;
    [SerializeField] private float textSpawnRandomRange;

    #region Singleton

    private static WorldCanvasManager Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    #endregion
    
    public static void SpawnHitMarker(Vector3 hitPosition)
    {
        var hitMarker = Instantiate(Instance.hitMarkerPrefab, hitPosition, Quaternion.identity);

        hitMarker.transform.LookAt(Instance.player);

        hitMarker.transform.Rotate(0, 180, 0);

        var distanceToPlayer = Vector3.Distance(hitPosition, Instance.player.position);
        hitMarker.transform.localScale = Vector3.one * distanceToPlayer * Instance.scaleFactor;

        Destroy(hitMarker, 0.1f);
    }
    
    public static void SpawnText(Vector3 spawnPosition, string text, bool crit)
    {
        var rand = Instance.textSpawnRandomRange;
        spawnPosition.x += Random.Range(-rand, rand);
        spawnPosition.y += Random.Range(-rand, rand);
        var textPrefab = Instantiate(Instance.textPrefab, spawnPosition, Quaternion.identity);
        var textScript = textPrefab.GetComponentInChildren<TextMeshProUGUI>();
        textScript.text = text;
        textScript.color = crit ? Color.red : Color.white;
        textScript.fontStyle = crit ? FontStyles.Italic : FontStyles.Normal;
        
        textPrefab.transform.LookAt(Instance.player);

        textPrefab.transform.Rotate(0, 180, 0);

        var distanceToPlayer = Vector3.Distance(spawnPosition, Instance.player.position);
        textPrefab.transform.localScale = Vector3.one * distanceToPlayer * Instance.textScaleFactor;

        Destroy(textPrefab, 0.4f);
    }
}