using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] LayerMask mouseLayerMask;
    private static MouseControl instance ;
    private void Awake() 
    {
        instance = this;
    }
    void Update()
    {
        transform.position = MouseControl.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mouseLayerMask);
        return hitInfo.point;
    }
}
