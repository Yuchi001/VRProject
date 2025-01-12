using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 5f;

    private bool _hit = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (_hit) return;
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        _hit = true;
        rb.Sleep();
        meshRenderer.enabled = false;
        Destroy(gameObject, 0.1f);
        if (!collision.gameObject.TryGetComponent(out Target target)) return;
        
        target.Hit(collision.GetContact(0).point);
    }

    private void OnTriggerEnter(Collider other)
    {
        _hit = true;
        rb.Sleep();
        meshRenderer.enabled = false;
        Destroy(gameObject, 0.1f);
        if (!other.gameObject.TryGetComponent(out Target target)) return;
        
        target.Hit(other.ClosestPoint(transform.position));
    }
}