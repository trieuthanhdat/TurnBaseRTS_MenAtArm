using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindingUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DestructableCrate.onAnyCrateDestroy += DestructableCrate_OnAnyCrateDestroy;
        DestructableBarrel.onAnyBarrelDestroy += DestructableBarrel_OnAnyDestructableBarrelDestroy;
    }

    private void DestructableBarrel_OnAnyDestructableBarrelDestroy(object sender, EventArgs e)
    {
        if(sender is DestructableBarrel)
        {
            DestructableBarrel DestructableBarrel = sender as DestructableBarrel;
            PathFinding.instance.SetGridPositionWalkable(DestructableBarrel.GetGridPosition(), true);
        }
    }

    private void DestructableCrate_OnAnyCrateDestroy(object sender, EventArgs eventArgs)
    {
        if(sender is DestructableCrate)
        {
            DestructableCrate crate = sender as DestructableCrate;
            PathFinding.instance.SetGridPositionWalkable(crate.GetGridPosition(), true);
        }

    }
}
