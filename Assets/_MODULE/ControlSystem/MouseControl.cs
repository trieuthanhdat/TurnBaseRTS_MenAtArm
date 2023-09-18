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
        Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mouseLayerMask);
        return hitInfo.point;
    }
    public static Vector3 GetPlaneDragPosition()
    {
        Plane  plane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.GetMouseScreenPosition());
        float entry;
        if(plane.Raycast(ray, out entry))
        {
            return ray.GetPoint(entry);
        }

        return Vector3.zero;
    }
}
