using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform popupPos;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private int maxAccuracyPoints;
    [SerializeField] private int bonusHeadShootPoints;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxDistance;

    private bool _move;
    private float _speed;
    private Vector2 _lastPos;

    public Target Setup(bool move)
    {
        animator.SetTrigger("enter");
        _move = move;
        _speed = 3;
        _lastPos = parent.position;
        return this;
    }

    public void Close()
    {
        animator.SetTrigger("exit");
        Destroy(gameObject, 0.15f);
    }

    private void Update()
    {
        if (!_move) return;
        
        parent.Translate(Vector3.forward * (_speed * Time.deltaTime));
        if (Vector3.Distance(parent.position, _lastPos) < maxDistance) return;

        _lastPos = parent.position;
        _speed *= -1;
    }

    /// <summary>
    /// Manages target hit logic
    /// </summary>
    /// <returns>Points calculated by accuracy</returns>
    public void Hit(Vector3 hit)
    {
        AudioManager.PlaySound(AudioManager.ESFXType.TargetHIt);
        
        _move = false;
        
        WorldCanvasManager.SpawnHitMarker(hit);
        
        var bodyHitPos = bodyTransform.InverseTransformPoint(hit);
        var bodyLocal = new Vector2(bodyHitPos.x, bodyHitPos.y);
        var distBody = bodyLocal.magnitude;
        
        var headHItPos = headTransform.InverseTransformPoint(hit);
        var headLocal = new Vector2(headHItPos.x, headHItPos.y);
        var distHead = headLocal.magnitude;

        var wasHeadShoot = distBody > distHead;
        
        var points = (_move ? 2 : 1) * (Mathf.CeilToInt((wasHeadShoot ? distHead : distBody) * 100) * maxAccuracyPoints) / 100 + (wasHeadShoot ? bonusHeadShootPoints : 0);
        
        WorldCanvasManager.SpawnText(popupPos.position, points.ToString(), wasHeadShoot);
        
        Close();
    }
}
