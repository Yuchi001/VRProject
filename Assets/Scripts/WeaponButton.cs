using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private float spawnWeaponCooldown;

    private bool _ready = true;
    
    /*private void OnHandHoverBegin()
    {
        if (!_ready) return;

        _ready = false;
        gameManager.RespawnWeapon();
        animator.SetTrigger("down");
        StartCoroutine(Cooldown());
    }*/

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(spawnWeaponCooldown);
        
        animator.SetTrigger("up");

        yield return new WaitForSeconds(0.3f);

        _ready = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (!_ready) return;

        _ready = false;
        gameManager.RespawnWeapon();
        animator.SetTrigger("down");
        StartCoroutine(Cooldown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (!_ready) return;

        _ready = false;
        gameManager.RespawnWeapon();
        animator.SetTrigger("down");
        StartCoroutine(Cooldown());
    }
}