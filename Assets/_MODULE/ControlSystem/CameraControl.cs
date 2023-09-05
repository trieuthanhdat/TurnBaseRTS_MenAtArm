using UnityEngine;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] float cameraMoveSpeed = 5f;
    [SerializeField] float cameraRotateSpeed = 5f;
    [SerializeField] float cameraZoomAmount = 1f;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    // Update is called once per frame
    private void Start() 
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    void Update()
    {
        HandleCameraMovement();
        HandleCameraRotate();
        HandleCameraZoom();
    }

    private void HandleCameraZoom()
    {
        if(cinemachineVirtualCamera && cinemachineTransposer) 
        {
            
            if(Input.mouseScrollDelta.y > 0)
            {
                targetFollowOffset.y -= cameraZoomAmount;
            }
            if(Input.mouseScrollDelta.y < 0)
            {
                targetFollowOffset.y += cameraZoomAmount;
            }
            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp( cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * cameraZoomAmount);
        }
    }

    private void HandleCameraMovement()
    {
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = 1f;
        }
        transform.position += moveDir * cameraMoveSpeed * Time.deltaTime;
    }
    private void HandleCameraRotate()
    {
        Vector3 rotateDir = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            rotateDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateDir.y = 1f;
        }
       
        transform.eulerAngles += rotateDir * cameraRotateSpeed * Time.deltaTime;
    }
}
