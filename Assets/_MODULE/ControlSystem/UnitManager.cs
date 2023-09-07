using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    public List<UnitControl> friends = new List<UnitControl>();
    public List<UnitControl> enemies  = new List<UnitControl>();
    public List<UnitControl> units = new List<UnitControl>();
    [HideInInspector] public bool HasRegisterEvents = false;
    private void Start() 
    {
        UnitControl.OnAnyUnitSpawned += UnitControl_OnAnyUnitSpawned;
        UnitControl.OnAnyUnitDead += UnitControl_OnAnyUnitDead;
        HasRegisterEvents = true;
    }
   
    private void UnitControl_OnAnyUnitDead(object sender, EventArgs e)
    {
        if((sender is UnitControl) == false) return;
       UnitControl unit = (UnitControl)sender;
        units.Remove(unit);
        Debug.Log("UNIT MANAGER: unit destroyed "+ unit.name);
        if(unit.IsEnemy())
        {
            enemies.Remove(unit);
        }
        else
        {
            friends.Remove(unit);
        }
    }

    private void UnitControl_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        if((sender is UnitControl) == false) return;
        UnitControl unit = (UnitControl)sender;
        units.Add(unit);
        Debug.Log("UNIT MANAGER: unit spawned "+ unit.name);
        if(unit.IsEnemy())
        {
            enemies.Add(unit);
        }
        else
        {
            friends.Add(unit);
        }
    }
    public List<UnitControl> GetUnitList()
    {
        return units;
    }
    public List<UnitControl> GetEnemyUnits()
    {
        return enemies;
    }
    public List<UnitControl> GetFriendUnits()
    {
        return friends;
    }
}
