using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable 
{
    public abstract void Destroy();
    public abstract void ApplyExplosionObjectParts(Transform root, float explostionForce, Vector3 explosionPos, float explosionRange);
}
