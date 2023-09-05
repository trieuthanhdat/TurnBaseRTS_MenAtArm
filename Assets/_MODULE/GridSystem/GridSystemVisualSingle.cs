using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]private MeshRenderer mesh;
    
    public MeshRenderer GetMeshRenderer()
    {
        return mesh;
    }
    public void Show()
    {
        if(mesh) mesh.enabled = true;
    }
    public void Hide()
    {
        if(mesh) mesh.enabled = false;
    }
    
}
