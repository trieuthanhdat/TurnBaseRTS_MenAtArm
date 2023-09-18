using UnityEngine;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] float cameraMoveSpeed = 5f;
    [SerializeField] float cameraDragMoveSpeed = 15f;
    [SerializeField] private float dragAcceleration;

    [SerializeField] float cameraRotateSpeed = 5f;
    [SerializeField] float cameraDragRotateSpeed =1f;
    [SerializeField] float cameraZoomAmount = 1f;
    [SerializeField] bool UseNewInputSystem = true;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    private static bool isDragging = false;
    private float damping = 15f;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;
    private Vector3 targetPosition;

    public static void SetIsDragging(bool value)
    {
        isDragging = value;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        return forward;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0f;
        return right;
    }
    // Update is called once per frame
    private void Start() 
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;

    }
    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = this.transform.position;
    }
    void LateUpdate()
    {
        UpdateVelocity();
        // if(!UseNewInputSystem)
        {
            HandleCameraMovement();
            HandleCameraRotate();
            HandleCameraZoom();
        }
        // else
        {
            HandleCameraDragMove();
            HandleCameraDragRotate();
        }
        
    }

    private void HandleCameraZoom()
    {
        if(cinemachineVirtualCamera && cinemachineTransposer) 
        {
            targetFollowOffset.y += InputManager.instance.GetCameraZoomAmount();
            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp( cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * cameraZoomAmount);
        }
    }

    private void HandleCameraMovement()
    {
        Vector2 inputMoveDir = InputManager.instance.GetCameraMoveVector();
        Vector3 convertMoveDir = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += convertMoveDir * cameraMoveSpeed * Time.deltaTime;
    }
    private void HandleCameraRotate()
    {
        Vector3 rotateDir = Vector3.zero;
        rotateDir.y = InputManager.instance.GetCameraRotateAmount();
       
        transform.eulerAngles += rotateDir * cameraRotateSpeed * Time.deltaTime;
    }
    private void HandleCameraDragRotate()
    {
        Vector3 rotateDir = Vector3.zero;
        rotateDir.y = InputManager.instance.GetCameraDragAndRotateAmount();
        transform.rotation = Quaternion.Euler(0f, rotateDir.y * cameraDragRotateSpeed + transform.rotation.eulerAngles.y, 0f);
        transform.eulerAngles += rotateDir * cameraDragRotateSpeed * Time.deltaTime;
    }
    private void HandleCameraDragMove()
    {
        Vector3 inputMoveDir = InputManager.instance.GetCameraDragAndMoveVector();
        targetPosition  = GetCameraForward() * inputMoveDir.z + GetCameraRight() * inputMoveDir.x;
        float speed = cameraDragMoveSpeed;
        if(targetPosition.sqrMagnitude > .1f)
        {
            speed = Mathf.Lerp(speed, cameraDragMoveSpeed, Time.deltaTime * dragAcceleration);
            transform.position += targetPosition * Time.deltaTime * speed ;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += Time.deltaTime * horizontalVelocity ;
        }
        targetPosition = Vector3.zero;
       
    }

}
