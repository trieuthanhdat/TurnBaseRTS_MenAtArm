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
    public void Show(Material material)
    {
        if(mesh) mesh.enabled = true;
        if(mesh) mesh.material = material;
    }
    public void Hide()
    {
        if(mesh) mesh.enabled = false;
    }
    
}
