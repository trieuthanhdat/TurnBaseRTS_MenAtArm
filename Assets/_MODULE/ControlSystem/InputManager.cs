#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    private PlayerInputActions playerInputActions;
    Vector3 dragStartPosition = Vector3.zero;
    Vector3 dragCurrentPosition = Vector3.zero;
    Vector3 newPosition;
    private bool isMoveDragging = false;
    private bool isRotateDragging = false;
    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        playerInputActions.Player.CameraDragMove.performed += ctx => OnCameraDragMovePerform(ctx);
        playerInputActions.Player.CameraDragRotate.performed += ctx => OnCameraDragRotatePerform(ctx);
        playerInputActions.Player.Enable();
    }   
    private void OnDisable()
    {
        playerInputActions.Player.CameraDragMove.performed -= ctx => OnCameraDragMovePerform(ctx);
        playerInputActions.Player.CameraDragRotate.performed -= ctx => OnCameraDragRotatePerform(ctx);
        playerInputActions.Player.Disable();
    }  

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }
    public bool IsLeftMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.LeftClick.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
    public bool IsRightMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.RightClick.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(1);
#endif
    }
    public bool IsMiddleMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.RightClick.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(2);
#endif
    }
    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.y= -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y = 1f;
        }
        return moveDir;
#endif
    }
    public Vector3 GetCameraDragAndMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        if(!isMoveDragging)
        {
            dragCurrentPosition = Vector3.zero;
            dragStartPosition = Vector3.zero;
            newPosition = Vector3.zero;
        }
        Vector3 normalizeDirection = newPosition.normalized;
        return normalizeDirection;
#else
       if(Input.GetMouseButton(0))
        {
            dragStartPosition = MouseControl.GetPlaneDragPosition();
        }
        if(Input.GetMouseButtonDown(0))
        {
            dragCurrentPosition = MouseControl.GetPlaneDragPosition();
        }
        if(Input.GetMouseButtonUp(0))
        {
            dragCurrentPosition = Vector3.zero;
            dragStartPosition = Vector3.zero;
        }
        newPosition = dragStartPosition - dragCurrentPosition;
        Vector2 normalizeDirection = newPosition.normalized;
        return normalizeDirection;
#endif
        
    }
    public float GetCameraDragAndRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        if(isRotateDragging)
        {
            return playerInputActions.Player.CameraDragRotate.ReadValue<Vector2>().x;
        }
        return 0;
#endif
    }
    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float amount = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            amount = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            amount = 1f;
        }
        return amount;
#endif

    }
    public float GetDragCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float amount = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            amount = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            amount = 1f;
        }
        return amount;
#endif

    }
    public void OnCameraDragMovePerform(InputAction.CallbackContext context)
    {
        if(playerInputActions.Player.LeftClick.IsPressed())
        {
            if(IsLeftMouseButtonDownThisFrame())
                dragStartPosition = MouseControl.GetPlaneDragPosition();
            else
            {
                dragCurrentPosition =  MouseControl.GetPlaneDragPosition();           
                newPosition = dragStartPosition - dragCurrentPosition;
            }
            isMoveDragging = true;
        }else
        {
            isMoveDragging = false;
        }

    }

    public void OnCameraDragRotatePerform(InputAction.CallbackContext context)
    {
        if(playerInputActions.Player.MiddleClick.IsPressed())
        {
            isRotateDragging = true;
        }else
        {
            isRotateDragging = false;
        }

    }
    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float amount = 0;
        if(Input.mouseScrollDelta.y > 0)
        {
            amount = -1f;
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            amount = 1f;
        }
        return amount;
#endif
    }
}
