using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    [SerializeField] float swordShakeIntensityModifier = 1f;
    [SerializeField] float grenadeShakeIntensityModifier = 5f;
    [SerializeField] float rifleShakeIntensityModifier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        ShootAction.onAnyShoot += ShootAction_StartSkaking;
        GrenadeProjectile.onAnyGrenadeExplode += GrenadeAction_StartShaking;
        SwordAction.onAnySwordHit += SwordAction_StartShaking;
    }
    private void GrenadeAction_StartShaking(object sender, EventArgs e)
    {
        ScreenShake.instance.ShakeScreen(grenadeShakeIntensityModifier);
    }
    private void SwordAction_StartShaking(object sender, EventArgs e)
    {
        ScreenShake.instance.ShakeScreen(swordShakeIntensityModifier);
    }
    private void ShootAction_StartSkaking(object sender, EventArgs e)
    {
        ScreenShake.instance.ShakeScreen(rifleShakeIntensityModifier);
    }

    
}
