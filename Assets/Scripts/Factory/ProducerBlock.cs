using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducerBlock : TickBlock
{
    [SerializeField] int tickRate;
    int currTick;

    [SerializeField] GameObject block;

    bool isStarted = false;

    public override void DoInitTick()
    {
        currTick = tickRate;
        isStarted = true;
    }
    public override void DoTick()
    {
        if(!isStarted)
        {
            return;
        }
        if(currTick <= 0)
        {
            currTick = tickRate;
            DoTickAction();
        }
        else
        {
            currTick--;
        }
    }

    void DoTickAction()
    {
        if(!CanProduce())
        {
            return;
        }
        GameObject newRoot = new GameObject();
        MovableRoot rootComponent = newRoot.AddComponent<MovableRoot>();
        BulletMod newMod = newRoot.AddComponent<BulletMod>();
        rootComponent.SetRootMod(newMod);
        newRoot.transform.SetParent(ConveyorSystemManager.Instance().blockParent);
        newRoot.transform.localRotation = transform.localRotation;
        newRoot.transform.position = transform.position + transform.forward;

        GameObject newBlock = Instantiate(block);
        rootComponent.AddBlock(newBlock.GetComponent<MovablePart>());

        
        newBlock.transform.localPosition = Vector3.zero;
        newBlock.transform.localRotation = Quaternion.identity;


        rootComponent.AddPartCoords();
        newRoot.transform.name = "BlockRoot";
    }

    bool CanProduce()
    {
        foreach (Vector3 space in ConveyorSystemManager.Instance().occupiedSpaces)
        {
            Vector3Int comparisonVector = new Vector3Int
                (
                    Mathf.RoundToInt(transform.localPosition.x),
                    Mathf.RoundToInt(transform.localPosition.y),
                    Mathf.RoundToInt(transform.localPosition.z) + 1
                );
            if (ConveyorSystemManager.Instance().occupiedSpaces.Contains(comparisonVector))
            {
                return false;
            }
        }
        return true;
    }
}
