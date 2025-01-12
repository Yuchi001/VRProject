using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform weaponSpawnPos;
    [SerializeField] private TargetSpawner targetSpawner;

    private Weapon _weaponInstance;
    private bool _started = false;

    private void StartGame()
    {
        _started = true;
        targetSpawner.ToggleSpawner(true);
        AudioManager.SetTheme(AudioManager.EMusicType.BaseTheme);
    }

    public void RespawnWeapon()
    {
        if (_weaponInstance != null) Destroy(_weaponInstance.gameObject);
        
        if(!_started) StartGame();

        _weaponInstance = Instantiate(weapon, weaponSpawnPos.position, weaponSpawnPos.rotation);
    }
}