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
}
