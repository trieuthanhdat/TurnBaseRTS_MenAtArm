using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    [SerializeField] GrenadeAction unitGrenadeAction;

    public void OnAnimationEventGrenadeThrow()
    {
        if(unitGrenadeAction) unitGrenadeAction.ThrowGrenade();
    }
    public void OnAnimationEventGrenadeSpawn()
    {
        if(unitGrenadeAction) unitGrenadeAction.SpawnGrenade();
    }
}
