using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBlock : InteractableBlock
{
    [SerializeField]
    bool hasMoved;
    [SerializeField]
    ConveyorBlock frontConveyor;

    void Update()
    {
        if(ConveyorSystemManager.Instance() == null)
        {
            return;
        }
        if (frontConveyor == null && ConveyorSystemManager.Instance().InteractableBlocks().Length > 0)
        {
            InteractableBlock frontInteractable = GetInteractable(transform.forward);
            if (frontInteractable != null)
            {
                ConveyorBlock conveyor = frontInteractable.GetComponent<ConveyorBlock>();
                if (conveyor != null)
                {
                    frontConveyor = conveyor;
                }
            }
        }
    }
    public void InitConveyor()
    {
        hasMoved = false;
    }

    public void DoMove()
    {
        hasMoved = true;
    }

    public bool HasMoved()
    { 
        return hasMoved; 
    }

    public ConveyorBlock GetFrontConveyor()
    {
        return frontConveyor;
    }
}