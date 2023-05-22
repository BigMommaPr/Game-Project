using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSt2 : Spawner
{
    public bool psiIsSpawned; 
    
    protected override void SpawnPsi()
    {
        if (psiIsSpawned) return;
        currentCapacity--;
        var transform1 = transform;
        var position = transform1.position;
        var obj = Instantiate(psi, position, transform1.rotation);
        var psiObj = obj.GetComponent<StaticPsi>();
        psiObj.SetTarget(position + transform1.up * 3);
        psiObj.direction = transform.up;
        psiObj.spawner = this;
        psiIsSpawned = true;
    }
}