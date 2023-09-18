using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool isOpen = false;
    [SerializeField] Animator animator;
    public event EventHandler OnDoorOpened;
    private GridPosition gridPosition;
    private event Action onInteractEnd;
    private float timer;
    private bool isActive = false;
    private void Start() 
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.SetInteractableAtGridPosition(gridPosition, this);
        if(isOpen)
            OpenDoor();
        else
            CloseDoor();
    }
    private void Update() 
    {
        if(!isActive) return;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            onInteractEnd?.Invoke();
            isActive = false;
        }
    }
    public void Interact(Action onInteractEnd)
    {
        isActive = true;
        timer = 0.6f;
        this.onInteractEnd = onInteractEnd;
        isOpen = !isOpen;
        if(isOpen)
            OpenDoor();
        else
            CloseDoor();
    }
    public void OpenDoor()
    {
        animator.SetBool("isOpen", isOpen);
        PathFinding.instance.SetGridPositionWalkable(gridPosition, true);
        OnDoorOpened?.Invoke(this, EventArgs.Empty);
    }
    public void CloseDoor()
    {
        animator.SetBool("isOpen", isOpen);
        PathFinding.instance.SetGridPositionWalkable(gridPosition, false);
    }

    
}
