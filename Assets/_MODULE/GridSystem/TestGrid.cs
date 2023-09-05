using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    [SerializeField] Transform gridDebugPrefab;

    private GridSystem gridSystem;

    private void Start() 
    {
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugPrefab);
    }
    private void Update() 
    {
        
    }
}
