using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BulletProjectile projectile_bulletPrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    private void Awake() 
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.onStartMoveAction += MoveAction_OnStartMoving;
            moveAction.onStopMoveAction += MoveAction_OnStopMoving;
        }
        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += ShootAction_OnShoot;
        }
        if(TryGetComponent<GrenadeAction>(out GrenadeAction grenadeAction))
        {
            grenadeAction.onThrowGrenade += GrenadeAction_OnThrow;
        }
        if(TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.onSwordActionStart += SwordAction_OnSwordActionStart;
            swordAction.onSwordActionEnd += SwordAction_OnSwordActionEnd;
        }
    }
    private void Start() {
        EquipGun();
    }
    private void SwordAction_OnSwordActionStart(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }
    private void SwordAction_OnSwordActionEnd(object sender, EventArgs e)
    {
        EquipGun();
    }
    private void GrenadeAction_OnThrow(object sender, EventArgs e)
    {
        animator.SetTrigger("TossGrenade");
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        BulletProjectile bullet =  Instantiate(projectile_bulletPrefab, shootPointTransform.position, quaternion.identity);
        Vector3 targetUnitShootAtPos = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPos.y = shootPointTransform.position.y;
        bullet.Setup(targetUnitShootAtPos);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", true);
    }
    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }
    private void EquipGun()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
