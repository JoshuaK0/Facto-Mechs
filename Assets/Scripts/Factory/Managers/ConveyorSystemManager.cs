using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConveyorSystemManager : TickBlock
{
    public Transform blockParent;

    [SerializeField] float smoothSpeed = 8;

    static ConveyorSystemManager instance;

    public List<Vector3Int> occupiedSpaces = new List<Vector3Int>();

    InteractableBlock[] interactableBlocks = new InteractableBlock[0];
    ConveyorBlock[] conveyorBlocks = new ConveyorBlock[0];

    MovablePart[] movableParts;
    MovableRoot[] movableRoots;
    TickBlock[] tickBlocks;

    [SerializeField] int movesPerFrame;

    [SerializeField]
    bool detailedMove;

    public static ConveyorSystemManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public override void DoInitTick()
    {
        interactableBlocks = FindObjectsOfType<InteractableBlock>();
        conveyorBlocks = FindObjectsOfType<ConveyorBlock>();

        foreach (InteractableBlock block in interactableBlocks)
        {
            if(block.GetComponent<ConveyorBlock>() == null)
            {
                block.AddCoords(Vector3.zero);
            }
        }
    }

    IEnumerator MoveBlocks()
    {
        int routineCounter = 0;

        interactableBlocks = FindObjectsOfType<InteractableBlock>(); 
        movableParts = FindObjectsOfType<MovablePart>();
        movableRoots = FindObjectsOfType<MovableRoot>();
        ConveyorBlock[] conveyors = FindObjectsOfType<ConveyorBlock>();

        foreach (MovableRoot root in movableRoots)
        {
            root.InitRoot();
        }

        foreach (ConveyorBlock block in conveyorBlocks)
        {
            block.InitConveyor();
        }

        if (detailedMove)
        {
            for (int i = 0; i < movableParts.Length; i++)
            {
                foreach (ConveyorBlock conveyor in conveyors)
                {
                    routineCounter++;
                    if(routineCounter >= movesPerFrame)
                    {
                        routineCounter = 0;
                        yield return null;
                    }
                    conveyor.ConveyorBlockMove();
                }
            }
        }
        else
        {
            foreach (ConveyorBlock conveyor in conveyors)
            {
                routineCounter++;
                if (routineCounter >= movesPerFrame)
                {
                    routineCounter = 0;
                    yield return null;
                }
                conveyor.ConveyorBlockMove();
            }
        }
    }
    public override void DoTick()
    {
        Instance().StartCoroutine(MoveBlocks());
    }

    public float GetBlockSmoothSpeed()
    {
        return smoothSpeed;
    }

    public void OnDrawGizmos()
    {
        if(blockParent == null)
        {
            return;
        }
        Gizmos.matrix = blockParent.localToWorldMatrix;
        Gizmos.color = Color.red;
        foreach(Vector3Int pos in occupiedSpaces)
        {
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }

    public InteractableBlock[] InteractableBlocks()
    {
        return interactableBlocks;
    }

    public void UpdateInteractables()
    {
        interactableBlocks = FindObjectsOfType<InteractableBlock>();
    }
}
