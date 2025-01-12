using System;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Interactable), typeof(Throwable), typeof(VelocityEstimator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean fireAction;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private ParticleSystem shootParticles;
    
    private Interactable _interactable;
    private Animator _animator;
    
    private float _shootTimer = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _interactable = GetComponent<Interactable>();
        _interactable.useHandObjectAttachmentPoint = false;
        _interactable.handFollowTransform = false;

        var throwable = GetComponent<Throwable>();
        throwable.attachmentFlags = Hand.AttachmentFlags.VelocityMovement | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach;
    }

    private void Update()
    {
        if (_interactable.attachedToHand == null) return;
        
        var source = _interactable.attachedToHand.handType;
        if (!fireAction[source].stateDown) return;

        _shootTimer = 0f;
        _animator.SetTrigger("shoot");
        AudioManager.PlaySound(AudioManager.ESFXType.PistolShoot);
        shootParticles.Play();
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    }
    
    private void OnHandHoverBegin(Hand hand)
    {
        
    }

    private void OnHandHoverEnd(Hand hand)
    {
        
    }
    
    private void OnHandHoverUpdate(Hand hand)
    {
        var grabType = hand.GetGrabStarting();
        var isGrabEnding = hand.IsGrabEnding(gameObject);

        if (_interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(_interactable);
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(_interactable);
        }
    }
}