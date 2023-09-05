using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private UnitRagdoll ragdollPrefab;
    [SerializeField] private Transform originalRootbone;
    private HealthSystem healthSystem;
    private void Awake() 
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
       UnitRagdoll ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
       ragdoll.Setup(originalRootbone);
    }
}
