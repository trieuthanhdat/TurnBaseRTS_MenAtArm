using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro txtDebug;
    private object gridObject;
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }
    private void Update() 
    {
        UpdateTextGridObject();
    }
    public virtual void UpdateTextGridObject()
    {
        txtDebug.text = gridObject.ToString();
    }
}
