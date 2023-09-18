using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]private MeshRenderer mesh;
    [SerializeField]private GameObject goSelected;
    private void Start() {
        HideSelected();
    }
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
    public void ShowSelected()
    {
        goSelected.SetActive(true);
    }
    public void HideSelected()
    {
        goSelected.SetActive(false);
    }
    
}
