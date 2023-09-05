using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool Invert = false;
    private Transform cameraTrans;
    private void Awake() 
    {
        cameraTrans = Camera.main.transform;
    }
    private void LateUpdate() 
    {
        if(Invert)
        {
            Vector3 direction = (transform.position - cameraTrans.position).normalized;
            transform.LookAt(transform.position + direction);
        }else
        {
            transform.LookAt(cameraTrans);
        }
    }
}
