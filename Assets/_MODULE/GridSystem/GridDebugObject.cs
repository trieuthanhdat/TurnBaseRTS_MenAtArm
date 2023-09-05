using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro txtDebug;
    private GridObject gridObject;
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
    private void Update() 
    {
        UpdateTextGridObject();
    }
    public void UpdateTextGridObject()
    {
        txtDebug.text = gridObject.ToString();
    }
}
