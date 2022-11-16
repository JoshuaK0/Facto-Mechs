using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovablePart : InteractableBlock
{
    float visualSmoothing;
    [SerializeField] Transform blockVisual;

    [SerializeField] MovableRoot root;

    [SerializeField]
    ConveyorBlock conveyorBlock;

    public BulletMod bulletMod;
    public void DoTick()
    {
        InteractableBlock downBlock = GetInteractable(-Vector3.up);
        if(downBlock == null)
        {
            DoMove(-Vector3.up);
        }
        else
        {
            ConveyorBlock conveyor = downBlock.GetComponent<ConveyorBlock>();
            if (conveyor != null)
            {
                conveyorBlock = conveyor;
                DoMove(ConveyorSystemManager.Instance().blockParent.InverseTransformDirection(conveyor.transform.forward));
            }
        }
    }

    public void DoMove(Vector3 moveDir)
    {
        root.DoMove(moveDir);
    }

    public MovableRoot GetRoot() 
    { 
        return root; 
    }
    public void SetRoot(MovableRoot newRoot)
    {
        root = newRoot;
    }
    void Start()
    {
        visualSmoothing = ConveyorSystemManager.Instance().GetBlockSmoothSpeed();
        blockVisual.transform.SetParent(transform.parent.parent);
    }
    void Update()
    {
        blockVisual.transform.position = Vector3.Slerp(blockVisual.transform.position, transform.position, visualSmoothing * Time.deltaTime);
    }
    public bool CanMove(Vector3 moveDir)
    {
        if(this == null)
        {
            Debug.Log("Error Part");
            return false;
        }
        if (conveyorBlock != null)
        {
            if (conveyorBlock.GetFrontConveyor() != null)
            {
                if (conveyorBlock.GetFrontConveyor().HasMoved())
                {
                    return false;
                }
            }
        }

        foreach (Vector3 space in ConveyorSystemManager.Instance().occupiedSpaces)
        {
            Vector3Int comparisonVector = new Vector3Int
                (
                    Mathf.RoundToInt(transform.localPosition.x + root.transform.localPosition.x),
                    Mathf.RoundToInt(transform.localPosition.y + root.transform.localPosition.y),
                    Mathf.RoundToInt(transform.localPosition.z + root.transform.localPosition.z)
                );
            Vector3Int moveDirInt = new Vector3Int
                (
                    Mathf.RoundToInt(moveDir.x),
                    Mathf.RoundToInt(moveDir.y),
                    Mathf.RoundToInt(moveDir.z)
                );
            if (ConveyorSystemManager.Instance().occupiedSpaces.Contains(comparisonVector + moveDirInt))
            {
                InteractableBlock frontInteractable = GetInteractable(transform.TransformDirection(moveDir));
                if (frontInteractable != null)
                {
                    MovablePart frontMovable = frontInteractable.GetComponent<MovablePart>();
                    {
                        if(frontMovable != null)
                        {
                            if (GetRoot().GetBlocks().Contains(frontMovable))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            return true;
        }
        return true;
    }
    
    public void MoveSuccess()
    {
        if(conveyorBlock != null)
        {
            conveyorBlock.DoMove();
            conveyorBlock = null;
        }
    }

    public void DestroyPart()
    {
        ConveyorSystemManager.Instance().occupiedSpaces.Remove(RoundVector(transform.localPosition + root.transform.localPosition));
        Destroy(blockVisual.gameObject);
        Destroy(gameObject);
    }
}
